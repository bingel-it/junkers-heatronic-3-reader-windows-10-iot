using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatronicUwpLib.Dto
{
    public class HeizgeraetDTO : HeatronicDTO
    {
        /// <summary>
        /// Vorlauf Soll-Temperatur.Bei Warmwasser max. Kesseltemperatur
        /// </summary>
        public Int32 SollTemperatur { get; set; }

        /// <summary>
        /// Vorlauf Ist-Temperatur
        /// </summary>
        public Double IstTemperatur { get; set; }

        /// <summary>
        /// Brennerleistung in %
        /// </summary>
        public byte BrennerLeistung { get; set; }

        /// <summary>
        /// Temperatur Kessel-Mischer
        /// </summary>
        public Double TemperaturKesselMischer { get; set; }

        /// <summary>
        /// Temperatur Kessel-Rücklauf
        /// </summary>
        public Double TemperaturKesselRuecklauf { get; set; }
        public byte KesselMaximaleLeistung { get; internal set; }
        public bool HeizungsMode { get; internal set; }
        public bool WarmwasserMode { get; internal set; }
        public bool BetriebsflammeAn { get; internal set; }
        /// <summary>
        /// Brenner an mit Vor-Nachlauf
        /// </summary>
        public bool BrennerAnMitVorlauf { get; internal set; }

        /// <summary>
        /// Zuendung des Brenner
        /// </summary>
        public bool ZuendungDesBrenners { get; internal set; }

        /// <summary>
        /// Heizungspumpe an/aus
        /// </summary>
        public bool HeizungsPumpeAn { get; internal set; }

        /// <summary>
        /// Speicherladepumpe an/aus
        /// </summary>
        public bool SpeicherladePumpeAn { get; internal set; }

        /// <summary>
        /// Zirkulationspumpe an/aus
        /// </summary>
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
