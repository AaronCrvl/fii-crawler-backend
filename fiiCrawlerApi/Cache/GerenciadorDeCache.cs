using fiiCrawlerApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace fiiCrawlerApi.Cache
{
    public class GerenciadorDeCache
    {
        public GerenciadorDeCache()
        {
        }

        public void SalvarCache(List<Fii> list)
        {
            var json = JsonConvert.SerializeObject(list);            
            string path = Path.Combine(Environment.CurrentDirectory, @"Cache\cache.json");

            //write string to file
            System.IO.File.WriteAllText(path, json);
        }
    }
}