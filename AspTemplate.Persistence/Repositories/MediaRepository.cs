using AspTemplate.Core.Interfaces.Repositories;
using AspTemplate.Core.Models;
using AspTemplate.Persistence.Entities;
using AspTemplate.Persistence.Mappings;

namespace AspTemplate.Persistence.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly AppDbContext _dbContext;
    private readonly MediaMapper _mediaMapper;

    public MediaRepository(
        AppDbContext dbContext,
        MediaMapper mediaMapper)
    {
        _dbContext = dbContext;
        _mediaMapper = mediaMapper;
    }

    public async Task Add(Media media)
    {
        var mediaEntity = _mediaMapper.ToEntity(media);
        _ = await _dbContext.Media.AddAsync(mediaEntity);
        await _dbContext.SaveChangesAsync();
    }
}