using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingShared.Commands
{
    public interface IRegisterOrderCommand
    {
        public string PictureUrl { get; set; }
        public string Email { get; set; }
        public byte[] ImageData { get; set; }
        public Guid OrderId { get; set; }
    }
}
