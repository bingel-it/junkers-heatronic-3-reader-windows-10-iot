using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic.Dto
{
    [DataContract]
    public class WarmWaterDTO : HeatronicDTO
    {
        [DataMember]
        public Int32 SollTemperatur { get; set; }

        [DataMember]
        public Double IstTemperatur { get; set; }

        [DataMember]
        public Double SpeicherOben { get; set; }

        /// <summary>
        /// Zähler Betriebsminuten Warmwasser-Erzeugung (Brenner an - Warmwasser)
        /// </summary>
        [DataMember]
        public TimeSpan Betriebszeit { get; set; }

        /// <summary>
        /// Zähler Brennerstarts für Warmwassererzeugung
        /// </summary>
        [DataMember]
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
