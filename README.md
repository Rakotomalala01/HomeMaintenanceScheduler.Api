# Home Maintenance Scheduler API

A lightweight **ASP.NET Core Web API** that manages recurring home maintenance tasks and automatically sends reminders to **Discord** when tasks are due or overdue.

The system tracks maintenance history, calculates the next due date automatically, and prevents duplicate reminders.

---

## Features

* Create and manage recurring maintenance tasks
* Track task completion history
* Automatically calculate next due dates
* Detect **due soon** and **overdue** tasks
* Send reminders to **Discord via webhook**
* Prevent duplicate reminder notifications
* Background worker that runs automatically
* SQLite database using **Entity Framework Core**
* Swagger UI for API testing

---

## Architecture Overview

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

## Technologies Used

* ASP.NET Core (.NET)
* Entity Framework Core
* SQLite
* Discord Webhooks
* BackgroundService worker
* Swagger (OpenAPI)
* Git

---

## Database Schema

### MaintenanceTasks

Stores recurring tasks.

| Field              | Description               |
| ------------------ | ------------------------- |
| Id                 | Primary key               |
| Title              | Task name                 |
| Category           | Task category             |
| RecurrenceType     | Days / Weeks / Months     |
| RecurrenceInterval | Frequency                 |
| StartDate          | Task start                |
| NextDueDate        | Next scheduled occurrence |
| IsActive           | Task enabled              |

---

### TaskCompletions

Stores completion history.

| Field       | Description             |
| ----------- | ----------------------- |
| Id          | Completion event ID     |
| TaskId      | Linked maintenance task |
| CompletedAt | Date completed          |
| Note        | Optional note           |

---

### ReminderLogs

Prevents duplicate reminders.

| Field   | Description                       |
| ------- | --------------------------------- |
| Id      | Log entry                         |
| TaskId  | Related task                      |
| DueDate | Due date associated with reminder |
| Type    | DueSoon / Overdue                 |
| SentAt  | Timestamp                         |
| Channel | Notification channel              |

---

## API Endpoints

### Tasks

```
POST   /tasks
GET    /tasks
GET    /tasks/{id}
POST   /tasks/{id}/complete
GET    /tasks/{id}/history
```

### Task Status

```
GET /tasks/due-soon?days=7
GET /tasks/overdue
```

### Reminder System

```
POST /reminders/run
```

---

## Reminder Logic

**Due Soon**

```
today <= NextDueDate <= today + configured days
```

Sent **once per due date**.

---

**Overdue**

```
NextDueDate < today
```

Sent **once per day** until completed.

---

## Running the Project

### 1. Clone the repository

```
git clone https://github.com/Rakotomalala01/home-maintenance-scheduler-api.git
cd home-maintenance-scheduler-api
```

---

### 2. Restore packages

```
dotnet restore
```

---

### 3. Create the database

```
dotnet tool run dotnet-ef database update
```

---

### 4. Configure Discord webhook

Use **.NET User Secrets** (recommended).

```
dotnet user-secrets init
dotnet user-secrets set "Discord:WebhookUrl" "YOUR_WEBHOOK_URL"
```

---

### 5. Run the API

```
dotnet run
```

Open Swagger:

```
http://localhost:5113/swagger
```

---

## Reminder Worker

A background worker runs periodically and scans for tasks that are:

* Due soon
* Overdue

It sends reminders through the configured Discord webhook.

Worker configuration:

```
ReminderSettings:
  DueSoonDays
  WorkerIntervalMinutes
```

---

## Example Discord Reminder

```
🔧 Due soon: Clean dryer vent (Due: 2026-03-11)
```

```
🚨 Overdue: Replace smoke detector batteries (Due: 2026-03-01)
```

---

## Project Structure

```
Controllers/
Background/
Services/
Entities/
Models/
Data/
```

---

## Future Improvements

* Docker support
* Discord embed messages
* Task categories enum
* Authentication for API
* Web UI dashboard

---

## License

MIT
