using CoworkingApi.src.Data;
using CoworkingApi.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoworkingApi.src.Repositories;

public class HolderRepository(DataContext _context, ILogger<HolderRepository> _logger) : IHolderRepository
{
    private readonly DataContext context = _context ?? throw new ArgumentNullException(nameof(_context));
    private readonly ILogger<HolderRepository> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));

    public async Task ChangeName(string oldName, string newName)
    {
        var creator = await context.Holders.Where(x => x.Username.Equals(oldName)).SingleOrDefaultAsync();
        if (creator is null)
            return;
        creator.Username = newName;
        context.Holders.Update(creator);
        int num = await context.SaveChangesAsync();
        if (num > 0)
            logger.LogInformation("[CoworkingApi] Username {oldname} changed to {newname}", oldName, newName);
    }

    public async Task TryAddHolder(long creatorId, string creatorName)
    {
        if ((await context.Holders.FindAsync(creatorId)) is null)
        {
            await context.Holders.AddAsync(new Data.Entities.Holder { Id = creatorId, Username = creatorName });
            await context.SaveChangesAsync();
        }
    }
}
