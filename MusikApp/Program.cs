
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MusikApp;

// Läs inställningar från appsettings.json
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("Default")
    ?? throw new InvalidOperationException(
        "Hittar ingen connection string 'Default' i appsettings.json.");

var db = new DbHandler(connectionString);


// Huvudmeny
bool running = true;

while (running)
{

    Console.Clear();

    Console.WriteLine();
    Console.WriteLine("====== MUSIKLISTAPPEN =====");
    Console.WriteLine();
    Console.WriteLine("1. Visa alla låtar");
    Console.WriteLine("2. Lägg till en låt");
    Console.WriteLine("3. Ta bort en låt");
    Console.WriteLine("4. Sök efter en låt");
    Console.WriteLine("5. Visa detaljer för en låt");
    Console.WriteLine("6. Avsluta");
    Console.WriteLine();
    Console.WriteLine("---------------------------");
    Console.WriteLine();

    Console.Write("Välj ett alternativ: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {

        case "1":
        var songs = db.GetAllSongs();

        Console.WriteLine("\n====== ALLA LÅTAR ======");

        Console.WriteLine();

            if (songs.Count == 0)
        {
            Console.WriteLine("Det finns inga låtar i listan.");
        }
        else
        {
            foreach (var song in songs)
            {
                Console.WriteLine(song.SongTitle);
            }
        }
        Console.WriteLine();
        Console.WriteLine("------------------------");
        Console.WriteLine();
        Console.WriteLine($"Totalt antal låtar: {songs.Count}");
        

        Console.WriteLine();
        Console.Write("Tryck Enter för att återgå till menyn...");
        Console.ReadLine();

        break;



    case "2":
            Console.WriteLine(); 
            Console.WriteLine("Lägg till en låt");
            Console.WriteLine();

            Console.Write("Ange låtens titel: ");
            string? title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Titeln får inte vara tom.");

            break;
            }

            Console.Write("Ange album-ID (lämna tomt om inget album): ");
            string? albumInput = Console.ReadLine();

            int? albumId = null;

            if (!string.IsNullOrWhiteSpace(albumInput))
            {
                if (!int.TryParse(albumInput, out int parsedAlbumId))
                {
                    Console.WriteLine("Ogiltigt album-ID.");

                break;
                }

                albumId = parsedAlbumId;
            }

            Console.Write("Ange låtens längd i sekunder (lämna tomt om okänt): ");
            string? lengthInput = Console.ReadLine();

            int? lengthSeconds = null;

            if (!string.IsNullOrWhiteSpace(lengthInput))
            {
                if (!int.TryParse(lengthInput, out int parsedLength))
                {
                    Console.WriteLine("Ogiltig låtlängd.");

                break;
                }

                lengthSeconds = parsedLength;
            }

            var newSong = new Song
            {
                SongTitle = title,
                AlbumId = albumId,
                LengthSeconds = lengthSeconds
            };

            db.AddSong(newSong);

            Console.WriteLine("Låten har lagts till!");

        Console.WriteLine();
        Console.Write("Tryck Enter för att återgå till menyn...");
        Console.ReadLine();

        break;



        case "3":
            Console.WriteLine("\n=== TA BORT EN LÅT ===");

            var songsToDelete = db.GetAllSongs();

            if (songsToDelete.Count == 0)
            {
                Console.WriteLine("Det finns inga låtar att ta bort.");
                break;
            }

            foreach (var song in songsToDelete)
            {
                Console.WriteLine(song.SongTitle);
            }

            Console.Write("\nAnge titeln på låten du vill ta bort: ");
            string? songTitle = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(songTitle))
            {
                Console.WriteLine("Du måste ange en låttitel.");
                break;
            }

            Console.Write($"Är du säker på att du vill ta bort \"{songTitle}\"? (j/n): ");
            string? confirmation = Console.ReadLine();

            if (confirmation?.Trim().ToLower() != "j")
            {
                Console.WriteLine("Borttagningen avbröts.");
                break;
            }

            bool deleted = db.DeleteSong(songTitle);

            if (deleted)
            {
                Console.WriteLine("Låten har tagits bort!");
            }
            else
            {
                Console.WriteLine("Låten hittades inte.");

            }
        Console.WriteLine();
        Console.Write("Tryck Enter för att återgå till menyn...");
        Console.ReadLine();

        break;



        case "4":
            Console.WriteLine("\n===== SÖK EFTER EN LÅT =====");
            Console.WriteLine();

            Console.Write("Ange låttitel eller sökord: ");
            
            string? searchText = Console.ReadLine();
            

            if (string.IsNullOrWhiteSpace(searchText))
            {
                Console.WriteLine("Du måste ange ett sökord.");
                Console.WriteLine("Tryck Enter för att återgå till menyn...");
                Console.ReadLine();
                break;
            }

            var searchResults = db.SearchSongs(searchText.Trim());

            if (searchResults.Count == 0)
            {
                Console.WriteLine("Inga låtar hittades.");
            }
            else
            {
                Console.WriteLine("\n===== SÖKRESULTAT =====");
                Console.WriteLine();

                foreach (var song in searchResults)
                {
                    Console.WriteLine(song.SongTitle);
                }
            }

        Console.WriteLine();
        Console.Write("Tryck Enter för att återgå till menyn...");
        Console.ReadLine();

        break;



        case "5":
            Console.WriteLine("\n===== VISA DETALJER FÖR EN LÅT =====");
            Console.WriteLine();

            Console.Write("Ange låtens titel: ");
            string? titleToFind = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(titleToFind))
            {
                Console.WriteLine("Du måste ange en låttitel.");
                break;
            }

            var songDetails = db.GetSongByTitle(titleToFind.Trim());

            if (songDetails == null)
            {
                Console.WriteLine("Låten hittades inte.");
            }
            else
            {
                Console.WriteLine("\n===== LÅTDETALJER =====");
                Console.WriteLine();
                Console.WriteLine($"Titel: {songDetails.SongTitle}");
                Console.WriteLine($"Album-ID: {songDetails.AlbumId?.ToString() ?? "Okänt"}");
                Console.WriteLine($"Längd: {songDetails.LengthSeconds?.ToString() ?? "Okänd"} sekunder");
            }

        Console.WriteLine();
        Console.Write("Tryck Enter för att återgå till menyn...");
        Console.ReadLine();

        break;


        case "6":
                running = false;
                Console.WriteLine("Tack för att du använde Musiklistappen!");
                break;

            default:
                Console.WriteLine("Ogiltigt val. Försök igen.");
                break;
        }
    }