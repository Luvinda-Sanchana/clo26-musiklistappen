
using Microsoft.Extensions.Configuration;

// Läs inställningar från appsettings.json
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("Default")
    ?? throw new InvalidOperationException(
        "Hittar ingen connection string 'Default' i appsettings.json.");

// Huvudmeny
bool running = true;

while (running)
{
    Console.WriteLine();
    Console.WriteLine("=== MUSIKLISTAPPEN ===");
    Console.WriteLine("1. Visa alla artister");
    Console.WriteLine("2. Visa alla album");
    Console.WriteLine("3. Visa alla låtar");
    Console.WriteLine("4. Sök efter artist");
    Console.WriteLine("5. Avsluta");
    Console.Write("Välj ett alternativ: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.WriteLine("Här kommer en lista senare");
            break;

        case "2":
            Console.WriteLine("Här kommer en lista senare");
            break;

        case "3":
            Console.WriteLine("Här kommer en lista senare");
            break;

        case "4":
            Console.WriteLine("Här kommer en sökning senare");
            break;

        case "5":
            running = false;
            Console.WriteLine("Tack för att du använde Musiklistappen!");
            break;

        default:
            Console.WriteLine("Ogiltigt val. Försök igen.");
            break;
    }
}

Console.WriteLine("Programmet avslutas.");
