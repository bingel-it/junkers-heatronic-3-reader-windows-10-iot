using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HeatronicUwpLib.Dto
{
    [DataContract]
    public class TimestampDTO : HeatronicDTO
    {
        [DataMember]
        public DateTime SystemTimestamp { get; set; }
    }
}
