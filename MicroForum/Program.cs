using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Threading.Tasks;


/// <summary>
/// Descrição resumida de Program
/// </summary>
namespace MicroForum
{
    public class Program
    {

        public Program()
        {
            //
            // TODO: Adicionar lógica do construtor aqui
            //
            MicroForum.Dados.IniciarVariaveis();
        }
        public static void Main(string[] args)
        {
            MicroForum.Dados.IniciarVariaveis();            
        }
    }
}