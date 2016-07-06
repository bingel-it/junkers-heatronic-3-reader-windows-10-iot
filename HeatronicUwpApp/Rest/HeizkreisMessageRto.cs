using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.Rest
{
    [DataContract]
    class HeizkreisMessageRto : BaseMessageRto
    {
        [DataMember]
        public double Aussentemperatur { get; internal set; }

    }
}
