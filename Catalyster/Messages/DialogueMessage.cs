using TinyMessenger;

namespace Catalyster.Messages
{
    public class DialogueMessage : TinyMessageBase
    {
        public string Source;
        public string Content;
        public DialogueMessage(object sender, string source, string content): base(sender)
        {
            Source = source;
            Content = content;
        }
    }
}
