namespace WebApp.Common.Utilities;

public static class TransactionDateValidator
{
    public const string FutureDateMessage = "Date cannot be in the future.";

    public static void EnsureNotFuture(DateTime date)
    {
        if (date.Date > DateTime.Today)
        {
            throw new ArgumentException(FutureDateMessage);
        }
    }
}
