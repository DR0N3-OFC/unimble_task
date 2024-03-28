# Unimble Task

Project developed for the `Unimble`'s internship selective process.

## Requirements

- `Visual Studio 2022` or `.NET SDK 6.0+`;
- `EntityFramework Core`:
  - For installing, type: `dotnet tool install --global dotnet-ef` and `dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL`;
- `Docker 4.19.0+`;
- Available `700 MB+` storage;

## Installation

- On the root directory type on command prompt: `docker-compose build`. It will build the containers for you (you must have your Docker app opened).
- After building, on the same directory command line, type: `docker-compose up -d`. It will turn on the containers and set it up for use.
  - Maybe, the `unimble_webapi` container may exit with the code 139, but you can turn it on manually.

## Notes

- For security purposes, the `.p12 certificate` is being hidden by the `.gitignore` and the `credentials.json` is empty, so the application may not work in local docker containers if built by the Dockerfiles. But if you just want to test, you can use the ready images that are on `docker-compose.yaml`.
- The credentials for `credentials.json` and the `.p12 certificate` can be acquired by creating an account on `Efí Bank` application.
- If you have an `Efí's .p12 certificate` in hands, paste it on `TODOBack/Certificate/` directory.

## Running the application

- If you've installed from `docker-compose.yaml`, the application can be accessed on `http://localhost:5260`.

## Directory Structure of the project

Below is the directory tree structure of the project. \
The project was developed using Visual Studio Community 2022, free for academic purposes.

```javascript
Unimble
├─ .gitignore
├─ docker-compose.yml
├─ README.md
├─ TODOBack
│  ├─ appsettings.Development.json
│  ├─ appsettings.json
│  ├─ Certificate
│  ├─ Controllers
│  │  ├─ BillingController.cs
│  │  ├─ TaskController.cs
│  │  └─ UserController.cs
│  ├─ Data
│  │  ├─ AppDbContext.cs
│  │  └─ DbInitializer.cs
│  ├─ Dockerfile
│  ├─ Migrations
│  │  ├─ 20240314173112_Initialize.cs
│  │  ├─ 20240314173112_Initialize.Designer.cs
│  │  └─ AppDbContextModelSnapshot.cs
│  ├─ Models
│  │  ├─ BillingModel.cs
│  │  ├─ PixCharge.cs
│  │  ├─ TaskModel.cs
│  │  └─ UserModel.cs
│  ├─ Program.cs
│  ├─ Properties
│  │  └─ launchSettings.json
│  ├─ TODOBack.csproj
│  └─ TODOBack.sln
└─ TODOFront
   ├─ appsettings.Development.json
   ├─ appsettings.json
   ├─ Dockerfile
   ├─ Models
   │  ├─ APIConnection
   │  │  └─ APIConnection.cs
   │  ├─ BillingModel.cs
   │  ├─ TaskModel.cs
   │  └─ UserModel.cs
   ├─ Pages
   │  ├─ Billing
   │  │  ├─ Index.cshtml
   │  │  └─ Index.cshtml.cs
   │  ├─ Index.cshtml
   │  ├─ Index.cshtml.cs
   │  ├─ Shared
   │  │  ├─ _MainLayout.cshtml
   │  │  ├─ _MenuPartialPage.cshtml
   │  │  └─ _ViewImports.cshtml
   │  ├─ Tasks
   │  │  ├─ Edit.cshtml
   │  │  ├─ Edit.cshtml.cs
   │  │  ├─ Index.cshtml
   │  │  ├─ Index.cshtml.cs
   │  │  ├─ Remove.cshtml
   │  │  └─ Remove.cshtml.cs
   │  ├─ User
   │  │  ├─ Edit.cshtml
   │  │  ├─ Edit.cshtml.cs
   │  │  ├─ Login.cshtml
   │  │  ├─ Login.cshtml.cs
   │  │  ├─ Profile.cshtml
   │  │  ├─ Profile.cshtml.cs
   │  │  ├─ Register.cshtml
   │  │  └─ Register.cshtml.cs
   │  ├─ _ViewImports.cshtml
   │  └─ _ViewStart.cshtml
   ├─ Program.cs
   ├─ Properties
   │  └─ launchSettings.json
   ├─ TODOFront.csproj
   ├─ TODOFront.sln
   └─ wwwroot
      ├─ css
      │  ├─ billings
      │  │  └─ index.css
      │  ├─ index.css
      │  ├─ site.css
      │  ├─ tasks
      │  │  ├─ edit.css
      │  │  ├─ index.css
      │  │  └─ remove.css
      │  └─ user
      │     ├─ edit.css
      │     ├─ login.css
      │     ├─ profile.css
      │     └─ register.css
      ├─ favicon.ico
      └─ images
         ├─ delete.png
         └─ edit.png
```
