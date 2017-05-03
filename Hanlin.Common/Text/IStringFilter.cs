namespace Hanlin.Common.Text
{
    public interface IStringFilter
    {
        string Filter(string input);
        string Filter(string input, StringFilterOptions options);
    }
}