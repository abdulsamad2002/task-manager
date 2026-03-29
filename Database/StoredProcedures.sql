-- Task Manager Database - Stored Procedures
-- Updated by Antigravity to include all required procedures

USE TaskManagerDB;
GO

-- 1. Validate User (Login)
CREATE PROCEDURE sp_ValidateUser
    @Email NVARCHAR(100),
    @Password NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Users WHERE Email = @Email AND Password = @Password;
END;
GO

-- 2. Register User
CREATE PROCEDURE sp_RegisterUser
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Password NVARCHAR(100),
    @RoleId INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        RETURN;
    END

    INSERT INTO Users (FullName, Email, Password, RoleId)
    VALUES (@FullName, @Email, @Password, @RoleId);

    SELECT * FROM Users WHERE UserId = SCOPE_IDENTITY();
END;
GO

-- 3. Get Tasks By User ID
CREATE PROCEDURE sp_GetTasksByUserId
    @UserId INT
AS
BEGIN
    SELECT t.*, u.FullName as AssignedToName
    FROM Tasks t
    JOIN Users u ON t.AssignedToUserId = u.UserId
    WHERE t.AssignedToUserId = @UserId
    ORDER BY t.DueDate DESC;
END;
GO

-- 4. Get All Tasks Admin View
CREATE PROCEDURE sp_GetAllTasksAdmin
AS
BEGIN
    SELECT t.*, u.FullName as AssignedToName
    FROM Tasks t
    JOIN Users u ON t.AssignedToUserId = u.UserId
    ORDER BY t.DueDate DESC;
END;
GO

-- 5. Create Task Procedure
CREATE PROCEDURE sp_CreateTask
    @Title NVARCHAR(200),
    @Description NVARCHAR(MAX),
    @Priority NVARCHAR(20),
    @DueDate DATETIME,
    @AssignedToUserId INT
AS
BEGIN
    INSERT INTO Tasks (Title, Description, Priority, DueDate, Status, AssignedToUserId)
    VALUES (@Title, @Description, @Priority, @DueDate, 'Pending', @AssignedToUserId);
    
    SELECT * FROM Tasks WHERE TaskId = SCOPE_IDENTITY();
END;
GO

-- 6. Update Task Status Procedure
CREATE PROCEDURE sp_UpdateTaskStatus
    @TaskId INT,
    @NewStatus NVARCHAR(20)
AS
BEGIN
    UPDATE Tasks 
    SET Status = @NewStatus 
    WHERE TaskId = @TaskId;
END;
GO

-- 7. Update Task Complete Details
CREATE PROCEDURE sp_UpdateTask
    @TaskId INT,
    @Title NVARCHAR(200),
    @Description NVARCHAR(MAX),
    @Priority NVARCHAR(20),
    @DueDate DATETIME,
    @Status NVARCHAR(20),
    @AssignedToUserId INT
AS
BEGIN
    UPDATE Tasks
    SET Title = @Title,
        Description = @Description,
        Priority = @Priority,
        DueDate = @DueDate,
        Status = @Status,
        AssignedToUserId = @AssignedToUserId
    WHERE TaskId = @TaskId;
END;
GO

-- 8. Delete Task Procedure
CREATE PROCEDURE sp_DeleteTask
    @TaskId INT
AS
BEGIN
    DELETE FROM Tasks WHERE TaskId = @TaskId;
END;
GO

-- 9. Get All Users (For Assignment)
CREATE PROCEDURE sp_GetAllUsers
AS
BEGIN
    SELECT UserId, FullName, Email, RoleId FROM Users;
END;
GO

