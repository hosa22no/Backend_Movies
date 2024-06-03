## Movies & Reviews API

### Filmer: 
Användare ska kunna lägga till, hämta, uppdatera och ta bort filmer. Varje film ska innehålla information om titel, releaseår och en lista med recensioner.
Lägg till en film
Hämta en film
Hämta alla filmer
Uppdatera en film
Ta bort en film
### Recensioner: 
Användare ska kunna lägga till och hämta recensioner för filmer. Varje recension ska innehålla en betygsättning, kommentar och referens till den film den tillhör.
Lägg till en recension
Hämta en recension
Hämta alla recensioner
Uppdatera en recension
Ta bort en recension
Säkerhet: Endast autentiserade användare ska kunna skapa, uppdatera och ta bort data. Alla användare, även oautentiserade, ska kunna hämta (läsa) information om filmer och deras recensioner.
Övriga operationer: Utöver CRUD operationer ska API:et även implementera följande operationer
Hämta alla recensioner för en film
Hämta alla recensioner skapade av den inloggade användaren
