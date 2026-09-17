# MediCore Hospital Management System

## Overview

MediCore is a professional Arabic-first Windows desktop hospital management system built with C#, WPF, MVVM, ADO.NET, and SQL Server.

## Goal

The project is designed to support private hospital operations such as patient management, appointments, billing, pharmacy, lab, radiology, surgeries, emergency, nursing, HR, audit logs, backup, and reporting.

## Solution structure

- `MediCoreHospital.UI` – WPF application and Arabic RTL front-end
- `MediCoreHospital.Application` – DTOs, contracts, business service interfaces
- `MediCoreHospital.Domain` – core entities and enums
- `MediCoreHospital.Infrastructure` – ADO.NET, SQL factory, authentication service
- `MediCoreHospital.Database` – SQL scripts and seed data
- `MediCoreHospital.Tests` – unit tests

## Prerequisites

- .NET 8 SDK
- SQL Server 2019 or later
- Visual Studio 2022 or VS Code with C# Dev Kit

## Setup

```bash
dotnet restore
dotnet build
```

## Database

Execute the SQL scripts in this order:

1. `MediCoreHospital.Database/Sql/01_CreateDatabase.sql`
2. `MediCoreHospital.Database/Sql/02_CreateTables.sql`
3. `MediCoreHospital.Database/Sql/03_CreateSeedData.sql`
4. `MediCoreHospital.Database/Sql/09_CreateAdmin.sql`

Update the connection string in `MediCoreHospital.UI/appsettings.json` if needed.

## Demo login

- Username: `admin`
- Password: `Admin@123`

> The default demo password must be initialized securely in production. This project uses modern BCrypt-based password verification patterns in the infrastructure layer.

## Notes

This repository contains the Phase 1 foundation: solution scaffolding, project references, Arabic RTL login UI, database schema starter, and architecture needed for the next phases of the hospital management system.
