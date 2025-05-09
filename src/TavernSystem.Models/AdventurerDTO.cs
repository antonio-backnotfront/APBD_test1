namespace Models;

public class AdventurerDTO
{
    public AdventurerDTO(string id, string nickname)
    {
        Id = id;
        Nickname = nickname;
    }

    private string Id { get; set; }
    private string Nickname { get; set; }
    
}