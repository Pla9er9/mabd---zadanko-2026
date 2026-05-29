package main

import (
	"encoding/base64"
	"errors"
	"log"
	"strconv"
	"strings"
	"time"

	"github.com/mabd-zadanko-2026/server/models"

	"github.com/gofiber/fiber/v2"
	"github.com/gofiber/fiber/v2/middleware/cors"
	"golang.org/x/crypto/bcrypt"
	"gorm.io/driver/sqlite"
	"gorm.io/gorm"
)

var db *gorm.DB

func main() {
	initDatabase()

	app := fiber.New()
	app.Use(cors.New(cors.Config{
		AllowOrigins: "http://localhost:4200,http://127.0.0.1:4200",
		AllowMethods: "GET,POST,PUT,PATCH,DELETE,OPTIONS",
		AllowHeaders: "Origin,Content-Type,Accept,Authorization",
	}))

	app.Post("/register", registerHandler)
	app.Post("/login", loginHandler)

	protected := app.Group("/api", basicAuthMiddleware)
	protected.Get("/profile", profileHandler)
	protected.Post("/tasks", createTaskHandler)
	protected.Get("/tasks", listTasksHandler)
	protected.Get("/tasks/report/month", monthlyTasksReportHandler)
	protected.Get("/tasks/:id", getTaskHandler)
	protected.Put("/tasks/:id", updateTaskHandler)
	protected.Patch("/tasks/:id/toggle", toggleTaskStatusHandler)
	protected.Delete("/tasks/:id", deleteTaskHandler)

	log.Fatal(app.Listen(":8080"))
}

func initDatabase() {
	var err error
	db, err = gorm.Open(sqlite.Open("tasks.db"), &gorm.Config{})
	if err != nil {
		log.Fatal("failed to connect database: ", err)
	}

	if err := db.AutoMigrate(&models.User{}, &models.Task{}); err != nil {
		log.Fatal("failed to migrate database: ", err)
	}
}

func registerHandler(c *fiber.Ctx) error {
	var req models.RegisterRequest
	if err := c.BodyParser(&req); err != nil {
		return fiber.NewError(fiber.StatusBadRequest, "invalid request payload")
	}

	if req.Username == "" || req.Password == "" {
		return fiber.NewError(fiber.StatusBadRequest, "username and password are required")
	}

	var existing models.User
	err := db.Where("username = ?", req.Username).First(&existing).Error
	if err == nil {
		return fiber.NewError(fiber.StatusConflict, "username already exists")
	}
	if !errors.Is(err, gorm.ErrRecordNotFound) {
		return fiber.NewError(fiber.StatusInternalServerError, "failed to check username")
	}

	hash, err := bcrypt.GenerateFromPassword([]byte(req.Password), bcrypt.DefaultCost)
	if err != nil {
		return fiber.NewError(fiber.StatusInternalServerError, "failed to hash password")
	}

	user := models.User{
		Username:     req.Username,
		PasswordHash: hash,
	}
	if err := db.Create(&user).Error; err != nil {
		return fiber.NewError(fiber.StatusInternalServerError, "failed to create user")
	}

	return c.Status(fiber.StatusCreated).JSON(fiber.Map{
		"message": "registration successful",
	})
}

func loginHandler(c *fiber.Ctx) error {
	var req models.LoginRequest
	if err := c.BodyParser(&req); err != nil {
		return fiber.NewError(fiber.StatusBadRequest, "invalid request payload")
	}

	var user models.User
	if err := db.Where("username = ?", req.Username).First(&user).Error; err != nil ||
		bcrypt.CompareHashAndPassword(user.PasswordHash, []byte(req.Password)) != nil {
		return fiber.NewError(fiber.StatusUnauthorized, "invalid credentials")
	}

	return c.JSON(fiber.Map{
		"message": "login successful",
	})
}

