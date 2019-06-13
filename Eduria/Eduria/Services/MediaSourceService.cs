﻿using System;
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