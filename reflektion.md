# Reflektion — Musiklistappen

**Namn:** Luvinda Weerasinghe
**Datum:** 2026.10.30

---

## Vad var svårast?

Det svåra var att upprätta kopplingen mellan C# och SQLite databasen och få den att fungera. Även kodningen i DBHandlar var utmanande, eftersom jag inte hade så stor erfarenhet av det.

---

## Vad lärde du dig?

Jag lärde mig hur man kopplar Visual Studio till databaser via C# och hur man kör queires från Visual Studio. Det var mycket intressant för mig.

---

## Hur använde du ADO.NET i praktiken — vad kändes naturligt och vad var förvirrande?

Tänk på de fem stegen: öppna anslutning, skapa kommando, skriv SQL, skicka in värden, kör.

Jag använde ADO.NET med Microsoft.Data.Sqlite för att kommunicera med databasen. Jag öppnade en anslutning, skapade SQL-kommandon, skrev SQL-frågor, skickade i värden med parametrar och körde kommandona. Det kändes naturligt att hämta och visa data, men i början var det lite förvirrande att förstå hur anslutningar, kommandon och parametrar fungerar tillsammans.

---

## Varför ska man använda parameteriserade queries?

Vad kan hända om man klistrar ihop SQL med strängar i stället? (Tänk på Bobby Tables.)

Parameteriserade queries skyddar mot SQL injektion och gör koden säkrare. Med parametrar behandlas användarens text som ett värde, inte som SQL kod.

---

## Om du hade mer tid — vad hade du lagt till eller gjort annorlunda?

I min original databas hade jag inte inkluderat SongID som primary key i tabellen Song. Det innebär att man måste ange låtens namn, inte dess SongID – för att söka efter detaljer om en låt (alternativ 5 i huvudmenyn). Jag insåg detta först mot slutet av projektet. Om jag hade haft tid skulle jag ha ändrat i databasen och åtgärdat problemet, men nu måste man alltså skriva in låtens namn för att få fram resultaten.

---

## (VG) Varför delade du upp koden som du gjorde?

Resonera kring ditt designval: varför ligger databaskoden i `DbHandler` och inte i `Program.cs`? Vad hade blivit svårare om allt låg i samma fil?

[Skriv här]
