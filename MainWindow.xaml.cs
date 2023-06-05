using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private static string getConnectionString()
        {
            string host = "Host=localhost;";
            string port = "Port=5432;";
            string dbName = "Database=farmersMarket;";
            string userName = "Username=postgres;";
            string password = "Password=0000;";

            string connectString = string.Format("{0}{1}{2}{3}{4}",host,port,dbName,userName,password);
            return connectString;
        }

        public static NpgsqlConnection con;
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

        //INDIVIDUAL THREAD
        private void InsertSQL_Click(object sender, RoutedEventArgs e)
        {
            Thread insertThread = new Thread(Insert_Product);
            insertThread.Start();
        }

        public void Insert_Product()
        {
            establishConnection();
            try
            {
                con.Open();
                string Query = "insert into products values(@id, @product_name, @amount, @price)";
                cmd = new NpgsqlCommand(Query, con);
                this.Dispatcher.Invoke(() =>
                {
                    cmd.Parameters.AddWithValue("@id", int.Parse(InsertID.Text));
                    cmd.Parameters.AddWithValue("@product_name", InsertPName.Text);
                    cmd.Parameters.AddWithValue("@amount", int.Parse(InsertAmount.Text));
                    cmd.Parameters.AddWithValue("@price", double.Parse(InsertPrice.Text));
                });
                
                cmd.ExecuteNonQuery();
                MessageBox.Show("Succesfully inserted in the Farmer's Market databse");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //INDIVIDUAL THREAD
        private void DeleteSQL_Click(object sender, RoutedEventArgs e)
        {
            Thread deleteThread = new Thread(Delete_Product);
            deleteThread.Start();
        }

        private void Delete_Product()
        {
            establishConnection();
            try
            {
                con.Open();
                string Query = "Delete from products where id=@id";
                cmd = new NpgsqlCommand(Query, con);
                this.Dispatcher.Invoke(() =>
                {
                    cmd.Parameters.AddWithValue("@id", int.Parse(InsertID.Text));
                });
                cmd.ExecuteNonQuery();
                MessageBox.Show("Succesfully deleted from the Farmer's Market databse");
                con.Close();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //INDIVIDUAL THREAD
        private void UpdateSQL_Click(object sender, RoutedEventArgs e)
        {
            Thread updateThread = new Thread(Update_Product);
            updateThread.Start();
        }

        private void Update_Product()
        {
            establishConnection();
            try
            {
                con.Open();
                string Query = "UPDATE public.products SET product_name=@product_name, amount=@amount, price=@price WHERE id=@id";
                cmd = new NpgsqlCommand(Query, con);
                this.Dispatcher.Invoke(() =>
                {
                    cmd.Parameters.AddWithValue("@product_name", InsertPName.Text);
                    cmd.Parameters.AddWithValue("@amount", int.Parse(InsertAmount.Text));
                    cmd.Parameters.AddWithValue("@price", double.Parse(InsertPrice.Text));
                    cmd.Parameters.AddWithValue("@id", int.Parse(InsertID.Text));
                });
                MessageBox.Show("Succesfully updated in the Farmer's Market databse");
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //INDIVIDUAL THREAD
        private void SelectSQL_Click(object sender, RoutedEventArgs e)
        {
            Thread selectThread = new Thread(Select_Products);
            selectThread.Start();
        }

        private void Select_Products()
        {
            
            establishConnection();
            try
            {
                con.Open();
                string Query = "select * from products";
                cmd = new NpgsqlCommand(Query, con);

                NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                npgsqlDataAdapter.Fill(dt);
                this.Dispatcher.Invoke(() =>
                {
                    dataGrid.ItemsSource = dt.AsDataView();
                    DataContext = npgsqlDataAdapter;
                });
                
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

        //INDIVIDUAL THREAD
        private void  SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread searchThread = new Thread(Search_Product);
            searchThread.Start();
        }

        private async void Search_Product()
        {
            establishConnection();
            try
            {
                con.Open();
                string Query = "select * from products where id=@id";
                // command adapter
                cmd = new NpgsqlCommand(Query, con);
                this.Dispatcher.Invoke(() =>
                {
                    cmd.Parameters.AddWithValue("@id", int.Parse(searchProductID.Text));               
                });
                var reader = await cmd.ExecuteReaderAsync();
                this.Dispatcher.Invoke(() =>
                {
                    InsertID.Text = reader.GetOrdinal("id").ToString();
                    InsertPName.Text = reader.GetOrdinal("product_name").ToString();
                    InsertAmount.Text = reader.GetOrdinal("amount").ToString();
                    InsertPrice.Text = reader.GetOrdinal("price").ToString();
                });
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
