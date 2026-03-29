# Task Manager - API Documentation

## 🔌 API Summary
- **Base URL**: `http://localhost:5000` (Local Development).
- **Format**: All requests and responses are in **JSON**.
- **Auth**: Use `Authorization: Bearer <TOKEN>` header.

## 🔐 Authentication (Auth)

### `POST /api/auth/login`
- **Body**: `{ "email": "...", "password": "..." }`
- **Response**: `{ "token": "...", "expires": "...", "user": { ... } }`

### `POST /api/auth/register`
- **Body**: `{ "fullName": "...", "email": "...", "password": "...", "roleId": 3 }`
- **Response**: `201 Created`

## 📝 Task Management (Tasks)

### `GET /api/tasks`
- **Auth**: Required.
- **Description**: Returns a list of all tasks assigned to the current user. Admins/Managers see all tasks.
- **Filter Params**: `?status=...&priority=...`

### `POST /api/tasks`
- **Auth**: Admin / Manager Only.
- **Body**: `{ "title": "...", "description": "...", "priority": "...", "dueDate": "...", "assignedToUserId": 1 }`
- **Response**: `201 Created`

### `PUT /api/tasks/{id}/status`
- **Auth**: Required.
- **Body**: `{ "newStatus": "Completed" }`
- **Response**: `200 OK`

### `DELETE /api/tasks/{id}`
- **Auth**: Admin / Manager Only.
- **Response**: `204 No Content`

## 🛠️ Global Responses
- **200 OK**: Request succeeded.
- **201 Created**: Resource created successfully.
- **401 Unauthorized**: Missing or invalid Bearer token.
- **403 Forbidden**: User lack permissions (role mismatch).
- **500 Error**: JSON error details with status code and message.
