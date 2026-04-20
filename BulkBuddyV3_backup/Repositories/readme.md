# Repositories

Deze map bevat de repositorylaag van de applicatie.

## Doel
Repositories zijn verantwoordelijk voor het ophalen, opslaan en aanpassen van data in de database.

## Wat hoort hier thuis
- IUserRepository
- UserRepository
- MealRepository
- FoodRepository

## Wat hoort hier niet thuis
- HTML of views
- request handling
- complete businessflows

## Architectuur
Deze map hoort bij de Data Access Layer.
Repositories vormen de brug tussen services en de database.