using WebApp.Common.Utilities;

namespace UnitTests;

public class KeywordFilterUtilityTests
{
    [Fact]
    public void SplitKeywords_should_return_empty_for_null_or_whitespace()
    {
        Assert.Empty(KeywordFilterUtility.SplitKeywords(null));
        Assert.Empty(KeywordFilterUtility.SplitKeywords("   "));
    }

    [Fact]
    public void SplitKeywords_should_trim_and_split_on_spaces()
    {
        var keywords = KeywordFilterUtility.SplitKeywords("  food   salary ").ToList();

        Assert.Equal(2, keywords.Count);
        Assert.Equal("food", keywords[0]);
        Assert.Equal("salary", keywords[1]);
    }

    [Fact]
    public void MatchesAnyKeyword_should_return_true_when_no_keywords()
    {
        var result = KeywordFilterUtility.MatchesAnyKeyword(["Groceries"], []);

        Assert.True(result);
    }

    [Fact]
    public void MatchesAnyKeyword_should_match_case_insensitive()
    {
        var result = KeywordFilterUtility.MatchesAnyKeyword(
            ["Monthly Salary", "1000"],
            ["salary"]);

        Assert.True(result);
    }

    [Fact]
    public void MatchesAnyKeyword_should_return_false_when_no_match()
    {
        var result = KeywordFilterUtility.MatchesAnyKeyword(
            ["Groceries", "Food"],
            ["transport"]);

        Assert.False(result);
    }
}
