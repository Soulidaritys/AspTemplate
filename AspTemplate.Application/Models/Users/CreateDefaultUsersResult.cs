namespace AspTemplate.Application.Models.Users;

public class CreateDefaultUsersResult
{
    public IReadOnlyCollection<RegisterUserModel> CreatedUsers { get; init; } = [];
}