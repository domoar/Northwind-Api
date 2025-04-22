# Northwind

[![Build, Test & Generate docs](https://github.com/domoar/Northwind-Api/actions/workflows/build.yaml/badge.svg)](https://github.com/domoar/Northwind-Api/actions/workflows/build.yaml)

## Api

To debug or run the api locally use

```bash
dotnet watch run --launch-profile "https"
```

## Testing

All Tests can be run by chaning the working directory of the unit or integration tests and then using the command:

```bash
cd tests/IntegrationTests
dotnet test --logger "console;verbosity=detailed"
```

or

```bash
cd tests/UnitTests
dotnet test --logger "console;verbosity=detailed"
```

Applying the `[Trait]` attribute at the **class** level allows, that every test method in that class can be run seperatly.

Example to run individual test parts:

```bash
dotnet test --filter "category=application"
```

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

All configuration files are located under:

```bash
extras/db/postgres/
```

### Setup Mode – Root Access, No TLS

Use this mode to fix file ownership and permissions. This runs the container without TLS and as the root user.

#### Start the Container in Setup Mode

```bash
cd __extras__/db/postgres/
docker compose --no-override up --build
This uses only docker-compose.yml and ignores docker-compose.override.yml.

Open a Shell Inside the Container
```bash
docker compose exec db bash
You should now see a root@postgres prompt.

Run Setup Commands
bash
Kopieren
Bearbeiten
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
docker compose up -d
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
psql --dbname=buch --username=buch --file=/sql/create-schema-northwind.sql
psql --dbname=postgres --username=postgres --file=/sql/northwind.sql
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
docker compose --no-override up
```

Production Mode:
Just run:

```bash
docker compose up -d
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
  --namespace Infrastructure.Entities \
  --context-namespace Infrastructure.Context \
  --schema northwind \
  --use-database-names \
  --no-onconfiguring \
  --force
```
