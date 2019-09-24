using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLZ.LuceneNet
{
    [Serializable]
    public class CustomWeightInfo
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public float Weight { get; set; }
    }
}
