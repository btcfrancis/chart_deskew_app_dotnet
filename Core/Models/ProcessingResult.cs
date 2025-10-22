namespace ChartDeskewApp.Core.Models;

/// <summary>
/// Generic result wrapper for processing operations
/// </summary>
/// <typeparam name="T">Type of the result data</typeparam>
public class ProcessingResult<T>
{
  /// <summary>
  /// Whether the operation was successful
  /// </summary>
  public bool IsSuccess { get; set; }

  /// <summary>
  /// The result data
  /// </summary>
  public T? Data { get; set; }

  /// <summary>
  /// Error message if operation failed
  /// </summary>
  public string ErrorMessage { get; set; } = string.Empty;

  /// <summary>
  /// Exception that occurred during processing
  /// </summary>
  public Exception? Exception { get; set; }

  /// <summary>
  /// Processing time in milliseconds
  /// </summary>
  public long ProcessingTimeMs { get; set; }

  /// <summary>
  /// Creates a successful result
  /// </summary>
  public static ProcessingResult<T> Success(T data, long processingTimeMs = 0)
  {
    return new ProcessingResult<T>
    {
      IsSuccess = true,
      Data = data,
      ProcessingTimeMs = processingTimeMs
    };
  }

  /// <summary>
  /// Creates a failed result
  /// </summary>
  public static ProcessingResult<T> Failure(string errorMessage, Exception? exception = null)
  {
    return new ProcessingResult<T>
    {
      IsSuccess = false,
      ErrorMessage = errorMessage,
      Exception = exception
    };
  }

  public override string ToString()
  {
    return IsSuccess ? $"Success: {Data}" : $"Failed: {ErrorMessage}";
  }
}