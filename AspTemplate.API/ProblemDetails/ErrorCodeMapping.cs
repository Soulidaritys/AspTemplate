using System.Collections.Frozen;
using AspTemplate.Core;

namespace AspTemplate.API.ProblemDetails;

public static class ErrorCodeMapping
{
    /// <summary>
    /// Mapping from error code to http status code.
    /// </summary>
    private static readonly FrozenDictionary<string, int> _statusCodeMap = new Dictionary<ErrorCodes, int>
    {
        // 2xx

        // 3xx

        // 4xx
        [ErrorCodes.InvalidData] = StatusCodes.Status400BadRequest,
        [ErrorCodes.InvalidEmail] = StatusCodes.Status400BadRequest,
        [ErrorCodes.UserUndefined] = StatusCodes.Status400BadRequest,
        
        [ErrorCodes.UserNotFound] = StatusCodes.Status404NotFound,

        // 5xx
        [ErrorCodes.Default] = StatusCodes.Status500InternalServerError,
    }.ToFrozenDictionary(x => x.Key.GetErrorCode(), x => x.Value);

    /// <summary>
    /// Map error code to status code.
    /// </summary>
    /// <param name="errorCode">ErrorCode.</param>
    /// <returns>HTTP status code</returns>
    public static int MapToStatusCode(string errorCode)
    {
        ArgumentNullException.ThrowIfNull(errorCode);

        if (!_statusCodeMap.TryGetValue(errorCode, out var statusCode))
        {
            return StatusCodes.Status500InternalServerError;
        }

        return statusCode;
    }

    /// <summary>
    /// Is error code caused by exceptional case.
    /// </summary>
    /// <param name="errorCode">Error code.</param>
    /// <returns>True, when error occured.</returns>
    public static bool IsError(string errorCode)
    {
        return MapToStatusCode(errorCode) >= 400;
    }
}
