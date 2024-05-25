using AspTemplate.Core.Interfaces.Services;
using AspTemplate.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace AspTemplate.Persistence.Services;

public class InitDbService
{
    private readonly ILogger? _logger;
    private readonly AppDbContext _dbContext;
    private readonly IUserService _userService;

    public InitDbService(
        ILogger? logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<InitDbResult> InitDb()
    {
        return new InitDbResult
        {
            
        };
    }
}