using System.ComponentModel.DataAnnotations;
using AspTemplate.Application.Models.Users;
using AspTemplate.Core.Enums;
using Mapster;

namespace AspTemplate.API.Contracts.Users;

public record RegisterUserRequest(
	[Required] string UserName,
	[Required] string Password,
	[Required] string Email,
	[Required] Role Role);

//public partial class DtoMappings : IRegister
//{
//    public void Register(TypeAdapterConfig config)
//    {
//		config.NewConfig<RegisterUserRequest, RegisterUserModel>()
//            .Map(dest => dest.UserName, src => src.UserName)
//            .Map(dest => dest.Password, src => src.Password)
//            .Map(dest => dest.Email, src => src.Email)
//            .Map(dest => dest.Roles, src => src.Role )
//            .EnableNonPublicMembers(true);
//    }
//}