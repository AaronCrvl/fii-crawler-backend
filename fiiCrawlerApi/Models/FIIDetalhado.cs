using System.Collections.Generic;
namespace fiiCrawlerApi.Models
{
    /// <summary>
    /// Modelo de transferência de dados detalhados de um FII's        
    /// </summary>
    public class FIIDetalhado
    {
        public string codigoFii;
        public string nomeCompleto;
        public string cota;
        public string variacao;
        public string valorizacao;
        public List<Dividendo> historicoDividendos;
        public Administrador administrador;
    }
}

public class Dividendo
{
    public string dataBase;
    public string dataPagamento;
    public string cotacaoBase;
    public string dividendoYeild;
    public string rendimento;
}

public class Administrador
{
    public string razaoSocial;
    public string cnpj;
    public string email;
    public string telefone;
    public string site;
    public string nomeNoPregao;
    public string numeroDeCotas;
    public string patrimonio;
    public string segmento;
    public string tipoGestao;
    public string publicoAlvo;
}