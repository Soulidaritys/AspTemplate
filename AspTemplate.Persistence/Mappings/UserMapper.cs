using AspTemplate.Core.Enums;
using AspTemplate.Core.Models;
using AspTemplate.Persistence.Entities;

namespace AspTemplate.Persistence.Mappings;

public class UserMapper
{
    private readonly MediaMapper _mediaMapper;

    public UserMapper(MediaMapper mediaMapper)
    {
        _mediaMapper = mediaMapper;
    }

    public UserEntity ToEntity(User model)
    {
        var target = new UserEntity();
        target.Id = model.Id.Value;
        target.Email = model.Email;
        target.UserName = model.UserName;
        target.PasswordHash = model.PasswordHash;
        target.SecurityStamp = model.SecurityStamp;
        target.FirstName = model.FirstName;
        target.LastName = model.LastName;
        target.CreatedAt = model.CreatedAt;
        target.UpdatedAt = model.UpdatedAt;

        if (model.Roles != null)
        {
            target.Roles = model.Roles.Select(x => new RoleEntity { Id = (int)x }).ToList();
        }

        if (model.Media != null)
        {
            target.MediaCollection = model.Media.Select(x => _mediaMapper.ToEntity(x)).ToList();
        }

        return target;
    }

    public User ToDomain(UserEntity entity)
    {
        var target = new User(
            new UserId(entity.Id),
            entity.UserName,
            entity.PasswordHash,
            entity.Email,
            entity.SecurityStamp,
            entity.FirstName,
            entity.LastName,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.Roles?.Select(x => (Role)x.Id).ToList(),
            entity.MediaCollection.Select(x => _mediaMapper.ToDomain(x)).ToList()
        );

        return target;
    }
}