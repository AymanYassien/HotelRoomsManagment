# 🛏️ Room Management System (Fullstack ASP.NET 8 MVC + API)

A full-stack Room Management system built using **ASP.NET Core 8**, leveraging a clean architecture that separates concerns between API services and the MVC client. It allows Admins to fully manage rooms and beds while regular users have read-only access.

---

## 🔧 Tech Stack

- **Backend**: ASP.NET Core 8 Web API  
- **Frontend**: ASP.NET Core 8 MVC 
- **Authentication**: ASP.NET Core Identity  
- **Authorization**: Role-based (`Admin`, `User`)  
- **Tokens**: JWT + Refresh Token (stored in **cookies**)  
- **Database**: SQL Server (LocalDB) using EF Core  
- **Design Patterns**: Clean separation, Generic API Responses  
- **Extra Features**: Pagination, Centralized Error Handling

---

## 🛡️ Authentication & Roles

The system uses **JWT authentication with refresh tokens**, where access tokens are issued on login, and **refresh tokens are securely stored in HttpOnly cookies**.

### Roles:
- 🔑 `Admin`  
  - Full CRUD operations on Rooms and Beds  
  - Can view users and manage the system

- 👤 `User`  
  - Can only **view** rooms and beds (read-only access)

---

📦 Features
✅ JWT + Refresh Token (cookie-based)

✅ ASP.NET Core Identity for user management

✅ Role-based Access Control (Admin/User)

✅ Generic API Responses for consistent output

✅ EF Core Relationships (Room ➝ Beds)

✅ Pagination Support

✅ Centralized Error Handling Middleware

✅ Secure MVC UI for both admin and user workflows

---
🚀 Getting Started
🖥️ Prerequisites
.NET 8 SDK

SQL Server or SQL Server LocalDB

📥 Clone & Run

// Run.Bash
git clone https://github.com/your-username/RoomManagementSystem.git
cd RoomManagementSystem
dotnet restore
dotnet ef database update
dotnet run

---
📂 Project Structure

/RoomManagementSolution
├── RoomManagement.API         # Web API (JWT Auth, Business Logic)
│   ├── Controllers
│   ├── Services
│   ├── DTOs
│   ├── Middleware
│   └── JWT Config & Extensions
│
├── RoomManagement.MVC         # ASP.NET MVC (UI Layer)
│   ├── Controllers
│   ├── Views
│   ├── Auth UI
│   └── Role-specific Views
│
└── RoomManagement.Domain      # Models & Identity
    ├── Entities (Room, Bed)
    ├── ApplicationUser.cs
    └── Enums / DTOs
