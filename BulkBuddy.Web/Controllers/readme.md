# Controllers

Deze map bevat de controllers van de applicatie.

## Doel
Controllers ontvangen HTTP-requests van de gebruiker en bepalen welke actie uitgevoerd moet worden.  
Een controller roept meestal een service aan en geeft daarna een view terug.

## Wat hoort hier thuis
- DashboardController
- AccountController
- MealsController

## Wat hoort hier niet thuis
- SQL-queries
- directe databaseverbindingen
- uitgebreide businesslogica

## Architectuur
Deze map hoort bij de Presentation Layer binnen de layered architecture.
Binnen MVC vormt de controller de schakel tussen model en view.