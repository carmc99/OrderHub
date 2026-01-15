namespace OrderHub.Core.Order.Models
{
    public class OrderHistoryItemModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
    }
}
