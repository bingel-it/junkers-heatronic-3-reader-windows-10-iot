using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatronicUwpLib.Dto
{
    public class WarmWaterDTO : HeatronicDTO
    {
        public Int32 SollTemperatur { get; set; }

        public Double IstTemperatur { get; set; }

        public Double SpeicherOben { get; set; }

        /// <summary>
        /// Zähler Betriebsminuten Warmwasser-Erzeugung (Brenner an - Warmwasser)
        /// </summary>
        public TimeSpan Betriebszeit { get; set; }

        /// <summary>
        /// Zähler Brennerstarts für Warmwassererzeugung
        /// </summary>
        public TimeSpan BrennerstartFuerWarmwasser { get; set; }

        public override string ToString()
        {
            return "Soll Temperatur = " + this.SollTemperatur +
                " Ist Temperatur = " + this.IstTemperatur +
                " Speicher Oben = " + this.SpeicherOben +
                " Betriebszeit = " + this.Betriebszeit +
                " Brennerstart Fuer Warmwasser = " + this.BrennerstartFuerWarmwasser;
        }
    }
}
