# Customer Management API

A simple HTTP-based RESTful Web API built with ASP.NET Core for managing customers.

---

## Features

The API supports the following operations:

- **Create a customer**  
  `POST /api/customers`

- **List all customers**  
  `GET /api/customers`

- **Remove a customer by ID**  
  `DELETE /api/customers/{id}`

---

## Technical Overview

- Built with **.NET 8** and **ASP.NET Core**
- Uses model classes with JSON attributes for data transfer
- In-memory repository with concurrency-safe operations
- Basic logging using `ILogger`
- Adheres to REST principles:
  - `201 Created` for successful creation
  - `200 OK` for successful reads
  - `204 NoContent` for successful deletion
  - `400 BadRequest` for invalid input
  - `404 NotFound` for missing resources
  - `409 Conflict` for duplicate customers

---

## Testing

The solution includes:

### Unit Tests
- Repository logic
- Controller behavior (using mocks)

### Integration Tests
- Verifies routing, serialization, and status codes

Each API operation is covered by tests.

---

## Running the API

From the API project directory:

```bash
dotnet run
```

The API will start locally (typically on `https://localhost:5001`).

---

## Running Tests

From the solution root:

```bash
dotnet test
```

---

## Notes

- The repository is in-memory for simplicity.
- In a production scenario, this would be replaced with a persistent data store and centralized exception handling middleware.
- The project is fully runnable and testable on a Windows machine.
