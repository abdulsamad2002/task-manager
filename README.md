# TaskFlow - Task Management System

A premium task management application built with **Angular**, **.NET 10 Web API**, and **SQL Server**.

## 🚀 Getting Started

Follow these steps to set up and run the project locally.

### 1. Database Setup (SQL Server)
The project uses SQL Server. You can use SQL Server Express or any variant.
1. Open **SQL Server Management Studio (SSMS)** or your preferred SQL tool.
2. Create a new database named `TaskManagerDB`.
3. Execute the scripts in the following order:
   - `Database/TableCreation.sql` (Creates tables and initial roles).
   - `Database/StoredProcedures.sql` (Adds required procedures like `sp_ValidateUser`, `sp_SaveTask`, etc.).

### 2. Backend Setup (.NET 10 API)
1. Open a terminal and navigate to: `Backend/TaskManagerAPI`.
2. Ensure you have the **.NET 10 SDK** installed.
3. Check the connection string in `appsettings.json`. By default, it uses:
   `Server=localhost\\SQLEXPRESS;Database=TaskManagerDB;Trusted_Connection=True;TrustServerCertificate=True;`
4. Run the API:
   ```bash
   dotnet watch run
   ```
5. The API will be available at `http://localhost:5253/` (the port may vary; check the console output). You can access Swagger at `/swagger/index.html`.

### 3. Frontend Setup (Angular)
1. Open a terminal and navigate to the `Frontend` directory.
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the application:
   ```bash
   npm start
   ```
4. Open your browser to `http://localhost:4200/`.

## 🛠️ Project Structure
- **/Frontend**: Angular standalone components, modern UI, and RxJS state management.
- **/Backend**: .NET 10 Clean Architecture with EF Core and JWT Authentication.
- **/Database**: SQL scripts for schema and stored procedures.

## ✅ Key Features
- Role-based Access Control (Admin, Manager, Employee).
- Full CRUD for Tasks (Create, View, Update, Delete).
- User Registration and Login.
- Responsive, Premium Glassmorphism Design.
