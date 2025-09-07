# Micro Social Platform

[![.NET](https://img.shields.io/badge/.NET-6.0-512BD4)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-6.0-512BD4)](https://docs.microsoft.com/en-us/aspnet/core/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6.0-512BD4)](https://docs.microsoft.com/en-us/ef/)

A modern, feature-rich social networking platform built with ASP.NET Core MVC, designed to facilitate meaningful connections and community building through secure user interactions, content sharing, and group collaboration.

## Overview

Micro Social Platform is a comprehensive social networking application that provides users with a secure and intuitive environment for social interaction. The platform implements role-based access control, privacy settings, and content management features to ensure a safe and engaging user experience.

## Key Features

### User Management & Authentication
- **Multi-tier User System**: Three distinct user roles with different access levels
  - **Guest Users**: Unregistered visitors with limited browsing capabilities
  - **Registered Users**: Full platform access with profile management and social features
  - **Administrators**: Complete platform oversight with content moderation capabilities

### Privacy & Security
- **Granular Profile Visibility**: Users can configure their profile visibility settings
  - **Public Profiles**: Visible to all platform users
  - **Private Profiles**: Restricted to friends and approved connections
- **Secure Authentication**: Implemented using ASP.NET Identity framework
- **Friend Request System**: Controlled access to private content through friendship connections

### Social Interaction Features
- **User Discovery**: Advanced search functionality to find and connect with other users
- **Profile Management**: Comprehensive profile creation and editing capabilities
- **Content Sharing**: Text-based posting system with engagement features
- **Discussion Threads**: Comment system for community discussions on posts
- **Content Ownership**: Users can edit and delete their own posts and comments

### Group Collaboration
- **Group Membership**: Users can join and participate in specialized interest groups
- **Group Messaging**: Dedicated communication channels within groups
- **Group Content Management**: Members can share, edit, and manage group-specific content

### Content Moderation
- **Administrative Oversight**: Comprehensive content moderation tools
- **Content Policy Enforcement**: Administrators can remove inappropriate content
- **Platform Maintenance**: Proactive monitoring and management of platform integrity

## Technical Architecture

### Core Technologies
- **Framework**: ASP.NET Core MVC 6.0
- **Authentication**: ASP.NET Identity for secure user management
- **Data Access**: Entity Framework Core with code-first migrations
- **Database**: MySQL with Pomelo EntityFrameworkCore provider
- **Frontend**: Bootstrap for responsive UI design
- **Runtime**: .NET 6.0

### Project Structure
```
MicroSocialPlatform/
├── Areas/              # Feature-specific areas (Identity, Admin)
├── Controllers/        # MVC Controllers
├── Data/              # Entity Framework DbContext and configurations
├── Models/            # Data models and view models
├── Views/             # Razor view templates
├── wwwroot/           # Static web assets (CSS, JS, images)
└── Program.cs         # Application entry point
```

### Dependencies
- **Microsoft.AspNetCore.Identity.EntityFrameworkCore** - User authentication and authorization
- **Microsoft.EntityFrameworkCore.SqlServer** - SQL Server database provider
- **Pomelo.EntityFrameworkCore.MySql** - MySQL database provider
- **Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore** - EF Core error page middleware
- **Microsoft.VisualStudio.Web.CodeGeneration.Design** - Code scaffolding tools

## Getting Started

### Prerequisites
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) or SQL Server
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Lung-Alexandra/Micro.Social.Platform.git
   cd Micro.Social.Platform
   ```

2. **Configure the database connection**
   - Update the connection string in `appsettings.json`
   - Ensure your database server is running

3. **Install dependencies and run migrations**
   ```bash
   dotnet restore
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run --project MicroSocialPlatform
   ```

5. **Access the application**
   - Navigate to `https://localhost:5001` in your web browser
   - Create an account or sign in to start using the platform

## Contributing

We welcome contributions to improve the Micro Social Platform. Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is available for educational and evaluation purposes. Please refer to the repository owner for licensing information.

## Support

For questions, issues, or feature requests, please [open an issue](https://github.com/Lung-Alexandra/Micro.Social.Platform/issues) on GitHub.