# Services

Deze map bevat de servicelaag van de applicatie.

## Doel
Services bevatten de businesslogica en use cases van het systeem.
Ze verwerken data en coördineren acties tussen controllers en repositories.

## Wat hoort hier thuis
- DashboardService
- CalorieCalculatorService
- MealsService
- AuthenticationService

## Wat hoort hier niet thuis
- directe view rendering
- losse SQL-queries in controllers
- pure database infrastructuur

## Architectuur
Deze map hoort bij de Application Layer.
Services houden controllers klein en verbeteren onderhoudbaarheid.