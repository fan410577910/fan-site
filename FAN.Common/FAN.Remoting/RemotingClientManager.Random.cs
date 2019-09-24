
using System;
using System.Threading;

namespace FAN.Remoting
{
    partial class RemotingClientManager
    {
        private static int __staticIncrement = 0;

        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="maxValue">最大值</param>
        /// <returns></returns>
        private static int GetRandom(int maxValue)
        {
            #region 原来的
            //Thread.Sleep(1);
            //long tick = DateTime.Now.Ticks;
            //Random r = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            //return r.Next(maxValue);
            #endregion
            long timestamp = DateTime.UtcNow.Ticks & 0x00ffffff; // only use low order 3 bytes
            if (__staticIncrement == int.MaxValue)
                __staticIncrement = 0;
            int increment = Interlocked.Increment(ref __staticIncrement) & 0x00ffffff; // only use low order 3 bytes
            return Convert.ToInt32(timestamp + increment) % maxValue;
        }
    }
}
