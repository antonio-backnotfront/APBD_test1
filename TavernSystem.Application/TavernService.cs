using Models;
using Repositories;

namespace Application;

public class TavernService : ITavernService
{
    ITavernRepository _tavernRepository;

    public TavernService(string connectionString)
    {
        _tavernRepository = new TavernRepository(connectionString);
    }
    public async Task<IEnumerable<AdventurerDTO>> GetAdventurers()
    {
        var list =  _tavernRepository.GetAdventurers().Result;
        return list.ToList();
    }

    public async Task<string> GetAdventurer(int id)
    {
        return await _tavernRepository.GetAdventurer(id);
    }

    public async Task<Adventurer> CreateAdventurer(Adventurer adventurer)
    {
        // Check if the Person associated with the Adventurer has a bounty
        var hasBounty = await _tavernRepository.PersonHasBounty(adventurer.PersonId);
    
        if (hasBounty)
        {
            throw new InvalidOperationException("Cannot add an adventurer. The associated Person has a bounty.");
        }
    
        // Proceed to create the Adventurer
        return await _tavernRepository.CreateAdventurer(adventurer);
    }
    
    
}