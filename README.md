# 📦 Order Management API (ASP.NET Core Web API)

## 📌 Overview

The **Order Management API** is a backend system built using **ASP.NET Core Web API** and **Entity Framework Core** with **SQL Server** as the database. It is designed to handle a multi-role workflow system involving **Customers, Riders, and Admins**, enabling end-to-end order processing from placement to delivery.

This project follows a layered architecture approach to ensure scalability, maintainability, and clean separation of concerns.

---

## 🚀 Features

### 👤 Customer Module
- Register and manage customer accounts
- Place new orders
- View order status and history
- Track assigned rider (if applicable)

### 🛵 Rider Module
- View available orders
- Accept or reject orders
- Update delivery status (e.g., Picked, In Transit, Delivered)
- Manage assigned deliveries

### 🧑‍💼 Admin Module
- Manage users (Customers & Riders)
- View and monitor all orders
- Assign or reassign riders (if required)
- Update and control order lifecycle
- Maintain system-level control over operations

---

## 🔄 Order Workflow

1. Customer places an order  
2. Admin validates and processes the order  
3. Rider accepts the assigned order  
4. Rider updates delivery status  
5. Admin monitors and finalizes the order lifecycle  

---

## 🛠️ Tech Stack

- Backend: ASP.NET Core Web API  
- ORM: Entity Framework Core  
- Database: SQL Server  
- Authentication: (Add if JWT or Identity used)  
- Architecture: Layered / Clean Architecture (if applicable)

---

## 🗄️ Database Design

The system includes core entities such as:

- Users (Customer, Rider, Admin roles)
- Orders
- Order Status
- Rider Assignments

---

## 🔐 Role-Based Access Control

The API implements role-based authorization:

- **Customer:** Can create and view their orders  
- **Rider:** Can manage assigned orders  
- **Admin:** Has full control over the system  

---

## 📈 Future Improvements

- JWT Authentication & Refresh Tokens  
- Real-time order tracking using SignalR  
- Notification system (Email / SMS / Push Notifications)  
- Payment gateway integration  
- Audit logs for admin actions  
- Enhanced Swagger documentation  

---

## ⚙️ Setup Instructions

1. Clone the repository:
```bash
   git clone https://github.com/faseeh1279/Yummy-Food-API.git
```

2. Navigate to the project folder 
```bash
cd Yummy-Food-API
```

3. Update database connection in appsettings.json 
```bash 
"ConnectionStrings": {
  "DefaultConnection": "Server=server_name;Database=database_name;Trusted_Connection=True;TrustServerCertificate=True" }
```

4. Add Migrations
```bash 
dotnet ef migrations add Initial-Create
```

5. Run the project 
```bash 
dotnet run 
``` 

6. Open Swagger at Browser

```bash 
http://localhost:5284/swagger/index.html
```