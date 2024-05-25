using AspTemplate.Application.Dto;
using AspTemplate.Core.Enums;
using AspTemplate.Core.Interfaces.Repositories;
using AspTemplate.Core.Models;
using AspTemplate.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;

namespace AspTemplate.Persistence.Repositories;
public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _context;
    private readonly UserMapper _userMapper;

    public UsersRepository(
        AppDbContext context, 
        UserMapper userMapper)
    {
        _context = context;
        _userMapper = userMapper;
    }

    public async Task Add(User user)
    {
        var userEntity = _userMapper.ToEntity(user);

        foreach (var roleEntity in userEntity.Roles)
            _context.Roles.Attach(roleEntity);

        _ = await _context.Users.AddAsync(userEntity);
        
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByEmail(string email)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .AsNoTracking()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();

        return user == null ? null : _userMapper.ToDomain(user);
    }

    public Task<UserJwtValidateDto?> GetUserJwtValidateDto(UserId userId)
    {
        return _context.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == userId.Value)
            .Select(u => new UserJwtValidateDto
            {
                UserId = new UserId(u.Id),
                SecurityStamp = u.SecurityStamp,
            })
            .FirstOrDefaultAsync();
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