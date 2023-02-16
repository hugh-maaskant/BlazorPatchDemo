using CommunityToolkit.Diagnostics;

namespace BlazorPatchDemo.Client.Infra;

/// <summary>
/// A generically typed Result class for errors
/// </summary>
/// <remarks>
/// A <see cref="Result{T}"/> is either a Success or a Fail. see <see cref="Result"/>.
/// In case of a Success <see cref="Result"/>,
/// the resulting value is available through the <see cref="Result"/> property.
/// In case of a Fail <see cref="Result"/>,
/// a failureMessage with the reason for the failure has been set.
/// </remarks>
public class Result<T> : Result
{
    // Holds the result value upon Success
    private readonly T _value = default!;
    
    /// <summary>
    /// Constructs a Fail <see cref="Result{T}"/> instance.
    /// </summary>
    /// <param name="failureMessage">The failure reason failureMessage.</param>
    protected Result(string failureMessage) : base(failureMessage)
    {
        // no-op
    }
    
    /// <summary>
    /// Constructs a Success <see cref="Result{T}"/> instance.
    /// </summary>
    /// <param name="value">The IsSuccess value to be set</param>
    protected Result(T value)
    {
        _value = value;
    }
    
    /// <summary>
    /// Returns the generically typed <c>T</c> value when <see cref="Result.IsSuccess"/> is <c>true</c>.
    /// </summary>
    /// <exception cref="InvalidOperationException"> - if accessed when IsSuccess is <c>false</c></exception>
    public T Value => IsSuccess
        ? _value
        : ThrowHelper.ThrowInvalidOperationException<T>();

    /// <summary>
    /// Factory for Fail <see cref="Result{T}"/> instances.
    /// </summary>
    /// <param name="failureMessage">The failure reason message.</param>
    /// <returns>A new Fail{T} <see cref="Result"/> instance</returns>
    /// <exception cref="ArgumentException">when <see cref="failureMessage"/> is null, empty, or whitespace only.</exception>
    public new static Result<T> Fail(string failureMessage) => new(failureMessage);

    /// <summary>
    /// Factory for Success <see cref="Result{T}"/> instances.
    /// </summary>
    /// <typeparamref name="T">The type for the generic parameter</typeparamref>
    /// <param name="value">The value for the generic parameter</param>
    /// <returns>A new IsSuccess <see cref="Result{T}"/> instance</returns>
    public static Result<T> Success(T value) => new(value);
}
