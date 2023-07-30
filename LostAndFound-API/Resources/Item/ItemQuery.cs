namespace LostAndFound_API.Resources.Item
{
    public class ItemQuery
    {
        public string Search { get; set; } = string.Empty;
        public SortBy SortBy { get; set; }
    }

    public enum SortBy
    {
        MostRecentLostItem = 0,
        OldLostItem = 1
    }
}
