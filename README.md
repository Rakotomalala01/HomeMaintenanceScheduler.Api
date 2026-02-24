# 🏠 Home Maintenance Scheduler API

A backend-only ASP.NET Core Web API built with .NET 9 and EF Core (SQLite) that manages recurring home maintenance tasks and sends automated reminders.

---

## 🚀 Project Goal

This project allows users to:

- Create recurring home maintenance tasks  
- Track completion history  
- Automatically calculate next due dates  
- Send reminder notifications (Discord webhook)  
- Prevent reminder spam using a ReminderLog system  

This is a weekend MVP focused on clean backend architecture.

---

## 🏗 Tech Stack

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger (API testing)
- BackgroundService (for reminder engine)
- Discord Webhooks (notifications)

---

## 📦 Database Structure

The SQLite database contains three main tables:

1. **MaintenanceTasks** – Stores recurring task definitions  
2. **TaskCompletions** – Stores completion history  
3. **ReminderLogs** – Prevents duplicate reminder notifications  

---

## ▶️ Running the Project

```bash
dotnet restore
dotnet build
dotnet run