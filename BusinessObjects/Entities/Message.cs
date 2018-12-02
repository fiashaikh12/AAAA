using System;

namespace Entities
{
    public class Messages
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public string IP { get; set; }
        public bool IsRead { get; set; }
        public DateTime ? MessageDate { get; set; }
        public string MessageContent { get; set; }
        public int MsgRecipeint { get; set; }
    }
}
