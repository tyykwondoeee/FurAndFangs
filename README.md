# FurAndFangs

FurAndFangs is an ASP.NET Core web application that allows users to ask questions about animals and receive informative responses based on optional details such as species, sex, diet, and weight.

The project combines a simple HTML/CSS/JavaScript frontend with a C# ASP.NET Core backend API, focusing on clean integration and graceful handling of optional input.

---

## Features

- Ask questions about animals and pet care
- Optional dropdown inputs for species, sex, diet, and weight
- Frontend served directly from the backend
- Enum-based validation on the API
- Graceful handling of missing or unselected inputs
- Swagger UI included for API exploration and testing

---

## Tech Stack

- ASP.NET Core 8.0
- C#
- HTML / CSS / JavaScript
- Entity Framework Core
- SQL Server LocalDB
- Swagger / OpenAPI

---

## Project Structure

- **Frontend**:  
  Static HTML, CSS, and JavaScript served from `wwwroot`

- **Backend**:  
  ASP.NET Core Web API with controllers, models, and enums

- **Models**:  
  Strongly typed request models and enums for species, sex, diet, and weight units

---

## Running the Application

1. Clone the repository:
```bash
git clone https://github.com/tyykwondoeee/FurAndFangs.git
