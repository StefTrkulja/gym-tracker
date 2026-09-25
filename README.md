# GymTracker

GymTracker is a full-stack application for tracking gym workouts. Users can register, log workouts (type, duration, calories, intensity, fatigue), and follow their weekly and monthly progress on a dashboard.
## Features

* Register and log in with email and password, or sign in with Google
* Add, edit, delete, filter and paginate workouts
* Dashboard with weekly stats and charts for each month
* Profile editing

## Tech Stack

* **Frontend:** Angular 21, Angular Material, Chart.js
* **Backend:** ASP.NET Core (.NET 10) Web API, Clean Architecture, Entity Framework Core
* **Database:** PostgreSQL 17
* **Authentication:** JWT stored in an HttpOnly cookie, BCrypt password hashing
* **Infrastructure:** Docker Compose

## Getting Started

### Prerequisites

* Docker v28.0 or higher
* Docker Compose

### Setup

1. Create your environment file:

   ```bash
   cp .env.example .env
   ```

   Open .env and set your own values. JWT_SECRET must be at least 32 characters long.

2. Build and start everything:

   ```bash
   docker compose up --build -d
   ```

3. Open [http://localhost:4200](http://localhost:4200) and register a new account.

The database tables are created automatically on the first start (db/migrate.sql). To reset the database, run docker compose down -v and start again.

| Service  | URL                                           |
| -------- | --------------------------------------------- |
| Web app  | [http://localhost:4200](http://localhost:4200) |
| API      | [http://localhost:5129](http://localhost:5129) |
| Database | localhost:5432                                |

> Google sign-in only works on [http://localhost:4200](http://localhost:4200).
