using CommunityToolkit.Diagnostics;

namespace BlazorPatchDemo.Client.Infra;

/// <summary>
/// A Result class for errors
/// </summary>
/// <remarks>
/// <para>
/// A <see cref="Result"/> is either a Success or a Fail <see cref="Result"/>.
/// In case of a Fail <see cref="Result"/>, a message with the reason for the failure has been set.
/// </para>
/// <para>
/// This is inspired by Vladimir Khorikov's blog post and implementation. See e.g.
/// https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/
/// and
/// https://enterprisecraftsmanship.com/posts/error-handling-exception-or-result/.
/// </para>
/// <para>
/// Vladimir's code can be found here:
/// https://gist.github.com/vkhorikov/7852c7606f27c52bc288.
/// </para>
/// </remarks>
public class Result
{
    // Holds the FailureMessage and, if not null, indicates Failure
    private readonly string? _failureMessage;

    /// <summary>
    /// Constructs a Fail <see cref="Result"/>.
    /// </summary>
    /// <param name="failureMessage">The failure reason message.</param>
    /// <exception cref="ArgumentException"> - when failureMessage is null, empty, or whitespace only.</exception>
    protected Result(string failureMessage) =>
        _failureMessage = string.IsNullOrWhiteSpace(failureMessage)
            ? ThrowHelper.ThrowArgumentException<string>
                (nameof(failureMessage), "Cannot be null, empty or whitespace only")
            : failureMessage;

    /// <summary>
    /// Constructs a Success <see cref="Result"/>.
    /// </summary>
    protected Result()
    {
        // no-op, _failureMessage is already null
    }

    /// <summary>
    /// Indicates a Failure <see cref="Result"/>.
    /// </summary>
    /// <remarks>
    /// FailureMessage is now defined and guaranteed not to be empty or whitespace.
    /// </remarks>
    public bool IsFailure => _failureMessage is not null;

    /// <summary>
    /// Indicates a Success <see cref="Result"/>.
    /// </summary>
    /// <remarks>
    /// FailureMessage is undefined and throws an <see cref=" InvalidOperationException"/> when accessed.
    /// </remarks>
    public bool IsSuccess => _failureMessage is null;
    
    /// <summary>
    /// Gets the Failure Message.
    /// </summary>
    /// <exception cref="InvalidOperationException">when accessed while <see cref="IsSuccess"/> is <c>true</c></exception>
    public string FailureMessage => _failureMessage ?? ThrowHelper.ThrowInvalidOperationException<string>(
            $"{nameof(FailureMessage)} is not defined when IsSuccess is true");
    
    /// <summary>
    /// Factory for Fail <see cref="Result"/> instances.
    /// </summary>
    /// <param name="message">The failure reason failureMessage</param>
    /// <returns>A new Fail <see cref="Result"/> instance</returns>
    /// <exception cref="ArgumentNullException"> - when failureMessage is null, empty, or whitespace only.</exception>
    public static Result Fail(string message) => new(message);
    
    /// <summary>
    /// Factory for Success <see cref="Result"/> instances.
    /// </summary>
    /// <returns>A new IsSuccess <see cref="Result"/> instance</returns>
    public static Result Success() => new();

    
    /// <summary>
    /// Combines multiple <see cref="Result"/>s into a single one, with an "and" function on IsSuccess
    /// </summary>
    /// <param name="results">The <see cref="Result"/>s to combine</param>
    /// <returns>
    /// A new IsSuccess <see cref="Result"/> iff all <see cref="Result"/>s in the argument list are Success
    /// <see cref="Result"/>s, or the first Fail  <see cref="Result"/> otherwise.
    /// </returns>
    public static Result Combine(params Result[] results)
    {
        foreach (Result result in results)
        {
            if (result.IsFailure)
                return result;
        }

        return Success();
    }
}
