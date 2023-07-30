namespace LostAndFound_API.Resources.Item
{
    public class CloseLostItemResource
    {
        public long UserId { get; set; }
        public int ItemId { get; set; }
        public string ClosingRemarks { get; set; } = string.Empty;
    }
}
