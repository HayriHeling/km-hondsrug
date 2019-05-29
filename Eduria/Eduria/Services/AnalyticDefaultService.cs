using Eduria.Models;
using EduriaData.Models.AnalyticLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eduria.Services
{
    public class AnalyticDefaultService : AService<AnalyticData>
    {
        public AnalyticDefaultService(EduriaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="analyticDefaultId"></param>
        /// <param name="analyticDataId"></param>
        public void AddDataHasDefault(int analyticDefaultId, int analyticDataId)
        {
            DataHasDefault dataHasDefault = new DataHasDefault
            {
                AnalyticDataId = analyticDataId,
                AnalyticDefaultId = analyticDefaultId
            };

            Context.DataHasDefaults.Add(dataHasDefault);
            Context.SaveChanges();
        }

        public void AddDefaultDataInput(int dataHasDefaultId, string text)
        {
            DefaultDataInput defaultDataInput = new DefaultDataInput
            {
                DataHasDefaultId = dataHasDefaultId,
                Text = text
            };

            Context.DefaultDataInputs.Add(defaultDataInput);
            Context.SaveChanges();
        }

        /// <summary>
        /// Retrieve all the static analytic names.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AnalyticDefault> GetAllAnalyticDefault()
        {
            return Context.AnalyticDefaults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<AnalyticData> GetAll()
        {
            return Context.AnalyticDatas;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override AnalyticData GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataHasDefault> GetAllDataHasDefaults()
        {
            return Context.DataHasDefaults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<AnalyticHasDefaultModel> GetAllDataByAnalyticDataId(int id)
        {
            var query = from dhd in Context.DataHasDefaults
                        join ad in Context.AnalyticDefaults on dhd.AnalyticDefaultId equals ad.AnalyticDefaultId
                        join dds in Context.DefaultDataScores on dhd.DataHasDefaultId equals dds.DataHasDefaultId into a
                        from dds in a.DefaultIfEmpty()
                        where dhd.AnalyticDataId == id
                        select new AnalyticHasDefaultModel
                        {
                            AnalyticDataId = dhd.AnalyticDataId,
                            AnalyticDefaultId = dhd.AnalyticDefaultId,
                            AnalyticDefaultName = ad.AnalyticDefaultName,
                            CategoryId = ad.AnalyticCategory,
                            Score = dds.Score
                    };

            return query.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<AnalyticHasDefaultModel>, IEnumerable<AnalyticDefaultModel>> GetCombinedAnalyticDefaultAndData(int id, int category)
        {
            IEnumerable<AnalyticHasDefaultModel> analyticHasDefaultModels = GetAllDefaultsByAnalyticDataIdAndCategoryName(id, category);
            IEnumerable<AnalyticDefaultModel> analyticDefaultModels = GetAllAnalyticDefaultByCategoryId(category);
            
            var tuple = Tuple.Create(analyticHasDefaultModels, analyticDefaultModels);
            
            return tuple;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public IEnumerable<AnalyticHasDefaultModel> GetAllDefaultsByAnalyticDataIdAndCategoryName(int id, int category)
        {
            return GetAllDataByAnalyticDataId(id).Where(x => x.CategoryId == category);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IEnumerable<AnalyticDefaultModel> GetAllAnalyticDefaultByCategoryId(int category)
        {
            var query = from ad in Context.AnalyticDefaults
                        where ad.AnalyticCategory == category
                        select new AnalyticDefaultModel
                        {
                            AnalyticDefaultId = ad.AnalyticDefaultId,
                            AnalyticDefaultName = ad.AnalyticDefaultName,
                        };

            return query.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="period"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public AnalyticData GetAnalyticDataByUserIdAndPeriodAndYear(int userId, int period, int year)
        {
            var query = from ad in Context.AnalyticDatas
                        where ad.UserId == userId && ad.Period == period && ad.Year == year
                        select new AnalyticData
                        {
                            AnalyticDataId = ad.AnalyticDataId
                        } ;

            return query.First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodParam"></param>
        /// <param name="analyticId"></param>
        public void AddToAnalytic(int[] methodParam, int analyticId)
        {
            if (methodParam.Length != 0)
            {
                foreach (var id in methodParam)
                {
                    AddDataHasDefault(id, analyticId);
                }
            }
        }

        /// <summary>
        /// Add all defined methods to the Analytic.
        /// </summary>
        /// <param name="methodParam">The array of integers that stands for the AnalyticDefault Id.</param>
        /// <param name="analyticId">The Analytic Id from a specific user.</param>
        /// <param name="ownMethod">The own method that the user specified.</param>
        public void AddToAnalytic(int[] methodParam, int analyticId, int defaultAnalyticId, string ownMethod)
        {
            if (methodParam.Length != 0)
            {
                foreach (var id in methodParam)
                {
                    AddDataHasDefault(id, analyticId);

                    if (id == defaultAnalyticId && ownMethod != null || ownMethod != "")
                    {
                        AddDataHasDefault(id, analyticId);
                        AddDefaultDataInput(id, ownMethod);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="analyticDefaultId"></param>
        /// <param name="analyticDataId"></param>
        /// <param name="score"></param>
        public void AddScoreToAnalyticDefault(int analyticDefaultId, int analyticDataId, int score)
        {
            if (analyticDefaultId != 0 && analyticDataId != 0)
            {
                DataHasDefault dataHasDefault = GetDataHasDefaultByAnalyticDefaultIdAndAnalyticDataId(analyticDefaultId, analyticDataId);

                if (dataHasDefault == null){
                    AddDataHasDefault(analyticDefaultId, analyticDataId);
                }

                DefaultDataScore defaultDataScore = new DefaultDataScore
                {
                    DataHasDefaultId = dataHasDefault.DataHasDefaultId,
                    Score = score
                };

                Context.DefaultDataScores.Add(defaultDataScore);
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="analyticDefaultId"></param>
        /// <param name="analyticDataId"></param>
        /// <returns></returns>
        public DataHasDefault GetDataHasDefaultByAnalyticDefaultIdAndAnalyticDataId(int analyticDefaultId, int analyticDataId)
        {
            var query = from dhd in Context.DataHasDefaults
                        where dhd.AnalyticDefaultId == analyticDefaultId && dhd.AnalyticDataId == analyticDataId
                        select new DataHasDefault
                        {
                            DataHasDefaultId = dhd.DataHasDefaultId,
                            AnalyticDataId = dhd.AnalyticDataId,
                            AnalyticDefaultId = dhd.AnalyticDefaultId
                        };

            return query.First();
        }
    }
}
