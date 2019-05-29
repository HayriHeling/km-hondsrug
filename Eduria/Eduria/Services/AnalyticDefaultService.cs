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
        /// <param name="AnalyticDefaultId"></param>
        /// <param name="AnalyticDataId"></param>
        public void AddDataHasDefault(int AnalyticDefaultId, int AnalyticDataId)
        {
            DataHasDefault dataHasDefault = new DataHasDefault
            {
                AnalyticDataId = AnalyticDataId,
                AnalyticDefaultId = AnalyticDefaultId
            };

            Context.Add(dataHasDefault);
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

        public override IEnumerable<AnalyticData> GetAll()
        {
            return Context.AnalyticDatas;
        }

        public override AnalyticData GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataHasDefault> GetAllDataHasDefaults()
        {
            return Context.DataHasDefaults;
        }

        public IEnumerable<AnalyticHasDefaultModel> GetAllDataByAnalyticDataId(int id)
        {
            var query = from dhd in Context.DataHasDefaults
                    join ad in Context.AnalyticDefaults on dhd.AnalyticDefaultId equals ad.AnalyticDefaultId
                    //join c in Context.Categories on ad.CategoryId equals c.CategoryId
                    where dhd.AnalyticDataId == id
                    select new AnalyticHasDefaultModel
                    {
                        AnalyticDataId = dhd.AnalyticDataId,
                        AnalyticDefaultId = dhd.AnalyticDefaultId,
                        AnalyticDefaultName = ad.AnalyticDefaultName,
                        //Category = c.CategoryName,
                        //Score = dhd.Score
                    };

            return query.ToList();
        }

        public Tuple<IEnumerable<AnalyticHasDefaultModel>, IEnumerable<AnalyticDefaultModel>> GetCombinedAnalyticDefaultAndData(int id, string category)
        {
            IEnumerable<AnalyticHasDefaultModel> analyticHasDefaultModels = GetAllDefaultsByAnalyticDataIdAndCategoryName(id, category);
            IEnumerable<AnalyticDefaultModel> analyticDefaultModels = GetAllAnalyticDefaultByCategoryName(category);
            
            var tuple = Tuple.Create(analyticHasDefaultModels, analyticDefaultModels);
            
            return tuple;
        }

        public IEnumerable<AnalyticHasDefaultModel> GetAllDefaultsByAnalyticDataIdAndCategoryName(int id, string category)
        {
            return GetAllDataByAnalyticDataId(id).Where(x => x.Category == category);
        }

        public IEnumerable<AnalyticDefaultModel> GetAllAnalyticDefaultByCategoryName(string category)
        {
            var query = from ad in Context.AnalyticDefaults
                        //join c in Context.Categories on ad.CategoryId equals c.CategoryId
                        //where c.CategoryName == category
                        select new AnalyticDefaultModel
                        {
                            AnalyticDefaultId = ad.AnalyticDefaultId,
                            AnalyticDefaultName = ad.AnalyticDefaultName,
                        };

            return query.ToList();
        }
    }
}
