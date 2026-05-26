using WebApp.Common.Utilities;

namespace UnitTests;

public class TransactionDateValidatorTests
{
    [Fact]
    public void EnsureNotFuture_should_not_throw_for_today()
    {
        var exception = Record.Exception(() => TransactionDateValidator.EnsureNotFuture(DateTime.Today));

        Assert.Null(exception);
    }

    [Fact]
    public void EnsureNotFuture_should_throw_for_future_dates()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            TransactionDateValidator.EnsureNotFuture(DateTime.Today.AddDays(1)));

        Assert.Equal(TransactionDateValidator.FutureDateMessage, ex.Message);
    }
}
