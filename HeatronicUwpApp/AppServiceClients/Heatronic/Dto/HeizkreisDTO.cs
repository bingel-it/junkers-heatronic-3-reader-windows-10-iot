using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic.Dto
{
    [DataContract]
    public class HeizkreisDTO : HeatronicDTO
    {

        [DataMember]
        public Double Aussentemperatur { get; set; }

        /// <summary>
        /// Brennerstarts Total (für Warmwasser und Heizung)
        /// </summary>
        [DataMember]
        public int BrennerStartsTotal { get; set; }

        /// <summary>
        /// Betriebsminuten Brenner Total (für Warmwasser und Heizung)
        /// </summary>
        [DataMember]
        public TimeSpan BrennerBetriebTotal { get; set; }

        /// <summary>
        /// Betriebsminuten Brenner (nur Heizung)
        /// </summary>
        [DataMember]
        public TimeSpan BrennerBetriebHeizung { get; set; }

        /// <summary>
        /// Brennerstarts (nur Heizung)
        /// </summary>
        [DataMember]
        public Int32 BrennerStartsHeitung { get; set; }

        public override string ToString()
        {
            return "Aussentemperatur = " + this.Aussentemperatur + " °C" +
                " BrennerStartsTotal = " + this.BrennerStartsTotal +
                " BrennerBetriebTotal = " + this.BrennerBetriebTotal +
                " BrennerBetriebHeizung = " + this.BrennerBetriebHeizung +
                " BrennerStartsHeitung = " + this.BrennerStartsHeitung;
        }
    }
}
