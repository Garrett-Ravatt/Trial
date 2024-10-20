using Arch.Core;
using Catalyster.Core;
using Catalyster.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace Catalyster.Messages
{
    public delegate void Decide(bool b);

    public class ConfirmationMessage : TinyMessageBase
    {
        public string Message;
        public Decide D;
        public ConfirmationMessage(object sender, string message, Decide d) : base(sender)
        {
            Message = message;
            D = d;
        }
    }
}
