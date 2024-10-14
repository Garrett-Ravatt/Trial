using TinyMessenger;

namespace Catalyster.Messages
{
    public class DialogueMessage : TinyMessageBase
    {
        // NOTE: Is Source an Entity? When would we use this
        public string Source;
        public string Content;
        public DialogueMessage(object sender, string source, string content): base(sender)
        {
            Source = source;
            Content = content;
        }
    }
}
