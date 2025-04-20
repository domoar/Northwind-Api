# Northwind

[![Build, Test & Generate docs](https://github.com/domoar/Northwind-Api/actions/workflows/build.yaml/badge.svg)](https://github.com/domoar/Northwind-Api/actions/workflows/build.yaml)

## Api

To debug or run the api locally use

```bash
dotnet watch run
```

## Testing

All Tests can be run by chaning the working directory of the unit or integration tests and then using the command:

```bash
dotnet test
```

Applying the `[Trait]` attribute at the **class** level allows, that every test method in that class can be run seperatly.

Example to run individual test parts:

```bash
dotnet test --filter "category=application"
```

## Third-Party Content Notice

This project includes data derived from:

- **Northwind PostgreSQL SQL Dump**
  - Original source: https://github.com/pthom/northwind_psql
  - License: MIT
  - Author: [Pierre THOMAS](https://github.com/pthom)

## Database

To set up the database in a Docker container, use the provided Compose files in __extras__/db/postgres/

1. Start in Setup Mode (with root access)
This allows you to run initialization commands as root inside the container.

```bash
docker compose -f docker-compose.yml -f docker-compose.override.yml up --build
```

Then in a second terminal, open a shell inside the running container:

```bash
docker exec -it db bash
```

You should now be in a root@postgres shell.

Run your required setup/configuration commands here (e.g., setting up permissions, copying files, adjusting volumes, etc.).

2. Stop and Restart in Production Mode
Once your setup is done, bring the container down:

```bash
docker compose down
```

Then start the database in secure (non-root) mode:

```bash
docker compose up -d
```

Now in a new terminal, open a shell again:

```bash
docker exec -it db bash
```

You should now be inside as postgres@postgres.

Run your SQL initialization scripts or psql commands here as needed.

You're now running the database securely with locked-down permissions. Use this dual-mode approach any time you need elevated setup access.

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
  --namespace Infrastructure.Entities \
  --context-namespace Infrastructure.Context \
  --schema northwind \
  --use-database-names \
  --no-onconfiguring \
  --force
```
