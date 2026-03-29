-- Task Manager Database - Stored Procedures
-- Created by Antigravity Polish Task

USE TaskManagerDB;
GO

-- 1. Create Task Procedure
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
    
    SELECT SCOPE_IDENTITY() AS NewTaskId;
END;
GO

-- 2. Update Task Status Procedure
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

-- 3. Delete Task Procedure
CREATE PROCEDURE sp_DeleteTask
    @TaskId INT
AS
BEGIN
    DELETE FROM Tasks WHERE TaskId = @TaskId;
END;
GO

-- 4. Get All Tasks Procedure (Admin View)
CREATE PROCEDURE sp_GetAllTasksAdmin
AS
BEGIN
    SELECT t.*, u.FullName as AssignedToName
    FROM Tasks t
    JOIN Users u ON t.AssignedToUserId = u.UserId
    ORDER BY t.DueDate DESC;
END;
GO
