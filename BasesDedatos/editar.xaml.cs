using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BasesDedatos
{
    /// <summary>
    /// Lógica de interacción para editar.xaml
    /// </summary>
    public partial class editar : Window
    {
        public int IdClient;
        public string name;
        public string direccion;
        public string telefono;
        public string poblacion;

        public editar()
        {
            InitializeComponent();
        }



        public void Button_Click(object sender, RoutedEventArgs e)
        {
            if (name != Boxname.Text || direccion != Boxdireccion.Text || telefono != Boxtelefono.Text || poblacion != Boxpoblacion.Text)
            {

                using (var conection = new ConectionSql.ConectionToSql().getConnection())
                {
                    conection.Open();

                    string consulta = "UPDATE clientes SET nombre = @Nnombre, Direccion = @Ndireccion, Poblacion =@Npoblacion, Telefono = @Ntelefono WHERE Id = @clientId";


                    using (SqlCommand comand = new SqlCommand(consulta, conection))
                    {

                        comand.Parameters.AddWithValue("@Nnombre", Boxname.Text);
                        comand.Parameters.AddWithValue("@Ndireccion", Boxdireccion.Text);
                        comand.Parameters.AddWithValue("@Npoblacion", Boxpoblacion.Text);
                        comand.Parameters.AddWithValue("@Ntelefono", Boxtelefono.Text);
                        comand.Parameters.AddWithValue("@clientId", IdClient);


                        comand.ExecuteNonQuery();


                    }

                    MessageBox.Show("Informacion Actualizada");

                    this.Close();

                }


            }
            else
            {
                MessageBox.Show("No has modificado ningun dato");
            }


        }


    }
}
