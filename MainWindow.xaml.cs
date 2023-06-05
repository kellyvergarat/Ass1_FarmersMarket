using System;
using System.Collections.Generic;
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
                string Query = "insert into products values(@id, @productName, @amount, @price)";
                cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@id", int.Parse(InsertID.Text));
                cmd.Parameters.AddWithValue("@productName", InsertPName.Text);
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

        }

        private void UpdateSQL_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectSQL_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
