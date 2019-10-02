using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitAPITeste.Models
{
    public class UsuarioModel
    {
        public string Login { get; }
        public string Nome { get; }
        public string Email { get; }
        public string Empresa { get; }
        public string Cidade { get; }
        public int Seguidores { get; }
        public string AvatarUrl { get; }

        public UsuarioModel(string login, string nome)
        {
            Login = login;
            Nome = nome;
        }

        public UsuarioModel(string login, string nome, string email, string empresa, string cidade, int seguidores, string avatarUrl)
        {
            Login = login;
            Email = email;
            Nome = nome;
            Empresa = empresa;
            Cidade = cidade;
            Seguidores = seguidores;
            AvatarUrl = avatarUrl;
        }
    }
}
