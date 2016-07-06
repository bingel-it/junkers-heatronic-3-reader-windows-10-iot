using HeatronicUwpLib.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatronicUwpLib
{
    class MessageDecoder
    {
        public TimestampDTO DecodeDateTimeMessage(byte[] data)
        {
            var year = (int)(data[4]) + 2000;
            var month = (int)data[5];
            var hour = (int)data[6];
            var day = (int)data[7];
            var minute = (int)data[8];
            var second = (int)data[9];
            var dayOfWeek = (int)data[10];

            return new TimestampDTO()
            {
                SystemTimestamp = new DateTime(year, month, day, hour, minute, second)
            };
        }

        public WarmWaterDTO DecodeWarmwasser(byte[] data)
        {
            var sollTemperatur = (data[4]);
            var istTemperatur = ((data[5]) * 265 + (data[6])) / 10.0;
            var speicherOben = ((data[7]) * 265 + (data[8])) / 10.0;

            var betriebsZeit = (data[14]) * 65536 + (data[15]) * 265 + (data[16]);
            var brennerstart = (data[17]) * 65536 + (data[18]) * 265 + (data[19]);

            var response = new WarmWaterDTO()
            {
                SollTemperatur = sollTemperatur,
                IstTemperatur = istTemperatur,
                SpeicherOben = speicherOben,
                Betriebszeit = new TimeSpan(0, betriebsZeit, 0),
                BrennerstartFuerWarmwasser = new TimeSpan(0, brennerstart, 0),
            };

            return response;
        }

        public HeizkreisDTO DecodeHeizkreis(byte[] data)
        {
            var result = new HeizkreisDTO();

            if (data[4] != 0xff)
            {
                result.Aussentemperatur = (data[4] * 256 + data[5]) / 10.0;
            }
            else
            {
                result.Aussentemperatur = (255 - data[5]) / -10.0;
            }

            result.BrennerStartsTotal = (data[14]) * 65536 + (data[15]) * 265 + (data[16]);
            result.BrennerBetriebTotal = new TimeSpan(0, (data[17]) * 65536 + (data[18]) * 265 + (data[19]), 0);
            result.BrennerBetriebHeizung = new TimeSpan(0, (data[23]) * 65536 + (data[24]) * 265 + (data[25]), 0);
            result.BrennerStartsHeitung = (data[26]) * 65536 + (data[27]) * 265 + (data[28]);
            return result;
        }

        public HeatronicDTO DecodeHeizgeraet(byte[] data)
        {
            var result = new HeizgeraetDTO();

            result.SollTemperatur = (data[4]);
            result.IstTemperatur = ((data[5]) * 265 + (data[6])) / 10.0;

            result.KesselMaximaleLeistung = data[7];

            result.BrennerLeistung = data[8];

            result.HeizungsMode = IsBitSet(data[9], 0);
            result.WarmwasserMode = IsBitSet(data[9], 1);
            result.BetriebsflammeAn = IsBitSet(data[9], 3);

            result.BrennerAnMitVorlauf = IsBitSet(data[11], 0);
            result.BrennerAnMitVorlauf = IsBitSet(data[11], 2);
            result.ZuendungDesBrenners = IsBitSet(data[11], 3);
            result.HeizungsPumpeAn = IsBitSet(data[11], 5);
            result.SpeicherladePumpeAn = IsBitSet(data[11], 6);
            result.ZirkulationsPumpeAn = IsBitSet(data[11], 7);


            result.TemperaturKesselMischer = ((data[13]) * 265 + (data[14])) / 10.0;
            result.TemperaturKesselRuecklauf = ((data[17]) * 265 + (data[18])) / 10.0;


            return result;
        }

        public HeizkreisSteuerungDTO DecodeHeizkreisSteuerung(byte[] data)
        {
            var result = new HeizkreisSteuerungDTO();

            //result.Heizreis = data[5] - 0x6F;
            //result.Betriebsart = data[6]

            return result;
        }


        public byte[] BuildMessage(byte v1, byte v2, byte v3, byte[] data)
        {
            var message = new byte[data.Length + 3];
            message[0] = v1;
            message[1] = v2;
            message[2] = v3;
            Array.Copy(data, 0, message, 3, data.Length);
            return message;
        }

        /// <summary>
        /// pos 0 is least significant bit, pos 7 is most.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

    }
}

