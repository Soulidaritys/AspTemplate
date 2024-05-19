using AspTemplate.Core.Enums;
using AspTemplate.Core.Interfaces.Repositories;
using AspTemplate.Core.Models;
using AspTemplate.Persistence.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace AspTemplate.Persistence.Repositories;
public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UsersRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Add(User user)
    {
        var userEntity = _mapper.Map<UserEntity>(user);

        foreach (var roleEntity in userEntity.Roles)
            _context.Roles.Attach(roleEntity);

        _ = await _context.Users.AddAsync(userEntity);
        
        await _context.SaveChangesAsync();
    }

    public async Task<T?> GetByEmail<T>(string email)
    {
        //var m = await _context.Users
        //    .Include(x => x.Roles)
        //    .Include(x => x.MediaCollection)
        //    .Include(x => x.UserProfile)
        //    .Where(x => x.Email == email)
        //    .FirstOrDefaultAsync();

        var user = await _context.Users
            .AsNoTracking()
            .Where(u => u.Email == email)
            .ProjectToType<T>()
            .FirstOrDefaultAsync();

        return user;
    }

    public async Task<HashSet<Permission>> GetUserPermissions(Guid userId)
    {
        var roles = await _context.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == userId)
            .Select(u => u.Roles)
            .FirstOrDefaultAsync();

        if (roles == null || roles.Count == 0)
            throw new Exception();

        return roles
            .SelectMany(r => r.Permissions)
            .Select(p => (Permission)p.Id)
            .ToHashSet();
    }
}