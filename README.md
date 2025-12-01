# Garage

En program som hanterar en fysisk garageplats till fordon. Programmet låter användaren lägga till, ta bort och visa fordon i garaget samt söka efter specifika fordon baserat på olika kriterier.

## Typ av Garage
Garage kan vara av typ `Vehicle`, `Car`, `Motorcycle`, `Airplane`, `Boat`, eller `Bus`.
Garaget av `Vehicle` typ kan innehålla alla typ av fordon.
Garaget av annan typer kan bara innehålla egen typ av fordon.

## Typ av fordon
Fordoner typ som kan användas är beroende på vilken typ av Garage är activa.
Det går inte att skapa fordon av typ `Vehicle`.
Fordoner kan vara ab typ: `Car`, `Motorcycle`, `Airplane`, `Boat`, eller `Bus`.

## Kapacitet
Garagets kapacitetsvärde `(Capacity)` representerar hur många fysiska platser det innehåller. Olika fordon kan ta upp olika mycket plats.

## AvailablePlaces
Garagets `AvailablePlaces` representerar hur många fysiska platser nuvarande finns kvar i garaget. Platser kvar beräknas vid hur mycket platser tar varje fordon som redan finns i garaget.   

Internalt i garaget, beräknas `_availablePlaces` på början som `Capacity * 3`. När man läser värde, får man `AvailablePLaces = _availablePlaces / 3`.
`AvailablePlaces` rå värde är inte helt meningsfulla och jag rekommenderar att man alltid formaterar den men `ToMixedFraction`.   

Vi gör så för att vi vill beräkna platser korret när det finns motorcyclar i garaget. Motorcyclar tar bara 1/3 fysisk plats.

`"Platser kvar nu: 3 2/3"` menar att det finns 3 hel fysisk platser kvar + ett plats där redan finns 1 motorcykel. Detta plats kan bara ta andra motorcykel.

## Config

Om det finns en `config.json` fil i `Garage\Garage`, filen kommer kopieras till `Garage\Garage\bin\Debug\net9.0\` om det byggs.  
Garaget från fil blir activa Garage.

En default config.json fil inkluderas med en `Vehicle` garage, som heter `Parkerings Garage`, `10` platser kapacitet.

config.json
````json
{
  "GarageTitle": "Parkerings Garage",
  "GarageType": "vehicle",
  "GarageCapacity": 10
}

````

## Söka i Garaget
Sökning är inte `case sensitive`.

Man kan söka ett fordon vid att matta in en registrering nummer, eller att skappa en `query` som innehåller fordons egenskapper.
Det är möjligt att söka fordon vid alla fäsltarr som finns, även egen fältar.

TEx: query `make=tesla;color=röd;trunkcontent=hund` returneraas alla `Tesla` fordon som är `röd` och innehåller `hund`

Om Garaget är av typ `Vehicle`, det går även att söka på en fordon typ:
TEx: query `type=car` returneraas alla fordon som är `Car` i garaget.
Detta query fungerar på alla garage typer också, men själv är inte menningsfulla och kommer returnera alla fordon, utan tillvidare filtrering.

**Fältar att använda i query**
Alla fordon typer: `type`, `make`, `model`, `color`
Motorcykel: `isutility` - `true or false`
Bil: `truckcontent`
Bus: `linjeID` - `433, 284B, etc`
Flyggplan: `wingspan`, `numberofengines`
Båt: `boatType` - `segelbåt, katamaran, etc`

## Populate
Man kan försöka att filla på garaget med slumpmässig fordoner.

Poplulate funktion tar en användare valde totalt nummer att fordon att skappas och försätta populära garaget till alla totalt fordon finns i garaget, eller till garaaget har inget plats kvar.
Om garaget populärar med minst 1 fordon, resultat blir `success`. Annars, får man en `error`.

## Spara Garaget
Om man väljer `Spara Garaget` i meny, en `JSON` fil med samma namn som Garaget kommer sparas med alla fordon som finns i garaget.   
Filen sparas i  `Garage\Garage\bin\Debug\net9.0\`.

## Ladda Garaget
Om de finns en fil med samma namn som nuvarande activa garage och filen innehåller minst 1 fordon, alla fordon i garaget kommer laddas.  