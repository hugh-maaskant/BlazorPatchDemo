using System.Net;
using CommunityToolkit.Diagnostics;

namespace BlazorPatchDemo.Client.Infra;

/// <summary>
/// Specialized <see cref="Result"/> for a sent <see cref="HttpRequestMessage"/>s.
/// </summary>
/// <typeparam name="T">The Type for the Response result when the request was successful.</typeparam>
/// <remarks>
/// Contains either a Success or Failure ApiResult{T} as indicated by <see cref="Result.IsSuccess"/> and <see cref="Result.IsFailure"/>.
/// In case of <see cref="Result.IsSuccess"/>, <see cref="Result{T}.Value"/> is defined, returning a valid <typeparamref name="T"/> instance.
/// In case of <see cref="Result.IsFailure"/>, <see cref="Result{T}.FailureMessage"/> is defined, returning a description of the failure.
/// The <see cref="ApiResult{T}.StatusCode"/> is always defined and valid.
/// </remarks>
public sealed class ApiResult<T> : Result<T>
{
    private readonly HttpStatusCode _statusCode;

    // Constructor for Fail
    private ApiResult(string message, HttpStatusCode statusCode) : base(message)
    {        
        if ((int)statusCode < 400) ThrowHelper.ThrowArgumentException(
                    $"Cannot construct a Fail {nameof(ApiResult<T>)} with {nameof(HttpStatusCode)} < 400 ({statusCode})");
        
        _statusCode = statusCode!;
    }

    // Constructor for Success ApiResult<T> instances.
    private ApiResult(T value, HttpStatusCode statusCode) : base(value)
    {
        if ((int) statusCode >= 400)
            ThrowHelper.ThrowArgumentException(
                $"Cannot construct a Success {nameof(ApiResult<T>)} with {nameof(statusCode)} >= 400 ({statusCode})");

        _statusCode = statusCode;
    }

    /// <summary>
    /// Factory for Fail <see cref="ApiResult{T}"/> instances.
    /// </summary>
    /// <param name="message">The Fail Message</param>
    /// <param name="statusCode">The <see cref="HttpStatusCode"/> returned from the request</param>
    /// <returns>A new Fail{T} <see cref="ApiResult{T}"/> instance.</returns>
    /// <exception cref="ArgumentException">when <paramref name="statusCode"/> is &lt; 400.</exception>
    /// <remarks>
    /// If the passed in <paramref name="statusCode"/> is <c>null</c>, <c>HttpStatusCode.Unused</c>
    /// will be used for <see cref="StatusCode"/>
    /// </remarks>
    public static ApiResult<T> Fail(string message, HttpStatusCode? statusCode)
    {
        return new ApiResult<T>(message, statusCode ?? HttpStatusCode.Unused);
    }
    
    /// <summary>
    /// Factory for Success <see cref="ApiResult{T}"/> instances.
    /// </summary>
    /// <typeparamref name="T">The type for the generic parameter</typeparamref>
    /// <param name="value">The value for the generic parameter</param>
    /// <param name="statusCode">The <see cref="HttpStatusCode"/> for the sent <see cref="HttpRequestMessage"/>.</param>
    /// <returns>A new Success <see cref="ApiResult{T}"/> instance</returns>
    /// <exception cref="InvalidOperationException">when <paramref name="statusCode"/> is &gt;= 400</exception>
    public static ApiResult<T> Success(T value, HttpStatusCode statusCode) => new(value, statusCode);

    /// <summary>
    /// Get the <see cref="HttpStatusCode"/> for the sent <see cref="HttpRequestMessage"/>, if any
    /// </summary>
    public HttpStatusCode StatusCode => _statusCode;
}
