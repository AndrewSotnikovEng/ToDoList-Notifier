using Notifier.ViewModels;
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

        public void CreateNewTask(string taskName, string targetSpace)
        {
            SQL = $"INSERT INTO Tasks (Name) VALUES ('{taskName}')";
            Command = new OleDbCommand(SQL, Connection);
            Command.ExecuteReader();

            //finding ID of recently added task
            SQL = $"SELECT ID FROM Tasks WHERE Name LIKE '{taskName}'";
            Command = new OleDbCommand(SQL, Connection);
            OleDbDataReader reader = Command.ExecuteReader();
            reader.Read();
            string taskId = reader["ID"].ToString();

            if (targetSpace == "") 
            { 

            }
            else if (targetSpace.Contains("Ящик")) //to current 
            {
                SQL = $"INSERT INTO Contexts(TaskID, SpaceID, ActualDate ) VALUES({taskId},{BACKLOG_TASK_SPACE_ID},FORMAT(#{GetCurDate()}# , 'yyyy/mm/dd'))";
                Command = new OleDbCommand(SQL, Connection);
                Command.ExecuteReader();
                reader.Read();
            }
            else if (targetSpace.Contains("Стол")) //to backlog
            {
                SQL = $"INSERT INTO Contexts(TaskID, SpaceID, ActualDate) VALUES({taskId},{CURRENT_TASK_SPACE_ID},FORMAT(#{GetCurDate()}# , 'yyyy/mm/dd'))";
                Command = new OleDbCommand(SQL, Connection);
                Command.ExecuteReader();
                reader.Read();
            }
        }

        public void MoveTaskToSpace(int taskId, string targetSpace)
        {

            OleDbDataReader reader = Command.ExecuteReader();
            if (targetSpace.Contains("Ящик")) //to current 
            {
                SQL = $"INSERT INTO Contexts(TaskID, SpaceID, ActualDate ) VALUES({taskId},{BACKLOG_TASK_SPACE_ID},FORMAT(#{GetCurDate()}# , 'yyyy/mm/dd'))";
                Command = new OleDbCommand(SQL, Connection);
                Command.ExecuteReader();
                reader.Read();
            }
            else if (targetSpace.Contains("Стол")) //to backlog
            {
                SQL = $"INSERT INTO Contexts(TaskID, SpaceID, ActualDate) VALUES({taskId},{CURRENT_TASK_SPACE_ID},FORMAT(#{GetCurDate()}# , 'yyyy/mm/dd'))";
                Command = new OleDbCommand(SQL, Connection);
                Command.ExecuteReader();
                reader.Read();
            }
        }


        internal List<TaskModel> GetAllTasks(bool verbose)
        {
            List<TaskModel> existedTasks = new List<TaskModel>();
            SQL = "SELECT ID, " +
                 "Name, " +
                 "CreationDate " +
                 "FROM Tasks " +
                 "ORDER BY CreationDate DESC";

            Command = new OleDbCommand(SQL, Connection);

            OleDbDataReader reader = Command.ExecuteReader();

            Console.WriteLine("------------Original data----------------");
            while (reader.Read())
            {
                int id = (int)reader["ID"];
                string name = reader["Name"].ToString();

                TaskModel task = new TaskModel(id, name);
                existedTasks.Add(task);
                if (verbose)
                {
                    Console.WriteLine(task);
                }
            }
            return existedTasks;
        }

        public void MarkTaskAsDone(int id)
        {


            SQL = $"SELECT IsTemplate FROM Tasks WHERE ID ={id}";
            Command = new OleDbCommand(SQL, Connection);
            OleDbDataReader reader = Command.ExecuteReader();
            reader.Read();
            bool isTemplate = reader["IsTemplate"].ToString() == "True" ? true : false;

            if (isTemplate) //if tempalte
            {
                SQL = $"UPDATE Tasks SET CompletionDate = FORMAT(#{GetCurDate()}# , 'yyyy/mm/dd') WHERE ID = {id}";
                Command = new OleDbCommand(SQL, Connection);
                reader = Command.ExecuteReader();
                reader.Read();

            }
            else //if not tempalte
            {

                SQL = $"UPDATE Tasks SET Completed = True, CompletionDate = FORMAT(#{GetCurDate()}# , 'yyyy/mm/dd') WHERE ID = {id}";
                Command = new OleDbCommand(SQL, Connection);
                reader = Command.ExecuteReader();
                reader.Read();

            }
            SQL = $"DELETE FROM Contexts WHERE TaskID = {id}";
            Command = new OleDbCommand(SQL, Connection);
            reader = Command.ExecuteReader();
            reader.Read();

        }

        public List<TaskModel> GetAllTasksFromCurrent(bool verbose)
        {
            List<TaskModel> taskNames = new List<TaskModel>();
            SQL = "SELECT Tasks.ID, " +
                 "Tasks.Name, " +
                 "Contexts.ActualDate, " +
                 "Contexts.SpaceID " +
                 "FROM Tasks " +
                 "INNER JOIN Contexts ON Tasks.ID = Contexts.TaskID " +
                 "WHERE(((Contexts.SpaceID) = 1));";

            Command = new OleDbCommand(SQL, Connection);

            OleDbDataReader reader = Command.ExecuteReader();

            Console.WriteLine("------------Original data----------------");
            while (reader.Read())
            {
                int id = (int)reader["ID"];
                string name = reader["Name"].ToString();

                TaskModel task = new TaskModel(id, name);
                taskNames.Add(task);
                if (verbose)
                {
                    Console.WriteLine(task);
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
