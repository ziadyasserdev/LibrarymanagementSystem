<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&height=280&color=0:4E54C8,50:8F94FB,100:4776E6&text=Library%20Management%20System&fontColor=ffffff&fontSize=48&fontAlignY=38&desc=ASP.NET%20Core%208%20|%20Clean%20Architecture%20|%20CQRS&descAlignY=58&animation=fadeIn"/>
</p>

<h1 align="center">📚 Library Management System</h1>

<h3 align="center">
Production-Ready Library Management API built with ASP.NET Core 8
</h3>

<p align="center">
<img src="https://readme-typing-svg.herokuapp.com?font=Fira+Code&weight=600&size=22&pause=1000&color=6C63FF&center=true&vCenter=true&width=700&lines=Library+Management+System;Clean+Architecture;CQRS+%2B+MediatR;ASP.NET+Core+8;Entity+Framework+Core;JWT+Authentication;Hangfire+Background+Jobs"/>
</p>

<p align="center">

<a href="YOUR_GITHUB_REPO">
<img src="https://img.shields.io/badge/Repository-181717?style=for-the-badge&logo=github&logoColor=white"/>
</a>

<a href="#">
<img src="https://img.shields.io/badge/API-Demo-00C853?style=for-the-badge"/>
</a>

<img src="https://img.shields.io/badge/.NET-8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>

<img src="https://img.shields.io/badge/Clean-Architecture-blue?style=for-the-badge"/>

<img src="https://img.shields.io/badge/CQRS-MediatR-success?style=for-the-badge"/>

<img src="https://img.shields.io/badge/License-MIT-orange?style=for-the-badge"/>

</p>

---

# 📖 Overview

The **Library Management System** is a production-ready backend application developed using **ASP.NET Core 8** following **Clean Architecture** and **CQRS** principles.

It provides a complete solution for managing books, authors, publishers, branches, reservations, borrowing, fines, payments, analytics, and notifications while maintaining scalability, maintainability, and high performance.

The project demonstrates enterprise-level backend development practices including:

- Clean Architecture
- CQRS using MediatR
- Repository & Unit of Work Pattern
- JWT Authentication
- ASP.NET Identity
- Hangfire Background Jobs
- Email Notifications
- Result Pattern
- FluentValidation
- AutoMapper

---

# ✨ Features

## 📚 Book Management

- Add Books
- Update Books
- Delete Books
- Search Books
- Book Details
- Book Copies

---

## 👨‍💼 User Management

- User Registration
- Login
- JWT Authentication
- Role-Based Authorization

---

## 🏢 Library Management

- Branches
- Locations
- Publishers
- Categories
- Authors

---

## 📖 Loan Management

- Borrow Books
- Return Books
- Loan History
- Due Date Tracking

---

## ⏳ FIFO Reservation System

One of the project's key features.

Users can reserve unavailable books.

The reservation queue follows **First In First Out (FIFO)**.

When a copy becomes available:

- The next user is selected automatically.
- Previous reservations are updated.
- Email notification is sent.
- Reservation expiration is handled automatically.

---

## 💰 Fine Management

- Automatic Fine Calculation
- Partial Payments
- Full Payments
- Remaining Balance
- Payment History

---

## 📧 Email Notifications

Automatic emails for:

- Reservation Available
- Borrow Confirmation
- Return Confirmation
- Overdue Reminder
- Payment Confirmation

---

## ⏰ Hangfire Background Jobs

Background processing for:

- Reservation Expiration
- Overdue Books
- Fine Calculations
- Email Notifications

---

## 📊 Reports & Analytics

Administrators can monitor:

- Total Revenue
- Fine Payments
- Active Loans
- Overdue Books
- Most Borrowed Books
- Reservation Statistics
- User Activity

---

## 🔒 Security

- JWT Authentication
- ASP.NET Identity
- Role-Based Authorization
- Refresh Tokens
- Secure Password Hashing
---

# 🏗️ Architecture

