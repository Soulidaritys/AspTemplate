using AspTemplate.Core.Interfaces.Services;
using AspTemplate.Persistence.Models;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace AspTemplate.Persistence.Services;

public class InitDbService
{
    private readonly ILogger? _logger;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly IUserService _userService;

    public InitDbService(
        ILogger? logger,
        IMapper mapper,
        AppDbContext dbContext)
    {
        _logger = logger;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<InitDbResult> InitDb()
    {
        return new InitDbResult
        {
            
        };
    }
}