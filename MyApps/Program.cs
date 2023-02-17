using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApps
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source = MyDatabase.sqlite;Version=3;";
            bool exists = false;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    exists = true;
                }
                catch (SQLiteException)
                {
                    // Базы данных не существует
                    exists = false;
                }
            }
            String connect_string = "Data Source = MyDatabase.sqlite; Version=3;";
            if (exists)
            {
                Console.WriteLine("Выберите пункт от 1 до 5");
                int Case = Convert.ToInt32(Console.ReadLine());
                switch (Case)
                {
                    case 1:
                        Console.WriteLine("Пункт создания таблицы");                        
                        

                        try
                        {
                            using (SQLiteConnection conn = new SQLiteConnection(connect_string))
                            {
                                conn.Open();
                                using (SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE People " +
                                    "(ID int IDENTITY(1,1) PRIMARY KEY, FirstName nvarchar(50), LastName nvarchar(50), " +
                                    "SurName nvarchar(50), DateOfBirth date, Gender nvarchar(10))", conn))
                                {
                                    cmd.ExecuteNonQuery();
                                }


                            }
                            Console.WriteLine("Table created successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error creating table: " + ex.Message);
                        }             
                        break;
                    case 2:
                        Console.WriteLine("Пункт создания записи, введите Фамилию Имя Отчество дату рождения Пол через пробелы \n Пример: \nПетров Анатолий Евгеньевич 22.04.1986 мужской");
                        string str = Console.ReadLine();
                        string[] userData = str.Split(' ');
                        string lastName = userData[0];
                        string firstName = userData[1];
                        string surName = userData[2];
                        DateTime dateOfBirth = DateTime.Parse(userData[3]);
                        string gender = userData[4];                       
                        try
                        {
                            using (SQLiteConnection conn = new SQLiteConnection(connect_string))
                            {
                                conn.Open();
                                using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO People(FirstName, LastName, SurName, DateOfBirth, Gender)" +
                                    "VALUES(@val1,@val2,@val3, @val4, @val5)", conn))
                                {                                    
                                    cmd.Parameters.AddWithValue("@val1", firstName);
                                    cmd.Parameters.AddWithValue("@val2", lastName);
                                    cmd.Parameters.AddWithValue("@val3", surName);
                                    cmd.Parameters.AddWithValue("@val4", dateOfBirth);
                                    cmd.Parameters.AddWithValue("@val5", gender);
                                    cmd.ExecuteNonQuery();
                                }


                            }
                            Console.WriteLine("Запись создана");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error:" + ex.Message);
                            Console.WriteLine("Возможно вы не создали таблицу, перезапустите приложение и выберите пункт 1");
                        }
                        break; 
                    case 3:
                        Console.WriteLine("Вывод всех строк с уникальным значением");
                        SQLiteDataReader sqlReader = null;
                        string Name;
                        string LastName;
                        string SurName;
                        DateTime dateOfBrith;
                        int age;
                        string Gender;
                        List<string> People = new List<string>();
                        using (SQLiteConnection conn = new SQLiteConnection(connect_string))
                        {
                            conn.Open();
                            SQLiteCommand command = new SQLiteCommand("SELECT FirstName, LastName, SurName, DateOfBirth, Gender FROM [People] GROUP BY LastName, DateOfBirth ORDER BY LastName;", conn);
                            try
                            {
                                sqlReader = command.ExecuteReader();
                                while (sqlReader.Read())
                                {
                                    Name = Convert.ToString(sqlReader["FirstName"]);
                                    LastName = Convert.ToString(sqlReader["LastName"]);
                                    SurName = Convert.ToString(sqlReader["SurName"]);
                                    dateOfBrith = Convert.ToDateTime(sqlReader["DateOfBirth"]);
                                    DateTime date = DateTime.Now;
                                    Gender = Convert.ToString(sqlReader["Gender"]);
                                    TimeSpan difference = date.Subtract(dateOfBrith);
                                    age = (int)(difference.TotalDays / 365.25);
                                    People.Add($"{LastName}||{Name}||{SurName}||{dateOfBrith.ToString("dd.MM.yyyy")}||{Gender}||{age}");
                                }
                                Console.WriteLine("  LastName||Name||SurName||DateOfBrith||Gender||Age");
                                foreach (string s in People) Console.WriteLine(s);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error:" + ex.Message);
                                Console.WriteLine("Возможно вы не создали таблицу, перезапустите приложение и выберите пункт 1");
                            }
                            finally
                            {
                                if (sqlReader != null)
                                    sqlReader.Close();
                            }
                        }
                        break; 
                    case 4:
                        Console.WriteLine("Создание 1млн Мужчин и Женщин, \nпри этом минимум 100 из них будут с фамилией на Ф");
                        Console.WriteLine("Перезапустите приложение, когда хватит сгенерированнных людей");
                        //Массивы для генерации мужчин
                        string[] FirstNameMans = new string[]
                        {
                "Сергей", "Анатолий", "Александр", "Федор", "Егор", "Денис", "Дмитрий", "Евгений", "Андрей"
                        };
                        string[] LastNameMans = new string[]
                        {
                "Песков", "Першин", "Разгуляев", "Фебов", "Февроним", "Федерякин"
                        };
                        string[] SurNameMans = new string[]
                        {
                "Артемович", "Александрович", "Федорович", "Евгеньевич", "Андреевич", "Егорович", "Сергеевич"
                        };


                        //Массивы для генерации женщин
                        string[] FirstNameWomans = new string[]
                        {
                "Анастасия", "Алена", "Евгения", "Александра", "Мария", "София", "Алина", "Дарья", "Эльвира"
                        };
                        string[] LastNameWomans = new string[]
                        {
                "Пескова", "Першина", "Разгуляева", "Фебова", "Февронима", "Федерякина"
                        };
                        string[] SurNameWomans = new string[]
                        {
                "Артемовна", "Александровна", "Федоровна", "Евгеньевна", "Андреевна", "Егоровна", "Сергеевна"
                        };                  
                        Random rand = new Random();
                        for (int i = 0; i < 500000; i++)
                        {
                            int FirstN_man = rand.Next(0, FirstNameMans.Length);
                            int LastN_man = rand.Next(0, LastNameMans.Length);
                            int SurN_man = rand.Next(0, SurNameMans.Length);
                            int FirstN_wo = rand.Next(0, FirstNameWomans.Length);
                            int LastN_wo = rand.Next(0, LastNameWomans.Length);
                            int SurN_wo = rand.Next(0, SurNameWomans.Length);
                            int Days1 = rand.Next(0, 16436);
                            int Days2 = rand.Next(0, 16436);
                            DateTime date1 = new DateTime(1960, 01, 01).Subtract(new TimeSpan(Days1, 0, 0, 0));
                            DateTime date2 = new DateTime(1965, 01, 01).Subtract(new TimeSpan(Days2, 0, 0, 0));


                            try
                            {
                                using (SQLiteConnection conn = new SQLiteConnection(connect_string))
                                {
                                    conn.Open();
                                    using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO People(FirstName, LastName, SurName, DateOfBirth, Gender)" +
                                        "VALUES(@val1,@val2,@val3, @val4, @val5)", conn))
                                    {

                                        cmd.Parameters.AddWithValue("@val1", FirstNameMans[FirstN_man]);
                                        cmd.Parameters.AddWithValue("@val2", LastNameMans[LastN_man]);
                                        cmd.Parameters.AddWithValue("@val3", SurNameMans[SurN_man]);
                                        cmd.Parameters.AddWithValue("@val4", date1);
                                        cmd.Parameters.AddWithValue("@val5", "мужчина");
                                        cmd.ExecuteNonQuery();
                                    }


                                }
                                //Console.WriteLine("Table updated successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error:" + ex.Message);
                                Console.WriteLine("Возможно вы не создали таблицу, перезапустите приложение и выберите пункт 1");
                            }
                            try
                            {
                                using (SQLiteConnection conn = new SQLiteConnection(connect_string))
                                {
                                    conn.Open();
                                    using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO People(FirstName, LastName, SurName, DateOfBirth, Gender)" +
                                        "VALUES(@val1,@val2,@val3, @val4, @val5)", conn))
                                    {

                                        cmd.Parameters.AddWithValue("@val1", FirstNameWomans[FirstN_man]);
                                        cmd.Parameters.AddWithValue("@val2", LastNameWomans[LastN_man]);
                                        cmd.Parameters.AddWithValue("@val3", SurNameWomans[SurN_man]);
                                        cmd.Parameters.AddWithValue("@val4", date2);
                                        cmd.Parameters.AddWithValue("@val5", "женщина");
                                        cmd.ExecuteNonQuery();
                                    }


                                }
                                //Console.WriteLine("Table updated successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error:" + ex.Message);
                            }
                            Console.Write(i + "||");
                        }
                        Console.WriteLine("Done!");
                        break; 
                    case 5:
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();                       
                        sqlReader = null;
                        using (SQLiteConnection conn = new SQLiteConnection(connect_string))
                        {
                            conn.Open();

                            SQLiteCommand command = new SQLiteCommand("SELECT FirstName, LastName, SurName, DateOfBirth, Gender FROM [People] WHERE Gender = 'мужчина' AND LastName LIKE 'Ф%' ", conn);
                            try
                            {
                                Console.WriteLine("LastName||FirstName||SurName||dateOfBrith||Gender||Age");
                                int I = 0;
                                sqlReader = command.ExecuteReader();
                                while (sqlReader.Read())
                                {
                                    if (I >= 100) break;
                                    I++;
                                    Name = Convert.ToString(sqlReader["FirstName"]);
                                    LastName = Convert.ToString(sqlReader["LastName"]);
                                    SurName = Convert.ToString(sqlReader["SurName"]);
                                    dateOfBrith = Convert.ToDateTime(sqlReader["DateOfBirth"]);
                                    DateTime date = DateTime.Now;
                                    Gender = Convert.ToString(sqlReader["Gender"]);
                                    TimeSpan difference = date.Subtract(dateOfBrith);
                                    age = (int)(difference.TotalDays / 365.25);
                                    Console.WriteLine($"{LastName}||{Name}||{SurName}||{dateOfBrith.ToString("dd.MM.yyyy")}||{Gender}||{age}");
                                }
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); Console.WriteLine("Возможно вы не создали таблицу, перезапустите приложение и выберите пункт 1"); }
                        }
                        stopwatch.Stop();
                        Console.WriteLine("Время выполнения: {0} мс", stopwatch.ElapsedMilliseconds);                       
                        break;
                    default: 

                        Console.WriteLine("Вы выбрали не существующий пункт");
                        break;

                        
                }
                Console.ReadKey();
            }
            else
            {
                SQLiteConnection.CreateFile("MyDatabase.sqlite");
            }
        }
    }
}
