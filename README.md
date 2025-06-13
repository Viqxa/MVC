# 🧾 RecipesApp

> **Kolekcja ulubionych przepisów kulinarnych**  
> _"Zarządzaj swoim odżywianiem lepiej dzięki sprawdzonym przepisom i inspiracjom kulinarnym"_

---

## 👩‍💻 Zespół projektowy

- **Wiktoria Elert (54327)**

---

## 📌 Opis projektu

Celem projektu jest stworzenie intuicyjnej aplikacji umożliwiającej użytkownikom tworzenie bazy swoich ulubionych przepisów kulinarnych.

Użytkownicy mogą:

- przeglądać przepisy innych użytkowników,
- dodawać i zarządzać własnymi przepisami dzięki funkcji **"Show My Recipes"**,
- automatycznie obliczać wartość kaloryczną potraw na podstawie składników i ich wagi.

To narzędzie pozwala na wygodne planowanie posiłków i wspiera prowadzenie zdrowszego stylu życia.

---

## ✅ Funkcjonalności

1. Użytkownik ma możliwość dodawania przepisów  
2. Użytkownik widzi tylko przepisy utworzone przez swoje ID  
3. Możliwość przypisania jednego lub wielu tagów do przepisu  
4. Rejestracja i logowanie użytkowników  
5. Wyszukiwanie składników z zewnętrznego źródła (USDA API)  
6. Pobieranie wartości kalorycznych składników z **FoodData Central API**  
7. Automatyczne obliczanie kalorii na podstawie wagi i danych z API  

---

## 📂 Spis treści (Galeria)

![image1](https://github.com/user-attachments/assets/6119d30a-b224-485a-9829-cad204c0c672)  
![image2](https://github.com/user-attachments/assets/ca2ab91a-5557-46ad-82b1-a206241c50a8)  
![image3](https://github.com/user-attachments/assets/1efc2f13-59da-4445-be7b-d02dc81a9f71)  
![image4](https://github.com/user-attachments/assets/99e38f10-aede-4400-81a5-20fa189d5e13)  
![image5](https://github.com/user-attachments/assets/025f95a7-c159-44c8-b15d-6b80b63200a3)  
![image6](https://github.com/user-attachments/assets/9c92d1a6-ab81-4e34-aa2b-a6c5c41b3254)  
![image7](https://github.com/user-attachments/assets/0fdddc4a-2396-4cf1-85e5-07689188985d)  
![image8](https://github.com/user-attachments/assets/7282d52e-ec14-4ca4-8997-2a1a4125a154)  

---

## 🛠️ Tworzenie i aktualizacja bazy danych

1. Zainstaluj paczki NuGet:

    ```bash
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore.Tools
    ```

2. Utwórz migrację:

    ```bash
    dotnet ef migrations add InitialCreate
    ```

3. Utwórz bazę danych:

    ```bash
    dotnet ef database update
    ```

---

## 🧰 Wykorzystywane narzędzia i technologie

- Visual Studio 2022  
- .NET 9.0  
- SQL Server  
- Bootstrap  

---

> Projekt realizowany w ramach zajęć programowania aplikacji webowych.
