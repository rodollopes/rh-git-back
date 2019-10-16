using GitAPITeste.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitAPITeste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersGitController : ControllerBase
    {
        [HttpPost]
        [Obsolete]
        public async Task<ActionResult<string>> ExecutaPesquisa([FromForm] PesquisaModel filtrosPesquisa)
        {
            GitHubClient api = AutenticacaoUsuario();

            if (filtrosPesquisa.TextoPesquisa == null)
                return "Não informou o nome para a pesquisa!";

            SearchUsersRequest pesquisa = new SearchUsersRequest(filtrosPesquisa.TextoPesquisa);

            pesquisa.AccountType = filtrosPesquisa.TipoConta == 'U' ? AccountSearchType.User : AccountSearchType.Org;            
            pesquisa.Order = filtrosPesquisa.TipoOrdenacao == 'A' ? SortDirection.Ascending : SortDirection.Descending;
            pesquisa.SortField = filtrosPesquisa.OrdenarPor == 'S' ? UsersSearchSort.Followers :
                                 filtrosPesquisa.OrdenarPor == 'R' ? UsersSearchSort.Repositories :
                                 UsersSearchSort.Joined;
            pesquisa.PerPage = filtrosPesquisa.ResultadosPorPagina;
            pesquisa.Page = filtrosPesquisa.Pagina == 0 ? 1 : filtrosPesquisa.Pagina;

            FiltrosAdicionaisPesquisa(ref pesquisa, filtrosPesquisa);

            return JsonConvert.SerializeObject(await ResultadoPesquisa(api, pesquisa));
        }

        private async Task<List<UsuarioModel>> ResultadoPesquisa(GitHubClient api, SearchUsersRequest pesquisa)
        {
            SearchUsersResult resultado = await api.Search.SearchUsers(pesquisa);
            List<UsuarioModel> usuarios = new List<UsuarioModel>();
            
            for (int j = 0; j < resultado.Items.Count(); j++)
            {
                // faz a requisição para buscar todos os dados do usuário
                User user = await api.User.Get(resultado.Items[j].Login);
                // adiciona o usuario a lista a ser retornada
                usuarios.Add(new UsuarioModel(resultado.Items[j].Login,
                                                        user.Name,
                                                        user.Email,
                                                        user.Company,
                                                        user.Location,
                                                        user.Followers,
                                                        user.AvatarUrl));
            }

            return usuarios;
        }

        private GitHubClient AutenticacaoUsuario()
        {
            // Paramentro = Nome App, Usuario Git ou Organização GIT
            ProductHeaderValue app = new ProductHeaderValue("rodollopes");
            // Autenticação basica, também pode ser feita via token, é a mais recomendada
            Credentials autenticacao = new Credentials("rodollopes", "R.l0p3s29", AuthenticationType.Basic);
            // cria uma instancia de acesso a API
            return new GitHubClient(app) { Credentials = autenticacao };
        }

        private IEnumerable<UserInQualifier> TipoPesquisa(string tipoPesquisa)
        {
            switch (tipoPesquisa)
            {
                case "L":
                    return new UserInQualifier[] { UserInQualifier.Username };
                case "E":
                    return new UserInQualifier[] { UserInQualifier.Email };
                default:
                    return new UserInQualifier[] { UserInQualifier.Fullname };
            }

        }

        [Obsolete]
        private void FiltrosAdicionaisPesquisa (ref SearchUsersRequest pesquisa, PesquisaModel filtrosPesquisa)
        {
            if (filtrosPesquisa.TipoPesquisa != "T") { pesquisa.In = TipoPesquisa(filtrosPesquisa.TipoPesquisa); }

            if (filtrosPesquisa.DataInicio != null && filtrosPesquisa.DataFim != null)
            {
                pesquisa.Created = new DateRange(filtrosPesquisa.DataInicio, filtrosPesquisa.DataFim);
            }
            else if (filtrosPesquisa.DataInicio != null)
            {
                pesquisa.Created = new DateRange(filtrosPesquisa.DataInicio, SearchQualifierOperator.GreaterThanOrEqualTo);
            }

            if (filtrosPesquisa.Linguagem > 0)
            {
                switch (filtrosPesquisa.Linguagem)
                {
                    case 1:
                        pesquisa.Language = Language.CSharp;
                        break;
                    case 2:
                        pesquisa.Language = Language.Java;
                        break;
                    case 3:
                        pesquisa.Language = Language.Go;
                        break;
                }
            }

            if (filtrosPesquisa.Cidade != null) { pesquisa.Location = filtrosPesquisa.Cidade; }

            //pesquisa.Repositories = new Range(1000)
        }
    }
}
