# RaceDay Management System - Part 2 (RESTful API Development)

## Project Overview
I developed the RaceDay Management System as a comprehensive backend RESTful Web API using ASP.NET Core and Entity Framework Core with SQLite. This API serves as the backbone for the entire system, handling secure data operations, role-based access control, event scheduling, category definition, participant enrolments, and real-time race timing results with automated leaderboards.

---

## User Roles and Permissions
I implemented strict Role-Based Access Control (RBAC) to ensure a clear distinction between administrative operations and participant actions:

1. Organizer / Admin:
   - I authorized this role to create, update, and delete athletic events and venues.
   - I enabled organizers to define and manage age and distance categories for each event (e.g., Senior, 10km, 21km).
   - I implemented endpoint capabilities to record finish times for participants and automatically compute leaderboard rankings based on elapsed time.

2. Participant:
   - I built endpoints allowing participants to register accounts and manage their profile details.
   - I enabled participants to browse available events and categories.
   - I structured the enrolment process so participants can enter events by selecting a category, automatically receiving a unique race number.
   - I provided access for participants to view official race results and leaderboards.

---

## Technology Stack
- Framework: ASP.NET Core Web API
- Database: SQLite / Entity Framework Core (Code-First Approach)
- Security: JSON Web Tokens (JWT) and BCrypt Password Hashing
- Testing Framework: xUnit and FluentAssertions
- CI/CD Automation: GitHub Actions

---
