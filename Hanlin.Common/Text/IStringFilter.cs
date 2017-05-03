namespace Hanlin.Common.Text
{
    public interface IStringFilter
    {
        string Filter(string input);
        string Filter<T>(string input, T options) where T : StringFilterOptions;
    }
}