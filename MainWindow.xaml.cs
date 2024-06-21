using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static WynajemAut.MainWindow;

namespace WynajemAut
{
    public partial class MainWindow : Window
    {
        public User _currentUser;

        public MainWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            DisplayUserData();
        }

        private void DisplayUserData()
        {
            if (_currentUser.Admin == "1")
            {
                LogoName.Content = $"LuxCarRent - Admin";
                Flota2.Content = $"Admin Panel";
                LoadAutaDoPotwierdzenia();
            }
            else
            {
                LogoName.Content = $"LuxCarRent";
                Flota2.Content = $"Flota";
            }

            if (_currentUser.PanPani != "1")
            {
                Pan.Content = "Pani:";
            }

            UpdateUserCarCount();
        }

        private void UpdateUserCarCount()
        {
            string connectionString = "Server=localhost;Database=LuxCaRent;Integrated Security=True;";

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }

                    string query = "SELECT Ile_aut FROM User2 WHERE id = @UserId";
                    SqlCommand cmd = new SqlCommand(query, sqlCon);
                    cmd.Parameters.AddWithValue("@UserId", _currentUser.id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int ileAut = reader.GetInt32(0);
                        TwojeAutaLabel.Content = $"Twoje auta: {ileAut}/2";
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Wyloguj_Click(object sender, RoutedEventArgs e)
        {
            Logowanie dashboard = new Logowanie();
            dashboard.Show();
            this.Close();
        }

        private void Konto_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser.Admin == "1")
            {
                LogoName.Content = $"LuxCarRent - Admin Panel";
            }
            else
            {
                LogoName.Content = $"LuxCarRent - Moje Konto";
            }

            Imie.Content = _currentUser.Imie;
            Nazwisko.Content = _currentUser.Nazwisko;
            Email.Content = _currentUser.Email;
            Telefon.Content = _currentUser.NumerTele;

            flota2cardgrid.Visibility = Visibility.Hidden;
            flota3cardgrid.Visibility = Visibility.Hidden;
            mojekontogrid.Visibility = Visibility.Visible;

