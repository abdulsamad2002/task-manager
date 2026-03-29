# Task Manager - High Level Overview

## 🏛️ System Architecture
The Task Manager project is a full-stack enterprise web application built using modern practices. It follows a decoupled frontend and backend architecture with a central SQL database.

### 🌐 Frontend (Angular)
- **Framework**: Angular 18+ (Standalone Components).
- **Styling**: Vanilla CSS with modern Design Tokens (Glassmorphism, Inter font).
- **Core Principles**: High performance, responsive design, visual excellence.
- **State Management**: Reactive signals and RxJS.

### 🔌 Backend (.NET API)
- **Framework**: .NET 8 / 9 Web API.
- **Patterns**: MVC (Model-View-Controller) with DTO (Data Transfer Object) separation.
- **Authentication**: JWT (JSON Web Token) with policy-based authorization (Admin, Manager, Employee).
- **Error Handling**: Global exception middleware for consistent JSON error responses.

### 💾 Database (SQL Server)
- **Engine**: MS SQL Server.
- **Structure**: Strongly typed schemas with Foreign Key integrity.
- **Performance**: Use of Stored Procedures for complex operations.

## ✨ Key Features
1. **User Authentication**: Secure Login/Register with role-based access control.
2. **Task Lifecyle**: Complete CRUD operations for task management.
3. **Advanced Filtering**: Real-time filtering by status, priority, and search.
4. **Interactive Dashboard**: Modern dark-mode interface with glassmorphism effects.
