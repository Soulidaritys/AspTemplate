using AspTemplate.Application.Auth;
using AspTemplate.Application.Models.Auth;
using AspTemplate.Application.Models.Users;
using AspTemplate.Core;
using AspTemplate.Core.Enums;
using AspTemplate.Core.Interfaces.Repositories;
using AspTemplate.Core.Interfaces.Services;
using AspTemplate.Core.Models;
using Microsoft.Extensions.Logging;

namespace AspTemplate.Application.Services;
public class UserService : IUserService
{
    private readonly ILogger? _logger;
    private readonly IUsersRepository _usersRepository;
    private readonly IMediaRepository _mediaRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public UserService(
        ILogger<UserService>? logger,
        IUsersRepository usersRepository,
        IMediaRepository mediaRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _logger = logger;
        _usersRepository = usersRepository;
        _mediaRepository = mediaRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task Register(RegisterUserModel registerUserModel)
    {
        var userByEmail = await _usersRepository.GetByEmail(registerUserModel.Email);
        if (userByEmail != null)
            throw ErrorCodes.UserWithEmailAlreadyExists.ToException();

        var hashedPassword = _passwordHasher.Generate(registerUserModel.Password);

        var userId = new UserId(Guid.NewGuid());

        var user = new User(
            new UserId(Guid.NewGuid()),
            registerUserModel.UserName,
            hashedPassword,
            registerUserModel.Email,
            GenerateSecurityStamp(),
            registerUserModel.FirstName,
            registerUserModel.LastName,
            DateTimeOffset.UtcNow,
            null,
            registerUserModel.Roles,
            null
        );

        await _usersRepository.Add(user);
    }

    public async Task<AppAuthToken> Login(string email, string password)
    {
        var user = await _usersRepository.GetByEmail(email);
        if (user == null)
            throw ErrorCodes.UserNotFound.ToException();

        var result = _passwordHasher.Verify(password, user.PasswordHash);
        if (result == false)
            throw ErrorCodes.AuthenticationFailed.ToException();

        var token = _jwtProvider.Generate(user);

        return token;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _usersRepository.GetByEmail(email);
        if (user == null)
            throw ErrorCodes.UserNotFound.ToException();

        return user;
    }

    public async Task<CreateDefaultUsersResult> CreateDefaultUsers()
    {
        var createdUsers = new List<RegisterUserModel>();
        var defaultUsers = GetDefaultUsers();

        await _mediaRepository.Add(_avatarMedia);
        foreach (var registerUserModel in defaultUsers)
        {
            try
            {
                await Register(registerUserModel);
                createdUsers.Add(registerUserModel);
            }
            catch (BusinessException e) 
                when (e.ErrorCode == ErrorCodes.UserWithEmailAlreadyExists.ToException().ErrorCode)
            {
                _logger?.LogDebug("User with '{email}' already exists", registerUserModel.Email);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Failed to create default user '{email}'", registerUserModel.Email);
            }
        }

        return new CreateDefaultUsersResult()
        {
            CreatedUsers = createdUsers,
        };
    }

    static readonly Media _avatarMedia = new Media(
            new MediaId("media1"),
            MediaType.UserAvatar,
            "default-avatar.png",
            null,
            DateTimeOffset.UtcNow,
            width: 1920,
            height: 1080
        );


    private static List<RegisterUserModel> GetDefaultUsers()
    {
        return new List<RegisterUserModel>
        {
            new RegisterUserModel(
                "admin@gmail.com",
                "administrator",
                "password",
                [Role.Admin],
                "Alex",
                "Adminovich"
            ),

            new RegisterUserModel(
                "consumer1@gmail.com",
                "consumer1337",
                "password",
                [Role.ConsumerUser],
                "Constantin",
                "Comsumerovich"
            ),
        };
    }

    private string GenerateSecurityStamp()
    {
        return Guid.NewGuid().ToString();
    }
}