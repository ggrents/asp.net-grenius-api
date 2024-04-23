namespace MessageContracts
{
    public class RatingMessage
    {
        public TypeEnum Type { get; set; }
        public DateTime DateTime { get; set; }
        public int UserId { get; set; }
    }
}
