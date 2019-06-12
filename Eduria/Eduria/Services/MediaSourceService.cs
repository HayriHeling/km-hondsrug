using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models;

namespace Eduria.Services
{
    public class MediaSourceService : AService<MediaSource>
    {
        public MediaSourceService(EduriaContext eduriaContext)
        {
            Context = eduriaContext;
        }
        public override IEnumerable<MediaSource> GetAll()
        {
            return Context.MediaSources;
        }

        public override MediaSource GetById(int id)
        {
            return Context.MediaSources.First(x => x.MediaSourceId == id);
        }
    }
}
