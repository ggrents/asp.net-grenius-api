namespace MessageContracts
{
    public class RatingMessage
    {
        public int EntityId { get; set; }
        public int UserId { get; set; }
        public TypeEnum Type { get; set; }
        public DateTime DateTime { get; set; }
    }
}
