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
        public IEnumerable<AnalyticDefaultModel> GetAllAnalyticDefault()
        {
            return Context.AnalyticDefaults.Select(result => new AnalyticDefaultModel
            {
                AnalyticDefaultId = result.AnalyticDefaultId,
                AnalyticDefaultName = result.AnalyticDefaultName,
                Category = result.CategoryId.ToString()
            });
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

        /// <summary>
        /// Get all the data by analytic data id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<AnalyticDefaultModel> GetAllDataByAnalyticDataId(int id)
        {
            var query = from dhd in Context.DataHasDefaults
                    join ad in Context.AnalyticDefaults on dhd.AnalyticDefaultId equals ad.AnalyticDefaultId
                    join c in Context.Categories on ad.CategoryId equals c.CategoryId
                    where dhd.AnalyticDataId == id
                    select new AnalyticDefaultModel
                    {
                        AnalyticDataId = dhd.AnalyticDataId,
                        AnalyticDefaultId = dhd.AnalyticDefaultId,
                        AnalyticDefaultName = ad.AnalyticDefaultName,
                        Category = c.CategoryName,
                        Score = dhd.Score
                    };

            return query.ToList();
        }

        /// <summary>
        /// Get all analytic methods that exists in the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AnalyticDefaultModel> GetAllAnalyticMethods()
        {
            return GetAllAnalyticDefault().Where(x => x.Category == "1");
        }
    }
}
