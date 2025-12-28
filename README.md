# QuizWeb 📝

QuizWeb is a **RESTful Web API** built with **ASP.NET Core 8** for managing and conducting online quizzes. It provides secure authentication using **JWT**, role-based user management with **ASP.NET Identity**, and a scalable architecture using the **Repository Pattern**.

This project serves as the backend for an online quiz platform where users can take exams, submit answers, and view results.

---

## 🚀 Features

- User authentication & authorization using **JWT Bearer Tokens**
- Secure user management with **ASP.NET Core Identity**
- Quiz, question bank, options, and exam management
- Store and evaluate user answers
- Calculate and store exam results
- Repository pattern for clean and maintainable code
- Swagger UI for API documentation and testing

---

## 🛠️ Tech Stack

- **ASP.NET Core 8 (Web API)**
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **ASP.NET Identity**
- **Swagger (Swashbuckle)**

---

## 📂 Project Structure

- `Models` – Entity models and Identity user
- `Repository` – Data access logic
- `Interface` – Repository interfaces
- `Program.cs` – Application startup and configuration
- `appsettings.json` – Application configuration

---

## ⚙️ Configuration

Before running the project, update the following in `appsettings.json`:

- **Database connection string**
- **JWT Issuer, Audience, and Secret Key**

---

## ▶️ Running the Project

1. Clone the repository  
   ```bash
   git clone https://github.com/hossam-adel99/QuizWeb.git
