using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.Tasks.Rto
{

    [DataContract]
    class WarmwasserMessageRto : BaseMessageRto
    {
        [DataMember]
        public Int32 SollTemperatur { get; set; }

        [DataMember]
        public Double IstTemperatur { get; set; }

    }
}
