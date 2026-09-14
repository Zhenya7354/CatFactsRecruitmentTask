# Cat Facts App

Cat Facts App is a small ASP.NET Core Minimal API that fetches a random cat fact from the external Cat Fact API, returns it to the caller, and stores the fact locally as JSON lines.

## Tech Stack

- .NET 10
- ASP.NET Core Minimal API
- HttpClientFactory
- Options pattern
- Result pattern
- OpenAPI and Scalar API reference
- Docker
- xUnit, NSubstitute, and FluentAssertions for unit tests

## Features

- Fetches a random cat fact from `https://catfact.ninja/fact`
- Returns the fetched fact through an HTTP endpoint
- Saves each fetched fact to a configurable local file
- Uses a repository abstraction for persistence
- Uses `Result` and `Result<T>` for expected operation failures
- Provides OpenAPI and Scalar documentation in Development environment
- Includes unit tests for the service, client, repository, and endpoint handler

## Getting Started

### Prerequisites

- .NET 10 SDK
- Docker, optional

### Run Locally

Restore dependencies:

```bash
dotnet restore
```

Run the application:

```bash
dotnet run
```

By default, the application starts using the URLs configured in `Properties/launchSettings.json`:

```text
http://localhost:5110
https://localhost:7073
```

The API endpoint is available at:

```text
http://localhost:5110/catfacts
```

In Development environment, Scalar API documentation is available at:

```text
http://localhost:5110/scalar/v1
```

## Tests

Run all tests from the solution directory:

```bash
dotnet test
```

The unit test project covers:

- `CatFactService` orchestration logic
- `CatFactClient` external API response handling
- `CatFactRepository` file persistence behavior
- `CatFactsEndpoints` HTTP result mapping

## API

### Get and Save a Cat Fact

```http
GET /catfacts
```

Fetches a random cat fact from the external API, saves it to the configured file, and returns the saved fact.

Example response:

```json
{
  "fact": "Cats have five toes on their front paws, but only four toes on their back paws.",
  "length": 78
}
```

## Configuration

Application settings are stored in `appsettings.json`.

```json
{
  "CatFactApi": {
    "BaseUrl": "https://catfact.ninja",
    "Endpoint": "/fact"
  },
  "CatFactFile": {
    "Path": "catfact.txt"
  }
}
```

### CatFactApi

- `BaseUrl` - external Cat Fact API base URL
- `Endpoint` - endpoint used to fetch a random cat fact

### CatFactFile

- `Path` - local file path where fetched facts are appended as JSON lines

## Docker

Build the Docker image:

```bash
docker build -t cat-facts-app .
```

Run the container:

```bash
docker run -p 8080:8080 cat-facts-app
```

The API will be available at:

```text
http://localhost:8080/catfacts
```

Because the Dockerfile sets the environment to Development, Scalar is also available at:

```text
http://localhost:8080/scalar/v1
```

## Project Structure

```text
Configurations/    Options classes for external API and file persistence settings
CustomResults/     Result and Result<T> operation result types
Endpoints/         Minimal API endpoint mappings
Exceptions/        Global exception handling
Extensions/        Service registration and configuration extensions
Models/            Application and external API response models
Repositories/      Cat fact persistence abstraction and implementation
Services/          Cat fact business flow and external API client
```

Unit tests are stored in the sibling `CatFactsApp.UnitTests` project.

## Architecture Notes

The application separates responsibilities into small layers:

- `CatFactClient` communicates with the external Cat Fact API and returns `Result<CatFact>`.
- `CatFactService` coordinates fetching and saving a cat fact and returns `Result<CatFact>`.
- `ICatFactRepository` defines persistence through a domain-oriented `SaveAsync` method.
- `CatFactRepository` currently stores cat facts in a local file, while the service depends only on the repository abstraction.
- `Result` and `Result<T>` are used for expected failures in external API calls, persistence operations, and service orchestration.
- `CatFactsEndpoints` keeps HTTP-specific mapping at the edge by converting service results to `Ok` or `Problem` responses.
