using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace FarmersMarket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        //Step 1. Database connection: connection String
        private static string getConnectionString()
        {
            //Dont add spaces to the string or it wont work
            string host = "Host=localhost;";
            string port = "Port=5432;";
            string dbName = "Database=farmersMarket;";
            string userName = "Username=postgres;";
            string password = "Password=0000;";

            string connectString = string.Format("{0}{1}{2}{3}{4}",host,port,dbName,userName,password);
            return connectString;
        }

        //Step 2. Database connection: establish connection

        //Connection adapter: to help connect to postgresql database
        public static NpgsqlConnection con;
        //Command adapter: helps to send/execute commands in the database
        public static NpgsqlCommand cmd;

        private static void establishConnection()
        {
            try
            {
                con = new NpgsqlConnection(getConnectionString());
            }catch(NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void InsertSQL_Click(object sender, RoutedEventArgs e)
        {
            establishConnection();
            try
            {
                con.Open();
                string Query = "insert into products values(@id, @product_name, @amount, @price)";
                cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@id", int.Parse(InsertID.Text));
                cmd.Parameters.AddWithValue("@product_name", InsertPName.Text);
                cmd.Parameters.AddWithValue("@amount",int.Parse(InsertAmount.Text));
                cmd.Parameters.AddWithValue("@price",double.Parse(InsertPrice.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Succesfully inserted in the Farmer's Market databse");
            }catch(NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteSQL_Click(object sender, RoutedEventArgs e)
        {
            establishConnection();
            try
            {
                con.Open();
                string Query = "Delete from products where id=@id";
                cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@id",int.Parse(InsertID.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Succesfully deleted from the Farmer's Market databse");
                con.Close();

            }catch(NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateSQL_Click(object sender, RoutedEventArgs e)
        {
            establishConnection();
            try
            {
                con.Open();
                string Query = "UPDATE public.products SET product_name=@product_name, amount=@amount, price=@price WHERE id=@id"; 
                cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@product_name", InsertPName.Text);
                cmd.Parameters.AddWithValue("@amount", int.Parse(InsertAmount.Text));
                cmd.Parameters.AddWithValue("@price", double.Parse(InsertPrice.Text));
                cmd.Parameters.AddWithValue("@id", int.Parse(InsertID.Text));
                MessageBox.Show("Succesfully updated in the Farmer's Market databse");
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectSQL_Click(object sender, RoutedEventArgs e)
        {
            // Establish the Connection
            establishConnection();
            // Open the Connection
            try
            {
                con.Open();
                string Query = "select * from products";
                cmd = new NpgsqlCommand(Query, con);
                /*
                 * as we are going to view our data table entries in GridView, we need
                 * a dataapater to pull the data entries and view them in the GridView
                 In the data adapter, you need to pass the command adpater you have created
                for your program. here, cmd is our commandadapter
                 
                After having the dataadpater, we need to create a datatable instance and
                add all our pulled record to that table, We need these table to 
                information our datagridview to know what columns and rows its going to view
                as table
                 */
                NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                npgsqlDataAdapter.Fill(dt); // this line helps to retrive the table
                // we have created using the Data adapter, pass the structure to our
                // regular datatable and get all the values to that table

                // The following line will help us to add the values to the dataGrid view
                dataGrid.ItemsSource = dt.AsDataView();

                //This will perform the dynamic binding, It will update the grid with the
                // passed data table information
                DataContext = npgsqlDataAdapter;
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sales_Click(object sender, RoutedEventArgs e)
        {   
            Sale newSale = new Sale();
            newSale.Show();
            this.Close();   
        }

        
        private async void  SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            establishConnection();
            try
            {
                con.Open();
                string Query = "select * from products where id=@id";
                // command adapter
                cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@id", int.Parse(searchProductID.Text));

                
                 /* in this program, we are going to add a reader to read one entry from the 
                 database*/
                 
                var reader = await cmd.ExecuteReaderAsync();

                InsertID.Text = reader.GetOrdinal("id").ToString();
                InsertPName.Text = reader.GetOrdinal("product_name").ToString();
                InsertAmount.Text = reader.GetOrdinal("amount").ToString();
                InsertPrice.Text = reader.GetOrdinal("price").ToString();
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
