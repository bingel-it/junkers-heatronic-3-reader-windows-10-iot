using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.Tasks.Rto
{
    [DataContract]
    class BaseMessageRto
    {
        [DataMember]
        public DateTime Timestamp { get; set; }

        public MessageTypeEnum MessageType { get; set; }

        [DataMember]
        public String MessageTypeString
        {
            get { return MessageType.ToString(); }
            set { MessageType = (MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), value); }
        }

    }
}
