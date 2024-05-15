using EventsApi.src.Data;
using EventsApi.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsApi.src.Repositories;

public class CreatorRepository(DataContext _context, ILogger<CreatorRepository> _logger) : ICreatorRepository
{
    private readonly DataContext context = _context ?? throw new ArgumentNullException(nameof(_context));
    private readonly ILogger<CreatorRepository> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));

    public async Task ChangeName(string oldName, string newName)
    {
        var creator = await context.Creators.Where(x => x.Username.Equals(oldName)).SingleOrDefaultAsync();
        if (creator is null)
            return;
        creator.Username = newName;
        context.Creators.Update(creator);
        int num = await context.SaveChangesAsync();
        if(num > 0)
            logger.LogInformation("[EventsApi] Username {oldname} changed to {newname}", oldName, newName);
    }

    public async Task TryAddCreator(long creatorId, string creatorName)
    {
        if((await context.Creators.FindAsync(creatorId)) is null)
        {
            await context.Creators.AddAsync(new Data.Entities.Creator { Id = creatorId, Username = creatorName });
            await context.SaveChangesAsync();
        }    
    }
}
