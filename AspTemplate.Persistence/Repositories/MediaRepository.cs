using AspTemplate.Core.Interfaces.Repositories;
using AspTemplate.Core.Models;
using AspTemplate.Persistence.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AspTemplate.Persistence.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public MediaRepository(
        AppDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Add(Media media)
    {
        var userEntity = _mapper.Map<MediaEntity>(media);
        
        _ = await _dbContext.Media.AddAsync(userEntity);

        await _dbContext.SaveChangesAsync();
    }
}