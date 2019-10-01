using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{name}")]
        public async Task<ActionResult<string>> GetAsync(string name)
        {
            // Paramentro = Nome App, Usuario Git ou Organização GIT
            var app = new ProductHeaderValue("rodollopes");
            // Autenticação basica, também pode ser feita via token, é a mais recomendada
            var autenticacao = new Credentials("rodollopes", "R.l0p3s29", AuthenticationType.Basic);
            // cria uma instancia de acesso a API
            var cliente = new GitHubClient(app) { Credentials = autenticacao };            

            name = name.Replace(" ", "%20");

            var dataInicio = new DateTimeOffset(new DateTime(2000, 1, 1));
            var cidade = "";

            var pesquisa = new SearchUsersRequest(name) {
                    // Tipo de Conta do Git
                //AccountType = AccountSearchType.User,
                    // Filtra contas criadas a partir de uma data
                    // Clsse DateRange possui os metodos para filtrar a data
                    // Classe SearchQualifierOperator, possui os operadores que pode ser utilizados
                //Created = new DateRange(dataInicio, SearchQualifierOperator.GreaterThanOrEqualTo),
                    // Filtra contas que possuam ao menos X seguidores
                    // Clsse Range possui os metodos para filtrar por seguidores
                    // Classe SearchQualifierOperator, possui os operadores que pode ser utilizados
                //Followers = new Range(0, SearchQualifierOperator.GreaterThanOrEqualTo),
                    // Define por quais campos a pesquisa ira buscar, senão informar o parametro ele busca em todos
                In = new UserInQualifier[] { UserInQualifier.Username },
                    // Filtra Por linguagem
                //Language = Language.CSharp,
                    // Filtra Por Cidade
                //Location = cidade,
                    // Tipo de Ordenação do Resultado
                Order = SortDirection.Descending,
                    // Filtra por Numero de Repositórios
                //Repositories = new Range(1000),
                    // Classifica pelo Campo determinado
                SortField = UsersSearchSort.Followers
            };

            var resultado = await cliente.Search.SearchUsers(pesquisa);
            var dadosRetorno = "";

            var paginas = resultado.TotalCount / resultado.Items.Count();

            if (resultado.TotalCount % resultado.Items.Count() != 0)
            {
                paginas++;
            }

            for (int i = 1; i <= paginas; i++)
            {
                for (int j = 0; j < resultado.Items.Count(); j++)
                {
                    // faz a requisição para buscar todos os dados do usuário
                    var user = await cliente.User.Get(resultado.Items[j].Login);
                    if (dadosRetorno != "")
                    {
                        dadosRetorno += ",";
                    }
                    dadosRetorno += $"{{\"Login\": \"{resultado.Items[j].Login}\", \"Nome\": \"{user.Name}\", \"Empresa\": \"{user.Company}\", \"Cidade\": \"{user.Location}\", \"Seguidores\": \"{user.Followers}\"}}";
                }
            }

            return "[" + dadosRetorno + "]";
        }
    }
}
