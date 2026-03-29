# Task Manager - Database Schema

## 📊 Entity Relationship Diagram (ERD)

```mermaid
erDiagram
    Roles ||--o{ Users : "has many"
    Users ||--o{ Tasks : "is assigned to"
    
    Roles {
        int RoleId PK
        nvarchar(50) RoleName
    }
    
    Users {
        int UserId PK
        nvarchar(100) FullName
        nvarchar(100) Email UK
        nvarchar(100) Password
        int RoleId FK
    }
    
    Tasks {
        int TaskId PK
        nvarchar(200) Title
        nvarchar(max) Description
        nvarchar(20) Priority
        datetime DueDate
        nvarchar(20) Status
        int AssignedToUserId FK
    }
```

## 🏗️ Table Definitions

### 1. Roles Table
Storage for application roles.
- `RoleId`: (INT, Identity) Unique ID.
- `RoleName`: (NVARCHAR 50) Role type (Admin, Manager, Employee).

### 2. Users Table
Application user data.
- `UserId`: (INT, Identity) Unique ID.
- `FullName`: (NVARCHAR 100) Full user name.
- `Email`: (NVARCHAR 100, Unique) Login email.
- `Password`: (NVARCHAR 100, Hashed) User password.
- `RoleId`: (INT, FK) Reference to Roles.

### 3. Tasks Table
Task entity storage.
- `TaskId`: (INT, Identity) Unique ID.
- `Title`: (NVARCHAR 200) Task summary.
- `Description`: (NVARCHAR MAX) Detailed task info.
- `Priority`: (NVARCHAR 20) High, Medium, Low.
- `DueDate`: (DATETIME) Task deadline.
- `Status`: (NVARCHAR 20) Pending, In-Progress, Completed.
- `AssignedToUserId`: (INT, FK) User responsible for task.

## ⚙️ Stored Procedures

### `sp_ValidateUser`
Authenticates a user by email and password. Returns the full user record if successful.

### `sp_RegisterUser`
Creates a new user account with specified role. Prevents duplicate email registration.

### `sp_GetTasksByUserId`
Retrieves all tasks assigned to a specific user, including the assigned user's full name.

### `sp_GetAllTasksAdmin`
Complex join query that returns all tasks across the system plus assigned user details.

### `sp_CreateTask`
Quickly inserts a new task and returns the created record.

### `sp_UpdateTaskStatus`
Updates only the status field of a given task.

### `sp_UpdateTask`
Updates all fields of an existing task (Title, Description, Priority, Due Date, Status, Assignee).

### `sp_DeleteTask`
Performs a hard-delete of a task record.

### `sp_GetAllUsers`
Returns basic details (ID, Name, Email, Role) for all users in the system.
