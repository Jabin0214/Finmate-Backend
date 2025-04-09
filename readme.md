# Stock Portfolio Management Backend API

This is the backend API for a Stock Portfolio Management application. It provides functionalities for managing user accounts, portfolios, stocks, and comments.

## Table of Contents

* [Technologies Used](#technologies-used)
* [Prerequisites](#prerequisites)
* [Getting Started](#getting-started)
    * [Cloning the Repository](#cloning-the-repository)
    * [Building the Project](#building-the-project)
    * [Running the Project](#running-the-project)
    * [Applying Migrations](#applying-migrations)
    * [API Key Configuration](#api-key-configuration)
* [API Endpoints](#api-endpoints)
* [Database](#database)
* [Docker](#docker)
* [Project Structure](#project-structure)
* [API Testing](#api-testing)
* [Deployment](#deployment)
* [Contributing](#contributing)
* [License](#license)

## Technologies Used

* **.NET 8.0:** The primary framework for building the API.
* **C#:** The programming language used.
* **ASP.NET Core:** Framework for building web APIs.
* **Entity Framework Core:** ORM for database interactions.
* **ASP.NET Core Identity:** For managing user authentication and authorization.
* **Newtonsoft.Json:** For JSON serialization and deserialization.
* **Swashbuckle/OpenAPI:** For generating API documentation (Swagger UI).
* **Potentially other libraries:** (e.g., for JWT authentication, logging, etc.)

## Prerequisites

Before you begin, ensure you have the following installed:

* [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Git](https://git-scm.com/) for version control.
* A database system (e.g., SQL Server, PostgreSQL, SQLite) - configured in `appsettings.json`.
* [Docker](https://www.docker.com/get-started/) (optional, for containerization).

## Getting Started

Follow these steps to get the backend API up and running on your local machine.

### Cloning the Repository

1.  Open your terminal or command prompt.
2.  Navigate to the directory where you want to clone the repository.
3.  Run the following command:

    ```bash
    git clone <repository_url>
    ```

    (Replace `<repository_url>` with the actual URL of your repository)

### Building the Project

1.  Navigate to the project directory:

    ```bash
    cd <project_directory>
    ```

    (Replace `<project_directory>` with the name of the cloned repository)

2.  Build the solution using the .NET CLI:

    ```bash
    dotnet build api.sln
    ```

### Running the Project

1.  Navigate to the project directory (if you are not already there).
2.  Run the API using the .NET CLI:

    ```bash
    dotnet run --project ./
    ```

    This will start the API server, usually on `https://localhost:5001` and `http://localhost:5000` by default. Check the console output for the exact URLs.

### Applying Migrations

If your project uses Entity Framework Core, you might need to apply database migrations to create or update the database schema.

1.  Navigate to the project directory.
2.  Run the following command:

    ```bash
    dotnet ef database update -p ./
    ```

    This command will apply any pending migrations to your configured database. Ensure your database connection string is correctly set up in `appsettings.json`. The migration files are located in the `Migrations/` directory.

### API Key Configuration

This application relies on external APIs for certain functionalities. You will need to configure the following API keys in your `appsettings.json` file:

1.  Open the `appsettings.json` file located in the project root directory.
2.  Locate or create a section for API keys (e.g., a top-level section or within another configuration section).
3.  Add the following keys with their respective values:

    ```json
    {
      // ... other configurations
      "NewsKey": "YOUR_NEWS_API_KEY",
      "FMPKey": "YOUR_FMP_API_KEY",
      "DeepSeekKey": "YOUR_DEEPSEEK_API_KEY"
      // ...
    }
    ```

    Replace `"YOUR_NEWS_API_KEY"`, `"YOUR_FMP_API_KEY"`, and `"YOUR_DEEPSEEK_API_KEY"` with the actual API keys you have obtained for the following services:

    * **NewsKey:** Used for accessing news data, likely through the `INewsService`.
    * **FMPKey:** Used for accessing financial data from Financial Modeling Prep, likely through the `IFMPService`.
    * **DeepSeekKey:** Used for interacting with the DeepSeek AI service, likely through the `IAIService`.

    **Important:** Treat these API keys as sensitive information. Do not commit them directly to version control. Consider using environment variables or other secure methods for managing sensitive configuration in production environments. You might also have an `appsettings.Development.json` file for development-specific configurations where you can store these keys during development.

## API Endpoints

Based on the controller names, here are some of the likely API endpoints:

* **Account:**
    * `/api/Account/Register` (POST): For registering new users.
    * `/api/Account/Login` (POST): For logging in existing users.
* **Stock:**
    * `/api/Stock` (GET, POST, PUT, DELETE): For managing stock information.
* **Comment:**
    * `/api/Comment` (GET, POST, PUT, DELETE): For managing comments on stocks.
* **Portfolio:**
    * `/api/Portfolio` (GET, POST, PUT, DELETE): For managing user portfolios.

For detailed information about all available endpoints, their request and response formats, and authentication requirements, please refer to the generated API documentation (likely accessible via Swagger UI at a URL like `https://localhost:5001/swagger/index.html` or `http://localhost:5000/swagger/index.html` when the application is running).

## Database

The application uses Entity Framework Core to interact with a relational database. The specific database provider (e.g., SQL Server, PostgreSQL, SQLite) and connection details are configured in the `appsettings.json` file.

## Docker

The project includes a `Dockerfile` for containerizing the application using Docker.

1.  Navigate to the project root directory (where the `Dockerfile` is located).
2.  Build the Docker image:

    ```bash
    docker build -t stock-portfolio-api .
    ```

    (Replace `stock-portfolio-api` with your desired image name)

3.  Run the Docker container:

    ```bash
    docker run -p 8080:80 stock-portfolio-api
    ```

    (Replace `8080` with the desired host port and `80` with the container port if different)

The API will then be accessible at `http://localhost:8080` (or the port you specified).

## API Testing

The file `api.http` suggests the presence of HTTP request definitions that can be used with tools like the REST Client extension in VS Code or similar HTTP client tools for testing the API endpoints.

## Deployment

The application can be deployed in various ways, including:

* **Directly on a server:** After building, the application can be run using `dotnet run` on a server environment. You might need to configure a web server like Nginx or Apache as a reverse proxy.
* **Using Docker:** The provided `Dockerfile` can be used to create a Docker image that can be deployed to container orchestration platforms like Kubernetes or Docker Compose.
* **Azure App Service:** The application can be easily deployed to Azure App Service.
* **AWS Elastic Beanstalk:** Similar deployment options are available on AWS.

The contents of the `/publish` directory are typically used for deployment.

## Contributing

Contributions to this project are welcome. Please follow these steps:

1.  Fork the repository.
2.  Create a new branch for your feature or bug fix.
3.  Make your changes and commit them.
4.  Push your changes to your fork.
5.  Submit a pull request.

## License

[Specify the license under which the project is distributed. If no license is specified, the default copyright laws apply.]