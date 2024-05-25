using AspTemplate.Core.Enums;
using AspTemplate.Core.Models;

namespace AspTemplate.Application.Dto;

public class UserJwtValidateDto
{
    public UserId UserId { get; init; }
    public string SecurityStamp { get; init; }
}