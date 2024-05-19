using Sstv.DomainExceptions;

namespace AspTemplate.Core;

[ExceptionConfig(ClassName = "BusinessException")]
[ErrorDescription(Prefix = "BL")]
public enum ErrorCodes
{
    [ErrorDescription(Description = "Unknown error")]
    Default = 0,

    [ErrorDescription(Description = "Invalid data")]
    InvalidData = 1,

    [ErrorDescription(Description = "Invalid email")]
    InvalidEmail = 2,

    [ErrorDescription(Description = "User undefined")]
    UserUndefined = 3,

    [ErrorDescription(Description = "User with this email already exists")]
    UserWithEmailAlreadyExists = 4,


    [ErrorDescription(Description = "User not found")]
    UserNotFound = 5,

    [ErrorDescription(Description = "Authentication failed")]
    AuthenticationFailed = 6,
}