using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class SearchResult<T>
    {
        public SearchResult()
        {
            Items = new List<T>();
        }

        public List<T> Items { get; set; }

        public int Total { get; set; }
    }
}
