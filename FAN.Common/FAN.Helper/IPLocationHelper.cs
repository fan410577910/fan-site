using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Helper
{
    /// <summary>
    ///通过IP获取国家信息
    /// </summary>
    public class IPLocationHelper
    {
        private static readonly byte[] _DataBuffers;
        private static readonly byte[] _IndexBuffers;
        private static readonly uint[] _Indexs = new uint[256];
        private static readonly int _Offset;
        static IPLocationHelper()
        {
            try
            {
                _DataBuffers = File.ReadAllBytes(System.AppDomain.CurrentDomain.BaseDirectory + "17monipdb.dat");
                _Offset = (int)BytesToLong(_DataBuffers[0], _DataBuffers[1], _DataBuffers[2], _DataBuffers[3]);
                _IndexBuffers = new byte[_Offset];
                Array.Copy(_DataBuffers, 4, _IndexBuffers, 0, _Offset);

                for (int loop = 0; loop < 256; loop++)
                {
                    _Indexs[loop] = BytesToLong(_IndexBuffers[loop * 4 + 3], _IndexBuffers[loop * 4 + 2], _IndexBuffers[loop * 4 + 1], _IndexBuffers[loop * 4]);
                }
            }
            catch { }
        }

        public IPLocationHelper()
        {
        }

        private static uint BytesToLong(byte a, byte b, byte c, byte d)
        {
            return ((uint)a << 24) | ((uint)b << 16) | ((uint)c << 8) | (uint)d;
        }
        /// <summary>
        /// 循环查找
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static string[] Loop(string ipAddress)
        {
            string[] result = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(ipAddress))
                {
                    string[] ipAddressArray = ipAddress.SplitToArray<string>('.');
                    if (ipAddressArray.Length >= 4)
                    {
                        int ipPrefixValue = int.Parse(ipAddressArray[0]);
                        long ip2LongValue = BytesToLong(byte.Parse(ipAddressArray[0]), byte.Parse(ipAddressArray[1]), byte.Parse(ipAddressArray[2]), byte.Parse(ipAddressArray[3]));
                        uint start = _Indexs[ipPrefixValue];
                        int maxCompareLength = _Offset - 1028;
                        long indexOffset = -1L;
                        int indexLength = -1;
                        byte @byte = 0;
                        for (start = start * 8 + 1024; start < maxCompareLength; start += 8)
                        {
                            if (BytesToLong(_IndexBuffers[start + 0], _IndexBuffers[start + 1], _IndexBuffers[start + 2], _IndexBuffers[start + 3]) >= ip2LongValue)
                            {
                                indexOffset = BytesToLong(@byte, _IndexBuffers[start + 6], _IndexBuffers[start + 5], _IndexBuffers[start + 4]);
                                indexLength = 0xFF & _IndexBuffers[start + 7];
                                break;
                            }
                        }
                        byte[] areaBytes = new byte[indexLength];
                        Array.Copy(_DataBuffers, _Offset + (int)indexOffset - 1024, areaBytes, 0, indexLength);
                        result = Encoding.UTF8.GetString(areaBytes).Split('\t');
                        Array.Clear(areaBytes, 0, areaBytes.Length);
                        areaBytes = null;
                        Array.Clear(ipAddressArray, 0, ipAddressArray.Length);
                    }
                    ipAddressArray = null;
                }

                if (result == null)
                {
                    result = new string[0];
                }
            }
            catch { }

            return result;
        }
        /// <summary>
        /// 根据IP地址查找国家
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public string[] Find(string ipAddress)
        {
            return Loop(ipAddress);
        }
    }
}