func basicAuthMiddleware(c *fiber.Ctx) error {
	auth := c.Get("Authorization")
	if auth == "" || !strings.HasPrefix(auth, "Basic ") {
		c.Set("WWW-Authenticate", "Basic realm=Restricted")
		return fiber.NewError(fiber.StatusUnauthorized, "authorization required")
	}

	payload, err := base64.StdEncoding.DecodeString(strings.TrimPrefix(auth, "Basic "))
	if err != nil {
		return fiber.NewError(fiber.StatusBadRequest, "invalid authorization header")
	}

	parts := strings.SplitN(string(payload), ":", 2)
	if len(parts) != 2 {
		return fiber.NewError(fiber.StatusBadRequest, "invalid authorization credentials")
	}

	username := parts[0]
	password := parts[1]

	var user models.User
	if err := db.Where("username = ?", username).First(&user).Error; err != nil ||
		bcrypt.CompareHashAndPassword(user.PasswordHash, []byte(password)) != nil {
		c.Set("WWW-Authenticate", "Basic realm=Restricted")
		return fiber.NewError(fiber.StatusUnauthorized, "invalid credentials")
	}

	c.Locals("username", username)
	c.Locals("userID", user.ID)
	return c.Next()
}

func profileHandler(c *fiber.Ctx) error {
	username := c.Locals("username").(string)
	userID := c.Locals("userID").(uint)

	return c.JSON(fiber.Map{
		"id":       userID,
		"username": username,
		"message":  "authenticated profile data",
	})
}

func createTaskHandler(c *fiber.Ctx) error {
	var req models.TaskRequest
	if err := c.BodyParser(&req); err != nil {
		return fiber.NewError(fiber.StatusBadRequest, "invalid request payload")
	}

	task, err := taskFromRequest(req, c.Locals("userID").(uint))
	if err != nil {
		return fiber.NewError(fiber.StatusBadRequest, err.Error())
	}

	if err := db.Create(&task).Error; err != nil {
		return fiber.NewError(fiber.StatusInternalServerError, "failed to create task")
	}

	return c.Status(fiber.StatusCreated).JSON(toTaskResponse(task))
}

func listTasksHandler(c *fiber.Ctx) error {
	userID := c.Locals("userID").(uint)
	var tasks []models.Task

	query := db.Where("user_id = ?", userID)

	isDone, hasIsDone, err := optionalBoolQuery(c, "isDone")
	if err != nil {
		return fiber.NewError(fiber.StatusBadRequest, "isDone must be true or false")
	}
	if hasIsDone {
		query = query.Where("is_done = ?", isDone)
	}

	overdue, hasOverdue, err := optionalBoolQuery(c, "overdue")
	if err != nil {
		return fiber.NewError(fiber.StatusBadRequest, "overdue must be true or false")
	}
	if hasOverdue {
		today := todayStartUTC()
		if overdue {
			query = query.Where("due_date < ?", today)
		} else {
			query = query.Where("due_date >= ?", today)
		}
	}

	category := strings.TrimSpace(c.Query("category"))
	if category != "" {
		query = query.Where("category = ?", category)
	}

	if err := query.Order("due_date asc").Find(&tasks).Error; err != nil {
		return fiber.NewError(fiber.StatusInternalServerError, "failed to list tasks")
	}

	responses := make([]models.TaskResponse, len(tasks))
	for i, task := range tasks {
		responses[i] = toTaskResponse(task)
	}

	return c.JSON(responses)
}

func optionalBoolQuery(c *fiber.Ctx, name string) (bool, bool, error) {
	value := strings.TrimSpace(c.Query(name))
	if value == "" {
		return false, false, nil
	}

	parsed, err := strconv.ParseBool(value)
	if err != nil {
		return false, true, err
	}

	return parsed, true, nil
}

func todayStartUTC() time.Time {
	now := time.Now()
	year, month, day := now.Date()
	return time.Date(year, month, day, 0, 0, 0, 0, time.UTC)
}

func monthlyTasksReportHandler(c *fiber.Ctx) error {
	userID := c.Locals("userID").(uint)
	username := c.Locals("username").(string)
	now := time.Now()
	from := now.AddDate(0, -1, 0)

	var tasks []models.Task
	err := db.Where("user_id = ? AND created_at >= ?", userID, from).
		Order("created_at desc").
		Find(&tasks).Error
	if err != nil {
		return fiber.NewError(fiber.StatusInternalServerError, "failed to generate task report")
	}

	report := buildMonthlyTasksPDF(username, from, now, tasks)
	c.Set(fiber.HeaderContentType, "application/pdf")
	c.Set(fiber.HeaderContentDisposition, `attachment; filename="tasks-monthly-report.pdf"`)
	return c.Send(report)
}

