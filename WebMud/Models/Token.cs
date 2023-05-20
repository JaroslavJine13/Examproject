namespace WebMud.Models
{
    public class Tokens
    {
        public Guid Id { get; set; }

        public string TokenValue { get; set; }    

        public DateTime CreatedDate { get; set; }
        
        public DateTime ExpiredDate { get; set; }

        public string UserID { get; set; }
    }
}
