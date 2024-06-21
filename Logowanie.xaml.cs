using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WynajemAut
{
    public partial class Logowanie : Window
    {
        public Logowanie()
        {
            InitializeComponent();
        }

        private void LogowanieSys_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string haslo = Haslo.Password;
            string connectionString = "Server=localhost;Database=LuxCaRent;Integrated Security=True;";

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }
                    string query = "SELECT * FROM [User2] WHERE Login=@Login AND Haslo=@Haslo";
                    SqlCommand sqlCMD = new SqlCommand(query, sqlCon);
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.Parameters.AddWithValue("@Login", login);
                    sqlCMD.Parameters.AddWithValue("@Haslo", haslo);
                    SqlDataReader reader = sqlCMD.ExecuteReader();
                    if (reader.Read())
                    {
                        User user = new User
                        {
                            id = reader["id"].ToString(),
                            Login = reader["Login"].ToString(),
                            Admin = reader["Admin"].ToString(),
                            Email = reader["email"].ToString(),
                            Imie = reader["Imie"].ToString(),
                            Nazwisko = reader["Nazwisko"].ToString(),
                            NumerTele = reader["NumerTele"].ToString(),
                            PanPani = reader["PanPani"].ToString()
                        };

                        MainWindow dashboard = new MainWindow(user);
                        dashboard.Show();
                        this.Close();
                    }
                    else
                    {
                        errorlogin.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void RejestracjaSys_Click(object sender, RoutedEventArgs e)
        {
            string imie = RegImie.Text;
            string nazwisko = RegNazwisko.Text;
            string email = RegEmail.Text;
            string telefon = RegTelefon.Text;
            string login = RegLogin.Text;
            string haslo = RegHaslo.Password;
            string haslopot = RegHasloPot.Password;
            string panPani = RadioPan.IsChecked == true ? "1" : "0";
            string connectionString = "Server=localhost;Database=LuxCaRent;Integrated Security=True;";

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }

                    
                    string checkQuery = "SELECT COUNT(*) FROM [User2] WHERE email=@Email OR NumerTele=@NumerTele OR Login=@Login23";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, sqlCon);
                    checkCmd.Parameters.AddWithValue("@Email", email);
                    checkCmd.Parameters.AddWithValue("@NumerTele", telefon);
                    checkCmd.Parameters.AddWithValue("@Login23", login);
                    int userCount = (int)checkCmd.ExecuteScalar();

                    if(haslo != haslopot)
                    {
                        MessageBox.Show("Hasła muszą być takie same!");
                        return;
                    }

                    if (userCount > 0)
                    {
                        MessageBox.Show("E-mail,numer telefonu lub login są już używane!");
                        return;
                    }

                    string query = "INSERT INTO [User2] (Login, Haslo, Admin, email, Imie, Nazwisko, NumerTele, PanPani) VALUES (@Login, @Haslo, @Admin, @Email, @Imie, @Nazwisko, @NumerTele, @PanPani)";
                    SqlCommand sqlCMD = new SqlCommand(query, sqlCon);
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.Parameters.AddWithValue("@Login", login);
                    sqlCMD.Parameters.AddWithValue("@Haslo", haslo);
                    sqlCMD.Parameters.AddWithValue("@Admin", "0");
                    sqlCMD.Parameters.AddWithValue("@Email", email);
                    sqlCMD.Parameters.AddWithValue("@Imie", imie);
                    sqlCMD.Parameters.AddWithValue("@Nazwisko", nazwisko);
                    sqlCMD.Parameters.AddWithValue("@NumerTele", telefon);
                    sqlCMD.Parameters.AddWithValue("@PanPani", panPani);

                    int rowsAffected = sqlCMD.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Rejestracja zakończona sukcesem!");
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił błąd podczas rejestracji.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnLogowanie_Click(object sender, RoutedEventArgs e)
        {
            WyborPanel.Visibility = Visibility.Hidden;
            LogowaniePanel.Visibility = Visibility.Visible;
        }

        private void BtnRejestracja_Click(object sender, RoutedEventArgs e)
        {
            WyborPanel.Visibility = Visibility.Hidden;
            RejestracjaPanel.Visibility = Visibility.Visible;
        }

        private void BtnWroc_Click(object sender, RoutedEventArgs e)
        {
            LogowaniePanel.Visibility = Visibility.Hidden;
            RejestracjaPanel.Visibility = Visibility.Hidden;
            WyborPanel.Visibility = Visibility.Visible;
        }
    }
}
