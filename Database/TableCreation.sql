-- Task Manager Database - Table Creation
-- Created by Antigravity Polish Task

CREATE DATABASE TaskManagerDB;
GO

USE TaskManagerDB;
GO

-- 1. Roles Table
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL -- Admin, Manager, Employee
);

-- 2. Users Table
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    RoleId INT FOREIGN KEY REFERENCES Roles(RoleId)
);

-- 3. Tasks Table
CREATE TABLE Tasks (
    TaskId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Priority NVARCHAR(20), -- High, Medium, Low
    DueDate DATETIME,
    Status NVARCHAR(20) DEFAULT 'Pending', -- Pending, In-Progress, Completed
    AssignedToUserId INT FOREIGN KEY REFERENCES Users(UserId)
);

-- Initial Seed Data
INSERT INTO Roles (RoleName) VALUES ('Admin'), ('Manager'), ('Employee');
