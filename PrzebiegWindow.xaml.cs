using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection.PortableExecutable;
using System.Windows;
using static WynajemAut.MainWindow;

namespace WynajemAut
{
    public partial class PrzebiegWindow : Window
    {
        private string _wynajemId;
        private User _currentUser;

        public PrzebiegWindow(string wynajemId, User currentUser)
        {

            


            
            InitializeComponent();
            _wynajemId = wynajemId;
            _currentUser = currentUser;
        }

        private void Pobierz_Przebieg(object sender, RoutedEventArgs e)
        {
            string connectionString2 = "Server=localhost;Database=LuxCaRent;Integrated Security=True;";

            using (SqlConnection sqlCon2 = new SqlConnection(connectionString2))
            {
                try
                {
                    if (sqlCon2.State == ConnectionState.Closed)
                    {
                        sqlCon2.Open();
                    }


                    string query2 = "SELECT Przebieg FROM Flota2 WHERE id = (SELECT id_auto FROM Wynajem2 WHERE id_wyn = @WynajemId)";
                    SqlCommand sqlCMD2 = new SqlCommand(query2, sqlCon2);
                    sqlCMD2.Parameters.AddWithValue("@WynajemId", _wynajemId);
                    SqlDataReader reader2 = sqlCMD2.ExecuteReader();

                    if (reader2.Read())
                    {
                        PrzebiegTextBox.Text = reader2["Przebieg"].ToString();

                    }
                    reader2.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }


            
        }

        private void Zatwierdz_Click(object sender, RoutedEventArgs e)
        {
            int nowyPrzebieg;
            if (!int.TryParse(PrzebiegTextBox.Text, out nowyPrzebieg))
            {
                MessageBox.Show("Wprowadź prawidłowy przebieg.");
                return;
            }

            string connectionString = "Server=localhost;Database=LuxCaRent;Integrated Security=True;";

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }

                    string updateQuery = "UPDATE Flota2 SET Przebieg = @Przebieg, Dostepny = 1, Akt_Zaj = 0 WHERE id = (SELECT id_auto FROM Wynajem2 WHERE id_wyn = @WynajemId)";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, sqlCon);
                    updateCmd.Parameters.AddWithValue("@Przebieg", nowyPrzebieg);
                    updateCmd.Parameters.AddWithValue("@WynajemId", _wynajemId);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Przebieg i status auta zostały zaktualizowane.");

                        string updateWynajemQuery = "UPDATE Wynajem2 SET Data_praw_odsta = @Today, Odstawione = 1 WHERE id_wyn = @WynajemId";
                        SqlCommand updateWynajemCmd = new SqlCommand(updateWynajemQuery, sqlCon);
                        updateWynajemCmd.Parameters.AddWithValue("@Today", DateTime.Now);
                        updateWynajemCmd.Parameters.AddWithValue("@WynajemId", _wynajemId);


                        updateWynajemCmd.ExecuteNonQuery();

                        string updateUserQuery2 = "UPDATE User2 SET Ile_aut = Ile_aut - 1 WHERE id = (SELECT id_user FROM Wynajem2 WHERE id_wyn = @WynajemId)";
                        SqlCommand updateUserCmd2 = new SqlCommand(updateUserQuery2, sqlCon);
                        updateUserCmd2.Parameters.AddWithValue("@WynajemId", _wynajemId);
                        updateUserCmd2.ExecuteNonQuery();

                        string updateQuery3 = "UPDATE Flota2 SET Dostepny = 1, Akt_Zaj = 0 WHERE id = (SELECT id_auto FROM Wynajem2 WHERE id_wyn = @WynajemId)";
                        SqlCommand updateCmd3 = new SqlCommand(updateQuery3, sqlCon);
                        updateCmd3.Parameters.AddWithValue("@WynajemId", _wynajemId);
                        updateCmd3.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił błąd podczas aktualizacji przebiegu.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            this.Close();
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PrzebiegTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {


 
        }
    }
}
