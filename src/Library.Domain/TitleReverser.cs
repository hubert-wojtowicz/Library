using System.Text.RegularExpressions;

namespace Library.Domain;

public class TitleReverser : ITitleReverser
{
    private readonly IEnumerable<char> _separators;

    public TitleReverser()
    {
        _separators = new[] { ',', ' ', ';', '.', '-' };
    }

    public string ReverseTitleLoosingSeparators(string toReverse)
    {
        return string.Join(' ', toReverse.Split(_separators.ToArray()).Reverse());
    }

    public string ReverseTitle(string toReverse)
    {
        string pattern = $"([{string.Join("", _separators.Select(c => Regex.Escape(c.ToString())))}])";

        string[] wordsAndSeparators = Regex.Split(toReverse, pattern);

        Array.Reverse(wordsAndSeparators);

        return string.Join("", wordsAndSeparators);
    }
}

public interface ITitleReverser
{
    string ReverseTitleLoosingSeparators(string toReverse);

    string ReverseTitle(string toReverse);
}