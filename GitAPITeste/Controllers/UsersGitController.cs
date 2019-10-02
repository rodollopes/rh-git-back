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
        [HttpGet("{name}/{pagina}")]
        public async Task<ActionResult<string>> GetAsync(string name, int pagina)
        {
            name = name.Replace(" ", "%20");

            // Paramentro = Nome App, Usuario Git ou Organização GIT
            ProductHeaderValue app = new ProductHeaderValue("rodollopes");
            // Autenticação basica, também pode ser feita via token, é a mais recomendada
            Credentials autenticacao = new Credentials("rodollopes", "R.l0p3s29", AuthenticationType.Basic);
            // cria uma instancia de acesso a API
            GitHubClient cliente = new GitHubClient(app) { Credentials = autenticacao };            

            // cria o parametro de pesquisa na API
            SearchUsersRequest pesquisa = new SearchUsersRequest(name);
            // passa os filtrs adicionas da pesquisa
            pesquisa.AccountType = AccountSearchType.User;
            // 0 - login ; 1 - email; 2 - nome completo
            pesquisa.In = new UserInQualifier[] { UserInQualifier.Fullname };
            pesquisa.Order = SortDirection.Descending;
            pesquisa.SortField = UsersSearchSort.Followers;
            //DateTimeOffset dataInicio = new DateTimeOffset(new DateTime(2000, 1, 1));
            //pesquisa.Created = new DateRange(dataInicio, SearchQualifierOperator.GreaterThanOrEqualTo)
            //pesquisa.Followers = new Range(0, SearchQualifierOperator.GreaterThanOrEqualTo)
            //pesquisa.Language = Language.CSharp ()
            //pesquisa.Location = String com o nome da cidade
            //pesquisa.Repositories = new Range(1000)
            
            // Define quantos resultados quero que retorne por pagina
            pesquisa.PerPage = 10;
            pesquisa.Page = pagina;

            SearchUsersResult resultado = await cliente.Search.SearchUsers(pesquisa);
            List<UsuarioModel> usuarios = new List<UsuarioModel>();

            if (resultado.Items.Count() == 0) { return $"{{\"erro\":\"Nenhum registro encontrado\"}}"; }

            for (int j = 0; j < resultado.Items.Count(); j++)
            {
                // faz a requisição para buscar todos os dados do usuário
                User user = await cliente.User.Get(resultado.Items[j].Login);
                // adiciona o usuario a lista a ser retornada
                usuarios.Add(new UsuarioModel(resultado.Items[j].Login,
                                                        user.Name,
                                                        user.Email,
                                                        user.Company,
                                                        user.Location,
                                                        user.Followers,
                                                        user.AvatarUrl));
            }

            return JsonConvert.SerializeObject(usuarios);
        }
    }
}