The project follows **Clean Architecture** to achieve separation of concerns, maintainability, and scalability.

```text
                 ┌──────────────────────┐
                 │     Presentation     │
                 │   ASP.NET Core API   │
                 └──────────┬───────────┘
                            │
                            ▼
                 ┌──────────────────────┐
                 │     Application       │
                 │ CQRS • MediatR        │
                 │ DTOs • Validators     │
                 │ Business Logic        │
                 └──────────┬───────────┘
                            │
                            ▼
                 ┌──────────────────────┐
                 │       Domain         │
                 │ Entities             │
                 │ Interfaces           │
                 │ Domain Rules         │
                 └──────────┬───────────┘
                            │
                            ▼
                 ┌──────────────────────┐
                 │    Infrastructure    │
                 │ EF Core              │
                 │ SQL Server           │
                 │ Identity             │
                 │ Hangfire             │
                 │ SMTP                 │
                 └──────────────────────┘
```

---

# 🛠️ Tech Stack

## Programming Languages

<p>

<img src="https://skillicons.dev/icons?i=cs"/>

</p>

---

## Backend

<p>

<img src="https://skillicons.dev/icons?i=dotnet"/>

<img src="https://skillicons.dev/icons?i=mysql"/>

</p>

<p>

<img src="https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Entity_Framework_Core-68217A?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Clean_Architecture-blue?style=for-the-badge"/>

<img src="https://img.shields.io/badge/CQRS-MediatR-success?style=for-the-badge"/>

<img src="https://img.shields.io/badge/JWT-Authentication-black?style=for-the-badge"/>

<img src="https://img.shields.io/badge/ASP.NET_Identity-purple?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Hangfire-red?style=for-the-badge"/>

<img src="https://img.shields.io/badge/FluentValidation-green?style=for-the-badge"/>

<img src="https://img.shields.io/badge/AutoMapper-orange?style=for-the-badge"/>

<img src="https://img.shields.io/badge/SMTP_Email-blue?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Serilog-008080?style=for-the-badge"/>

</p>

---

## Tools

<p>

<img src="https://skillicons.dev/icons?i=git"/>

<img src="https://skillicons.dev/icons?i=github"/>

<img src="https://skillicons.dev/icons?i=visualstudio"/>

<img src="https://skillicons.dev/icons?i=postman"/>

</p>

---

# 📂 Project Structure

```text
LibraryManagementSystem
│
├── Library.API
│
├── Library.Application
│
├── Library.Domain
│
├── Library.Infrastructure
│
└── Library.Persistence
```

---

# 📦 Main Modules

| Module | Description |
|----------|-------------|
| 📚 Books | Manage books and book copies |
| 👨 Authors | Author management |
| 🏢 Publishers | Publisher management |
| 🏷 Categories | Book categories |
| 📍 Locations | Shelf locations |
| 🏢 Branches | Multiple library branches |
| 👤 Users | Authentication & Roles |
| 📖 Loans | Borrow & Return books |
| ⏳ Reservations | FIFO Reservation Queue |
| 💰 Fines | Automatic Fine Calculation |
| 💳 Payments | Partial & Full Payments |
| ⭐ Feedback | Ratings & Reviews |
| 📊 Reports | Financial & Library Analytics |

---

# 🔥 Key Features

## 📚 Smart Reservation Queue

When a book is unavailable,

users can reserve it.

As soon as a copy becomes available:

✔ Next reservation is selected automatically

✔ Queue is updated

✔ Email notification is sent

✔ Previous reservation expires automatically

---

## 💰 Fine Payment System

Supports:

- Partial Payments

- Full Payments

- Remaining Balance

- Payment History

- Financial Reports

---

## 📧 Email Notification Service

Automatic notifications for:

- Reservation Ready

- Borrow Success

- Return Success

- Overdue Reminder

- Fine Payment

---

## ⏰ Hangfire Automation

Background jobs handle:

- Reservation Expiration

- Fine Calculation

- Email Sending

- Overdue Detection

