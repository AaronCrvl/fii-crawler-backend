using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fiiCrawlerApi.Models
{
    public class Usuario
    {
        public int id { get; set; }        
        public string username { get; set; }
        public string senha { get; set; }
        public string categoria { get; set; }       
    }
}
