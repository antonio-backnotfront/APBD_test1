namespace Models;

public class Adventurer
{
    public Adventurer(int id, string nickname, int raceId, int experienceId, string personId)
    {
        Id = id;
        Nickname = nickname;
        RaceId = raceId;
        ExperienceId = experienceId;
        PersonId = personId;
    }

    public int Id { get; set; }
    public string Nickname { get; set; }
    public int RaceId { get; set; }
    public int ExperienceId { get; set; }
    public string PersonId { get; set; }
}