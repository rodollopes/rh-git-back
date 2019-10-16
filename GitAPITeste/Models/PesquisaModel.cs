using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitAPITeste.Models
{
    public class PesquisaModel
    {
        public string TextoPesquisa { get; set; }
        // U - Usuário; E - Empresa
        public char TipoConta { get; set; }
        // L - Login; E - Email; N - Nome
        public string TipoPesquisa { get; set; }
        // A - Ascendente; D - Descendente
        public char TipoOrdenacao { get; set; }
        // S - Seguidores; R - Reposiorios; A - Ambos
        public char OrdenarPor { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        // 1 - C#; 2 - Java; 3 - GO
        public int Linguagem { get; set; }
        public string Cidade { get; set; }
        public int ResultadosPorPagina { get; set; }
        public int Pagina { get; set; }
    }
}