without manual intervention.

---

# 🔐 Authentication & Authorization

✔ JWT Authentication

✔ ASP.NET Identity

✔ Role-Based Authorization

Supported Roles:

- Admin

- Librarian

- Member

---

# 📊 Reporting

Generate reports for:

- Revenue Summary

- Fine Payments

- Monthly Statistics

- Active Loans

- Overdue Loans

- Popular Books

- User Activity

------

# 🚀 Getting Started

Follow these steps to run the project locally.

## 1️⃣ Clone the Repository

```bash
git clone https://github.com/ziadyasserdev/LibrarymanagementSystem.git
```

```bash
cd LibrarymanagementSystem
```

---

## 2️⃣ Configure the Database

Update the connection string inside:

```text
appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=LibraryManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

## 3️⃣ Apply Migrations

```bash
dotnet ef database update
```

---

## 4️⃣ Run the Project

```bash
dotnet run
```

---

## 5️⃣ Open Swagger

```text
https://localhost:xxxx/swagger
```

---

# 📡 API Features

The API provides endpoints for:

### Authentication

- Register
- Login
- Refresh Token
- Revoke Token

---

### Books

- Create Book
- Update Book
- Delete Book
- Get Books
- Search Books

---

### Authors

- CRUD Operations

---

### Categories

- CRUD Operations

---

### Publishers

- CRUD Operations

---

### Branches

- CRUD Operations

---

### Locations

- CRUD Operations

---

### Book Copies

- Add Copy
- Update Copy
- Copy Availability

---

### Loans

- Borrow Book
- Return Book
- Loan History

---

### Reservations

- Reserve Book
- Cancel Reservation
- Reservation Queue
- Automatic FIFO Processing

---

### Fines

- Calculate Fine
- Fine Details
- Outstanding Balance

---

### Payments

- Partial Payment
- Full Payment
- Payment History

---

### Feedback

- Add Rating
- Add Review
- Update Review

---

### Reports

- Revenue Report
- Fine Summary
- Borrowing Statistics
- Most Borrowed Books

---

# 📸 API Preview

<p align="center">

<img src="images/swagger-home.png" width="900"/>

</p>

> Replace the image above with your Swagger screenshot.

---

# 📈 Future Improvements

- Docker Support

- Redis Caching

- Unit Testing

- Integration Testing

- Azure Blob Storage

- CI/CD Pipeline

- Kubernetes Deployment

- Microservices Architecture

---

# 🤝 Contributing

Contributions are always welcome.

If you'd like to improve the project:

1. Fork the repository

2. Create a feature branch

3. Commit your changes

4. Push your branch

5. Open a Pull Request

---

# 📄 License

This project is licensed under the MIT License.

---

# 👨‍💻 Author

## Zeyad Yasser

Backend .NET Developer

Passionate about building scalable backend systems using ASP.NET Core and Clean Architecture.

<p align="center">

<a href="mailto:ziadyasser.dev@gmail.com">
<img src="https://img.shields.io/badge/Email-EA4335?style=for-the-badge&logo=gmail&logoColor=white"/>
</a>

<a href="https://www.linkedin.com/in/ziad-yasser-6155b828b">
<img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white"/>
</a>

<a href="https://github.com/ziadyasserdev">
<img src="https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white"/>
</a>

<a href="https://wa.me/201033724845">
<img src="https://img.shields.io/badge/WhatsApp-25D366?style=for-the-badge&logo=whatsapp&logoColor=white"/>
</a>

</p>

<p align="center">

📧 ziadyasser.dev@gmail.com &nbsp;|&nbsp;
📱 +20 103 372 4845 &nbsp;|&nbsp;
📍 Egypt

</p>

---

<p align="center">

<img src="https://capsule-render.vercel.app/api?type=waving&height=120&section=footer&color=0:4E54C8,50:8F94FB,100:4776E6"/>

</p>

<h3 align="center">

⭐ If you found this project useful, don't forget to leave a star! ⭐

</h3>
