using Microsoft.Data.Sqlite;

namespace MusikApp;

/// <summary>
/// ALL databaskod bor här. Program.cs ska aldrig innehålla SQL eller SqliteConnection.
///
/// Det här är ett skelett: metoderna finns, kommentarerna visar vad varje metod ska göra,
/// men koden är din. Alla metoder kastar NotImplementedException tills du har skrivit dem.
///
/// ADO.NET i fem steg (samma i varje metod som pratar med databasen):
///   1. Öppna en anslutning        -> new SqliteConnection(...) och connection.Open()
///   2. Skapa ett kommando         -> connection.CreateCommand()
///   3. Skriv SQL:en               -> command.CommandText = "..."
///   4. Skicka in värden säkert    -> command.Parameters.AddWithValue("@namn", värde)
///   5. Kör kommandot              -> ExecuteReader() för SELECT, ExecuteNonQuery() för INSERT/DELETE
///
/// Använd "using var" på anslutning och kommando så stängs de av sig själva.
/// Bygg SQL:en med parametrar (@titel), aldrig genom att klistra ihop strängar.
/// </summary>
/// 
public class DbHandler
{
    private readonly string _connectionString;

    public DbHandler(string connectionString)
    {
        _connectionString = connectionString;
    }

    // ---------------------------------------------------------------
    // G-nivå: du behöver minst tre av de fem här (välj själv vilka)
    // ---------------------------------------------------------------

    /// <summary>Hämtar alla låtar, sorterade på titel.</summary>


    public List<Song> GetAllSongs()
    {
        var songs = new List<Song>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
        SELECT Song_Title, Album_ID, Length_Seconds
        FROM Songs
        ORDER BY Song_Title;
        """;

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var song = new Song
            {
                SongTitle = reader.GetString(0),
                AlbumId = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                LengthSeconds = reader.IsDBNull(2) ? null : reader.GetInt32(2)
            };

            songs.Add(song);
        }

        return songs;
    }



    /// <summary>Hämtar en specifik låt. Returnerar null om den inte finns.</summary>

    public Song? GetSongByTitle(string title)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
    SELECT Song_Title, Album_ID, Length_Seconds
    FROM Songs
    WHERE Song_Title = @title;
    """;

        command.Parameters.AddWithValue("@title", title);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            var song = new Song
            {
                SongTitle = reader.GetString(0),
                AlbumId = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                LengthSeconds = reader.IsDBNull(2) ? null : reader.GetInt32(2)
            };

            return song;
        }

        return null;
    }

    public List<Song> SearchSongs(string searchText)
    {
        var songs = new List<Song>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
    SELECT Song_Title, Album_ID, Length_Seconds
    FROM Songs
    WHERE Song_Title LIKE @search
    ORDER BY Song_Title;
    """;

        command.Parameters.AddWithValue("@search", "%" + searchText + "%");

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var song = new Song
            {
                SongTitle = reader.GetString(0),
                AlbumId = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                LengthSeconds = reader.IsDBNull(2) ? null : reader.GetInt32(2)
            };

            songs.Add(song);
        }

        return songs;
    }


    /// <summary>Lägger till en ny låt.</summary>

    public void AddSong(Song song)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
    INSERT INTO Songs (Song_Title, Album_ID, Length_Seconds)
    VALUES (@title, @albumId, @length);
    """;

        command.Parameters.AddWithValue("@title", song.SongTitle);

        command.Parameters.AddWithValue(
            "@albumId",
            song.AlbumId.HasValue ? song.AlbumId.Value : DBNull.Value);

        command.Parameters.AddWithValue(
            "@length",
            song.LengthSeconds.HasValue ? song.LengthSeconds.Value : DBNull.Value);

        command.ExecuteNonQuery();
    }


    /// <summary>Tar bort en låt. Returnerar true om en rad faktiskt togs bort.</summary>

    public bool DeleteSong(string songTitle)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
    DELETE FROM Songs
    WHERE Song_Title = @title;
    """;

        command.Parameters.AddWithValue("@title", songTitle);

        int rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }


    // ---------------------------------------------------------------
    // VG-nivå: du behöver minst EN av dessa två (och reflektionen i README)
    // ---------------------------------------------------------------

    /// <summary>(VG, alternativ A) Låtar tillsammans med albumets namn. Kräver en JOIN.</summary>
    public List<(string SongTitle, string AlbumName)> GetSongsWithAlbum()
    {
        // TODO: SELECT Låtar.Titel, Album.Namn FROM Låtar JOIN Album ON ... ORDER BY ...
        throw new NotImplementedException();
    }

    /// <summary>(VG, alternativ B) Låtar som är längre än medellängden. Kräver en subquery.</summary>
    public List<Song> GetSongsLongerThanAverage()
    {
        // TODO: SELECT ... WHERE Längd > (SELECT AVG(Längd) FROM Låtar).
        throw new NotImplementedException();
    }
}
