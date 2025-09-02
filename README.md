
# FinShark - Stock Portfolio API

FinShark is a robust RESTful API built with ASP.NET Core for tracking stock portfolios. It provides a comprehensive set of features including user authentication, stock and comment management, and personal portfolio tracking, all powered by real-time financial data from the Financial Modeling Prep (FMP) API.

This project was developed as a foundational exercise in building modern, scalable, and secure backend services using the .NET ecosystem, demonstrating a clean separation of concerns through the Repository and Service patterns.

## ✨ Key Features

*   **User Authentication & Security**: Secure user registration and login system using ASP.NET Core Identity and JWT Bearer Tokens for authenticated API requests.
*   **CRUD Functionality for Stocks**: Full create, read, update, and delete operations for stocks in the database.
*   **Dynamic Commenting System**: Users can add, view, update, and delete comments on any stock.
*   **Personalized Stock Portfolios**: Authenticated users can create and manage their own stock portfolios by adding or removing stocks.
*   **External API Integration**: Dynamically fetches and integrates real-time stock data from the Financial Modeling Prep (FMP) API if a stock does not exist in the local database.
*   **Advanced Querying**: Supports filtering, sorting, and pagination for stock and comment data to ensure efficient data retrieval.
*   **Repository Design Pattern**: Clean and maintainable data access layer that separates business logic from data persistence.

## 🛠️ Tech Stack

*   **Framework**: ASP.NET Core 8
*   **Database**: Microsoft SQL Server
*   **ORM**: Entity Framework Core 8
*   **Authentication**: ASP.NET Core Identity & JWT (JSON Web Tokens)
*   **API Testing**: Postman / Swagger
*   **External Services**: [Financial Modeling Prep API](https://site.financialmodelingprep.com/) for stock data.
*   **Primary Language**: C#

## 🏗️ Project Architecture

The API is structured following best practices for separation of concerns to ensure the codebase is scalable and maintainable.

*   **Controllers**: Handle incoming HTTP requests, validate input, and return appropriate HTTP responses. They orchestrate the flow of data between the client and the application.
*   **Repositories**: Abstract the data access logic. The controllers do not interact directly with Entity Framework Core; instead, they use repository interfaces, making the application more modular and easier to test.
*   **Services**: Contain business logic that is independent of the controllers. This includes external API calls (`FMPService`) and JWT generation (`TokenService`).
*   **DTOs (Data Transfer Objects) & Mappers**: Used to shape incoming and outgoing data, preventing over-posting and ensuring that the API's public-facing models are decoupled from the internal database entities.
*   **Models**: Represent the database entities, defined using Entity Framework Core code-first conventions.

## 🚀 Getting Started

To get a local copy up and running, follow these simple steps.

### Prerequisites

*   [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
*   [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (e.g., Express Edition)
*   A code editor like [Visual Studio](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
*   An API key from [Financial Modeling Prep](https://site.financialmodelingprep.com/developer)

### Installation & Setup

1.  **Clone the repository**
    ```sh
    git clone https://github.com/YOUR_USERNAME/FinShark.git
    cd FinShark
    ```

2.  **Configure your settings in `appsettings.json`**
    Open `appsettings.json` and update the following sections:
    *   Set up your database connection string.
    *   Add your JWT secret key, issuer, and audience.
    *   Add your Financial Modeling Prep API key.
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=FinShark;Trusted_Connection=True;TrustServerCertificate=True;"
      },
      "JWT": {
        "Issuer": "http://localhost:5095",
        "Audience": "http://localhost:5095",
        "SigningKey": "YOUR-SUPER-SECRET-SIGNING-KEY-GOES-HERE-12345"
      },
      "FMPKey": "YOUR_FMP_API_KEY_HERE"
    }
    ```

3.  **Apply Entity Framework Migrations**
    Run the following commands in your terminal to create the database and apply the schema.
    ```sh
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

4.  **Run the application**
    ```sh
    dotnet run
    ```
    The API will be available at `https://localhost:7188` (or a similar port). You can access the Swagger UI for testing at `https://localhost:7188/swagger`.

## Endpoints API

The following is a detailed breakdown of the available API endpoints.

---

### 👤 Account Management

*   **`POST /api/account/register`**
    *   **Description**: Registers a new user.
    *   **Request Body**:
        ```json
        {
          "username": "testuser",
          "email": "test@example.com",
          "password": "Password123!"
        }
        ```
    *   **Response**: `200 OK` with user details and a JWT.

*   **`POST /api/account/login`**
    *   **Description**: Authenticates a user and provides a JWT.
    *   **Request Body**:
        ```json
        {
          "userName": "testuser",
          "password": "Password123!"
        }
        ```
    *   **Response**: `200 OK` with user details and a JWT.

---

### 📈 Stock Management

*All stock endpoints require authentication (Bearer Token).*

*   **`GET /api/stock`**
    *   **Description**: Retrieves a paginated list of all stocks. Supports filtering and sorting.
    *   **Query Parameters**: `sortBy`, `companyName`, `symbol`, `pageNumber`, `pageSize`.
    *   **Response**: `200 OK` with a list of stocks.

*   **`GET /api/stock/{id}`**
    *   **Description**: Retrieves a single stock by its unique ID.
    *   **Response**: `200 OK` with the stock object.

*   **`POST /api/stock`**
    *   **Description**: Creates a new stock entry in the database.
    *   **Response**: `201 Created` with the newly created stock.

*   **`PUT /api/stock/{id}`**
    *   **Description**: Updates an existing stock.
    *   **Response**: `200 OK` with the updated stock object.

*   **`DELETE /api/stock/{id}`**
    *   **Description**: Deletes a stock from the database.
    *   **Response**: `204 No Content`.

---

### 💼 Portfolio Management

*All portfolio endpoints require authentication (Bearer Token).*

*   **`GET /api/portfolio`**
    *   **Description**: Retrieves all stocks in the current user's portfolio.
    *   **Response**: `200 OK` with a list of stocks from the user's portfolio.

*   **`POST /api/portfolio`**
    *   **Description**: Adds a stock to the current user's portfolio using its symbol.
    *   **Query Parameter**: `symbol` (e.g., `AAPL`).
    *   **Response**: `201 Created`.

*   **`DELETE /api/portfolio`**
    *   **Description**: Deletes a stock from the current user's portfolio.
    *   **Query Parameter**: `symbol` (e.g., `GOOGL`).
    *   **Response**: `200 OK`.

---

### 💬 Comment Management

*All comment endpoints require authentication (Bearer Token).*

*   **`GET /api/Comment`**
    *   **Description**: Retrieves a paginated list of all comments. Supports filtering by stock symbol.
    *   **Query Parameters**: `symbol`, `pageNumber`, `pageSize`, `sortBy`.
    *   **Response**: `200 OK` with a list of comments.

*   **`GET /api/Comment/{id}`**
    *   **Description**: Retrieves a single comment by its unique ID.
    *   **Response**: `200 OK` with the comment object.

*   **`POST /api/Comment/{symbol}`**
    *   **Description**: Creates a new comment on a stock, identified by its symbol.
    *   **Request Body**:
        ```json
        {
          "title": "Great Stock!",
          "content": "Planning to hold this for the long term."
        }
        ```
    *   **Response**: `201 Created` with the newly created comment.

*   **`PUT /api/Comment/{id}`**
    *   **Description**: Updates an existing comment.
    *   **Response**: `200 OK` with the updated comment object.

*   **`DELETE /api/Comment/{id}`**
    *   **Description**: Deletes a comment.
    *   **Response**: `204 No Content`.

---