# Internship manager API
Project met als doel het ontwikkelen van een webapplicatie om stageaanvragen voor de PXL Hogeschool Hasselt te vereenvoudigen. Het doel is om een geïntegreerde omgeving te creëren waar potentiële stagebedrijevn opdrachten kunnen indienen, de stagecoördinator en de PXL-Digital-docenten deze opdrachten kunnen evalueren en de studenten deze kunnen bekijken en kiezen voor hun stage. 
Dit is het back-end gedeelte van het project.

## Teamleden (Team 1 | Academiejaar 2020-2021)
* Eljakim Lindenburg
* Kim Richter
* Laure Sleven
* Zoé Charbonnier
* Jochen DeVry

## Teamleden (Stage | Academiejaar 2020-2021)
* Maarten Warson (stagiair)
* Kris Hermans (hogeschoolpromotor)
* Marijke Willems (bedrijfspromotor, klant)
* Bart Stukken (klant)

## Teamleden (Team 2 | Academiejaar 2019-2020)
* Bram Bogaerts
* Jens Michielsen
* Rocco Nardiello
* Christopher Pradhan
* Steven Van Suetendael
* Giel Vranken
* Maarten Warson

## Installatie
### Het project lokaal runnen
1. Zorg dat er een SQL Server geïnstalleerd is. Dit project gebruikt voor lokale ontwikkeling Microsoft SQL Server Express LocalDB. Dit is te installeren via de Visual Studio Installer.

2. Zorg dat de connectiestring juist is voor de database. Deze staat reeds geconfigureerd voor SQL Server Express LocalDB. Indien een andere SQL server gebruikt wordt voer dan de volgende stappen uit:
	a. Open het project StagebeheerAPI
  	b. Vouw het appsettings.json bestand uit. Hieronder staan 3 bestanden.
  	c. In het appsettings.Development.json bestand, voeg de connectiestring voor de lokale database toe. Hieronder een voorbeeld. 
  ```
  {
    "ConnectionStrings": 
    {
      "StagebeheerDB": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Stagebeheer;Integrated Security=True;"
    }
  }
  ```
  d. Voeg de juiste connectiestrings voor de online omgevingen toe in appsettings.Test.json en appsettings.Production.json:
  ```
  {
    "ConnectionStrings": 
    {
      "StagebeheerDB": "[My connection string]"
    }
  }
  ```
3. Rechtermuisklik op de solution en klik op 'Multiple startup projects'. Selecteer 'start' bij StagebeheerAPI en EventAPI. StagebeheerAPI moet bovenaan de lijst staan en wordt dan als eerste gestart. Nu wordt bij de start van het project ook automatisch de microservice EventAPI gestart.

4. Rechtermuisklik op StagebeheerAPI, klik op 'Manage User Secrets' en vul daar het volgende in:
```
  {
  "Jwt": {
    "Key": "vraag een van de ontwikkelaars van team 1 (academiejaar 2020-2021) voor de sleutel"
     }
  }
  ```
5. Rechtermuisklik op EventAPI, klik op 'Manage User Secrets' en vul daar het volgende in:
```
  {
  "Jwt": {
    "Key": "vraag een van de ontwikkelaars van team 1 (academiejaar 2020-2021) voor de sleutel"
     }
  }
  ```
6. Run het project. De startup van dit project bouwt de databases op en vult de tabellen met testdata.

## Usage
### API Usage
Bij het opstarten van het project wordt de Swagger-documentatie geladen in de browser.
Alvorens de API-endpoints aangesproken kunnen worden, dient er eerst geautoriseerd te worden. Dit kan men doen aan de hand van het Account/Login-endpoint.

Bij het inloggen krijgt de gebruiker een JWT token terug. Deze dient ingevuld te worden bovenaan op de Swagger-pagina na het drukken op de Authorize-knop. 
Voor verdere informatie over de endpoints verwijzen we door naar de documentatie in Swagger.

### EmailService Usage
De EmailService maakt gebruik van mailjet. Indien het project overgenomen wordt, dient er een nieuw Mailjet account aangemaakt te worden.
De accountgegevens kunnen daarna ingevuld worden in appsettings.json.

### Infrastructure
De root folder bevat een azure-pipelines.yml file waarin de Azure pipeline beschreven staat. 

## Teams link
Voor het creeren van een teams link kan best Microsoft Graph gebruikt worden. Documentatie over deze library kan men vinden via onderstaande link.   <br />
https://docs.microsoft.com/en-us/graph/overview?view=graph-rest-1.0  <br />
De applicatie moet worden geregistreed. Onderstaande link is een stappenplan hoe dit moet.  <br />
https://docs.microsoft.com/en-us/graph/auth-register-app-v2  <br />
Wanneer een bedrijf een afspraak met een student bevestigd dient er een online meeting worden aangemaakt. Onderstaande link legt uit hoe dit moet worden geïmplementeerd.  <br />
https://docs.microsoft.com/en-us/graph/api/application-post-onlinemeetings?view=graph-rest-1.0&tabs=csharp  <br />

## License
* PXL Switch2IT - Team 1 (academiejaar 2020-2021)
* PXL Switch2IT - Maarten Warson (academiejaar 2020-2021)
* PXL Switch2IT - Team 2 (academiejaar 2019-2020)

Commit SHA backend (branch Demo-Switch2IT): 931e587bc493b777fe346acf1abede97d8bd6900
