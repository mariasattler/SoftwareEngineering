using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEUebung
{
    public class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string port;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "db-mysql-fra1-87608-do-user-6564514-0.db.ondigitalocean.com";
            database = "mariaswedb";
            uid = "maria";
            password = "pq16uamqfg0w3un1";
            port = "25060";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "PORT=" + port+ ";DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";SSL Mode=Required";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        ///Insert statement
        public void Create()
        {
            
            string query = @"Create Table Temperatur(
                            id numeric not null primary key,
                            temp numeric not null,
                            tag numeric not null,
                            monat numeric not null,
                            jahr numeric not null,
                            uhrzeit varchar(10) not null);";

            if (OpenConnection()==true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery(); //execute;

                //close connection
                this.CloseConnection();
            }
        }

        public void drop()
        {
            string query = "drop table Temperatur";
            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery(); //execute;

                //close connection
                this.CloseConnection();
            }
        }
        public void Insert()
        {
            int id = 1;
            Random rnd = new Random();
            StringBuilder query = new StringBuilder();
            for (int year = 2008; year < 2019; year++) //jahre
            {
                for(int month = 1; month<13; month++ )
                {
                    for(int day = 1; day <32; day++)
                    {
                        query.Append($"insert into Temperatur (id, temp, tag, monat, jahr, uhrzeit) values ({id}, {rnd.Next(-20,40)},{day},{month},{year},'8 AM');"); id++;
                        query.Append($"insert into Temperatur (id, temp, tag, monat, jahr, uhrzeit) values ({id}, {rnd.Next(-20, 40)},{day},{month},{year},'12 AM');"); id++;
                        query.Append($"insert into Temperatur (id, temp, tag, monat, jahr, uhrzeit) values ({id}, {rnd.Next(-20, 40)},{day},{month},{year},'3 PM');"); id++;
                    }
                }
            }

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query.ToString(), connection);
                cmd.ExecuteNonQuery(); //execute;

                //close connection
                this.CloseConnection();
            }
        }

        //Select statement
        public List<DBEntity> Select(bool withDay, string year = null, string month = null, string day = null)
        {
            string query = $"SELECT * FROM Temperatur where jahr = {year} and monat = {month} and tag = {day}";
            if(!withDay) //first attempt
                query = $"SELECT * FROM Temperatur where jahr = {year} and monat = {month}";

            //Create a list to store the result
            List<DBEntity> list = new List<DBEntity>();


            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command

                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list.Add(new DBEntity(Int32.Parse(dataReader["id"].ToString()), Int32.Parse(dataReader["tag"].ToString()),
                        Int32.Parse(dataReader["monat"].ToString()), Int32.Parse(dataReader["jahr"].ToString()), Int32.Parse(dataReader["temp"].ToString()), 
                        dataReader["uhrzeit"].ToString()));
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
    }
}
