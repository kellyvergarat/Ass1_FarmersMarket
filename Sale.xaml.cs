using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
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

namespace FarmersMarket
{
    /// <summary>
    /// Interaction logic for Sale.xaml
    /// </summary>
    public partial class Sale : Window
    {

        public static NpgsqlConnection con;
        public static NpgsqlCommand cmd;
        private DataGrid salesDataGrid { set; get; }
        private DataTable productsTable;
    

        public Sale()
        {

            // Initialize database connection
            string connectionString = "Host=localhost;Port=5432;Database=farmersMarket;Username=postgres;Password=0000;";
            con = new NpgsqlConnection(connectionString);

            productsTable = new DataTable();

            InitializeComponent();
           
            LoadProductsData();

        }

        private static string getConnectionString()
        {
            string connectString = "Host=localhost;Port=5432;Database=farmersMarket;Username=postgres;Password=0000;";
            return connectString;
        }


        private static void establishConnection()
        {
            try
            {
                con = new NpgsqlConnection(getConnectionString());
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                System.Windows.Application.Current.Shutdown();
            }
        }

        public void LoadProductsData()
        {
            try
            {
                con.Open();
                string query = "SELECT id AS Product_ID, product_name AS Product_Name, price AS Price, amount AS Amount_Desired FROM products";
                cmd = new NpgsqlCommand(query, con);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(productsTable);           

                foreach (DataRow row in productsTable.Rows)
                {
                    row["Amount_Desired"] = 0;
                }
                salesDataGrid = dataGridSale;
                salesDataGrid.ItemsSource = productsTable.DefaultView;

                DataContext = adapter;
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
            double total = 0;

            foreach (DataRow row in productsTable.Rows)
            {
                double amount = Convert.ToDouble(row["Amount_Desired"]);
                double price = Convert.ToDouble(row["Price"]);
                total += amount * price;
            }

            showTotal.Content = "$ " + total.ToString("0.00");
        }


        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {
            string customerName = search.Text;
            string message = $"Thank you, {customerName}. Your purchase was successfully processed.";

            try
            {
                con.Open();

                foreach (DataRow row in productsTable.Rows)
                {
                    int productId = Convert.ToInt32(row["Product_ID"]);
                    int amountDesired = Convert.ToInt32(row["Amount_Desired"]);

                    // Update the database with the deducted amount
                    string updateQuery = "UPDATE products SET amount = amount - @amountDesired WHERE id = @productId";
                    cmd = new NpgsqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@amountDesired", amountDesired);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show(message);

                // Clear the amount column in the data grid
                foreach (DataRow row in productsTable.Rows)
                {
                    row["Amount_Desired"] = 0;
                }

                // Refresh the data grid
                salesDataGrid.Items.Refresh();
                showTotal.Content = "$ 0.0 ";
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void _return_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Close();
        }
    }
}
