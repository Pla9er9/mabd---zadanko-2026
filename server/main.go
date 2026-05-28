package main

import (
	"encoding/base64"
	"strings"
	"sync"
	"github.com/mabd-zadanko-2026/server/models"

	"github.com/gofiber/fiber/v2"
	"golang.org/x/crypto/bcrypt"
)

var (
	users     = make(map[string]*models.User)
	userMutex sync.RWMutex
)

func main() {
	app := fiber.New()

	app.Post("/register", registerHandler)
	app.Post("/login", loginHandler)

	protected := app.Group("/api", basicAuthMiddleware)
	protected.Get("/profile", profileHandler)

	app.Listen(":8080")
}

func registerHandler(c *fiber.Ctx) error {
	var req models.RegisterRequest
	if err := c.BodyParser(&req); err != nil {
		return fiber.NewError(fiber.StatusBadRequest, "invalid request payload")
	}

	if req.Username == "" || req.Password == "" {
		return fiber.NewError(fiber.StatusBadRequest, "username and password are required")
	}

	userMutex.Lock()
	defer userMutex.Unlock()

	if _, exists := users[req.Username]; exists {
		return fiber.NewError(fiber.StatusConflict, "username already exists")
	}

	hash, err := bcrypt.GenerateFromPassword([]byte(req.Password), bcrypt.DefaultCost)
	if err != nil {
		return fiber.NewError(fiber.StatusInternalServerError, "failed to hash password")
	}

	users[req.Username] = &models.User{
		Username:     req.Username,
		PasswordHash: hash,
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

	userMutex.RLock()
	user, exists := users[req.Username]
	userMutex.RUnlock()

	if !exists || bcrypt.CompareHashAndPassword(user.PasswordHash, []byte(req.Password)) != nil {
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

	userMutex.RLock()
	user, exists := users[username]
	userMutex.RUnlock()

	if !exists || bcrypt.CompareHashAndPassword(user.PasswordHash, []byte(password)) != nil {
		c.Set("WWW-Authenticate", "Basic realm=Restricted")
		return fiber.NewError(fiber.StatusUnauthorized, "invalid credentials")
	}

	c.Locals("username", username)
	return c.Next()
}

func profileHandler(c *fiber.Ctx) error {
	username := c.Locals("username").(string)

	return c.JSON(fiber.Map{
		"username": username,
		"message":  "authenticated profile data",
	})
}
