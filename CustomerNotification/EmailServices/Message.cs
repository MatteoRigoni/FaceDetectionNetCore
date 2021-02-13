using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmailServices
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<byte[]> Attachments { get; set; }
        public Message(IEnumerable<string> to, string subject, string content, List<byte[]> attachents)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x)));

            Subject = subject;
            Content = content;
            Attachments = attachents;
        }
    }
}
