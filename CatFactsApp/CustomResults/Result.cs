namespace CatFactsApp.CustomResults;

public sealed class Result
{
    private Result(bool isSuccess, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }   
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }
    public static Result Success() => new(true, null);
    public static Result Failure(string errorMessage) => new(false, errorMessage);
}
