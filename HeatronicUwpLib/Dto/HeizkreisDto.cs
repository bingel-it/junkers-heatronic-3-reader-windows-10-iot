using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatronicUwpLib.Dto
{
    public class HeizkreisDTO : HeatronicDTO
    {

        public Double Aussentemperatur { get; set; }

        /// <summary>
        /// Brennerstarts Total (für Warmwasser und Heizung)
        /// </summary>
        public int BrennerStartsTotal { get; set; }

        /// <summary>
        /// Betriebsminuten Brenner Total (für Warmwasser und Heizung)
        /// </summary>
        public TimeSpan BrennerBetriebTotal { get; set; }

        /// <summary>
        /// Betriebsminuten Brenner (nur Heizung)
        /// </summary>
        public TimeSpan BrennerBetriebHeizung { get; set; }

        /// <summary>
        /// Brennerstarts (nur Heizung)
        /// </summary>
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
