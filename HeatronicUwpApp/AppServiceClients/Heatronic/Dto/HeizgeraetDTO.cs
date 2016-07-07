using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic.Dto
{
    [DataContract]
    public class HeizgeraetDTO : HeatronicDTO
    {
        /// <summary>
        /// Vorlauf Soll-Temperatur.Bei Warmwasser max. Kesseltemperatur
        /// </summary>
        [DataMember]
        public Int32 SollTemperatur { get; set; }

        /// <summary>
        /// Vorlauf Ist-Temperatur
        /// </summary>
        [DataMember]
        public Double IstTemperatur { get; set; }

        /// <summary>
        /// Brennerleistung in %
        /// </summary>
        [DataMember]
        public byte BrennerLeistung { get; set; }

        /// <summary>
        /// Temperatur Kessel-Mischer
        /// </summary>
        [DataMember]
        public Double TemperaturKesselMischer { get; set; }

        /// <summary>
        /// Temperatur Kessel-Rücklauf
        /// </summary>
        [DataMember]
        public Double TemperaturKesselRuecklauf { get; set; }

        [DataMember]
        public byte KesselMaximaleLeistung { get; internal set; }

        [DataMember]
        public bool HeizungsMode { get; internal set; }

        [DataMember]
        public bool WarmwasserMode { get; internal set; }

        [DataMember]
        public bool BetriebsflammeAn { get; internal set; }
        /// <summary>
        /// Brenner an mit Vor-Nachlauf
        /// </summary>
        [DataMember]
        public bool BrennerAnMitVorlauf { get; internal set; }

        /// <summary>
        /// Zuendung des Brenner
        /// </summary>
        [DataMember]
        public bool ZuendungDesBrenners { get; internal set; }

        /// <summary>
        /// Heizungspumpe an/aus
        /// </summary>
        [DataMember]
        public bool HeizungsPumpeAn { get; internal set; }

        /// <summary>
        /// Speicherladepumpe an/aus
        /// </summary>
        [DataMember]
        public bool SpeicherladePumpeAn { get; internal set; }

        /// <summary>
        /// Zirkulationspumpe an/aus
        /// </summary>
        [DataMember]
        public bool ZirkulationsPumpeAn { get; internal set; }

        public override string ToString()
        {
            return "Soll Temperatur = " + this.SollTemperatur +
                " Ist Temperatur = " + this.IstTemperatur +
                " Brenner Leistung = " + this.BrennerLeistung + "%" +
                " KesselMaximaleLeistung = " + this.KesselMaximaleLeistung + "%" +
                " HeizungsMode = " + this.HeizungsMode +
                " WarmwasserMode = " + this.WarmwasserMode +
                " BetriebsflammeAn = " + this.BetriebsflammeAn +
                " Temperatur Kessel Mischer = " + this.TemperaturKesselMischer + "°C " +
                " Temperatur Kessel Mischer = " + this.TemperaturKesselMischer + "°C";
        }
    }
}
