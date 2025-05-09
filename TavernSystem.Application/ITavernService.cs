using Models;

namespace Application;

public interface ITavernService
{
    Task<IEnumerable<AdventurerDTO>> GetAdventurers();
    Task<string> GetAdventurer(int id);
    Task<Adventurer> CreateAdventurer(Adventurer adventurer);
}