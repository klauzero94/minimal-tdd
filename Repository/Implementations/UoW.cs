using Repository.Interfaces;

namespace Repository.Implementations;

public class UoW : IUoW
{
    private readonly IContext _context;

    public UoW(IContext context) => _context = context;

    public async Task<bool> CommitAsync()
    {
        var changeAmount = await _context.SaveChangesAsync();
        return changeAmount > 0;
    }

    public void Dispose() => _context.Dispose();
}