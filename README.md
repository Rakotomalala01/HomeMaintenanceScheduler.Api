# Home Maintenance Scheduler API

A lightweight **ASP.NET Core Web API** that manages recurring home maintenance tasks and automatically sends reminders to **Discord** when tasks are due soon or overdue.

The system tracks maintenance history, calculates the next due date automatically, and prevents duplicate reminder notifications.

This project demonstrates **backend API design, background workers, database modeling, containerization, and third-party integration with Discord**.

---

# Features

- Create and manage recurring maintenance tasks
- Track task completion history
- Automatically calculate next due dates
- Detect **due soon** and **overdue** tasks
- Send reminders to **Discord via webhook**
- Prevent duplicate reminder notifications
- Background worker that runs automatically
- SQLite database using **Entity Framework Core**
- Dockerized application for portable deployment
- Swagger UI for API testing

---

# Demo

## Swagger API

Use Swagger to interact with the API.

```
http://localhost:8080/swagger
```

Example operations:

- Create a maintenance task
- Complete a task
- Check overdue tasks
- Trigger reminders manually

## Discord Reminder Example

Example reminder messages sent by the system:

```
🔧 Due soon: Clean dryer vent (Due: 2026-03-11)
```

```
🚨 Overdue: Replace smoke detector batteries (Due: 2026-03-01)
```

---

# Architecture Overview

```
Client / Swagger
        │
        ▼
TasksController
        │
        ▼
ReminderScanService
        │
        ▼
DiscordNotifier
        │
        ▼
Discord Webhook
```

Database (SQLite):

```
MaintenanceTasks
TaskCompletions
ReminderLogs
```

---

# Technologies Used

- ASP.NET Core (.NET)
- Entity Framework Core
- SQLite
- Discord Webhooks
- BackgroundService Worker
- Swagger / OpenAPI
- Docker
- Git

---

# Database Schema

## MaintenanceTasks

Stores recurring maintenance tasks.

| Field | Description |
|------|-------------|
| Id | Primary key |
| Title | Task name |
| Category | Task category |
| RecurrenceType | Days / Weeks / Months |
| RecurrenceInterval | Frequency |
| StartDate | Task start |
| NextDueDate | Next scheduled occurrence |
| IsActive | Task enabled |

---

## TaskCompletions

Stores completion history.

| Field | Description |
|------|-------------|
| Id | Completion event ID |
| TaskId | Linked maintenance task |
| CompletedAt | Date completed |
| Note | Optional note |

---

## ReminderLogs

Prevents duplicate reminders.

| Field | Description |
|------|-------------|
| Id | Log entry |
| TaskId | Related task |
| DueDate | Due date associated with reminder |
| Type | DueSoon / Overdue |
| SentAt | Timestamp |
| Channel | Notification channel |

---

# API Endpoints

## Tasks

```
POST   /tasks
GET    /tasks
GET    /tasks/{id}
POST   /tasks/{id}/complete
GET    /tasks/{id}/history
```

## Task Status

```
GET /tasks/due-soon?days=7
GET /tasks/overdue
```

## Reminder System

```
POST /reminders/run
```

This endpoint allows manual triggering of reminders for demonstration or testing.

---

# Reminder Logic

## Due Soon

```
today <= NextDueDate <= today + configured days
```

Sent **once per due date**.

---

## Overdue

```
NextDueDate < today
```

Sent **once per day** until the task is completed.

---

# Running the Project

## 1. Clone the repository

```
git clone https://github.com/Rakotomalala01/home-maintenance-scheduler-api.git
cd home-maintenance-scheduler-api
```

---

## 2. Create environment variables

Create a `.env` file in the project root.

Example:

```
Discord__WebhookUrl=https://discord.com/api/webhooks/YOUR_WEBHOOK_URL
```

The `.env` file allows Docker to securely inject environment variables into the application.

---

## 3. Run with Docker

Build and start the API:

```
docker compose up --build
```

Docker will:

- Build the ASP.NET Core API container
- Start the application
- Create the SQLite database automatically

---

## 4. Open Swagger

Once the container is running, open:

```
http://localhost:8080/swagger
```

You can now test all API endpoints.

---

# Docker Environment Variables

Example `.env` file:

```
Discord__WebhookUrl=https://discord.com/api/webhooks/your_webhook_here
```

ASP.NET configuration mapping:

```
Discord__WebhookUrl → Discord:WebhookUrl
```

---

# Database

The application uses **SQLite** for persistence.

Docker mounts a volume so the database persists between container restarts.

```
home_maintenance.db
```

Tables created by EF Core:

```
MaintenanceTasks
TaskCompletions
ReminderLogs
```

---

# Reminder Worker

A background worker periodically scans the database for tasks that are:

- Due soon
- Overdue

It sends reminders through the configured Discord webhook.

Worker configuration:

```
ReminderSettings:
  DueSoonDays
  WorkerIntervalMinutes
```

Example configuration:

```
DueSoonDays = 3
WorkerIntervalMinutes = 1440
```

The worker runs **once per day** by default.

---

# Project Structure

```
Controllers/
Background/
Services/
Entities/
Models/
Data/
```

---

# Future Improvements

- Web UI dashboard
---

# License

MIT