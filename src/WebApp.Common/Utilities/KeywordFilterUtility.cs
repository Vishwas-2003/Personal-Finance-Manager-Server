namespace WebApp.Common.Utilities;

public static class KeywordFilterUtility
{
    public static IEnumerable<string> SplitKeywords(string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return [];
        }

        return searchText.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    public static bool MatchesAnyKeyword(IEnumerable<string> searchableValues, IEnumerable<string> keywords)
    {
        var keywordList = keywords.ToList();
        if (keywordList.Count == 0)
        {
            return true;
        }

        return keywordList.Any(keyword =>
            searchableValues.Any(value =>
                value.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
    }
}
