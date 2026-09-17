# MediCore Hospital Management System

A professional WPF-based hospital management system for Arabic-speaking users, built with C# .NET 8, MVVM, ADO.NET, and SQL Server.

## Overview

MediCore is a production-oriented Windows desktop application designed for private hospitals. It provides modules for patient management, appointments, billing, pharmacy, lab, radiology, emergency, admissions, HR, reports, security, and settings.

## Stack

- C#
- .NET 8 WPF
- XAML
- MVVM (CommunityToolkit.Mvvm)
- ADO.NET
- SQL Server
- Dependency Injection
- Serilog-style logging abstraction

## Project structure

- `MediCoreHospital.UI` – WPF application and Arabic RTL interface
- `MediCoreHospital.Application` – services, DTOs, validators, contracts
- `MediCoreHospital.Domain` – business entities and enums
- `MediCoreHospital.Infrastructure` – database access, security, settings
- `MediCoreHospital.Database` – SQL scripts and seed data
- `MediCoreHospital.Tests` – basic tests

## Run locally

Prerequisites:

- Visual Studio 2022 or VS Code with C# Dev Kit
- .NET 8 SDK
- SQL Server 2019+ or SQL Server LocalDB

Commands:

```bash
dotnet restore
dotnet build
cd MediCoreHospital.UI
dotnet run
```

## Database setup

1. Open SQL Server Management Studio.
2. Execute the scripts in the `MediCoreHospital.Database/Sql/` folder in order.
3. Update the connection string in `appsettings.json` if needed.

## Demo access

The project includes a seeded administrator account for demonstration:

- Username: `admin`
- Password: `Admin@123`

> Note: The application is designed for an Arabic-first hospital workflow and includes RTL support by default.

## Notes

This is a clean architectural foundation and enterprise-style starter for a real hospital management platform. It includes a professional Arabic WPF shell, dashboard structure, patient module, and database design skeleton for further expansion.
