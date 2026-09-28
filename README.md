# Inventory Management System

A RESTful inventory management API built with ASP.NET Core, Entity Framework Core, and SQL Server.

The project focuses on backend API development, database integration, inventory validation, and automated testing.

## Technologies

- C#
- ASP.NET Core Web API
- .NET 10
- Entity Framework Core
- SQL Server
- xUnit
- EF Core In-Memory Database for integration testing

## Features

- Create, read, update, and delete products
- Adjust product inventory levels
- Prevent inventory from dropping below zero
- Search products by name
- Filter products by low-stock threshold
- Sort products by name, price, or quantity
- Validate product input
- Separate API request models from database entities
- Manage database schema changes with Entity Framework migrations
- Unit and integration testing

## API Endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create a product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |
| PATCH | `/api/products/{id}/stock` | Adjust inventory |
| GET | `/api/products?search={name}` | Search products |
| GET | `/api/products?sortBy={field}` | Sort products |
| GET | `/api/products/low-stock?threshold={amount}` | Get low-stock products |

## Example Product Request

```http
POST /api/products
Content-Type: application/json

{
  "name": "Keyboard",
  "description": "Mechanical keyboard",
  "price": 79.99,
  "quantity": 15
}
```

## Stock Management

Inventory is adjusted through the stock endpoint rather than directly modifying the product quantity.

Example:

```http
PATCH /api/products/10/stock
Content-Type: application/json

{
  "change": -3
}
```

Stock adjustments are validated before being saved, preventing inventory from becoming negative.

## Database

The application uses SQL Server with Entity Framework Core.

Database schema changes are managed through Entity Framework migrations, and product prices use explicit decimal precision for currency values.

## Testing

The project includes both unit and integration tests.

Unit tests cover stock calculations such as:

- Increasing stock
- Decreasing stock
- Preventing negative inventory
- Preventing integer overflow

Integration tests verify API behavior including:

- Product creation
- Input validation
- Stock adjustment
- Low-stock filtering
- `404 Not Found` responses

Integration tests use an in-memory database so the real SQL Server database is not modified.

Run tests in Visual Studio using:

**Test → Test Explorer → Run All Tests**

## Running the Project

1. Clone the repository.
2. Configure the SQL Server connection string.
3. Apply the Entity Framework migrations.
4. Run the `InventoryManagement.Api` project.
5. Use the included `.http` file to send requests to the API.

## Project Structure

```text
InventoryManagementSystem
│
├── InventoryManagement.Api
│   ├── Controllers
│   ├── Data
│   ├── Models
│   ├── Services
│   ├── Migrations
│   └── Program.cs
│
└── InventoryManagement.Tests
    ├── StockCalculatorTests.cs
    ├── ProductsApiTests.cs
    └── CustomWebApplicationFactory.cs
```
