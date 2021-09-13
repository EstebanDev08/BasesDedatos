
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using ConectionSql;




namespace BasesDedatos
{

  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {


        public MainWindow()
        {
            InitializeComponent();

            muestraclientes();
            mostrarAllPedidos();


        }





        private  ConectionToSql conectionToSql = new();


        public void muestraclientes()
        {


            string consulta = "select *, concat('nombre: ',nombre,' ciudad: ',Poblacion,'') as info from clientes";

            SqlDataAdapter miSqlDataAdapter = new SqlDataAdapter(consulta, conectionToSql.getConnection());


            using (miSqlDataAdapter)
            {
                DataTable clientesDataTable = new DataTable();
                miSqlDataAdapter.Fill(clientesDataTable);
                ListBoxclientes.DisplayMemberPath = "info";
                ListBoxclientes.SelectedValuePath = "Id";
                ListBoxclientes.ItemsSource = clientesDataTable.DefaultView;
            }

        }



        public void muestraPedidos()
        {
            var id = ListBoxclientes.SelectedValue;

            string consulta = "select * from Pedido where cCliente=@idClient";

            var conection = new ConectionToSql().getConnection();

            SqlCommand command = new SqlCommand(consulta, conection);

            SqlDataAdapter miSqlDataAdapter = new SqlDataAdapter(command);

            using (miSqlDataAdapter)
            {

                command.Parameters.AddWithValue("@idClient", id);

                DataTable pedidoDataTable = new DataTable();

                

                miSqlDataAdapter.Fill(pedidoDataTable);


                ListBoxPedidos.DisplayMemberPath = "fechaPedido";
                ListBoxPedidos.SelectedValuePath = "Id";
                ListBoxPedidos.ItemsSource = pedidoDataTable.DefaultView;
                

            }


        }


        public void mostrarAllPedidos()
        {

            string consulta = "select *from Pedido";

            SqlDataAdapter miSqlDataAdapter = new SqlDataAdapter(consulta, conectionToSql.getConnection());


            using (miSqlDataAdapter)
            {
                DataTable allPedidosDataTable = new DataTable();
                miSqlDataAdapter.Fill(allPedidosDataTable);

                listaAllPedidos.DisplayMemberPath = "Id";
                listaAllPedidos.SelectedValuePath = "Id";
                listaAllPedidos.ItemsSource = allPedidosDataTable.DefaultView;
            }

        }

        private void BorrarButton_Click(object sender, RoutedEventArgs e)
        {
            if (listaAllPedidos.SelectedValue != null)
            {
                var msm = MessageBox.Show("esta seguro de eliminar?", "Borrar Elemento", MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (msm == MessageBoxResult.Yes)
                {
                    
                    string consulta = "Delete from Pedido where Id=@idPedido";

                    var conection = new ConectionToSql().getConnection();
                    SqlCommand comando = new SqlCommand(consulta, conection);

                    using (conection)
                    {
                        conection.Open();

                        comando.Parameters.AddWithValue("@idPedido", listaAllPedidos.SelectedValue);
                        comando.ExecuteNonQuery();

                        conection.Close();

                        muestraPedidos();
                        mostrarAllPedidos();
                        
                    }
                }



            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Insert into Pedido values (@idCliente,@fecha,@formaPago)";

                var conection = new ConectionToSql().getConnection();
                SqlCommand comando = new SqlCommand(consulta, conection);

                using (conection)
                {
                    conection.Open();

                    comando.Parameters.AddWithValue("@idCliente", int.Parse(TBcliente.Text));
                    comando.Parameters.AddWithValue("@fecha", DpFechaPago.SelectedDate);
                    comando.Parameters.AddWithValue("@formaPago", TbFormaPago.Text);

                    comando.ExecuteNonQuery();

                    conection.Close();

                    TBcliente.Clear();
                    TbFormaPago.Clear();
                    DpFechaPago.SelectedDate = null;


                    mostrarAllPedidos();
                    muestraPedidos();

                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("los datos introducidos son invalidos");
            }
            
              
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (ListBoxclientes.SelectedValue != null)
            {


                editar frmEditar = new editar();
                frmEditar.Closing += cerrando;




                using (var conection = new ConectionToSql().getConnection()) 
                {

                    conection.Open();

                    string consulta = "select * from clientes where Id=@idcliente";
                    


                    using (SqlCommand comand = new SqlCommand(consulta, conection))
                    {

                        

                        comand.Parameters.AddWithValue("@idcliente", ListBoxclientes.SelectedValue);
                        SqlDataReader reader = comand.ExecuteReader();


                        if (reader.HasRows)
                        {


                            while (reader.Read())
                            {
                                frmEditar.name = reader.GetString(1);
                                frmEditar.direccion = reader.GetString(2);
                                frmEditar.poblacion = reader.GetString(3);
                                frmEditar.telefono = reader.GetString(4);
                                


                                frmEditar.Boxname.Text = reader.GetString(1);
                                frmEditar.Boxdireccion.Text = reader.GetString(2);
                                frmEditar.Boxpoblacion.Text = reader.GetString(3);
                                frmEditar.Boxtelefono.Text = reader.GetString(4);
                                frmEditar.IdClient = reader.GetInt32(0);

                            }

                            frmEditar.ShowDialog();

                        }
                        
                        

                    } 
                    
                    
                    
                    


                }
                
            }
            else
            {
                MessageBox.Show("selecciona un cliente para editar");
            }

            
        }

        private void cerrando(object sender, CancelEventArgs e)
        {
            muestraclientes();
        }

        private void ListBoxclientes_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            muestraPedidos();
        }
    }
}