            LoadUserCars(Int32.Parse(_currentUser.id));
        }

        private void Flota2_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser.Admin == "1")
            {
                LogoName.Content = $"LuxCarRent - Admin Panel";
                mojekontogrid.Visibility = Visibility.Hidden;
                flota3cardgrid.Visibility = Visibility.Visible;

                LoadAutaDoPotwierdzenia();
            }
            else
            {
                LogoName.Content = $"LuxCarRent - Nasza Flota";
                mojekontogrid.Visibility = Visibility.Hidden;
                flota2cardgrid.Visibility = Visibility.Visible;

                LoadUserCars(0);
            }
        }

        private void LoadUserCars(int userId)
        {
            string connectionString = "Server=localhost;Database=LuxCaRent;Integrated Security=True;";
            List<Flota> flotaList = new List<Flota>();
            List<Flota> oczekujaceList = new List<Flota>();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "";
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }
                    if (userId == 0)
                    {
                        query = "SELECT f.id, f.Marka, f.Model, f.Vin, f.Przebieg, f.Numer, f.Paliwo, f.Poj_Silnika, f.Cena, f.Dostepny, f.Akt_Zaj FROM Flota2 f LEFT JOIN Wynajem2 w ON f.id = w.id_auto";
                    }
                    else if (userId == -1)
                    {
                        query = "SELECT f.id, f.Marka, f.Model, f.Vin, f.Przebieg, f.Numer, f.Paliwo, f.Poj_Silnika, f.Cena, f.Dostepny, f.Akt_Zaj FROM Flota2 f LEFT JOIN Wynajem2 w ON f.id = w.id_auto";
                    }
                    else
                    {
                        query = "SELECT f.id, f.Marka, f.Model, f.Vin, f.Przebieg, f.Numer, f.Paliwo, f.Poj_Silnika, f.Cena, f.Dostepny, f.Akt_Zaj, w.Data_wyp, w.Data_odsta, w.Data_praw_odsta, w.potwierdzenie FROM Flota2 f JOIN Wynajem2 w ON f.id = w.id_auto WHERE w.id_user = @UserId";
                    }

                    SqlCommand sqlCMD = new SqlCommand(query, sqlCon);
                    if (userId > 0)
                    {
                        sqlCMD.Parameters.AddWithValue("@UserId", userId);
                    }
                    SqlDataReader reader = sqlCMD.ExecuteReader();

                    while (reader.Read())
                    {
                        Flota flota = new Flota
                        {
                            id = reader["id"].ToString(),
                            Marka = reader["Marka"].ToString(),
                            Model = reader["Model"].ToString(),
                            Vin = reader["Vin"].ToString(),
                            Przebieg = reader["Przebieg"].ToString(),
                            Numer = reader["Numer"].ToString(),
                            Paliwo = reader["Paliwo"].ToString(),
                            Poj_Silnika = reader["Poj_Silnika"].ToString(),
                            Cena = reader["Cena"].ToString(),
                            Dostepny = reader["Dostepny"].ToString(),
                            Akt_Zaj = reader["Akt_Zaj"].ToString()
                        };

                        if (userId > 0)
                        {
                            flota.Data_wyp = Convert.ToDateTime(reader["Data_wyp"]);
                            flota.Data_odsta = Convert.ToDateTime(reader["Data_odsta"]);
                            flota.Data_praw = Convert.ToDateTime(reader["Data_praw_odsta"]);
                            if (reader["potwierdzenie"].ToString() == "0")
                            {
                                oczekujaceList.Add(flota);
                            }
                            else
                            {
                                flotaList.Add(flota);
                            }
                        }
                        else
                        {
                            flotaList.Add(flota);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            AutaListBox.ItemsSource = flotaList;
            OczekujaceAutaListBox.ItemsSource = oczekujaceList;
            AutaListBox2.ItemsSource = flotaList;
            AutaOczekListBox.ItemsSource = flotaList;
        }

        private void Odstawione_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string wynajemId = button.CommandParameter.ToString();
            var przebiegWindow = new PrzebiegWindow(wynajemId, _currentUser);
            przebiegWindow.ShowDialog();
            LoadUserCars(0);
        }

        private void Wynajmij_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string carId = button.CommandParameter.ToString();
            RentCar(carId);
        }

        private void RentCar(string carId)
        {
            KalendarzWindow kalendarzWindow = new KalendarzWindow(_currentUser, carId);
            kalendarzWindow.ShowDialog();
            LoadUserCars(0);
        }

        private void Akceptuj_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string wynajemId = button.CommandParameter.ToString();
            UpdateWynajemStatus(wynajemId, 1);
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string wynajemId = button.CommandParameter.ToString();
            UpdateWynajemStatus(wynajemId, -1);
        }

        private void UpdateWynajemStatus(string wynajemId, int status)
        {
            string connectionString = "Server=localhost;Database=LuxCaRent;Integrated Security=True;";

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }

                    string query = "";
                    if (status == 1)
                    {
                        query = "UPDATE Wynajem2 SET potwierdzenie = 1, id_admin = @AdminId WHERE id_wyn = @WynajemId";
                        SqlCommand cmd = new SqlCommand(query, sqlCon);
                        cmd.Parameters.AddWithValue("@WynajemId", wynajemId);
                        cmd.Parameters.AddWithValue("@AdminId", _currentUser.id);
                        cmd.ExecuteNonQuery();

                        string updateUserQuery = "UPDATE User2 SET Ile_aut = Ile_aut + 1 WHERE id = (SELECT id_user FROM Wynajem2 WHERE id_wyn = @WynajemId)";
                        SqlCommand updateUserCmd = new SqlCommand(updateUserQuery, sqlCon);
                        updateUserCmd.Parameters.AddWithValue("@WynajemId", wynajemId);
                        updateUserCmd.ExecuteNonQuery();

                        MessageBox.Show("Wynajem został zaakceptowany.");
                    }
                    else
                    {


                        string updateUserQuery = "UPDATE Flota2 SET Dostepny = 1, Akt_Zaj = 0 WHERE id = (SELECT id_auto FROM Wynajem2 WHERE id_wyn = @WynajemId)";
                        SqlCommand updateUserCmd = new SqlCommand(updateUserQuery, sqlCon);
                        updateUserCmd.Parameters.AddWithValue("@WynajemId", wynajemId);
                        updateUserCmd.ExecuteNonQuery();

                        query = "DELETE FROM Wynajem2 WHERE id_wyn = @WynajemId";
                        SqlCommand cmd = new SqlCommand(query, sqlCon);
                        cmd.Parameters.AddWithValue("@WynajemId", wynajemId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Wynajem został anulowany.");
                    }

                    LoadAutaDoPotwierdzenia();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void LoadAutaDoPotwierdzenia()
        {
            string connectionString = "Server=localhost;Database=LuxCaRent;Integrated Security=True;";
            List<Wynajem> wynajemList = new List<Wynajem>();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }

                    string query = "SELECT w.id_wyn, w.id_user, w.id_auto, w.Data_wyp, w.Data_odsta, w.Data_praw_odsta, w.potwierdzenie, w.Odstawione, f.Cena FROM Wynajem2 w JOIN Flota2 f ON w.id_auto = f.id";
                    SqlCommand sqlCMD = new SqlCommand(query, sqlCon);
                    SqlDataReader reader = sqlCMD.ExecuteReader();

                    while (reader.Read())
                    {
                        Wynajem wynajem = new Wynajem
                        {
                            id_wyn = reader["id_wyn"].ToString(),
                            id_user = reader["id_user"].ToString(),
                            id_auto = reader["id_auto"].ToString(),
                            Data_wyp = Convert.ToDateTime(reader["Data_wyp"]),
                            Data_odsta = Convert.ToDateTime(reader["Data_odsta"]),
                            Data_praw = Convert.ToDateTime(reader["Data_praw_odsta"]),
                            potwierdzenie = reader.GetInt32(reader.GetOrdinal("potwierdzenie")),
                            Cena = reader.GetInt32(reader.GetOrdinal("Cena")),
                            AkceptujVisibility = reader.GetInt32(reader.GetOrdinal("potwierdzenie")) == 0 ? Visibility.Visible : Visibility.Collapsed,
                            AnulujVisibility = reader.GetInt32(reader.GetOrdinal("potwierdzenie")) == 0 ? Visibility.Visible : Visibility.Collapsed,
                            OdstawioneVisibility = reader.GetInt32(reader.GetOrdinal("potwierdzenie")) == 1 & reader.GetInt32(reader.GetOrdinal("Odstawione")) == 0 ? Visibility.Visible : Visibility.Collapsed,
                            DodatkowaOplata = reader.GetInt32(reader.GetOrdinal("potwierdzenie")) == 1 ? CalculateLateFee(reader.GetDateTime(reader.GetOrdinal("Data_odsta")), reader.GetInt32(reader.GetOrdinal("Cena"))) : 0
                        };

                        wynajemList.Add(wynajem);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            AutaOczekListBox.ItemsSource = wynajemList;
        }

        private int CalculateLateFee(DateTime dueDate, int dailyRate)
        {
            int daysLate = (DateTime.Now - dueDate).Days;
            if (daysLate > 0)
            {
                return daysLate * dailyRate + 200;
            }
            return 0;
        }

        public class Flota
        {
            public string id { get; set; }
            public string Marka { get; set; }
            public string Model { get; set; }
            public string Vin { get; set; }
            public string Przebieg { get; set; }
            public string Numer { get; set; }
            public string Paliwo { get; set; }
            public string Poj_Silnika { get; set; }
            public string Cena { get; set; }
            public DateTime Data_wyp { get; set; }
            public DateTime Data_odsta { get; set; }
            public DateTime Data_praw { get; set; }
            public string Akt_Zaj { get; set; }
            public string Dostepny { get; set; }

            public string LiczbaDni
            {
                get
                {
                    int daysRemaining = (Data_odsta - DateTime.Now.Date).Days;
                    return daysRemaining <= 0 ? "0" : daysRemaining.ToString();
                }
                set { }
            }

            public int Cena_za_msc
            {
                get
                {
                    int daysRemaining = (Data_odsta - Data_wyp).Days;
                    int cena = Int32.Parse(Cena) * daysRemaining;
                    return cena;
                }
                set { }
            }

            public string Odstawienie
            {
                get
                {
                    if (Dostepny == "1")
                    {
                        return "Tak";
                    }
                    int daysRemaining = (Data_odsta - DateTime.Now.Date).Days;
                    return daysRemaining <= 0 ? "Tak" : "Nie";
                }
                set { }
            }

            public string kiedy
            {
                get
                {

                        return "Tak";
 
                }
                set { }
            }

            public string zajete
            {
                get
                {
                    if (Dostepny == "1")
                    {
                        return "był";
                    }
                    int daysRemaining = (Data_odsta - DateTime.Now.Date).Days;
                    return daysRemaining <= 0 ? "był" : "jest";
                }
                set { }
            }

            public string zajete2
            {
                get
                {
                    if (Dostepny == "1")
                    {
                        return "";
                    }
                    int daysRemaining = (Data_odsta - DateTime.Now.Date).Days;
                    return daysRemaining <= 0 ? "" : "od";
                }
                set { }
            }
        }

        public class Wynajem
        {
            public string id_wyn { get; set; }
            public string id_user { get; set; }
            public string id_auto { get; set; }
            public DateTime Data_wyp { get; set; }
            public DateTime Data_odsta { get; set; }
            public DateTime Data_praw { get; set; }
            public int potwierdzenie { get; set; }
            public int Cena { get; set; }
            public int DodatkowaOplata { get; set; }
            public Visibility AkceptujVisibility { get; set; }
            public Visibility AnulujVisibility { get; set; }
            public Visibility OdstawioneVisibility { get; set; }
        }
    }
}
