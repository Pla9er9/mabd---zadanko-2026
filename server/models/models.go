package models

import "time"

type User struct {
	ID           uint   `gorm:"primaryKey" json:"id"`
	Username     string `gorm:"uniqueIndex;not null" json:"username"`
	PasswordHash []byte `gorm:"not null" json:"-"`
	Tasks        []Task `json:"tasks,omitempty"`
}

type Task struct {
	ID          uint      `gorm:"primaryKey" json:"id"`
	CreatedAt   time.Time `json:"created_at"`
	Title       string    `gorm:"not null" json:"title"`
	Description string    `json:"description"`
	Category    string    `gorm:"not null" json:"category"`
	DueDate     time.Time `gorm:"not null" json:"due_date"`
	IsDone      bool      `gorm:"not null;default:false" json:"isDone"`
	UserID      uint      `gorm:"not null;index" json:"user_id"`
	User        User      `json:"-"`
}

type RegisterRequest struct {
	Username string `json:"username"`
	Password string `json:"password"`
}

type LoginRequest struct {
	Username string `json:"username"`
	Password string `json:"password"`
}

type TaskRequest struct {
	Title       string `json:"title"`
	Description string `json:"description"`
	Category    string `json:"category"`
	DueDate     string `json:"due_date"`
}

type TaskResponse struct {
	ID          uint   `json:"id"`
	CreatedAt   string `json:"created_at"`
	Title       string `json:"title"`
	Description string `json:"description"`
	Category    string `json:"category"`
	DueDate     string `json:"due_date"`
	IsDone      bool   `json:"isDone"`
	UserID      uint   `json:"user_id"`
}
