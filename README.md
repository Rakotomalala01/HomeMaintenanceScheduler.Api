# Running the Project

## 1. Clone the repository

```
git clone https://github.com/Rakotomalala01/home-maintenance-scheduler-api.git
cd home-maintenance-scheduler-api
```

---

## 2. Configure environment variables

Create a `.env` file in the project root.

Example:

```
Discord__WebhookUrl=https://discord.com/api/webhooks/YOUR_WEBHOOK_URL
```

The `.env` file is used by Docker to inject environment variables into the application.

---

## 3. Start the application with Docker

Build and run the API using Docker Compose:

```
docker compose up --build
```

This will:

- Build the ASP.NET Core API container
- Start the API server
- Create the SQLite database automatically

---

## 4. Open Swagger

Once the container is running, open:

```
http://localhost:8080/swagger
```

Swagger allows you to test all API endpoints.

---

# Docker Environment Variables

The application reads configuration using environment variables.

Example `.env` file:

```
Discord__WebhookUrl=https://discord.com/api/webhooks/your_webhook_here
```

The double underscore (`__`) maps to ASP.NET configuration sections:

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

# Stopping the Application

To stop the Docker container:

```
docker compose down
```

---

# Future Improvements

- Task category enums
- Web UI dashboard