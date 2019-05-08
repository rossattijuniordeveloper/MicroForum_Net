using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace MicroForum
{

    /// <summary>
    /// Descrição resumida de Dados
    /// </summary>
    public class Dados
    {

        public Dados()
        {

        }
        //
        //---------------------------------------------------------
        //        
        public static void IniciarVariaveis()
        {
            MicroForum.Startup.strCnx = "Data Source=DESKTOP-1SO8CFA\\SQLEXPRESS01;Initial Catalog=teste;Integrated Security=True";
            MicroForum.Startup.expressaoSQL = string.Empty;
            MicroForum.Startup.Ligacao = new SqlConnection(MicroForum.Startup.strCnx);
        }
        //
        //---------------------------------------------------------
        //        
        private static void LimparVariaveis()
        {
            MicroForum.Startup.expressaoSQL = null;            
            MicroForum.Startup.comando.Connection = null;
            MicroForum.Startup.comando.CommandText = null;
            MicroForum.Startup.comando = null;
        }
        //
        //---------------------------------------------------------
        //
        private static string MontarQuery_PesquisarUsuario(string _Nome)
        {
            string str = "select id_usuario from usuario where Nome='";
            str += _Nome + "'";
            return str;
        }
        //
        //--------------------------------------------------------------------
        //
        public static bool PesquisarUsuario(string _usuario)
        {
            bool retorno = false;
            try
            {
                MicroForum.Startup.comando = new SqlCommand();
                MicroForum.Startup.comando.Connection = MicroForum.Startup.Ligacao;
                MicroForum.Startup.expressaoSQL = string.Empty;
                MicroForum.Startup.expressaoSQL = MontarQuery_PesquisarUsuario(_usuario);
                MicroForum.Startup.comando.CommandText = string.Empty;
                MicroForum.Startup.comando.CommandText = MicroForum.Startup.expressaoSQL;
                MicroForum.Startup.comando.Connection.Open();
                SqlDataReader dr = MicroForum.Startup.comando.ExecuteReader();
                while ((dr.Read()))
                {
                    retorno = true;
                }
                MicroForum.Startup.comando.Connection.Close();
            }
            catch (Exception)
            {
                retorno = false;
            }
            return retorno;
        }
        //
        //--------------------------------------------------------------------
        //
        public static int RetornarIdUsuario(string _usuario)
        {
            int retorno = -1;
            try
            {
                MicroForum.Startup.comando = new SqlCommand();
                MicroForum.Startup.comando.Connection = MicroForum.Startup.Ligacao;
                MicroForum.Startup.comando.CommandText = string.Empty;
                MicroForum.Startup.comando.CommandText = MontarQuery_PesquisarUsuario(_usuario);
                MicroForum.Startup.comando.Connection.Open();
                SqlDataReader dr = MicroForum.Startup.comando.ExecuteReader();
                while ((dr.Read()))
                {
                    retorno = (int)dr["id_usuario"];
                }
                MicroForum.Startup.comando.Connection.Close();
                MicroForum.Startup.comando.CommandText = "";
            }
            catch (Exception)
            {
                retorno = -1;
            }
            return retorno;
        }
        //
        //--------------------------------------------------------------------
        //
        public static bool IncluirUsuario(string _Nome, string _senha)
        {
            bool result = true;
            try
            {
                // encriptar a senha
                encriptar enc = new encriptar();
                string senhaEnc = enc.CriarMD5(_senha);
                //preparar e executar a gravação do novo registro;
                MicroForum.Startup.expressaoSQL = MontarQuery_IncluirUsuario(RetornarNovoId(), _Nome, senhaEnc);
                MicroForum.Startup.comando = new SqlCommand();
                MicroForum.Startup.comando.CommandText = MicroForum.Startup.expressaoSQL;
                MicroForum.Startup.comando.Connection = MicroForum.Startup.Ligacao;
                MicroForum.Startup.comando.Connection.Open();
                MicroForum.Startup.comando.ExecuteNonQuery();
                MicroForum.Startup.comando.Connection.Close();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            MicroForum.Startup.comando.Connection.Close();
            return result;
        }
        //
        //--------------------------------------------------------------------
        //
        private static string MontarQuery_IncluirUsuario(int _id, string _Nome, string _senha)
        {
            string str = "insert into usuario (id_usuario,nome,senha) values (";
            str += _id + ",";
            str += "'" + _Nome + "',";
            str += "'" + _senha + "')";
            return str;
        }
        //
        //--------------------------------------------------------------------
        //
        private static int RetornarNovoId()
        {
            int _id = -1;
            try
            {
                //----------  PREPARANDO O ID DO NOVO REGISTRO -------------
                MicroForum.Startup.expressaoSQL = "SELECT MAX(Id_usuario) as ID from usuario";
                MicroForum.Startup.comando = new SqlCommand();
                MicroForum.Startup.comando.CommandText = MicroForum.Startup.expressaoSQL;
                MicroForum.Startup.comando.Connection = MicroForum.Startup.Ligacao;
                MicroForum.Startup.comando.Connection.Open();
                object UltimoID = MicroForum.Startup.comando.ExecuteScalar();
                if (UltimoID == DBNull.Value)
                {
                    _id = 1;
                }
                else
                {
                    _id = (int)UltimoID + 1;
                }
            }
            catch (Exception)
            {
                _id = -1;
            }
            MicroForum.Startup.comando.Connection.Close();
            return _id;
        }
        //
        //--------------------------------------------------------------------
        //
        private static string MontarQuery_RetornarSenha(string _Nome)
        {
            string str = "select senha from usuario where Nome='";
            str += _Nome + "'";
            return str;
        }
        //
        //--------------------------------------------------------------------
        //
        public static string RetornarSenha(string _usuario)
        {
            string retorno = "";
            try
            {
                MicroForum.Startup.comando = new SqlCommand();
                MicroForum.Startup.comando.Connection = new SqlConnection(MicroForum.Startup.strCnx);
                MicroForum.Startup.comando.CommandText = MontarQuery_RetornarSenha(_usuario);
                MicroForum.Startup.comando.Connection.Open();
                SqlDataReader dr = MicroForum.Startup.comando.ExecuteReader();
                while ((dr.Read()))
                {
                    retorno = dr["senha"].ToString();
                }
                MicroForum.Startup.comando.Connection.Close();
            }
            catch (Exception)
            {
                retorno = "";
            }
            return retorno;
        }
        //
        //--------------------------------------------------------------------
        //
        public static bool VerificarSenha(string _usuario, string _senha)
        {
            encriptar enc = new encriptar();
            string pass = enc.CriarMD5(_senha);
            bool Retorno = false;
            if (pass == MicroForum.Dados.RetornarSenha(_usuario))
            {
                Retorno = true;
            }
            return Retorno;
        }
        //
        //--------------------------------------------------------------------
        //
        public static bool IncluirPosts( int _Id_usuario, string _Titulo, string _Mensagem,DateTime _atualizacao)
        {
            bool result = false;
            try
            {
                //preparar e executar a gravação do novo registro;
                int _Id_posts = RetornarNovoIdPosts();
                LimparVariaveis();
                MicroForum.Startup.expressaoSQL = MontarQuery_IncluirPosts(_Id_posts , _Id_usuario,_Titulo,_Mensagem,_atualizacao);
                MicroForum.Startup.comando = new SqlCommand();
                MicroForum.Startup.comando.CommandText = MicroForum.Startup.expressaoSQL;
                MicroForum.Startup.comando.Connection = new SqlConnection(MicroForum.Startup.strCnx);
                MicroForum.Startup.comando.Connection.Open();
                MicroForum.Startup.comando.ExecuteNonQuery();
                MicroForum.Startup.comando.Connection.Close();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }            
            return result;
        }
        //
        //--------------------------------------------------------------------
        //
        private static string MontarQuery_IncluirPosts(int _id, int _Id_usuario, string _Titulo, string _Mensagem, DateTime _atualizacao)
        {
            string str = "insert into posts (id_posts,id_usuario,titulo,mensagem,atualizacao) values (";
            str += _id + ",";
            str += _Id_usuario + ",";
            str += "'" + _Titulo + "',";
            str += "'" + _Mensagem + "',";            
            str += "'" + _atualizacao.ToString()+ "')";
            return str;
        }
        //
        //--------------------------------------------------------------------
        //
        private static int RetornarNovoIdPosts()
        {
            int _id = -1;
            try
            {
                //----------  PREPARANDO O ID DO NOVO REGISTRO -------------
                LimparVariaveis();
                MicroForum.Startup.expressaoSQL = string.Empty;
                MicroForum.Startup.expressaoSQL = "SELECT MAX(id_posts) as ID from posts";
                MicroForum.Startup.comando = new SqlCommand();
                MicroForum.Startup.comando.CommandText = string.Empty;
                MicroForum.Startup.comando.CommandText = MicroForum.Startup.expressaoSQL;
                MicroForum.Startup.comando.Connection  = new SqlConnection(MicroForum.Startup.strCnx);
                MicroForum.Startup.comando.Connection.Open();
                object UltimoID = MicroForum.Startup.comando.ExecuteScalar();
                if (UltimoID == DBNull.Value)
                {
                    _id = 1;
                }
                else
                {
                    _id = (int)UltimoID + 1;
                }
            }
            catch (Exception)
            {
                _id = -1;
            }
            MicroForum.Startup.comando.Connection.Close();
            return _id;
        }
        //
        //--------------------------------------------------------------------
        //
        public static SqlDataReader RetornarPosts(string _usuario, int _opcao)
        {

            SqlDataReader dr;    
            
            try
            {
                LimparVariaveis();
                MicroForum.Startup.comando = new SqlCommand();
                MicroForum.Startup.comando.Connection = new SqlConnection(MicroForum.Startup.strCnx);
                string _sSQL = MontarQuery_RetornarPosts(_usuario,_opcao);
                MicroForum.Startup.comando.CommandText = string.Empty;
                MicroForum.Startup.comando.CommandText = _sSQL;
                MicroForum.Startup.comando.Connection.Open();
                dr = MicroForum.Startup.comando.ExecuteReader();                
                return dr;
            }
            catch (Exception)
            {                
            }
            return null;
        }
        //
        //--------------------------------------------------------------------
        //
        private static string MontarQuery_RetornarPosts(string _Usuario,int Opcao)
        {
            int id_usuario = RetornarIdUsuario(_Usuario);
            string str = string.Empty;
            // a opção Zero  vem da chamda da pagina Index, portanto só os 5 ultimos registros do usuario
            if (Opcao == 0)
            {
                str += "SELECT";
                str += " top 5  ";
                str += " id_posts,id_usuario,titulo,mensagem,atualizacao FROM posts ";
                str += " ORDER BY atualizacao DESC";
            }
            else
            {
                str += "SELECT";
                str += " id_posts,id_usuario,titulo,mensagem,atualizacao FROM posts WHERE id_usuario=" + id_usuario;
                str += " ORDER BY atualizacao DESC";
            }
            return str;
        }
        //
        //--------------------------------------------------------------------
        //
        public static SqlDataReader RetornarPost(int _IdPost)
        {
            SqlDataReader dr;
            try
            {
                LimparVariaveis();
                MicroForum.Startup.comando = new SqlCommand();
                MicroForum.Startup.comando.Connection =  new SqlConnection(MicroForum.Startup.strCnx);
                string _sSQL = MontarQuery_RetornarPost(_IdPost);                
                MicroForum.Startup.comando.CommandText = _sSQL;
                MicroForum.Startup.comando.Connection.Open();
                dr = MicroForum.Startup.comando.ExecuteReader();
                return dr;
            }
            catch (Exception)
            {
            }
            return null;
        }
        //
        //--------------------------------------------------------------------
        //
        private static string MontarQuery_RetornarPost(int IdPost)
        {            
            string str = string.Empty;
            str += "SELECT";
            str += " id_posts,id_usuario,titulo,mensagem,atualizacao FROM posts WHERE id_posts=" + IdPost.ToString();
            str += " ORDER BY atualizacao DESC";
            return str;
        }
        //
        //--------------------------------------------------------------------
        //
        private static string MontarQuery_AlterarPost(int _id, string _titulo, string _mensagem, DateTime _atualizacao)
        {
            string str = String.Empty;
            str += "UPDATE posts ";
            str += "SET ";
            str += " titulo ='" + _titulo+"'";
            str += ",mensagem='"  + _mensagem + "'";
            str += ",atualizacao='" + _atualizacao.ToString() + "'";
            str += " WHERE id_posts=" + _id;
            return str;
        }
        //
        //--------------------------------------------------------------------
        //
        public static bool AlterarPosts(int _Id, string _Titulo, string _Mensagem, DateTime _atualizacao)
        {
            bool result = false;
            try
            {
                LimparVariaveis();
                //preparar e executar a gravação do novo registro;
                MicroForum.Startup.expressaoSQL = MontarQuery_AlterarPost(_Id, _Titulo, _Mensagem, _atualizacao);
                MicroForum.Startup.comando = new SqlCommand();
                MicroForum.Startup.comando.CommandText = MicroForum.Startup.expressaoSQL;
                MicroForum.Startup.comando.Connection = new SqlConnection(MicroForum.Startup.strCnx);
                MicroForum.Startup.comando.Connection.Open();
                MicroForum.Startup.comando.ExecuteNonQuery();
                MicroForum.Startup.comando.Connection.Close();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        //
        //--------------------------------------------------------------------
        //
        private static string MontarQuery_ApagarPost(int _idPosts)
        {
            string str = String.Empty;
            str += "DELETE FROM posts ";
            str += " WHERE id_posts=" + _idPosts.ToString();
            return str;
        }
        //
        //--------------------------------------------------------------------
        //
        public static bool ApagarPosts(int Idposts)
        {
            bool result = false;
            try
            {
                LimparVariaveis();
                //preparar e executar a gravação do novo registro;
                MicroForum.Startup.expressaoSQL = MontarQuery_ApagarPost(Idposts);
                MicroForum.Startup.comando = new SqlCommand();
                MicroForum.Startup.comando.CommandText = MicroForum.Startup.expressaoSQL;
                MicroForum.Startup.comando.Connection = new SqlConnection(MicroForum.Startup.strCnx);
                MicroForum.Startup.comando.Connection.Open();
                MicroForum.Startup.comando.ExecuteNonQuery();
                MicroForum.Startup.comando.Connection.Close();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        //
        //--------------------------------------------------------------------
        //
    }
}