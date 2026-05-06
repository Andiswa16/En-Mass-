# En Massé Global Logistics Management System (ERLMS)

> A web-based enterprise logistics platform for multimodal freight coordination across road, air, sea, and rail networks.

---

## Project Info

| Field | Details |
|---|---|
| **Module** | PROG7311 |
| **Group** | En Massé |
| **Team** | Andiswa Mavundla · Mbali Ndlovu · Sphosethu Ximba · Sthabile Cele · Zisanda Hlongwa |
| **Stack** | ASP.NET Core MVC · C# · Entity Framework Core · SQL Server · Razor Views |

---

## Overview

En Massé ERLMS is an enterprise-grade web application that digitises the logistics operations of a global freight company. It replaces manual coordination with a centralised platform for managing clients, transport assets, delivery requests, and real-time trip tracking across multiple user roles.

---

## Features

### By Role

**Customer (Client Company)**
- Register a company account and log in
- Submit delivery requests (pickup/destination address, cargo description, weight, date, special instructions)
- View delivery status with a live progress indicator (Placed → In Transit → Delivered)
- Access delivery reports and history
- Log support tickets

**Logistics Operations Manager**
- View dashboard with live stats (active drivers, pending requests, in-transit deliveries)
- Review and approve pending delivery requests
- Assign drivers to trips
- Create new trips manually

**Driver**
- View assigned deliveries with timeline cards
- Update delivery status (In Transit / Delivered)
- View delivery report history
- Reset password via the driver portal

**System Administrator**
- Full dashboard with fleet overview, delivery performance metrics, and exception analytics
- Manage client accounts (search, view, delete)
- Manage driver records
- Update delivery statuses across all clients
- View fleet utilisation stats

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 8) |
| Language | C# |
| ORM | Entity Framework Core |
| Database | SQL Server (LocalDB for development) |
| Frontend | Razor Views, custom CSS (dark theme), Bootstrap |
| Session | ASP.NET Core distributed memory cache |
| Auth | Session-based role authentication |

---

## Project Structure

```
EnMasse/
├── Controllers/
│   ├── AdminController.cs          # Admin dashboard, clients, deliveries, drivers
│   ├── AuthController.cs           # Login, register, logout
│   ├── CustomerController.cs       # Customer dashboard, delivery requests, tickets
│   ├── DriverDashboardController.cs # Driver view, status updates
│   ├── HomeController.cs           # Public landing page
│   └── ManagerController.cs        # Manager dashboard, trip creation, driver assignment
│
├── Models/
│   ├── AppDbContext.cs             # EF Core DB context
│   ├── Delivery.cs                 # Delivery entity
│   ├── Driver.cs                   # Driver entity
│   ├── Registration.cs             # Company registration entity
│   ├── User.cs                     # User entity
│   ├── AdminDashboardViewModel.cs
│   ├── DashboardViewModel.cs
│   ├── LoginViewModel.cs
│   ├── ForgotPasswordViewModel.cs
│   └── Order.cs
│
├── Data/
│   └── MockDriverData.cs           # Seed data for driver testing
│
├── Views/
│   ├── Admin/                      # Dashboard, Clients, Deliveries, Drivers
│   ├── Auth/                       # Login, Register
│   ├── Customer/                   # Dashboard, RequestDelivery, DeliveryStatus, Report, LogTicket, HelpSettings
│   ├── DriverDashboard/            # Dashboard, Report, Login, ForgotPassword
│   ├── Manager/                    # Dashboard, OrderDetails, CreateTrip
│   ├── Home/                       # Index (landing), About
│   └── Shared/
│       ├── _AdminLayout.cshtml     # Admin shell with sidebar
│       └── _Layout.cshtml          # Customer/public shell with navbar
│
├── wwwroot/
│   ├── css/site.css                # Global dark theme + component styles
│   └── images/
│
├── appsettings.json                # DB connection string
└── Program.cs                      # App bootstrap and middleware
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 (recommended) or VS Code with C# extension

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-repo/enmasse.git
   cd enmasse
   ```

2. **Configure the database connection** in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EnMasseDB;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
   ```

3. **Apply migrations** to create the database:
   ```bash
   dotnet ef database update
   ```

4. **Run the application**:
   ```bash
   dotnet run
   ```
   Then open `https://localhost:5001` in your browser.

---

## Default Credentials

The following mock accounts are seeded at startup for testing:

| Role | Username | Password |
|---|---|---|
| Admin | `admin` | `Admin123!` |
| Manager | `manager` | `Manager123!` |
| Driver | `driver1` | `Driver123!` |

Customer accounts are created via the **Register** page. Each customer's username is their registered company name.

---

## Delivery Status Flow

```
Pending  →  In Transit  →  Delivered
```

- Customers submit a request → status starts as **Pending**
- Manager approves and assigns a driver → status moves to **In Transit**
- Driver marks the delivery complete → status changes to **Delivered**

---

## Key Design Decisions

- **Session-based auth** — role stored in session (`Admin`, `Manager`, `Driver`, `Customer`) and checked at the top of every controller action.
- **Separate layouts** — `_AdminLayout.cshtml` for the admin portal; `_Layout.cshtml` adapts dynamically for customers (sidebar) vs. public visitors (navbar).
- **Mock users for staff** — drivers, managers, and admins are currently hardcoded in `AuthController` to simplify demo setup; customers are persisted to the database.
- **UTC timestamps** — delivery dates and creation timestamps are stored in UTC-compatible format to support future multi-timezone operations.

---

## Out of Scope (Current Version)

- Warehouse inventory management
- Payroll or financial accounting
- Customs clearance processing
- Automated route optimisation
- Airline or port authority integrations

---

## Functional Requirements Coverage

| # | Requirement | Owner | Status |
|---|---|---|---|
| 1 | Client registration & login | — | ✅ |
| 2 | Delivery request submission | Andiswa | ✅ |
| 3 | Transport mode selection | Andiswa | ✅ |
| 4 | Trip creation & assignment | Sphosethu | ✅ |
| 5 | Trip status tracking | Sthabile | ✅ |
| 6 | Fleet & asset management | Enhle | ✅ |
| 7 | Reporting & analytics | Mbali | ✅ |

---

## Non-Functional Requirements

- **Security** — role-based access control; session isolation per user
- **Performance** — target: < 3 seconds response time; supports 500+ concurrent users
- **Availability** — 99% uptime target with daily backup support
- **Scalability** — horizontally scalable architecture; supports growing client base
- **Usability** — responsive dark-themed UI accessible on modern browsers
- **Globalization** — UTC timestamp storage; international address support

---

## License

This project was developed for academic purposes as part of PROG7311 at IIE (The Independent Institute of Education). All rights reserved by the En Massé group.
