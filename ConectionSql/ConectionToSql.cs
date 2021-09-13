using System;
using System.Data.SqlClient;

namespace ConectionSql
{
    public class ConectionToSql
    {

        


            //string para referencia del servidor
            private readonly string connectionString;


            //constructor que guarda la referencia
            public ConectionToSql()
            {
                connectionString = "server=DESKTOP-M68C4U0;Database= ClasesYt;Persist Security Info = True; User ID = sa; Password = Estepro_123";

            }


            //funcion que devuelve la coneccion al servidor
            public SqlConnection getConnection()
            {
                return new SqlConnection(connectionString);
            }


        

    }
}
