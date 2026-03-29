# Task Manager - Setup Guide

## 🚀 Getting Started

Follow these steps to set up the Task Manager on your local machine.

### 📋 Prerequisites
- **Node.js**: v18 or later (for Angular).
- **.NET SDK**: v8.0 or later (for Backend).
- **SQL Server**: (Express or Developer Edition).
- **IDE**: VS Code (with Angular Language Service) or Visual Studio 2022.

---

## 💾 1. Database Setup
1. Open **SQL Server Management Studio (SSMS)**.
2. Execute the `Database\TableCreation.sql` script.
3. Execute the `Database\StoredProcedures.sql` script.
4. Verify that `TaskManagerDB` is created with 3 tables (Roles, Users, Tasks).

---

## 🔌 2. Backend Setup (.NET)
1. Navigate to the `Backend/TaskManagerAPI` folder.
2. Open `appsettings.json`.
3. Update the `ConnectionStrings:DefaultConnection` with your local SQL Server instance name.
4. Run the application:
   ```bash
   dotnet watch run
   ```
5. The API will be available at `http://localhost:5000`. Swagger documentation is at `/swagger`.

---

## 🌐 3. Frontend Setup (Angular)
1. Navigate to the `Frontend` folder.
2. Install dependencies:
   ```bash
   npm install
   ```
3. Run the development server:
   ```bash
   npm run start
   ```
4. Access the app at `http://localhost:4200`.

---

## 🛠️ 4. Polish & Cleanup (Maintenance)
- The 프로젝트 (Project) has been polished to remove legacy root files (`app.js`, `index.html`, etc.).
- Boilerplate code (`WeatherForecastController.cs`) has been removed.
- All documentation is placed in the `/documents` folder for easy reference.
