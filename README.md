# Northwind

[![Build and Test .NET Project](https://github.com/domoar/Northwind/actions/workflows/build.yaml/badge.svg?branch=main)](https://github.com/domoar/Northwind/actions/workflows/build.yaml?branch=main)

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

# Usage

Want the full parametric experience?

Use

```bash
git clone https://github.com/domoar/Northwind.git
cd Northwind
```

to clone the repository then customize with 

```bash
dotnet new cleanarch --name MyApp --SolutionName "MyApp"
```