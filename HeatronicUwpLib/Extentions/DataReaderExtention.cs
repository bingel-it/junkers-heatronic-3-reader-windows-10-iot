using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace HeatronicUwpLib.Extentions
{
    static class DataReaderExtention
    {
        public static async Task<byte> ReadByteAsync(this DataReader dataReader)
        {
            var task = dataReader.LoadAsync(1);
            var bytesRead = await task;
            return dataReader.ReadByte();
        }

        public static async Task<byte[]> ReadBytesAsync(this DataReader dataReader, uint length)
        {
            var task = dataReader.LoadAsync(length);
            var bytesRead = await task;
            var result = new byte[length];
            dataReader.ReadBytes(result);
            return result;
        }


        /*
                public static byte ReadByteAsync(this DataReader dataReader)
        {
            var task = Task<byte>.Run(async () => {
                await dataReader.LoadAsync(1);
                return dataReader.ReadByte();
            });
            return task.Result;
        }

        public static byte[] ReadBytesAsync(this DataReader dataReader, uint length)
        {
            var task = Task<byte[]>.Run(async () => {
                await dataReader.LoadAsync(length);
                var result = new byte[length];
                dataReader.ReadBytes(result);
                return result;
            });
            return task.Result;
        }

        */
    }
}
