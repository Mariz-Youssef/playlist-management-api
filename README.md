# Playlist Management API

A RESTful API for managing user playlists and their songs, built with **ASP.NET Core 10**, **Entity Framework Core**, and **SQL Server**.

## Features

* User registration and JWT authentication
* Create playlists
* Add songs to playlists
* Retrieve the authenticated user's playlists
* Update playlists
* Delete playlists
* Remove songs from playlists
* Playlist ownership and authorization
* Input validation and centralized exception handling
* SQL Server database with Entity Framework Core
* Unit tests

## Architecture

The application follows a **layered architecture** within a single ASP.NET Core Web API project:

**Controller → Service → Repository → Entity Framework Core → SQL Server**

The project is also organized by feature to keep related functionality grouped together and allow the application to be extended easily.

## Technology Stack

* **ASP.NET Core 10**
* **C#**
* **Entity Framework Core**
* **SQL Server**
* **JWT Authentication**
* **Swagger / OpenAPI**
* **xUnit**
* **Moq**

## Documentation

Additional project documentation is available in the **documents folder**.

### Submission Guide

**Submission_Guide.pdf** contains:

* Instructions for running the project
* Project demo links
* Reusable code resources used during development
* AI usage and related AI chat contexts

**Architecture&Design_Document.pdf** contains: 

The architecture and database design documentation covers:

* Architecture design and justification
* Layer responsibilities
* Design patterns and SOLID principles
* Database design and justification
* Database relationships
* Business logic and business rules

## Running the Project

For complete setup and execution instructions, including database and JWT configuration, please refer to:

**Submission_Guide.pdf**

## API Documentation

Once the application is running, Swagger UI can be used to explore and test the available API endpoints.

The API includes authentication endpoints as well as protected playlist management endpoints.

## Testing

The solution includes separate projects for:

* **Unit Tests** — testing business logic in isolation.

## Scope

The application focuses on playlist management. Songs are treated as an existing catalog and are seeded into the database for the scope of this assessment.

Song catalog management is outside the current scope. It could be introduced as a future enhancement with role-based authorization for administrative Song CRUD operations.
