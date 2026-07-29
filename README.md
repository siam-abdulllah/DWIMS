# DWIMS

This repository contains a .NET API and an Angular client application.

## Overview

- API: ASP.NET Core Web API (run from the `API`/`api` project folder).
- Client: Angular application (in the `client` folder).

See `client/README.md` for client-specific instructions.

## Prerequisites

- .NET SDK (6+ recommended)
- Node.js and npm
- Angular CLI (if you plan to run the client locally)

## Run the API (development)

1. Open the project in VS Code or your IDE of choice.
2. In a terminal, change to the API project folder:
   cd API
3. Run the API in watch mode:
   dotnet watch run
4. The API should be available at https://localhost:5001 (HTTP may be available on another port).
5. Swagger UI is available at:
   https://localhost:5001/swagger/index.html

The example API endpoint used by the client is:

https://localhost:5001/api/post

## Run the client (development)

1. Open another terminal and change to the client folder:
   cd client
2. Install dependencies (if not already installed):
   npm install
3. Run the Angular development server:
   ng serve
4. Browse to the client app at:
   http://localhost:4200/

## Database migrations (EF Core - Code First)

To add a migration (example name `InitialCreate`) and update the database, run these commands from the repository root or adjust the paths as needed:

Add a migration:

dotnet ef migrations add InitialCreate -p Infrastructure -s API -o Data/Migrations -c StoreContext

Update the database:

dotnet ef database update -p Infrastructure -s API -c StoreContext

Notes:
- Replace `InitialCreate` with an appropriate migration name for your change.
- Ensure the `dotnet-ef` tool and EF Core packages are installed.

## Testing the API

Open the Swagger UI (see link above) to explore and test API endpoints.

## Contributing

If you plan to contribute, please:
- Open an issue describing the change or bug.
- Create a branch for your work and open a pull request when ready.

## Additional files

- client/README.md — contains Angular client development instructions.

---

Generated / updated by repository maintainer instructions.
