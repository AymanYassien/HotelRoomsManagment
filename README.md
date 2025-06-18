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
- **Design Patterns**: Clean 3-tier separation, Generic API Responses  
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

## 🏨 Domain Model Overview

**One-to-Many Relationship:**
- A **Room** contains many **Beds**
- Each **Bed** has a unique **Bed Number** within the room

### Sample Entities:
```csharp
// Room.cs
public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Bed> Beds { get; set; }
}

// Bed.cs
public class Bed
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
}
