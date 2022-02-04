# Internship manager frontend
Project met als doel het ontwikkelen van een webapplicatie om stageaanvragen voor de PXL Hogeschool Hasselt te vereenvoudigen. Het doel is om een geïntegreerde omgeving te creëren waar potentiële stagebedrijven opdrachten kunnen indienen, de stagecoördinator en de PXL-Digital-docenten deze opdrachten kunnen evalueren en de studenten deze kunnen bekijken en kiezen voor hun stage. 
Dit is het front-end gedeelte van het project en is gegenereerd met [Angular CLI](https://github.com/angular/angular-cli) version 8.3.20.

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
1.  Zorg dat je de backend API lokaal hebt runnen (zowel Stagebeheer API als Event API).
2.  Zorg dat je Node Package Manager (NPM) hebt geïnstalleerd.
3.  Open een bash of terminal op de locatie van het project en geef de volgende commando's op:
        a. Installatie van packages
            npm install
        b. Runnen van project
            ng serve (-o voor direct openen in de browser)
4. Het project runt nu lokaal op het adres http://localhost:4200/
5. E-mailadres om aan te melden kan je vinden in tabel dbo.Users.
6. Het gebruikte wachtwoord voor iedere gebruiker is Test123*.

## Uitbreiding Handshake Event
* Momenteel draaien de Stagebeheer API en de Event API op een aparte poort. De basis URL kan je terugvinden in de environment bestanden (*eventUrl*). 
* Voor de uitbreiding rond het Handshake Event is er een aparte module opgezet, terug te vinden in de directory */src/app/components/event*. In deze directory zijn ook de aparte modules per rol terug te vinden (company - student - coordinator).
* Er is gebruik gemaakt van [Angular Material](https://material.angular.io/) voor de basis UI elementen. De modules hiervoor zitten in een aparte MaterialModule, die ook geïmporteerd is in de SharedModule. Het is aan te raden om bij nieuwe modules de SharedModule te importeren.
* Voor tijdsberekeningen (timeslots van de agenda, eventuele overlappingen, ...) hebben we [Luxon](https://moment.github.io/luxon/docs) gebruikt.

## Sonar
### Sonar lokaal runnen in een Docker container
1. Installeer Docker Desktop (incl. WSL2) -- https://docs.docker.com/docker-for-windows/install/
2. Pull Sonar container en volg instructies -- https://docs.sonarqube.org/latest/setup/get-started-2-minutes/

### Sonar code coverage
1. Genereer code coverage bestand(en) -- 'ng test --code-coverage'.
2. Zorg dat sonar properties bestand de juiste configuratie bevat.

## License
* PXL Switch2IT - Team 1 (academiejaar 2020-2021)
* PXL Switch2IT - Maarten Warson (academiejaar 2020-2021)
* PXL Switch2IT - Team 2 (academiejaar 2019-2020)

Commit SHA frontend (branch Demo-Switch2IT): 088839c2aab4437b41a5d54987009e8d56987616
