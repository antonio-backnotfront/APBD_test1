using Microsoft.Data.SqlClient;
using Models;

namespace Repositories;

public class TavernRepository : ITavernRepository
{
    string _connectionString;

    public TavernRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public async Task<IEnumerable<AdventurerDTO>> GetAdventurers()
    {
        var list = new List<AdventurerDTO>();
        const string queryString = "select Id, Nickname from Adventurer";
        
        using (SqlConnection com = new SqlConnection(_connectionString))
        {
            await com.OpenAsync();
            SqlCommand cmd = new SqlCommand(queryString, com);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                AdventurerDTO adventurer = new AdventurerDTO( reader["Nickname"].ToString(),reader["Id"].ToString());
                list.Add(adventurer);
            }

            return list;
        }
    }
    
    public async Task<string> GetAdventurer(int id)
    {
        const string queryString = @"
        SELECT A.Id, A.Nickname, R.Name AS RaceName, E.Name AS ExperienceName, P.Name AS PersonName
        FROM Adventurer A
        JOIN Race R ON A.RaceId = R.Id
        JOIN ExperienceLevel E ON A.ExperienceId = E.Id
        JOIN Person P ON A.PersonId = P.Id
        WHERE A.Id = @Id";

        using (SqlConnection com = new SqlConnection(_connectionString))
        {
            await com.OpenAsync();

            using (SqlCommand cmd = new SqlCommand(queryString, com))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var result = new
                        {
                            Id = reader["Id"],
                            Nickname = reader["Nickname"],
                            RaceName = reader["RaceName"],
                            ExperienceName = reader["ExperienceName"],
                            PersonName = reader["PersonName"]
                        };

                        return System.Text.Json.JsonSerializer.Serialize(result);
                    }
                }
            }
        }

        return "{}";
    }
    
    public async Task<Adventurer> CreateAdventurer(Adventurer adventurer)
    {
        // The SQL insert query
        const string queryString = @"
            INSERT INTO Adventurer (Nickname, RaceId, ExperienceId, PersonId)
            OUTPUT INSERTED.Id
            VALUES (@Nickname, @RaceId, @ExperienceId, @PersonId)";

        using (SqlConnection com = new SqlConnection(_connectionString))
        {
            await com.OpenAsync();

            using (SqlCommand cmd = new SqlCommand(queryString, com))
            {
                // Add parameters to the query
                cmd.Parameters.AddWithValue("@Nickname", adventurer.Nickname);
                cmd.Parameters.AddWithValue("@RaceId", adventurer.RaceId);
                cmd.Parameters.AddWithValue("@ExperienceId", adventurer.ExperienceId);
                cmd.Parameters.AddWithValue("@PersonId", adventurer.PersonId);

                // Execute the query and get the newly inserted Id
                var newId = (int)await cmd.ExecuteScalarAsync();

                // Set the new Id to the adventurer object and return it
                adventurer.Id = newId;
                return adventurer;
            }
        }
    }
    
    public async Task<bool> PersonHasBounty(string personId)
    {
        const string queryString = "SELECT HasBounty FROM Person WHERE Id = @Id";

        using (SqlConnection com = new SqlConnection(_connectionString))
        {
            await com.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(queryString, com))
            {
                cmd.Parameters.AddWithValue("@Id", personId);

                var result = await cmd.ExecuteScalarAsync();
                if (result != null && result is bool hasBounty)
                {
                    return hasBounty;
                }
            }
        }
        return false; // Return false if the person does not exist or HasBounty is not found
    }
}