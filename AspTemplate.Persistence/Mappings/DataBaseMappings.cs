using AspTemplate.Core.Enums;
using AspTemplate.Core.Models;
using AspTemplate.Persistence.Entities;
using Mapster;
using System.Linq;

namespace AspTemplate.Persistence.Mappings;
public class DataBaseMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        ForRoles(config);
        ForMedia(config);
        ForUsers(config);
    }
    private static void ForRoles(TypeAdapterConfig config)
    {
        config.NewConfig<Role, RoleEntity>()
            .Map(dest => dest.Id, src => (int)src)
            .Map(dest => dest.Name, src => src.ToString());

        config.NewConfig<RoleEntity, Role>()
            .MapWith(entity => (Role)entity.Id);
    }

    private static void ForMedia(TypeAdapterConfig config)
    {
        config.NewConfig<MediaId, string>()
            .Map(dest => dest, src => src.Value);
        config.NewConfig<string, MediaId>()
            .Map(dest => dest, src => new MediaId(src));

        config.NewConfig<MediaEntity, Media>()
            .Map(dest => dest.ObjectKey, src => src.ObjectKey.Adapt<MediaId>())
            .Map(dest => dest.MediaType, src => src.MediaType)
            .Map(dest => dest.OriginalFileName, src => src.OriginalFileName)
            .Map(dest => dest.CreatedByUserId, src => src.CreatedByUserId.Adapt<UserId>())
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.Width, src => src.Width)
            .Map(dest => dest.Height, src => src.Height)
            .Map(dest => dest.DurationInMs, src => src.DurationInMs)
            .MapToConstructor(true);

        config.NewConfig<Media, MediaEntity>();
    }

    private static void ForUsers(TypeAdapterConfig config)
    {
        config.NewConfig<UserId, Guid>()
            .Map(dest => dest, src => src.Value);
        config.NewConfig<UserId?, Guid?>()
            .Map(dest => dest, src => src);
        config.NewConfig<Guid, UserId>()
            .Map(dest => dest, src => new UserId(src));
        config.NewConfig<Guid?, UserId?>()
            .Map(dest => dest, src => src);

        config.NewConfig<UserEntity, User>()
            .Map(dest => dest.Id, src => new UserId(src.Id))
            .Map(dest => dest.UserName, src => src.UserName)
            .Map(dest => dest.PasswordHash, src => src.PasswordHash)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.UserProfile, src => src.UserProfile.Adapt<UserProfile>())
            .Map(dest => dest.Roles, src => src.Roles.Adapt<List<Role>>())
            .Map(dest => dest.Media, src => src.MediaCollection)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
            .MapToConstructor(true);

        config.NewConfig<User, UserEntity>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .AfterMapping((model, entity) =>
            {
                if (model.UserProfile?.Avatar != null)
                {
                    entity.UserToMediaChains.Add(new UserMediaEntity()
                    {
                        ObjectKey = model.UserProfile.Avatar.ObjectKey.Value,
                        UserId = model.Id.Value,
                    });
                }
            })
        ;
        // ----------------------------------------
        config.NewConfig<UserProfileEntity, UserProfile>()
            .Map(dest => dest.UserId, src => src.UserId.Adapt<UserId>())
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.Avatar,
                src => src.User.UserToMediaChains.FirstOrDefault().Media)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
            .MapToConstructor(true);

        config.NewConfig<UserProfile, UserProfileEntity>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
        ;

        // ---------------------------------------
        
    }
}