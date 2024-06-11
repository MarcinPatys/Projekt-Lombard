using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace ProjektLombardWPF_3
{
    public class BaseModel
    {
        int offset = 0;//Zmienna dzięki której wiemy od którego wiersza czytać dane z bazy danych
        DataGridModel dataGridModel1 = new DataGridModel();// tworzymy obiekt klasy DataGridModel abyśmy mogli skorzystać z metody do wypisywania danych do dataGrida

        /// <summary>
        /// Metoda ConnectionBase sprawdza czy istnieje baza danych jeśli nie istnieje tworzy nową
        /// </summary>
        public void ConnectionBase()
        {
            if (!File.Exists("Baza_Uzytkownicy"))// sprawdzamy czy istnieje baza danych
            {
                SQLiteConnection.CreateFile("Baza_Uzytkownicy");//Tworzymy folder Baza_Uzytkownicy
                SQLiteConnection con = new SQLiteConnection("Data Source=Baza_Uzytkownicy");//Tworzymy połączenie z bazą danych podając jej nazwę
                con.Open();//otwieramy połączenie z bazą
                string table = "CREATE TABLE Uzytkownicy(Id integer PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, Imie TEXT, Nazwisko TEXT, PrawoJazdy TEXT, Plec TEXT, Informacje TEXT, PhoneNumber integer)";//Tworzymy nową tabele w bazie
                
                SQLiteCommand Comand = new SQLiteCommand(table, con);//tworzymy obiket Comand klasy SQLiteCommand, konstruktora klsay SQLiteCommand przekazujemy zapytanie które chcemy wykonać oraz na jakiej bazie ma zoastać wykonane zapytanie SQL
                
                Comand.ExecuteNonQuery();//Wykonujemy zapytanie
               

                MessageBox.Show("Utworzono nową bazę");//Wyswietlamy komunikat
                con.Close();//zamyakmy połączenie z baza
            }
            else
            {
                MessageBox.Show("Baza już istnieje");//Jeśli baza istneije wyświetlamuy komunikat
            }
        }
        /// <summary>
        /// Metoda SelectBase odpowiada za wybieranie danych z bazy przyjmuje jeden argument klasy DataGrid
        /// </summary>
        /// <param name="dataGrid1"></param>
        public void SelectBase(DataGrid dataGrid1)
        {

            if (!File.Exists("Baza_Uzytkownicy"))//Sprawdzamy czy istnieje baza 
            {
                ConnectionBase();//wywołanie metody która tworzy bazę
            }

            DataGridModel dataGridModel = new DataGridModel();//tworzymy obiekt klasy DataGridModel abyśmy mogli skorzystać z metody do wypisywania danych do dataGrida
            SQLiteConnection con = new SQLiteConnection("Data Source=Baza_Uzytkownicy");//Tworzymy połączenie z bazą danych podając jej nazwę

            con.Open();//otwieramy połączenie z bazą
            string query = "SELECT Id,Imie,Nazwisko,PhoneNumber FROM Uzytkownicy LIMIT 5";// zmienna query przechowuje zapytanie sql które wybierna Id, imie , naziwsko , numer telefonu z bazy

            SQLiteCommand myComand = new SQLiteCommand(query, con);//tworzymy obiket myComand klasy SQLiteCommand, konstruktora klsay SQLiteCommand przekazujemy zapytanie które chcemy wykonać oraz na jakiej bazie ma zoastać wykonane zapytanie SQL
            SQLiteDataReader result = myComand.ExecuteReader(); // do zmiennej result przypisujemy rezultat wykonenego zapytania

            while (result.Read())//Pela wykonuje się do momentu kiedy przeczyta wszystkie wiersze z bazy danych
            {
                UserModel userModel = new UserModel();//Tworzymy obiekt userModel
                userModel.id = Convert.ToInt32(result[0]);// do pola id przypisujemy wartość pierwszego wiersza w bazie
                userModel.imie = result[1].ToString();// do pola imie przypisujemy wartość drugiego wiersza
                userModel.nazwisko = result[2].ToString();// do pola nazwisko przypisujemy wartość trzeciego wiersz
                userModel.PhoneNumber = Convert.ToInt32(result[3]);// do pola phoneNumber przypisujemy wartość czwartego wiersza
                dataGridModel.WrtieDataGrid(dataGrid1, userModel);//wywołujemy metodę WriteDataGrid do której przekazujmy datagrid oraz osobę z czytanymi wartościami z bazy, metodę wywołujemy aby do dataGrida wypisać osobe
                offset++;// zwiększamy zmienną offset
            }

            con.Close();//Zamykamy połączenie z bazą
        }
        public void AddDataToBase(string Name_txt, string Surname_txt, string _gender, string drivingLicense, string RichTxt, int phoneNumber_int, DataGrid dataGrid)
        {
            string name = Name_txt;
            string surname = Surname_txt;
            string gender = _gender;
            string license = drivingLicense;
            string information = RichTxt;
            int phoneNumber = phoneNumber_int;

            if (name == "" || surname == "")
            {
                MessageBox.Show("Podaj poprawne dane");
                return;
            }
            else
            {

                SQLiteConnection con = new SQLiteConnection("Data Source=Baza_Uzytkownicy");
                con.Open();
                try
                {
                    var cmd = new SQLiteCommand(con);
                    cmd.CommandText = "INSERT INTO Uzytkownicy (Imie, Nazwisko, PrawoJazdy, Plec, Informacje, PhoneNumber) VALUES (@name,@surname,@drivingLicense,@gender,@information,@PhoneNumber)";

                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@surname", surname);
                    cmd.Parameters.AddWithValue("@drivingLicense", license);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@information", information);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    cmd.ExecuteNonQuery();
                    dataGrid.Items.Clear();
                    SelectBase(dataGrid);
                    MessageBox.Show("Dodano nową osobę");
                }
                catch
                {
                    MessageBox.Show("Nie udało się dodanie nowej osoby");
                }

                con.Close();
            }
        }
        public void NextPage(DataGrid dataGrid1)
        {
            if (dataGrid1.Items.Count < 5)
            {

                return;
            }
            else
            {

                DataGridModel dataGridModel = new DataGridModel();
                SQLiteConnection con = new SQLiteConnection("Data Source=Baza_Uzytkownicy");

                con.Open();
                var cmd = new SQLiteCommand(con);
                cmd.CommandText = "SELECT Id,Imie,Nazwisko,PhoneNumber FROM Uzytkownicy LIMIT 5 OFFSET @offset";

                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.ExecuteNonQuery();
                SQLiteDataReader result = cmd.ExecuteReader();
                if (result.HasRows)
                {
                    dataGrid1.Items.Clear();
                    while (result.Read())
                    {
                        UserModel userModel = new UserModel();
                        userModel.id = Convert.ToInt32(result[0]);
                        userModel.imie = result[1].ToString();
                        userModel.nazwisko = result[2].ToString();
                        userModel.PhoneNumber = Convert.ToInt32(result[3]);

                        dataGridModel.WrtieDataGrid(dataGrid1, userModel);
                        offset++;
                    }
                }
            }

        }
        public void PreviousPage(DataGrid dataGrid1)
        {
            if (offset <= 5)
            {

                return;
            }
            else
            {

                DataGridModel dataGridModel = new DataGridModel();
                SQLiteConnection con = new SQLiteConnection("Data Source=Baza_Uzytkownicy");

                con.Open();
                var cmd = new SQLiteCommand(con);
                if (dataGrid1.Items.Count < 5)
                {
                    offset = offset - dataGrid1.Items.Count - 5;
                }
                else
                {
                    offset = offset - 10;
                }
                cmd.CommandText = "SELECT Id,Imie,Nazwisko,PhoneNumber FROM Uzytkownicy LIMIT 5 OFFSET @offset";

                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.ExecuteNonQuery();
                SQLiteDataReader result = cmd.ExecuteReader();
                dataGrid1.Items.Clear();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        UserModel userModel = new UserModel();
                        userModel.id = Convert.ToInt32(result[0]);
                        userModel.imie = result[1].ToString();
                        userModel.nazwisko = result[2].ToString();
                        userModel.PhoneNumber = Convert.ToInt32(result[3]);
                        dataGridModel.WrtieDataGrid(dataGrid1, userModel);
                        offset++;
                    }
                }
            }
        }
        public void DeleteUser(DataGrid dataGrid1)
        {

            int selectRow = dataGrid1.SelectedIndex;
            
            if (selectRow == -1)
            {
                return;
            }
            else
            {
                TextBlock x = dataGrid1.Columns[0].GetCellContent(dataGrid1.Items[selectRow]) as TextBlock;

                string id = x.Text;
                SQLiteConnection con = new SQLiteConnection("Data Source=Baza_Uzytkownicy");

                con.Open();

                var cmd = new SQLiteCommand(con);
                try
                {
                    cmd.CommandText = "DELETE FROM Uzytkownicy WHERE Id=@id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    dataGrid1.Items.Clear();
                    SelectBase(dataGrid1);
                    MessageBox.Show("Usunięto użytkownika");
                }
                catch (Exception)
                {
                    MessageBox.Show("Nie udało się usunąć użytkownika");
                }
            }
        }

        public void WriteDateModyfy(DataGrid dataGrid, modification Modification)
        {

            int selectRow = dataGrid.SelectedIndex;
            MessageBox.Show(selectRow.ToString());
            TextBlock x = dataGrid.Columns[0].GetCellContent(dataGrid.Items[selectRow]) as TextBlock;

            string Id = x.Text;
            if (selectRow == -1)
            {
                return;
            }
            else
            {


                SQLiteConnection con = new SQLiteConnection("Data Source=Baza_Uzytkownicy");
                var cmd = new SQLiteCommand(con);
                con.Open();
                cmd.CommandText = "SELECT * FROM Uzytkownicy WHERE Id=@Id";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Id", Id);
                SQLiteDataReader result = cmd.ExecuteReader();

                while (result.Read())
                {
                    string prawoJazdy;
                    string plec;
                    UserModel userModel = new UserModel();
                    Modification.Name_txt.Text = result[1].ToString();
                    Modification.Surname_txt.Text = result[2].ToString();
                    Modification.phone.Text = result[6].ToString();
                    prawoJazdy = result[3].ToString();
                    plec = result[4].ToString();

                    if (prawoJazdy.Contains(" B1"))
                    {
                        Modification.B1.IsChecked = true;
                    }
                    if (prawoJazdy.Contains(" A"))
                    {
                        Modification.A.IsChecked = true;
                    }
                    if (prawoJazdy.Contains(" A1"))
                    {
                        Modification.A1.IsChecked = true;
                    }
                    if (prawoJazdy.Contains(" A2"))
                    {
                        Modification.A2.IsChecked = true;
                    }
                    if (prawoJazdy.Contains(" C"))
                    {
                        Modification.C.IsChecked = true;
                    }
                    if (prawoJazdy.Contains(" C+E"))
                    {
                        Modification.CE.IsChecked = true;
                    }
                    if (prawoJazdy.Contains(" D1"))
                    {
                        Modification.D1.IsChecked = true;
                    }
                    if (prawoJazdy.Contains(" B1"))
                    {
                        Modification.B1.IsChecked = true;
                    }
                    if (prawoJazdy.Contains(" T"))
                    {
                        Modification.T.IsChecked = true;
                    }
                    if (plec.Contains("Mężczyzna"))
                    {
                        Modification.Mezczynza.IsChecked = true;
                    }
                    if (plec.Contains("Kobieta"))
                    {
                        Modification.Kobieta.IsChecked = true;
                    }
                }

                con.Close();
            }
        }
        public void ModifyBase(string name, string surname, string drivingLicense, string gender, int phoneNumber, DataGrid dataGrid)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=Baza_Uzytkownicy");
            con.Open();
            var cmd = new SQLiteCommand(con);

            int selectRow = dataGrid.SelectedIndex;
            TextBlock x = dataGrid.Columns[0].GetCellContent(dataGrid.Items[selectRow]) as TextBlock;

            string Id = x.Text;
            if (selectRow == -1)
            {
                return;
            }
            else
            {

                cmd.CommandText = "UPDATE Uzytkownicy SET Imie=@name, Nazwisko=@surname, PrawoJazdy=@drivingLicense, Plec=@gender,PhoneNumber=@phoneNumber WHERE Id=@id";

                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.Parameters.AddWithValue("@drivingLicense", drivingLicense);
                cmd.Parameters.AddWithValue("@gender", gender);              
                cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Zmodyfikowano uzytkownika");
            }
            
        }
        public void searchData(DataGrid dataGridView, string search)
        {
            string searchDate = search;

            SQLiteConnection con = new SQLiteConnection("Data Source=Baza_Uzytkownicy");
            var cmd = new SQLiteCommand(con);
            con.Open();


            cmd.CommandText = "SELECT Id, Imie,Nazwisko FROM Uzytkownicy WHERE nazwisko LIKE '%' || @search || '%' LIMIT 1";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@search", searchDate);
            cmd.ExecuteNonQuery();
            SQLiteDataReader result1 = cmd.ExecuteReader();
            while (result1.Read())
            {
                dataGridView.Items.Clear();

                UserModel userModel = new UserModel();

                userModel.id = Convert.ToInt32(result1[0]);
                userModel.imie = result1[1].ToString();
                userModel.nazwisko = result1[2].ToString();

                dataGridModel1.WrtieDataGrid(dataGridView, userModel);
            }
            
        }
        public void ProgresBar()
        {
            
        }
    }
}
