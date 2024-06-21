using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace WynajemAut
{
    public partial class KalendarzWindow : Window
    {
        private User _currentUser;
        private string _carId;
        private List<(DateTime Start, DateTime End)> occupiedDates = new List<(DateTime Start, DateTime End)>();
        private int cenaZaDzien;

        public KalendarzWindow(User currentUser, string carId)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _carId = carId;
            LoadCalendarData();
            StartDatePicker.SelectedDateChanged += StartDatePicker_SelectedDateChanged;
            EndDatePicker.SelectedDateChanged += EndDatePicker_SelectedDateChanged;
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateDates();
            UpdateCena();
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateDates();
            UpdateCena();
        }

        private void ValidateDates()
        {
            DateTime startDate = StartDatePicker.SelectedDate ?? DateTime.Now;
            DateTime endDate = EndDatePicker.SelectedDate ?? DateTime.Now;

            foreach (var dateRange in occupiedDates)
            {
                if ((startDate <= dateRange.End && endDate >= dateRange.Start) || endDate < startDate)
                {
                    MessageBox.Show("Wybrane daty kolidują z istniejącym wynajmem lub są nieprawidłowe. Proszę wybrać inne daty.");
                    return;
                }
            }
        }

        private void UpdateCena()
        {
            DateTime startDate = StartDatePicker.SelectedDate ?? DateTime.Now;
            DateTime endDate = EndDatePicker.SelectedDate ?? DateTime.Now;

            if (endDate <= startDate)
            {
                CenaLabel.Content = "Cena za wynajem: 0 PLN";
                return;
            }

            int dni = (endDate - startDate).Days;
            int cena = dni * cenaZaDzien;
            CenaLabel.Content = $"Cena za wynajem: {cena} PLN";
        }

        private void LoadCalendarData()
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

                    string query = "SELECT id_user, Data_wyp, Data_odsta FROM Wynajem2 WHERE id_auto = @CarId";
                    SqlCommand sqlCMD = new SqlCommand(query, sqlCon);
                    sqlCMD.Parameters.AddWithValue("@CarId", _carId);
                    SqlDataReader reader = sqlCMD.ExecuteReader();

                    while (reader.Read())
                    {
                        string userId = reader["id_user"].ToString();
                        DateTime start = Convert.ToDateTime(reader["Data_wyp"]);
                        DateTime end = Convert.ToDateTime(reader["Data_odsta"]);
                        occupiedDates.Add((start, end));
                    }
                    reader.Close();

                    query = "SELECT Cena FROM Flota2 WHERE id = @CarId";
                    sqlCMD = new SqlCommand(query, sqlCon);
                    sqlCMD.Parameters.AddWithValue("@CarId", _carId);
                    reader = sqlCMD.ExecuteReader();

                    if (reader.Read())
                    {
                        cenaZaDzien = reader.GetInt32(0);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            List<string> occupiedDateStrings = new List<string>();
            int i = 0;
            foreach (var dateRange in occupiedDates)
            {
                i++;
                occupiedDateStrings.Add($"{i}. Zajęte od {dateRange.Start:dd.MM.yyyy} do {dateRange.End:dd.MM.yyyy}");
            }

            ListaZajetych.ItemsSource = occupiedDateStrings;
        }

        private void PotwierdzWynajem_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDate = StartDatePicker.SelectedDate ?? DateTime.Now;
            DateTime endDate = EndDatePicker.SelectedDate ?? DateTime.Now;

            if (endDate <= startDate || IsDateOccupied(startDate, endDate))
            {
                MessageBox.Show("Wybrane daty kolidują z istniejącym wynajmem lub są nieprawidłowe. Proszę wybrać inne daty.");
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

                    string insertQuery = "INSERT INTO Wynajem2 (id_user, id_auto, Data_wyp, Data_odsta, potwierdzenie, id_admin, Data_praw_odsta) VALUES (@UserId, @CarId, @StartDate, @EndDate, 0, NULL, @EndDate)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, sqlCon);
                    insertCmd.Parameters.AddWithValue("@UserId", _currentUser.id);
                    insertCmd.Parameters.AddWithValue("@CarId", _carId);
                    insertCmd.Parameters.AddWithValue("@StartDate", startDate);
                    insertCmd.Parameters.AddWithValue("@EndDate", endDate);

                    int rowsAffected = insertCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Wynajem został potwierdzony.");
                        UpdateCarStatus();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił błąd podczas potwierdzania wynajmu.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private bool IsDateOccupied(DateTime startDate, DateTime endDate)
        {
            foreach (var dateRange in occupiedDates)
            {
                if (startDate <= dateRange.End && endDate >= dateRange.Start)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateCarStatus()
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

                    string updateQuery = "UPDATE Flota2 SET Dostepny = 0, Akt_Zaj = @UserId WHERE id = @CarId";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, sqlCon);
                    updateCmd.Parameters.AddWithValue("@CarId", _carId);
                    updateCmd.Parameters.AddWithValue("@UserId", _currentUser.id);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Status auta został zaktualizowany.");
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił błąd podczas aktualizacji statusu auta.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void Wroc_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
