using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitAPITeste.Models
{
    public class PesquisaModel
    {
        public string TextoPesquisa { get; }
        // U - Usuário; E - Empresa
        public char TipoConta { get; }
        // L - Login; E - Email; N - Nome
        public string TipoPesquisa { get; }
        // A - Ascendente; D - Descendente
        public char TipoOrdenacao { get; }
        // S - Seguidores; R - Reposiorios; A - Ambos
        public char OrdenarPor { get; }
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }
        // 1 - C#; 2 - Java; 3 - GO
        public int Linguagem { get; }
        public string Cidade { get; }
        public int ResultadosPorPagina { get; }
        public int Pagina { get; }
    }
}
