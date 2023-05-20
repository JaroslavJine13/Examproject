

namespace WebMud.Models
{
    public class UsersNotified: Users
    {
        public int MessageCount { get; set; }

        public string LastMessage { get; set; }

        public DateTime LastDate { get; set; }

      
    }
}
