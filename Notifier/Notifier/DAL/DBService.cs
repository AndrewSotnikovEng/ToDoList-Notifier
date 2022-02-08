using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifier.DataLayer
{
    class DBService
    {

        const int CURRENT_TASK_SPACE_ID = 1;
        const int BACKLOG_TASK_SPACE_ID = 2;

        OleDbConnection Connection { get; set; }
        OleDbCommand Command { get; set; }
        string SQL { get; set; }
        string ConnectionString { get; set; }

        // Open connecton    

        public DBService(string dbFile)
        {   
            if (!File.Exists(dbFile))
            {
                Console.WriteLine("Database wasn't found!");
                return;
            }
            ConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"{dbFile}\"";
            
            Connection = new OleDbConnection(ConnectionString);
            Connection.Open();
        }

        public void CreateNewTask(string taskName, bool isCurrent )
        {
            SQL = $"INSERT INTO Tasks (Name) VALUES ('{taskName}')";
            Command = new OleDbCommand(SQL, Connection);
            Command.ExecuteReader();

            

            if (isCurrent)
            {
                //finding ID of recently added task
                SQL = $"SELECT ID FROM Tasks WHERE Name LIKE '{taskName}'";
                Command = new OleDbCommand(SQL, Connection);
                OleDbDataReader reader = Command.ExecuteReader();
                reader.Read();
                string taskId = reader["ID"].ToString();


                //adding it to context
                SQL = $"INSERT INTO Contexts(TaskID, SpaceID, ActualDate) VALUES({taskId},{CURRENT_TASK_SPACE_ID},FORMAT(#{GetCurDate()}# , 'yyyy/mm/dd'))";
                Command = new OleDbCommand(SQL, Connection);
                Command.ExecuteReader();
                reader.Read();
            }
        }

        public List<string> GetAllTaskNamesFromCurrent(bool verbose)
        {
            List<String> taskNames = new List<string>();
            SQL = "SELECT Tasks.ID, " +
                 "Tasks.Name, " +
                 "Contexts.ActualDate, " +
                 "Contexts.SpaceID " +
                 "FROM Tasks " +
                 "INNER JOIN Contexts ON Tasks.ID = Contexts.TaskID " +
                 "WHERE(((Contexts.SpaceID) = 1));";

            // Create a command and set its connection    
            Command = new OleDbCommand(SQL, Connection);

            // Execute command    
            OleDbDataReader reader = Command.ExecuteReader();

            Console.WriteLine("------------Original data----------------");
            while (reader.Read())
            {
                string taskName = String.Format("{0} : {1}", reader["Name"].ToString(), reader["ID"].ToString());
                taskNames.Add(taskName);
                if (verbose)
                {
                    Console.WriteLine(taskName);
                }
            }
            return taskNames;
        }

        string GetCurDate()
        {
            string curDate = $"{DateTime.Now.Year.ToString("00")}/{DateTime.Now.Month.ToString("00")}/{DateTime.Now.Day.ToString("00")}";

            return curDate;
        }



    }
}
