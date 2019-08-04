using System;
using System.Collections.Generic;
using System.Text;

namespace Models.HelperModels
{
    public class CacheKeySearchModel
    {
        public string KeyFilter { get; set; }

        public DateTimeOffset? MinDateOffset { get; set; }

        public DateTimeOffset? MaxDateOffset { get; set; }
    }
}
