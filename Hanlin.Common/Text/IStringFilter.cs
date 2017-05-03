namespace Hanlin.Common.Text
{
    public interface IStringFilter<in T> where T : StringFilterOptions
    {
        string Filter(string input);
        string Filter(string input, T options);
    }
}