func getTaskHandler(c *fiber.Ctx) error {
	task, err := findUserTask(c)
	if err != nil {
		return err
	}

	return c.JSON(toTaskResponse(task))
}

func updateTaskHandler(c *fiber.Ctx) error {
	task, err := findUserTask(c)
	if err != nil {
		return err
	}

	var req models.TaskRequest
	if err := c.BodyParser(&req); err != nil {
		return fiber.NewError(fiber.StatusBadRequest, "invalid request payload")
	}

	updatedTask, err := taskFromRequest(req, c.Locals("userID").(uint))
	if err != nil {
		return fiber.NewError(fiber.StatusBadRequest, err.Error())
	}

	task.Title = updatedTask.Title
	task.Description = updatedTask.Description
	task.Category = updatedTask.Category
	task.DueDate = updatedTask.DueDate

	if err := db.Save(&task).Error; err != nil {
		return fiber.NewError(fiber.StatusInternalServerError, "failed to update task")
	}

	return c.JSON(toTaskResponse(task))
}

func deleteTaskHandler(c *fiber.Ctx) error {
	task, err := findUserTask(c)
	if err != nil {
		return err
	}

	if err := db.Delete(&task).Error; err != nil {
		return fiber.NewError(fiber.StatusInternalServerError, "failed to delete task")
	}

	return c.SendStatus(fiber.StatusNoContent)
}

func toggleTaskStatusHandler(c *fiber.Ctx) error {
	task, err := findUserTask(c)
	if err != nil {
		return err
	}

	task.IsDone = !task.IsDone
	if err := db.Save(&task).Error; err != nil {
		return fiber.NewError(fiber.StatusInternalServerError, "failed to toggle task status")
	}

	return c.JSON(toTaskResponse(task))
}

func findUserTask(c *fiber.Ctx) (models.Task, error) {
	id, err := strconv.Atoi(c.Params("id"))
	if err != nil || id <= 0 {
		return models.Task{}, fiber.NewError(fiber.StatusBadRequest, "invalid task id")
	}

	var task models.Task
	err = db.Where("id = ? AND user_id = ?", id, c.Locals("userID").(uint)).First(&task).Error
	if errors.Is(err, gorm.ErrRecordNotFound) {
		return models.Task{}, fiber.NewError(fiber.StatusNotFound, "task not found")
	}
	if err != nil {
		return models.Task{}, fiber.NewError(fiber.StatusInternalServerError, "failed to find task")
	}

	return task, nil
}

func taskFromRequest(req models.TaskRequest, userID uint) (models.Task, error) {
	if req.Title == "" || req.Category == "" || req.DueDate == "" {
		return models.Task{}, errors.New("title, category and due_date are required")
	}

	dueDate, err := parseDueDate(req.DueDate)
	if err != nil {
		return models.Task{}, errors.New("due_date must be in YYYY-MM-DD or RFC3339 format")
	}

	return models.Task{
		Title:       req.Title,
		Description: req.Description,
		Category:    req.Category,
		DueDate:     dueDate,
		UserID:      userID,
	}, nil
}

func parseDueDate(value string) (time.Time, error) {
	if dueDate, err := time.Parse("2006-01-02", value); err == nil {
		return dueDate, nil
	}

	return time.Parse(time.RFC3339, value)
}

func toTaskResponse(task models.Task) models.TaskResponse {
	return models.TaskResponse{
		ID:          task.ID,
		CreatedAt:   task.CreatedAt.Format(time.RFC3339),
		Title:       task.Title,
		Description: task.Description,
		Category:    task.Category,
		DueDate:     task.DueDate.Format("2006-01-02"),
		IsDone:      task.IsDone,
		UserID:      task.UserID,
	}
}
