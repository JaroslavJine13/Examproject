

namespace WebMud.Models
{
    public class Message
    {
        //public Message(string username, string body, bool mine, string fromUserId, string toUserId, bool isaligment, DateTime createddate)
        //{
        //    Username = username;
        //    Body = body;
        //    Mine = mine;
        //    FromUserId = fromUserId;
        //    ToUserId = toUserId;
        //    IsAligment = isaligment;
        //    CreatedDate = createddate;
        //}

        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Body { get; set; }

        public bool Mine { get; set; }

        public bool IsAligment { get; set; }

        public bool IsNotice { get; set; }

        public string CSS => Mine ? "mud-theme-primary py-4 px-6 mx-4 rounded-xl" : "mud-theme-secondary py-4 px-6 mx-4 rounded-xl";

        public string Aligment => IsAligment ? "display: flex; justify-content: flex-end" : "display: flex; justify-content: flex-start";

        public DateTime CreatedDate { get; set; }

        public string FromUserId { get; set; }

        public string ToUserId { get; set; }


    }
}
