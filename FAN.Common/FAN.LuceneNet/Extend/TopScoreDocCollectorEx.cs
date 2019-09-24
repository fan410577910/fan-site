#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2015 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  TopScoreDocCollectorEx 
     * 版本号：  V1.0.0.0 
     * 创建人：  wangyunpeng 
     * 创建时间：2015/5/2 11:32:08 
     * 描述    :
     * =====================================================================
     * 修改时间：2015/5/2 11:32:08 
     * 修改人  ：Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucene.Net.Search
{

    /// <summary>
    /// http://search-lucene.com/c/Lucene:core/src/java/org/apache/lucene/search/TopScoreDocCollector.java||TopScoreDocCollector
    /// http://search-lucene.com/c/Lucene:core/src/java/org/apache/lucene/search/IndexSearcher.java||IndexSearcher
    /// http://search-lucene.com/c/Lucene:core/src/java/org/apache/lucene/search/TopDocsCollector.java||TopDocsCollector
    /// 重写TopScoreDocCollector类，Create方法增加ScoreDoc类型的参数对象
    /// wyp增加
    /// </summary>
    public abstract class TopScoreDocCollectorEx : TopDocsCollector<ScoreDoc>
    {
        #region Inner Class
        /// <summary>
        /// Assumes docs are scored in order.
        /// </summary>
        private class InOrderTopScoreDocCollector : TopScoreDocCollectorEx
        {
            internal InOrderTopScoreDocCollector(int numHits)
                : base(numHits)
            {
            }

            public override void Collect(int doc)
            {
                float score = base.Scorer.Score();

                // This collector cannot handle these scores:
                System.Diagnostics.Debug.Assert(score != float.NegativeInfinity);
                System.Diagnostics.Debug.Assert(!float.IsNaN(score));

                base.internalTotalHits++;
                if (score <= base.PqTop.Score)
                {
                    // Since docs are returned in-order (i.e., increasing doc Id), a document
                    // with equal score to pqTop.score cannot compete since HitQueue favors
                    // documents with lower doc Ids. Therefore reject those docs too.
                    return;
                }
                base.PqTop.Doc = doc + base.DocBase;
                base.PqTop.Score = score;
                base.PqTop = base.pq.UpdateTop();
            }

            public override bool AcceptsDocsOutOfOrder
            {
                get { return false; }
            }
        }

        /// <summary>
        /// Assumes docs are scored in order.
        /// wyp增加
        /// </summary>
        private class InOrderPagingScoreDocCollector : TopScoreDocCollectorEx
        {
            private readonly ScoreDoc _after;
            private int _afterDoc;
            //private int _collectedHits;//wyp 去掉java代码
            internal InOrderPagingScoreDocCollector(ScoreDoc after, int numHits)
                : base(numHits)
            {
                this._after = after;
            }

            public override bool AcceptsDocsOutOfOrder
            {
                get { return false; }
            }

            public override void Collect(int doc)
            {
                float score = base.Scorer.Score();

                // This collector cannot handle these scores:                
                System.Diagnostics.Debug.Assert(score != float.NegativeInfinity);
                System.Diagnostics.Debug.Assert(!float.IsNaN(score));

                base.internalTotalHits++;
                if (score > this._after.Score || (score == this._after.Score && doc <= this._afterDoc))
                {
                    // hit was collected on a previous page
                    return;
                }
                if (score <= base.PqTop.Score)
                {
                    // Since docs are returned in-order (i.e., increasing doc Id), a document
                    // with equal score to pqTop.score cannot compete since HitQueue favors
                    // documents with lower doc Ids. Therefore reject those docs too.
                    return;
                }
                //this._collectedHits++;//wyp 去掉java代码
                base.PqTop.Doc = doc + base.DocBase;
                base.PqTop.Score = score;
                base.PqTop = base.pq.UpdateTop();
            }

            public override void SetNextReader(Index.IndexReader reader, int base_Renamed)
            {
                base.SetNextReader(reader, base_Renamed);
                this._afterDoc = this._after.Doc - base_Renamed;
            }

            public override TopDocs NewTopDocs(ScoreDoc[] results, int start)
            {
                return results == null ? new TopDocs(this.TotalHits, new ScoreDoc[0], System.Single.NaN) : new TopDocs(this.TotalHits, results, System.Single.NaN);
            }
        }

        /// <summary>
        /// Assumes docs are scored out of order.
        /// </summary>
        private class OutOfOrderTopScoreDocCollector : TopScoreDocCollectorEx
        {
            internal OutOfOrderTopScoreDocCollector(int numHits)
                : base(numHits)
            {
            }

            public override void Collect(int doc)
            {
                float score = base.Scorer.Score();

                // This collector cannot handle NaN
                System.Diagnostics.Debug.Assert(!float.IsNaN(score));

                base.internalTotalHits++;
                doc += base.DocBase;
                if (score < base.PqTop.Score || (score == base.PqTop.Score && doc > base.PqTop.Doc))
                {
                    return;
                }
                base.PqTop.Doc = doc;
                base.PqTop.Score = score;
                base.PqTop = base.pq.UpdateTop();
            }

            public override bool AcceptsDocsOutOfOrder
            {
                get { return true; }
            }
        }

        /// <summary>
        /// Assumes docs are scored out of order.
        /// wyp增加
        /// </summary>
        private class OutOfOrderPagingScoreDocCollector : TopScoreDocCollectorEx
        {
            private readonly ScoreDoc _after;
            private int _afterDoc;
            //private int _collectedHits;//wyp 去掉java代码

            internal OutOfOrderPagingScoreDocCollector(ScoreDoc after, int numHits)
                : base(numHits)
            {
                this._after = after;
            }

            public override bool AcceptsDocsOutOfOrder
            {
                get { return true; }
            }

            public override void Collect(int doc)
            {
                float score = base.Scorer.Score();

                // This collector cannot handle NaN
                System.Diagnostics.Debug.Assert(!float.IsNaN(score));

                base.internalTotalHits++;
                if (score > this._after.Score || (score == this._after.Score && doc <= this._afterDoc))
                {
                    // hit was collected on a previous page
                    return;
                }
                if (score < base.PqTop.Score)
                {
                    // Doesn't compete w/ bottom entry in queue
                    return;
                }
                doc += base.DocBase;
                if (score == base.PqTop.Score && doc > base.PqTop.Doc)
                {
                    // Break tie in score by doc ID:
                    return;
                }
                //this._collectedHits++;//wyp 去掉java代码
                base.PqTop.Doc = doc;
                base.PqTop.Score = score;
                base.PqTop = base.pq.UpdateTop();
            }

            public override void SetNextReader(IndexReader reader, int base_Renamed)
            {
                base.SetNextReader(reader, base_Renamed);
                this._afterDoc = this._after.Doc - base_Renamed;
            }
            public override TopDocs NewTopDocs(ScoreDoc[] results, int start)
            {
                return results == null ? new TopDocs(this.TotalHits, new ScoreDoc[0], System.Single.NaN) : new TopDocs(this.TotalHits, results, System.Single.NaN);
            }
        }
        #endregion

        /// <summary>
        /// Creates a new <see cref="TopScoreDocCollectorEx" /> given the number of hits to
        /// collect and whether documents are scored in order by the input
        /// <see cref="Scorer" /> to <see cref="SetScorer(Scorer)" />.
        /// <p/><b>NOTE</b>: The instances returned by this method
        /// pre-allocate a full array of length
        /// <c>numHits</c>, and fill the array with sentinel
        /// objects.
        /// </summary>
        public static TopScoreDocCollectorEx Create(int numHits, bool docsScoredInOrder)
        {
            return Create(numHits, null, docsScoredInOrder);
        }

        /// <summary>
        /// Creates a new <see cref="TopScoreDocCollectorEx" /> given the number of hits to
        /// collect and whether documents are scored in order by the input
        /// <see cref="Scorer" /> to <see cref="SetScorer(Scorer)" />.
        /// <p/><b>NOTE</b>: The instances returned by this method
        /// pre-allocate a full array of length
        /// <c>numHits</c>, and fill the array with sentinel
        /// objects.
        /// wyp增加
        /// </summary>
        /// <param name="numHits"></param>
        /// <param name="after"></param>
        /// <param name="docsScoredInOrder"></param>
        /// <returns></returns>
        public static TopScoreDocCollectorEx Create(int numHits, ScoreDoc after, bool docsScoredInOrder)
        {
            if (numHits <= 0)
            {
                throw new ArgumentException("numHits must be > 0; please use TotalHitCountCollector if you just need the total hit count");
            }
            if (docsScoredInOrder)
            {
                if (after == null)
                {
                    return new InOrderTopScoreDocCollector(numHits);
                }
                else
                {
                    return new InOrderPagingScoreDocCollector(after, numHits);
                }
            }
            else
            {
                if (after == null)
                {
                    return new OutOfOrderTopScoreDocCollector(numHits);
                }
                else
                {
                    return new OutOfOrderPagingScoreDocCollector(after, numHits);
                }
            }
        }

        internal ScoreDoc PqTop;
        internal int DocBase = 0;
        internal Scorer Scorer;

        // prevents instantiation
        private TopScoreDocCollectorEx(int numHits)
            : base(new HitQueue(numHits, true))
        {
            // HitQueue implements getSentinelObject to return a ScoreDoc, so we know
            // that at this point top() is already initialized.
            this.PqTop = base.pq.Top();
        }

        public /*protected internal*/ override TopDocs NewTopDocs(ScoreDoc[] results, int start)
        {
            if (results == null)
            {
                return EMPTY_TOPDOCS;
            }

            // We need to compute maxScore in order to set it in TopDocs. If start == 0,
            // it means the largest element is already in results, use its score as
            // maxScore. Otherwise pop everything else, until the largest element is
            // extracted and use its score as maxScore.
            float maxScore = System.Single.NaN;
            if (start == 0)
            {
                maxScore = results[0].Score;
            }
            else
            {
                for (int i = base.pq.Size(); i > 1; i--)
                {
                    base.pq.Pop();
                }
                maxScore = base.pq.Pop().Score;
            }

            return new TopDocs(internalTotalHits, results, maxScore);
        }

        public override void SetNextReader(IndexReader reader, int base_Renamed)
        {
            this.DocBase = base_Renamed;
        }

        public override void SetScorer(Scorer scorer)
        {
            this.Scorer = scorer;
        }

    }
}
