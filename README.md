# Northwind

[![CI - Build, Format, Test](https://github.com/domoar/Northwind-Api/actions/workflows/build.yml/badge.svg)](https://github.com/domoar/Northwind-Api/actions/workflows/build.yml) [![Generate documentation](https://github.com/domoar/Northwind-Api/actions/workflows/documentation.yml/badge.svg)](https://github.com/domoar/Northwind-Api/actions/workflows/documentation.yml) [![Generate Code Coverage](https://github.com/domoar/Northwind-Api/actions/workflows/codecoverage.yml/badge.svg)](https://github.com/domoar/Northwind-Api/actions/workflows/codecoverage.yml)

This project was created using the template from [Clean Architecture Template](https://github.com/domoar/CleanArchitectureTemplate), and will be redone once github templates allows variable names in templates. [Discussion](https://github.com/orgs/community/discussions/5336)

## ToC

 1. [Api](#api)
 2. [Docker](#dockerfile-and-dockercompose-for-the-project)
 3. [Database](#postgresql-database-setup-with-docker-compose)
 4. [Migration](#migration)
 5. [Architecture](#architecture)
 6. [CI/CD](#continuous-integration-und-continuous-deployment-cicd)
 7. [Tools](#tools)
 8. [Git Hooks](#git-hooks)

## Api

To debug or run the api locally use

```bash
dotnet watch run --launch-profile "https"
```

### Curl

e.g.

```bash
curl -X 'GET' \
  'https://localhost:7104/api/Employee/GetEmployee?employeeId=1' \
  -H 'accept: application/json'
```

### Swagger

[Swagger UI - localhost DBG](https://localhost:7104/swagger/index.html)

## DockerFile and DockerCompose for the project

There is a Dockerfile and a docker-compose.yml in the root directory. The DockerFile can be build with:

```bash
docker build .
```

The compose file starts five services:

- api – ASP.NET Core application.
- db – PostgreSQL 17 with SSL.
- pgadmin – Web UI for Postgres.
- seq – Centralised structured log server.
- jaeger – Distributed tracing system.

Start / Shutdown these services with

```bash
docker compose up -d 
docker compose down
```

### Seq for Logging

Seq captures and can query structures logs.

#### How to setup Seq

### Jaeger for Tracing

Jaeger displays distributed traces.

#### How to setup Jaeger

##### Additonal compose files for different purposes (.dcproj)

- `docker-compose.yml` + `docker-compose.override.yml` – default local-dev stack (override is loaded automatically).
- `docker-compose.dev.yml` – adds optional developer tooling (e.g. ).
- `docker-compose.prod.yml` – production-only tweaks (harder restart policies, resource limits, etc.).

```bash
# development 
docker compose -f docker-compose.yml -f docker-compose.prod.yml up -d

# production
docker compose -f docker-compose.yml -f docker-compose.dev.yml up -d
```

to run the developement stack.

[Docker merge strategies](https://docs.docker.com/compose/how-tos/multiple-compose-files/merge/)

## Testing

All Tests can be run by chaning the working directory of the unit or integration tests and then using the command:
Applying the `[Trait]` attribute at the **class** level allows, that every test method in that class can be run seperatly.

Example to run individual test parts:

```bash
dotnet test --filter "category=application"
```

### IntegrationTests

```bash
cd tests/IntegrationTests
dotnet test --logger "console;verbosity=detailed"
```

### UnitTests

```bash
cd tests/UnitTests
dotnet test --logger "console;verbosity=detailed"
```

### Code Coverage

Code Coverage is generated from unittests and integrationtests and merged into a report via GitHub Actions using the [ReportGenerator](https://github.com/danielpalme/ReportGenerator) tool.

[View Full Coverage Report](https://domoar.github.io/Northwind-Api/codecoverage/index.html)

### Postman

The testsuite contains a postman collection that can be used via the postman application or its vscode extension. In `.extras/postman`.

### Testsuite client

The testsuite also contains a .http file with environments pre configured. In `.extras/client` you can either use the vscode extension [Rest-Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) or the built in feature from VS2022.

## Third-Party Content Notice

This project includes data derived from:

- **Northwind PostgreSQL SQL Dump**
  - Original source: [Northwind PostgreSQL Sample on GitHub](https://github.com/pthom/northwind_psql)
  - License: MIT
  - Author: [Pierre THOMAS](https://github.com/pthom)

## PostgreSQL Database Setup with Docker Compose

This project supports a dual-mode setup for PostgreSQL using Docker Compose:

- **Setup Mode** (with root access): Use this to perform initial configuration, set permissions, and prepare mounted volumes.
- **Production Mode** (with TLS and `postgres` user): Use this mode for secure, day-to-day database operations.

Before starting the container for the first time:

1. Create the directory `C:/DockerVolumes/postgres/tls`

    ```bash
    mkdir C:\DockerVolumes\postgres\tls
    ```

2. Place your SSL certificate and key inside:
   - `certificate.crt`
   - `key.pem`

    ```bash
    copy .\.extras\tls\certificate.crt C:\DockerVolumes\postgres\tls\
    copy .\.extras\tls\key.pem C:\DockerVolumes\postgres\tls\
    ```

3. Create the directory `C:/DockerVolumes/postgres/sql/northwind`

    ```bash
    mkdir C:\DockerVolumes\postgres\tls
    ```

4. Place the .sql files from `.extras/db/postgres/sql` inside:
   - `create-db-northwind.sql`
   - `create-schema-northwind.sql`
   - `northwind.sql`

    ```bash
    copy .\.extras\db\postgres\sql\create-db-northwind.sql C:\DockerVolumes\postgres\sql\northwind\
    copy .\.extras\db\postgres\sql\create-schema-northwind.sql C:\DockerVolumes\postgres\sql\northwind\
    copy .\.extras\db\postgres\sql\northwind.sql C:\DockerVolumes\postgres\sql\northwind\
    ```

These files are mounted read-only into the container and required for PostgreSQL to start with SSL.

All configuration files are located under:

`.extras/db/postgres/`

### Setup Mode – Root Access, No TLS

Use this mode to fix file ownership and permissions. This runs the container without TLS and as the root user.

#### Start the Container in Setup Mode

```bash
cd .extras/db/postgres/
docker compose up --build
```

This uses only docker-compose.yml and ignores docker-compose.override.yml.

Open a Shell Inside the Container

```bash
docker compose exec db bash
```

You should now see a root@postgres prompt.

Run Setup Commands

```bash
chown postgres:postgres /var/lib/postgresql/tablespace
chown postgres:postgres /var/lib/postgresql/tablespace/northwind
chown postgres:postgres /var/lib/postgresql/key.pem
chown postgres:postgres /var/lib/postgresql/certificate.crt
chmod 400 /var/lib/postgresql/key.pem
chmod 400 /var/lib/postgresql/certificate.crt
exit
```

Stop the Container

```bash
docker compose down
```

### Production Mode – Secure, With TLS

Now that setup is complete, start the database securely using TLS and the postgres user.

Start the Container in Production Mode

```bash
docker compose -f docker-compose.yml up
```

This automatically combines docker-compose.yml and docker-compose.override.yml.

Open a Shell Inside the Container

```bash
docker compose exec db bash
```

You should now be logged in as postgres@postgres.

Run SQL Initialization Scripts

```bash
psql --dbname=postgres --username=postgres --file=/sql/create-db-northwind.sql
psql --dbname=northwind --username=postgres --file=/sql/create-schema-northwind.sql
psql --dbname=northwind --username=postgres --file=/sql/northwind.sql
exit
```

Stop the Container When Done

```bash
docker compose down
```

Switching Modes
Setup Mode:
Use --no-override to ignore the secure config:

```bash
docker compose up
```

Production Mode:
Just run:

```bash
docker compose -f docker-compose.yml up
```

This flexible dual-mode approach allows for secure database operations while preserving the ability to make low-level system changes when necessary.

## Migration

After setting up the database dotnet ef migrations can be generated using these commands

```bash
dotnet ef database update --project src/Infrastructure --startup-project src/Api
```

```bash
dotnet ef dbcontext scaffold \
  Npgsql.EntityFrameworkCore.PostgreSQL \
  --output-dir Entities \
  --context-dir Context \
  --context NorthwindContext \
  --project src/Infrastructure \
  --startup-project src/Api \
  --namespace Infrastructure.entity \
  --context-namespace Infrastructure.context \
  --schema northwind \
  --use-database-names \
  --no-onconfiguring \
  --force
```

## Architecture

## Continuous Integration und Continuous Deployment (CI/CD)

CI/CD is handled with Github Actions see the workflows in `.github/workflows`.

## Tools

Using the .editorconfig file all projects can be formatted using

```bash
cd dotnet format .\__Northwind__.sln --verbosity diagnostic
```

## Git Hooks

This repo ships with a pre-commit hook in **`.githooks/pre-commit`**. Enable it once per clone by running

```bash
git config core.hooksPath .githooks
```

to enable the hook after cloning the project.
[Git Hooks](https://git-scm.com/book/en/v2/Customizing-Git-Git-Hooks)
The pre-commit checks for formatting/ linting and also if the solution compiles and can be build without errors.
