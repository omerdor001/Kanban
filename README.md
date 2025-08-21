# 🗂️ Kanban Board Application

**Team Project | C# | WPF (MVVM) | SQLite | First Year, Second Semester**

A desktop Kanban board application developed by a team of three, built with **C# and WPF**, and structured using the **MVVM (Model-View-ViewModel)** design pattern. The project follows a clean, multi-layered architecture for maintainability and scalability.

---

## 🧱 Architecture Overview

- **Frontend (WPF)**  
  Designed with XAML using WPF, implementing MVVM principles for a clean and responsive user interface.

- **Service Layer**  
  Manages application logic and persistence using **SQLite**, acting as a bridge between the UI, domain, and data storage.

- **Domain Layer**  
  Defines the core business entities and encapsulates the logic for tasks, boards, and columns.

- **Data Access Layer**  
  Contains **Data Transfer Objects (DTOs)** and **mappers** to translate between domain models and persistent data, ensuring a decoupled and flexible architecture.

---

## ✅ Features

- 📝 Task creation, editing, and deletion
- 📌 Tasks organized by state: *Backlog*, *In Progress*, and *Done*  
- 👥 **User login system** for personalized access  
- 📎 **Task assignment** to users  
- 💾 Persistent data storage using **SQLite**  
- 🧪 Suite of **unit tests** to ensure logic reliability and code quality  
- 🔁 Built entirely with **pair programming** — all team members contributed across the full stack


---

## 🛠️ How to Run

- Open the solution in **Visual Studio**.
- Run the main project to launch the Kanban application.
- The **unit tests** are located in a separate test project within the solution and can be executed using their main.cs.

---

## 🗂️ Class Diagram

A full class diagram illustrating the structure and relationships within the system is included in the repository under documents/design.pdf.

---

## 🤝 Team Collaboration

This project was developed through **pair programming**, ensuring all team members were actively involved in every part of the codebase — from UI design to business logic and database integration.
