# Projekt

## RecipesApp  
**Kolekcja ulubionych przepisów kulinarnych**  
_"Zarządzaj swoim odżywianiem lepiej dzięki sprawdzonym przepisom i inspiracjom kulinarnym"_

---

### Skład zespołu:  
Wiktoria Elert (54327)

---

## Ogólny opis projektu:

Celem projektu jest stworzenie intuicyjnej aplikacji umożliwiającej użytkownikom tworzenie bazy swoich ulubionych przepisów kulinarnych. Użytkownicy mogą przeglądać stworzone przez innych użytkowników przepisy jak również tworzyć i zarządzać swoimi przepisami, dzięki **"Show My Recipes"** wszystkie swoje przepisy znajdziesz w jednym miejscu. Dodatkowym atutem jest automatyczne obliczanie wartości kalorycznej potraw na podstawie wagi składników, co pozwala na prowadzenie zdrowszego stylu życia.

---

## Funkcjonalności

**3.1.** Użytkownik ma możliwość dodawania przepisów  
**3.2.** Użytkownik ma możliwość zobaczenia przepisów utworzonych tylko przez swoje ID  
**3.3.** Użytkownik może przypisać do przepisu jeden lub wiele tagów  
**3.4.** Rejestracja i logowanie użytkowników  
**3.5.** Podłączenie do USDA API, wyszukiwanie istniejących tam składników  
**3.6.** Pobieranie Kcal z zewnętrznego USDA API (FoodData Central API)  
**3.7.** Program automatycznie wylicza kcal na podstawie wagi składnika, z kcal z API  

---

## Spis treści

![image](https://github.com/user-attachments/assets/6119d30a-b224-485a-9829-cad204c0c672)  
![image](https://github.com/user-attachments/assets/ca2ab91a-5557-46ad-82b1-a206241c50a8)  
![image](https://github.com/user-attachments/assets/1efc2f13-59da-4445-be7b-d02dc81a9f71)  
![image](https://github.com/user-attachments/assets/99e38f10-aede-4400-81a5-20fa189d5e13)  
![image](https://github.com/user-attachments/assets/025f95a7-c159-44c8-b15d-6b80b63200a3)  
![image](https://github.com/user-attachments/assets/9c92d1a6-ab81-4e34-aa2b-a6c5c41b3254)  
![image](https://github.com/user-attachments/assets/0fdddc4a-2396-4cf1-85e5-07689188985d)  
![image](https://github.com/user-attachments/assets/7282d52e-ec14-4ca4-8997-2a1a4125a154)  

---

## Tworzenie i aktualizacja bazy danych

**1. Instalacja NuGet**:
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer  
dotnet add package Microsoft.EntityFrameworkCore.Tools
