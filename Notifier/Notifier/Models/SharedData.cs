using IniParser;
using Notifier.DataLayer;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifier.Models
{
    class SharedData
    {

        public static Container container;

        public static void InitContainer() 
        {
            container = new Container();
            container.Register<DBService>(Lifestyle.Singleton);
            container.Verify();
        }

        public static void InitDbService()
        {
            //parsing db
            var parser = new FileIniDataParser();
            IniParser.Model.IniData data = parser.ReadFile("config.ini", Encoding.UTF8);
            string dbFilePath = data["Settings"]["DbFile"];

            //set db for instance
            DBService dbService = container.GetInstance<DBService>();
            dbService.InitConnection(dbFilePath);
        }
    }
}
