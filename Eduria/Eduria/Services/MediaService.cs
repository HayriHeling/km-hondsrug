using EduriaData.Models;
using System.Collections.Generic;
using System.Linq;
using EduriaData.Models.ExamLayer;

namespace Eduria.Services
{
    public class MediaService : AService<MediaSource>
    {
        public MediaService(EduriaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Get all media from the database.
        /// </summary>
        /// <returns>A List with all media objects.</returns>
        public override IEnumerable<MediaSource> GetAll()
        {
            return Context.MediaSources;
        }

        /// <summary>
        /// Get an specific exam. 
        /// </summary>
        /// <param name="id">The id from the exam.</param>
        /// <returns>The specific category.</returns>
        public override MediaSource GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.MediaSourceId == id);
        }

        public MediaSource GetBySource(string source)
        {
            return GetAll().FirstOrDefault(x => x.Source == source);
        }
        public MediaSource GetByMediaType(int type)
        {
            return GetAll().FirstOrDefault(x => x.MediaType == type);
        }
    }
}
