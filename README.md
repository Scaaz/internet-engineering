# WISIELEC
## Prerequistes
- Visual Studio 2019
- .NET CORE 3.1 https://dotnet.microsoft.com/download/dotnet-core/3.1 

## HOW TO RUN
 - Visual Studio: F5
 - CLI: `dotnet run`

## Host default address
http://localhost:5000
 
## DATABASE
### CREATE DATABASE
In Visual Studio --> Tools --> NuGet Package Manager --> Package Manager Console
In this console type:
`Update-Database`

This should create database file with name `Wisielec.db`

### Update Database
`Add-Migratrion migration_name` 

### Database program
SQLite Db Browser https://sqlitebrowser.org/dl/



