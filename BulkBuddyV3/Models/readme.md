# Models

Deze map bevat de modellen van de applicatie.

## Doel
Models beschrijven de data die in de applicatie gebruikt wordt.

## Indeling
Deze map kan verder opgesplitst worden in:
- Domain: kernobjecten van het systeem
- ViewModels: data voor views
- InputModels: data uit formulieren

## Wat hoort hier thuis
- User
- MealEntry
- DashboardViewModel
- LoginInputModel

## Wat hoort hier niet thuis
- SQL-queries
- controllerlogica
- serviceverantwoordelijkheden

## Architectuur
Models worden gebruikt in meerdere lagen, afhankelijk van het type model.
Domain models horen bij de domeinlaag.
ViewModels horen bij de presentation layer.