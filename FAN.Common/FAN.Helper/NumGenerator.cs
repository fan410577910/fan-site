#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2017 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-JLFFS1KMVG 
     * 文件名：  NumGenerator 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间： 2017/10/27 15:23:48 
     * 描述    :
     * =====================================================================
     * 修改时间：2017/10/27 15:23:48 
     * 修改人  ： wangyunpeng
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;

namespace FAN.Helper
{
    /// <summary>
    ///  唯一编码生成类
    ///  符号位(1位) + Timestamp(41位) + WorkId(10位) + sequence(12位)  = 编号Id (64位)
    ///  https://github.com/KevinWG/OSS.Common/blob/master/OSS.Common/ComUtils/NumGenerator.cs
    /// </summary>
    public class NumGenerator
    {
        /// <summary>
        /// 【sequence 部分】  随机序列  12位
        /// </summary>
        static long sequence = 0L;
        const long MAX_SEQUENCE = -1L ^ (-1L << SEQUENCE_BIT_LENGTH);
        const int SEQUENCE_BIT_LENGTH = 12;

        /// <summary>
        /// 【WorkId部分】 工作Id 10位
        /// </summary>
        const long MAX_WORKER_ID = -1L ^ (-1L << WORKER_ID_BIT_LENGTH);
        const int WORKER_LEFT_SHIFT = SEQUENCE_BIT_LENGTH;
        const int WORKER_ID_BIT_LENGTH = 10;

        /// <summary>
        ///  当前的工作id 最大值不能超过（2的11次方 - 1）
        /// </summary>
        public long WorkId { get; }

        /// <summary>
        /// 【Timestamp部分】
        /// </summary>
        const int TIMESTAMP_LEFT_SHIFT = SEQUENCE_BIT_LENGTH + WORKER_ID_BIT_LENGTH;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="workId">当前的工作id 最大值不能超过（2的11次方 - 1）</param>
        public NumGenerator(long workId)
        {
            if (workId > MAX_WORKER_ID || workId < 0)
            {
                throw new ArgumentException("workId", $"worker Id can't be greater than {workId} or less than 0");
            }
            WorkId = workId;
        }

        private long lastTimestamp = 0;
        public long NextNum()
        {
            var timestamp = GetTimestamp();
            if (timestamp < lastTimestamp)
            {
                //如果当前时间小于上一次ID生成的时间戳，说明系统时钟回退过这个时候应当抛出异常
                throw new ArgumentException($"Clock moved backwards.  Refusing to generate id for {lastTimestamp - timestamp} milliseconds");
            }

            // 如果是同一时间生成的，则进行毫秒内序列
            if (lastTimestamp == timestamp)
            {
                sequence = (sequence + 1) & MAX_SEQUENCE;

                //毫秒内序列溢出
                //阻塞到下一个毫秒,获得新的时间戳
                if (sequence == 0)
                    timestamp = WaitNextMillis();
            }
            //时间戳改变，毫秒内序列重置
            else
                sequence = 0L;

            //上次生成ID的时间截
            lastTimestamp = timestamp;
            return (timestamp << TIMESTAMP_LEFT_SHIFT)
                   | (WorkId << WORKER_LEFT_SHIFT)
                   | sequence;
        }

        /// <summary>
        ///  当前毫秒内序列使用完，等待下一毫秒
        /// </summary>
        /// <returns></returns>
        protected long WaitNextMillis()
        {
            long timeTicks;
            do
            {
                timeTicks = GetTimestamp();
            }
            while (timeTicks <= lastTimestamp);
            return timeTicks;
        }

        private static readonly long timeStartTicks = new DateTime(2015, 1, 1).Ticks;

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private static long GetTimestamp()
        {
            return (DateTime.UtcNow.Ticks - timeStartTicks) / 10000;
        }
    }
    /// <summary>
    ///  唯一数字编码生成静态通用类
    /// </summary>
    public class NumUtil
    {
        private static readonly NumGenerator _Generator = new NumGenerator(0);
        /// <summary>
        /// 单机生成唯一数字编号
        /// </summary>
        /// <returns></returns>
        public static long UniNum()
        {
            return _Generator.NextNum();
        }
    }
}
