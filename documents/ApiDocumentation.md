# Task Manager - API Documentation

## 🔌 API Summary
- **Base URL**: `http://localhost:5000` (Local Development).
- **Format**: All requests and responses are in **JSON**.
- **Auth**: Use `Authorization: Bearer <TOKEN>` header.

## 🔐 Authentication (Auth)

### `POST /api/Auth/login`
- **Body**: `{ "email": "...", "password": "..." }`
- **Response**: `{ "token": "...", "user": { "userId": 1, "fullName": "...", "email": "...", "roleId": 3 } }`

### `POST /api/Auth/register`
- **Body**: `{ "fullName": "...", "email": "...", "password": "...", "roleId": 3 }`
- **Response**: `200 OK` (User object)

## 📝 Task Management (Task)

### `GET /api/Task/my-tasks`
- **Auth**: Required (All Roles).
- **Description**: Returns a list of all tasks assigned to the currently authenticated user.

### `GET /api/Task/all-tasks`
- **Auth**: Admin / Manager Only.
- **Description**: Returns a list of all tasks in the system.

### `POST /api/Task/create`
- **Auth**: Admin / Manager Only.
- **Body**: `{ "title": "...", "description": "...", "priority": "...", "dueDate": "...", "assignedToUserId": 1 }`
- **Response**: `200 OK` (Created Task message)

### `PUT /api/Task/update`
- **Auth**: Admin / Manager Only.
- **Description**: Fully updates an existing task.
- **Body**: `{ "taskId": 1, "title": "...", "description": "...", "priority": "...", "dueDate": "...", "status": "...", "assignedToUserId": 2 }`
- **Response**: `200 OK`

### `PUT /api/Task/update-status/{taskId}`
- **Auth**: Required (All Roles).
- **Body**: `"In Progress"` (Plain string in body)
- **Response**: `200 OK`

### `DELETE /api/Task/delete/{taskId}`
- **Auth**: Admin Only.
- **Response**: `200 OK`

### `GET /api/Task/users`
- **Auth**: Admin / Manager Only.
- **Description**: Returns all users in the system for task assignment.
- **Response**: `200 OK` (Array of user objects)

## 🛠️ Global Responses
- **200 OK**: Request succeeded.
- **401 Unauthorized**: Missing or invalid Bearer token.
- **403 Forbidden**: User lacks permissions (role mismatch).
- **500 Error**: JSON error details with status code and message.
