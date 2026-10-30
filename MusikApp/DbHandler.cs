using Microsoft.Data.Sqlite;

namespace MusikApp;

public class DbHandler
{
    private readonly string _connectionString;

    public DbHandler(string connectionString)
    {
        _connectionString = connectionString;
    }



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

}
