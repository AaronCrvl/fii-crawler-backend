namespace fiiCrawlerApi.Models
{
    public class Noticia
    {
        public string titulo { get; set; }
        public string descricao { get; set; }

        public string tempoPassado { get; set; }
        public string fonte { get; set; }
        public string url { get; set; }
        public string urlImagem { get; set; }
        public string? svgLogo { get; set; }
    }
}
