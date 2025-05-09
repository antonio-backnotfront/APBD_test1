using Models;

namespace Repositories;

public interface ITavernRepository
{
    Task<IEnumerable<AdventurerDTO>> GetAdventurers();
    Task<string> GetAdventurer(int id);
    Task<Adventurer> CreateAdventurer(Adventurer adventurer);
    public Task<bool> PersonHasBounty(string personId);

}