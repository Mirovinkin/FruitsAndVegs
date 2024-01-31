using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace HW1801
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private DbConnection connection;
        private DbProviderFactory factory;
        private string selectedDatabase;
        public string SelectedDatabase
        {
            get => selectedDatabase;
            set
            {
                if (selectedDatabase != value)
                {
                    selectedDatabase = value;
                    OnPropertyChanged(nameof(SelectedDatabase));
                }
            }
        }

        private ObservableCollection<string> databaseList;
        public ObservableCollection<string> DatabaseList
        {
            get => databaseList;
            set
            {
                if (databaseList != value)
                {
                    databaseList = value;
                    OnPropertyChanged(nameof(DatabaseList));
                }
            }
        }

        private ObservableCollection<string> GetDatabaseNamesFromConfig()
        {
            ObservableCollection<string> databaseNames = new ObservableCollection<string>();

            ConnectionStringSettingsCollection connectionStrings = ConfigurationManager.ConnectionStrings;

            foreach (ConnectionStringSettings connectionString in connectionStrings)
            {
                if (!String.IsNullOrEmpty(connectionString.Name))
                {
                    databaseNames.Add(connectionString.Name);
                }
            }

            return databaseNames;
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext= this;

            DatabaseList = GetDatabaseNamesFromConfig();

            /*_connectionString = ConfigurationManager
            .ConnectionStrings["MyConnectionString"]
            .ConnectionString;*/
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("Npgsql", NpgsqlFactory.Instance);

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(selectedDatabase))
                {
                    MessageBox.Show("Выберите провайдера и укажите строку подключения.");
                    return;
                }

                var connectionStringSettings = ConfigurationManager.ConnectionStrings[selectedDatabase];

                if (connectionStringSettings == null)
                {
                    MessageBox.Show($"Не удалось найти строку подключения для провайдера {selectedDatabase}.");
                    return;
                }

                try
                {
                    factory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
                    connection = factory.CreateConnection();
                    connection.ConnectionString = connectionStringSettings.ConnectionString;

                    connection.Open();
                    MessageBox.Show($"Успешное подключение к базе Vegetable And Fruits {connection.ConnectionString}");
                }
                catch (DbException ex)
                {
                    MessageBox.Show($"Не удалось создать подключение {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Возникла ошибка при подключении {ex.Message}");
                }
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindows.Close();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                string query = "SELECT * FROM product";
                var adapter = new NpgsqlDataAdapter(query, conn);
                var ds = new DataSet();
                adapter.Fill(ds, productTableName);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                });
            });


        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                string query = "SELECT name FROM product";
                var adapter = new NpgsqlDataAdapter(query, conn);
                var ds = new DataSet();
                adapter.Fill(ds, productTableName);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                });

            });

        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                string query = "SELECT colour FROM product";
                var adapter = new NpgsqlDataAdapter(query, conn);
                var ds = new DataSet();
                adapter.Fill(ds, productTableName);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                });
            });

        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                string query = "SELECT \"calorie_value\" FROM product ORDER BY \"calorie_value\" DESC LIMIT 1";
                var adapter = new NpgsqlDataAdapter(query, conn);
                var ds = new DataSet();
                adapter.Fill(ds, productTableName);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                });
            });

        }

        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                string query = "SELECT \"calorie_value\" FROM product ORDER BY \"calorie_value\" ASC LIMIT 1";
                var adapter = new NpgsqlDataAdapter(query, conn);
                var ds = new DataSet();
                adapter.Fill(ds, productTableName);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                });
            });

        }

        private async void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                string query = "SELECT AVG(\"calorie_value\")::numeric(10,3) FROM product";
                var adapter = new NpgsqlDataAdapter(query, conn);
                var ds = new DataSet();
                adapter.Fill(ds, productTableName);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                });
            });

        }

        private async void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                string query = "SELECT COUNT(type) FROM product WHERE type LIKE 'Fruit';";
                var adapter = new NpgsqlDataAdapter(query, conn);
                var ds = new DataSet();
                adapter.Fill(ds, productTableName);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                });
            });

        }

        private async void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                string query = "SELECT COUNT(type) FROM product WHERE type LIKE 'Vegetable';";
                var adapter = new NpgsqlDataAdapter(query, conn);
                var ds = new DataSet();
                adapter.Fill(ds, productTableName);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                });
            });

        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                var colour = tb.Text.Length != 0 ? tb.Text.ToString() : "";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                if (colour != "")
                {
                    string query = $"SELECT COUNT(type) FROM product WHERE colour LIKE '{colour}';";
                    var adapter = new NpgsqlDataAdapter(query, conn);
                    var ds = new DataSet();
                    adapter.Fill(ds, productTableName);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                    });
                }
                else
                {
                    MessageBox.Show("Ошибка: вы не ввели цвет");
                }
            });

        }

        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                string query = $"SELECT colour, COUNT(DISTINCT colour) FROM product GROUP BY colour;";
                var adapter = new NpgsqlDataAdapter(query, conn);
                var ds = new DataSet();
                adapter.Fill(ds, productTableName);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                });
            });

        }

        private async void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                var calories = tb2.Text.Length != 0 ? tb2.Text.ToString() : "";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                if (calories != "")
                {
                    string query = $"SELECT name FROM product WHERE calorie_value < {calories};";
                    var adapter = new NpgsqlDataAdapter(query, conn);
                    var ds = new DataSet();
                    adapter.Fill(ds, productTableName);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                    });
                }
                else
                {
                    MessageBox.Show("Ошибка: вы не ввели калории");
                }
            });

        }

        private async void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                var calories = tb2.Text.Length != 0 ? tb2.Text.ToString() : "";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                if (calories != "")
                {
                    string query = $"SELECT name FROM product WHERE calorie_value > {calories};";
                    var adapter = new NpgsqlDataAdapter(query, conn);
                    var ds = new DataSet();
                    adapter.Fill(ds, productTableName);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                    });
                }
                else
                {
                    MessageBox.Show("Ошибка: вы не ввели калории");
                }
            });

        }

        private async void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                var calories = tb2.Text.Length != 0 ? tb2.Text.ToString() : "";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                if (calories != "")
                {
                    string query = $"SELECT name FROM product WHERE calorie_value = {calories};";
                    var adapter = new NpgsqlDataAdapter(query, conn);
                    var ds = new DataSet();
                    adapter.Fill(ds, productTableName);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                    });
                }
                else
                {
                    MessageBox.Show("Ошибка: вы не ввели калории");
                }
            });

        }

        private async void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var productTableName = "product";
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                string query = $"SELECT name FROM product WHERE colour LIKE 'Red' OR colour LIKE 'Yellow';";
                var adapter = new NpgsqlDataAdapter(query, conn);
                var ds = new DataSet();
                adapter.Fill(ds, productTableName);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dg.ItemsSource = ds.Tables[productTableName]?.DefaultView;
                });
            });
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            var connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\User\\source\\repos\\HW1801\\HW1801\\FruitsAndVegs.mdf;Integrated Security=True";
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {
                MessageBox.Show("Success");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

