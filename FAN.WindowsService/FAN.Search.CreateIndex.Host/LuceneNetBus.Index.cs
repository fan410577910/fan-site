using FAN.Helper;
using FAN.LuceneNet;
using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TLZ.LuceneNet;

namespace FAN.Search.CreateIndex.Host
{
    public partial class LuceneNetBus
    {
        private const int PAGE_SIZE = 3000;
        /// <summary>
        /// 是否正在构建索引库信息
        /// </summary>
        private static volatile bool IsBuilding = false;
        /// <summary>
        /// 过滤SPU状态
        /// </summary>
       // private static readonly EProduct_SPU_Modify_Status[] _StatusArrary = { EProduct_SPU_Modify_Status.上架, EProduct_SPU_Modify_Status.定时上架 };

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="cultures"></param>
        /// <returns>返回错误信息，null表示没有错误。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string Build(List<string> cultures)
        {
            string message = null;
            if (!IsBuilding)
            {
                IsBuilding = true;
                string luceneDirectory = GetLuceneDirectory();
                LuceneNetConfig.LuceneDirectory = luceneDirectory;
                LuceneNetConfig.LuceneDictDirectory = LuceneDictDirectory;
                string luceneBuildDir = null;
                Directory directory = null;
                IndexWriter indexWriter = null;
                ColumnField[] columnFields = GetColumnFields();
                List<View_seo_keys_culture> viewSEOKeysCultureList = SelectAllViewSEOKeysCulture();
                foreach (string culture in cultures)
                {
                    luceneBuildDir = IOHelper.CombinePath(luceneDirectory, culture.ToUpper());
                    IOHelper.ClearDirectory(luceneBuildDir);
                    directory = LuceneBus.GetDirectory(luceneBuildDir);
                    if (LuceneBus.IsLocked(directory))
                    {
                        LuceneBus.UnLock(directory);
                    }
                    try
                    {
                        indexWriter = LuceneBus.GetWriter(culture, directory);
                        //http://blog.jobbole.com/80464/
                        int pageCount = Select(culture);
                        if (pageCount > 0)
                        {
                            int spuID = 0;
                            List<LuceneSpuModel> luceneSpuModelList = null;
                            for (int pageIndex = 1; pageIndex <= pageCount; pageIndex++)
                            {
                                luceneSpuModelList = Select(viewSEOKeysCultureList, culture, ref spuID);
                                if (luceneSpuModelList != null && luceneSpuModelList.Count > 0)
                                {
                                    LuceneBus.Insert<LuceneSpuModel>(indexWriter, luceneSpuModelList, columnFields);
                                    luceneSpuModelList.Clear();
                                    luceneSpuModelList = null;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (luceneSpuModelList != null)
                            {
                                if (luceneSpuModelList.Count > 0)
                                {
                                    luceneSpuModelList.Clear();
                                }
                                luceneSpuModelList = null;
                            }
                            LuceneBus.MaybeMerge(indexWriter);
                            LuceneBus.Commit(indexWriter);
                        }
                    }
                    catch (Exception ex)
                    {
                        message = "Build(Language[] cultures)出现错误，错误信息：" + ex.Message;
                    }
                    finally
                    {
                        if (indexWriter != null)
                        {
                            LuceneBus.Close(indexWriter);
                            indexWriter = null;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        break;
                    }
                }
                Change();//索引生成完切换前台搜索目录
                if (columnFields != null)
                {
                    Array.Clear(columnFields, 0, columnFields.Length);
                    columnFields = null;
                }
                IsBuilding = false;
            }
            return message;
        }

        private static ColumnField[] GetColumnFields()
        {
            PropertyInfo[] propertyInfos = typeof(LuceneSpuModel).GetProperties();
            int propertyInfoCount = propertyInfos.Length;
            ColumnField[] columnFields = new ColumnField[propertyInfoCount];
            PropertyInfo propertyInfo = null;
            ColumnField columnField = null;
            for (int i = 0; i < propertyInfoCount; i++)
            {
                propertyInfo = propertyInfos[i];
                columnField = new ColumnField();
                string column = columnField.Column = propertyInfo.Name;
                columnField.Store = Lucene.Net.Documents.Field.Store.YES;
                if (propertyInfo.PropertyType.Name == FieldType.STRING)
                {
                    columnField.Index = Field.Index.ANALYZED;//分词且索引，支持搜索
                    switch (columnField.Column)
                    {
                        case "Title":
                            columnField.Boost = 5;
                            break;
                        case "LeiMuNameJSON":
                        case "Keys":
                            columnField.Boost = 4;
                            break;
                        case "PropertyText":
                            columnField.Boost = 3;
                            break;
                        case "RequiredText":
                            columnField.Boost = 2;
                            break;
                        case "NameValue":
                        default:
                            columnField.Index = Field.Index.NOT_ANALYZED_NO_NORMS;//不分词且索引，不支持权重，不支持搜索
                            break;
                    }
                }
                else
                {
                    if (column == "SPUID"
                        || column == "CultureID"
                        || column == "PID")
                    {
                        columnField.Index = Field.Index.ANALYZED_NO_NORMS;//分词建索引，不支持权重，支持搜索
                    }
                    else
                    {
                        columnField.Index = Field.Index.NOT_ANALYZED_NO_NORMS;//不分词且索引，不支持权重，不支持搜索
                    }
                }
                columnFields[i] = columnField;
            }
            Array.Clear(propertyInfos, 0, propertyInfos.Length);
            propertyInfos = null;
            return columnFields;
        }

        /// <summary>
        /// 通过多语言ID从MongoDB中获取Lucene格式的数据源
        /// 增加搜索排序功能.wangwei.2016-3-10
        /// </summary>
        /// <param name="cultureID"></param>
        /// <param name="spuID"></param>
        /// <returns></returns>
        private static List<LuceneSpuModel> Select(List<View_seo_keys_culture> viewSEOKeysCultureList, int cultureID, ref int spuID)
        {
            List<LuceneSpuModel> luceneSpuModelList = null;
            List<MongoDBSpuItemModel> mongoDBSpuItemModelList = Select(cultureID, spuID);
            List<MongoDBSpuListSortModel> mongoDBSpuListSortModelList = Select(cultureID, mongoDBSpuItemModelList.Select(item => item.SPUID).ToList(), EPlatform.PC端.GetValue());//获取排序信息
            if (mongoDBSpuItemModelList.Count == 0)
            {
                mongoDBSpuItemModelList = null;
                return luceneSpuModelList;
            }
            spuID = mongoDBSpuItemModelList[mongoDBSpuItemModelList.Count - 1].SPUID;
            luceneSpuModelList = new List<LuceneSpuModel>(mongoDBSpuItemModelList.Count);
            SPU spu = null;
            List<string> propertyList = null, requiredList = null;
            View_seo_keys_culture viewSEOKeysCulture = null;
            MongoDBSpuListSortModel mongoDBSpuListSortModel = null;
            string keys = string.Empty;
            decimal price = decimal.Zero;
            int saleCount = 0;
            decimal disCount = decimal.Zero;
            int customerRatingCount = 0;

            foreach (MongoDBSpuItemModel model in mongoDBSpuItemModelList)
            {
                try
                {
                    spu = JsonHelper.ConvertStrToJsonT<SPU>(model.SPUJSON);
                }
                catch
                {
                }
                if (spu == null)
                {
                    continue;
                }
                else
                {//需要处理材质和颜色必选区值
                    TLZ.COM.Module.WebSite.BIZ.SPUBIZ.HandleRenderSPU(spu);
                }
                if (_StatusArrary.Contains(spu.Status)
                    && spu.Render == EProduct_SPU_Modify_Render.显示)
                {
                    #region 拼接描述属性字符串
                    propertyList = new List<string>();
                    foreach (var property in spu.Properties)
                    {
                        if (property.IsShow)
                        {
                            foreach (var value in property.Values)
                            {
                                if (value.IsUsed)
                                {
                                    propertyList.Add(string.Format("{0}={1}", property.Text, value.Text));
                                }
                            }
                        }
                    }
                    propertyList.TrimExcess();
                    string propertyText = string.Join("&", propertyList);
                    #endregion

                    #region 拼接描必选区字符串
                    requiredList = new List<string>();
                    foreach (var required in spu.Requireds)
                    {
                        if (required.IsUsed)
                        {
                            if (!required.IsApplyAllValue)
                            {
                                foreach (var value in required.Values)
                                {
                                    if (value.IsUsed)
                                    {
                                        requiredList.Add(string.Format("{0}={1}", required.Text, value.Text));
                                    }
                                }
                            }
                        }
                    }
                    requiredList.TrimExcess();
                    string requiredText = string.Join("&", requiredList);
                    #endregion

                    propertyList.AddRange(requiredList);
                    propertyList.TrimExcess();
                    string nameValue = string.Join("&", propertyList); //既可以搜必选区又可以搜索描述属性
                    propertyList.Clear();
                    propertyList = null;
                    requiredList.Clear();
                    requiredList = null;

                    #region 设置排序数据
                    mongoDBSpuListSortModel = mongoDBSpuListSortModelList.Find(item => item.SPUID == model.SPUID);
                    if (mongoDBSpuListSortModel != null)
                    {
                        price = mongoDBSpuListSortModel.ActivePrice > 0 ? mongoDBSpuListSortModel.ActivePrice : mongoDBSpuListSortModel.SellPrice;
                        saleCount = mongoDBSpuListSortModel.SaleCount;
                        disCount = price / mongoDBSpuListSortModel.MarketPrice;
                        customerRatingCount = mongoDBSpuListSortModel.CustomerRatingCount;
                    }
                    else
                    {
                        price = decimal.Zero;
                        saleCount = 0;
                        disCount = decimal.Zero;
                        customerRatingCount = 0;
                    }
                    #endregion

                    #region 拼接返回列表
                    //从数据库获取产品keys信息，并且索引。wangwei
                    if (viewSEOKeysCultureList != null)
                    {
                        viewSEOKeysCulture = viewSEOKeysCultureList.Find(item => item.PID == spu.PID && item.CultureID == cultureID && item.PlatformJSON == JsonHelper.ConvertJsonToStr(spu.Platforms));
                        if (viewSEOKeysCulture != null)
                        {
                            keys = viewSEOKeysCulture.Keys;
                        }
                    }
                    luceneSpuModelList.Add(new LuceneSpuModel
                    {
                        SPUID = model.SPUID,
                        Status = (int)spu.Status,
                        Title = spu.Title,
                        // TitleNotAnalyzed=spu.Title,
                        LeiMuNameJSON = spu.LeiMuNames,
                        ImageJSON = JsonHelper.ConvertJsonToStr(spu.Images),
                        DescriptionFull = spu.DescriptionFull,
                        PropertyText = propertyText,
                        RequiredText = requiredText,
                        NameValue = nameValue,
                        CultureID = spu.CultureID,
                        IsHot = (int)spu.IsHot,
                        IsMutilColor = (int)spu.IsMutilColor,
                        UpTime = spu.UpTime,
                        DescriptionShort = spu.DescriptionShort,
                        PID = spu.PID,
                        Sort = spu.Sort,
                        Price = price,
                        SaleCount = saleCount,
                        DisCount = disCount,
                        CustomerRatingCount = customerRatingCount,
                        Keys = keys
                    });
                    #endregion
                }
            }
            mongoDBSpuItemModelList.Clear();
            mongoDBSpuItemModelList = null;
            luceneSpuModelList.TrimExcess();
            return luceneSpuModelList;
        }


        /// <summary>
        /// 获取某一个语言的页数
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        private static int Select(int cultureID)
        {
            IMongoQuery query = Query<MongoDBSpuItemModel>.EQ(item => item.CultureID, cultureID);
            long recordCount = MongoDBHelper.Count<MongoDBSpuItemModel>(query);
            int pageCount = TypeParseHelper.StrToInt32(recordCount / PAGE_SIZE);
            if (recordCount % PAGE_SIZE != 0)
            {
                pageCount += 1;
            }
            return pageCount;
        }

        /// <summary>
        /// 分页获取MongoDB数据
        /// </summary>
        /// <param name="cultureID"></param>
        /// <param name="spuID"></param>
        /// <returns></returns>
        private static List<MongoDBSpuItemModel> Select(int cultureID, int spuID)
        {
            List<IMongoQuery> queryList = new List<IMongoQuery>();
            queryList.Add(Query<MongoDBSpuItemModel>.GT(item => item.SPUID, spuID));
            queryList.Add(Query<MongoDBSpuItemModel>.EQ(item => item.CultureID, cultureID));
            IMongoQuery query = Query.And(queryList);
            queryList.Clear();
            queryList = null;
            SortByBuilder<MongoDBSpuItemModel> sort = SortBy<MongoDBSpuItemModel>.Ascending(item => item.SPUID);
            return MongoDBHelper.Select<MongoDBSpuItemModel>(PAGE_SIZE, query, sort) ?? new List<MongoDBSpuItemModel>(0);
        }
        private static List<MongoDBSpuListSortModel> Select(int cultureID, List<int> spuIdList, int platform)
        {
            List<IMongoQuery> queryList = new List<IMongoQuery>();
            queryList.Add(Query<MongoDBSpuListSortModel>.In(item => item.SPUID, spuIdList));
            queryList.Add(Query<MongoDBSpuListSortModel>.EQ(item => item.CultureID, cultureID));
            queryList.Add(Query<MongoDBSpuListSortModel>.EQ(item => item.Platform, platform));
            IMongoQuery query = Query.And(queryList);
            queryList.Clear();
            queryList = null;
            SortByBuilder<MongoDBSpuListSortModel> sort = SortBy<MongoDBSpuListSortModel>.Ascending(item => item.SPUID);
            return MongoDBHelper.Select<MongoDBSpuListSortModel>(PAGE_SIZE, query, sort) ?? new List<MongoDBSpuListSortModel>(0);
        }
        /// <summary>
        /// 获取SEO Keys集合
        /// </summary>
        /// <returns></returns>
        private static List<View_seo_keys_culture> SelectAllViewSEOKeysCulture()
        {
            return View_seo_keys_cultureBLL.Select();
        }
    }
}
