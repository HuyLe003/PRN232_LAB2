# PRN232.LMS - Learning Management System API

ASP.NET Core RESTful API for a Learning Management System using 3-layer architecture.

## Quick Start - Docker Compose

### Prerequisites
- Docker Desktop installed and running
- Windows 10+ or Linux/Mac with Docker installed

### Running with Docker Compose

1. Navigate to the project root directory:
```bash
cd d:\PRN232\PRN232.LMS.DiepHuy
```

2. Start the services:
```bash
docker-compose up --build
```

This will:
- Create and run SQL Server database on port 1433
- Build and run the API on http://localhost:5000
- Automatically initialize the database with seed data

3. Access the application:
- **API**: http://localhost:5000
- **Swagger Documentation**: http://localhost:5000/swagger
- **SQL Server**: localhost:1433 (User: sa, Password: YourPassword@123)

### Stopping Services
```bash
docker-compose down
```

## Running Locally (Without Docker)

### Prerequisites
- .NET 9.0 SDK
- SQL Server (local or remote)

### Setup

1. Update connection string in `PRN232.LMS.API/appsettings.json`:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=PRN232_LMS;User Id=sa;Password=YOUR_PASSWORD;Encrypt=false;"
}
```

2. Create and apply migrations:
```bash
# From the project root
dotnet ef migrations add InitialCreate -p Repositories
dotnet ef database update -p PRN232.LMS.API
```

3. Run the API:
```bash
cd PRN232.LMS.API
dotnet run
```

4. Access Swagger: http://localhost:5000/swagger

## API Endpoints

### Students
- `GET /api/students` - Get all students (with paging, search, sort support)
- `GET /api/students/{id}` - Get student by ID
- `POST /api/students` - Create student
- `PUT /api/students/{id}` - Update student
- `DELETE /api/students/{id}` - Delete student

### Courses
- `GET /api/courses` - Get all courses
- `GET /api/courses/{id}` - Get course by ID
- `POST /api/courses` - Create course
- `PUT /api/courses/{id}` - Update course
- `DELETE /api/courses/{id}` - Delete course

### Enrollments
- `GET /api/enrollments` - Get all enrollments
- `GET /api/enrollments/{id}` - Get enrollment by ID
- `POST /api/enrollments` - Create enrollment
- `PUT /api/enrollments/{id}` - Update enrollment
- `DELETE /api/enrollments/{id}` - Delete enrollment

### Semesters
- `GET /api/semesters` - Get all semesters
- `GET /api/semesters/{id}` - Get semester by ID
- `POST /api/semesters` - Create semester
- `PUT /api/semesters/{id}` - Update semester
- `DELETE /api/semesters/{id}` - Delete semester

### Subjects
- `GET /api/subjects` - Get all subjects
- `GET /api/subjects/{id}` - Get subject by ID
- `POST /api/subjects` - Create subject
- `PUT /api/subjects/{id}` - Update subject
- `DELETE /api/subjects/{id}` - Delete subject

## Query Parameters

All list endpoints support:

### Paging
```
?page=1&pageSize=10
```

### Search
```
?search=nguyen
```

### Sorting (comma-separated, prefix with '-' for descending)
```
?sort=fullName,-dateOfBirth
```

### Expansion (include related data)
```
?expand=enrollments,courses
```

### Example Queries
```
GET /api/students?page=1&pageSize=20&search=nguyen&sort=fullName,-dateOfBirth&expand=enrollments

GET /api/enrollments?search=active&sort=-enrollDate&page=1&pageSize=50&expand=student,course
```

## Response Format

All API responses follow this format:

### Success Response (200)
```json
{
  "success": true,
  "message": "Request processed successfully",
  "data": {
    "studentId": 1,
    "fullName": "Nguyen Van A 1",
    "email": "student1@lms.edu.vn"
  },
  "errors": null
}
```

### List Response (200)
```json
{
  "success": true,
  "message": "Students retrieved successfully",
  "data": {
    "data": [...],
    "pagination": {
      "page": 1,
      "pageSize": 10,
      "totalItems": 100,
      "totalPages": 10
    }
  },
  "errors": null
}
```

### Error Response (404/500)
```json
{
  "success": false,
  "message": "Student with ID 999 not found",
  "data": null,
  "errors": []
}
```

## Project Architecture

### 3-Layer Architecture
- **API Layer** (PRN232.LMS.API): Controllers handling HTTP requests
- **Service Layer** (Services): Business logic
- **Repository Layer** (Repositories): Data access

### Data Models
- **Entity Models**: Database mapping (Repositories/Models/Entities)
- **Business Models (DTOs)**: For business processing (Services/Models)
- **Request Models (DTOs)**: Client input (Services/Models)
- **Response Models (DTOs)**: API output (Services/Models)

## Database Schema

### Seed Data
- 5 Semesters
- 50 Students
- 10 Subjects
- 20 Courses
- 500 Enrollments

## Technologies

- ASP.NET Core 9.0
- Entity Framework Core 9.0
- SQL Server
- Docker & Docker Compose
- Swagger/OpenAPI

## Notes

- No authentication/authorization implemented
- No global exception handling (as per requirements)
- For production, use environment-specific appsettings files
- Connection string can be overridden via environment variables for Docker
