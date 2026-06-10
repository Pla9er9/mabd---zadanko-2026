# Dokumentacja API — Server

Base URL: `http://localhost:8080`

## Autoryzacja
- Publiczne: `/register`, `/login` (POST)
- Wszystkie endpointy pod `/api` wymagają Basic Auth (nagłówek `Authorization: Basic base64(username:password)`).

---

## Schematy (JSON)

**RegisterRequest**
{
  "username": "string",
  "password": "string"
}

**LoginRequest**
{
  "username": "string",
  "password": "string"
}

**TaskRequest**
{
  "title": "string",         // wymagane
  "description": "string",   // opcjonalne
  "category": "string",      // wymagane
  "due_date": "YYYY-MM-DD"   // wymagane (albo RFC3339)
}

**TaskResponse**
{
  "id": number,
  "created_at": "RFC3339 string",
  "title": "string",
  "description": "string",
  "category": "string",
  "due_date": "YYYY-MM-DD",
  "isDone": boolean,
  "user_id": number
}

---

## Endpointy

### 1) Rejestracja
- Method: POST
- Path: `/register`
- Auth: brak
- Body: `RegisterRequest`
- Responses:
  - 201 Created: `{ "message": "registration successful" }`
  - 400 Bad Request: niepoprawny payload
  - 409 Conflict: nazwa użytkownika istnieje
  - 500 Internal Server Error
- Przykład:
  curl -X POST http://localhost:8080/register \
    -H "Content-Type: application/json" \
    -d '{"username":"user","password":"pass"}'

### 2) Logowanie
- Method: POST
- Path: `/login`
- Auth: brak
- Body: `LoginRequest`
- Responses:
  - 200 OK: `{ "message": "login successful" }`
  - 400 / 401 / 500
- Przykład:
  curl -X POST http://localhost:8080/login \
    -H "Content-Type: application/json" \
    -d '{"username":"user","password":"pass"}'

### 3) Profil (autoryzowane)
- Method: GET
- Path: `/api/profile`
- Auth: Basic
- Responses:
  - 200 OK: `{ "id": number, "username": string, "message": "authenticated profile data" }`
  - 401 Unauthorized
- Przykład:
  curl -u user:pass http://localhost:8080/api/profile

### 4) Utwórz zadanie
- Method: POST
- Path: `/api/tasks`
- Auth: Basic
- Body: `TaskRequest`
- Responses:
  - 201 Created: `TaskResponse`
  - 400 Bad Request: brak pól wymaganych lub niepoprawny format `due_date`
  - 500 Internal Server Error
- Przykład:
  curl -u user:pass -X POST http://localhost:8080/api/tasks \
    -H "Content-Type: application/json" \
    -d '{"title":"Kup mleko","category":"Zakupy","due_date":"2026-06-30"}'

### 5) Lista zadań (filtry)
- Method: GET
- Path: `/api/tasks`
- Auth: Basic
- Query params (opcjonalne):
  - `isDone` — `true`/`false` (filtrowanie po statusie)
  - `overdue` — `true`/`false` (czy zaległe wg daty UTC)
  - `category` — string (filtrowanie po kategorii)
- Responses:
  - 200 OK: `[]TaskResponse`
  - 400 / 500
- Przykład:
  curl -u user:pass "http://localhost:8080/api/tasks?isDone=false&category=Zakupy"

### 6) Raport miesięczny (PDF)
- Method: GET
- Path: `/api/tasks/report/month`
- Auth: Basic
- Response:
  - 200 OK: plik PDF (nagłówki `Content-Type: application/pdf`, `Content-Disposition: attachment; filename="tasks-monthly-report.pdf"`)
  - 500 Internal Server Error
- Przykład:
  curl -u user:pass http://localhost:8080/api/tasks/report/month --output tasks-report.pdf

### 7) Pobierz zadanie
- Method: GET
- Path: `/api/tasks/:id`
- Auth: Basic
- Responses:
  - 200 OK: `TaskResponse`
  - 400 Bad Request: nieprawidłowe id
  - 404 Not Found: zadanie nie istnieje lub nie należy do użytkownika
- Przykład:
  curl -u user:pass http://localhost:8080/api/tasks/1

### 8) Aktualizuj zadanie
- Method: PUT
- Path: `/api/tasks/:id`
- Auth: Basic
- Body: `TaskRequest` (pola wymagane: `title`, `category`, `due_date`)
- Responses:
  - 200 OK: `TaskResponse` (zaktualizowane)
  - 400 / 404 / 500
- Przykład:
  curl -u user:pass -X PUT http://localhost:8080/api/tasks/1 \
    -H "Content-Type: application/json" \
    -d '{"title":"Nowy tytuł","category":"Inne","due_date":"2026-07-01"}'

### 9) Przełącz status zadania (toggle)
- Method: PATCH
- Path: `/api/tasks/:id/toggle`
- Auth: Basic
- Responses:
  - 200 OK: `TaskResponse` (z nowym `isDone`)
  - 400 / 404 / 500
- Przykład:
  curl -u user:pass -X PATCH http://localhost:8080/api/tasks/1/toggle

### 10) Usuń zadanie
- Method: DELETE
- Path: `/api/tasks/:id`
- Auth: Basic
- Responses:
  - 204 No Content
  - 400 / 404 / 500
- Przykład:
  curl -u user:pass -X DELETE http://localhost:8080/api/tasks/1

---

## Uwagi implementacyjne
- Serwer używa Fiber (Go) i GORM + SQLite (`tasks.db`).
- Daty: `due_date` w żądaniach przyjmowane w formacie `YYYY-MM-DD` lub RFC3339; w odpowiedziach `due_date` jest w `YYYY-MM-DD`, a `created_at` w RFC3339.
- Filtr `overdue` porównuje datę `due_date` z początkiem dnia UTC.

---

Plik z dokumentacją: [server/API.md](server/API.md)

Jeśli chcesz, mogę wygenerować również specyfikację OpenAPI (`server/openapi.yaml`).
