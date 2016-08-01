using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.Tasks.Rto
{
    [DataContract]
    class HeizgeraetMessageRto : BaseMessageRto
    {
        [DataMember]
        public bool BetriebsflammeAn { get; internal set; }
        [DataMember]
        public byte BrennerLeistung { get; internal set; }
        [DataMember]
        public bool HeizungsMode { get; internal set; }
        [DataMember]
        public bool HeizungsPumpeAn { get; internal set; }
        [DataMember]
        public bool ZirkulationsPumpeAn { get; internal set; }
    }
}
