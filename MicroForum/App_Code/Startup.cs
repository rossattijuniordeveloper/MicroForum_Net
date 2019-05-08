using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace MicroForum
{

    public class Startup
    {
        // variaveis globais
        //---------------------------------------------------------
        //  
        public string pastaBD;
        public static string nomeBD;
        public static string strCnx;
        public static SqlConnection Ligacao;
        public static SqlCommand comando;
        public static string expressaoSQL;
    }

    //
    //---------------------------------------------------------
    //

}
