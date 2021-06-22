using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Helper;
using DataAccessLayer;

namespace DbTestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileCreatePath = ConfigurationManager.AppSettings["XmlFileCreate"];

            HelperClass hc = new HelperClass();
            hc.CreateFile(fileCreatePath);

            DataConnection dc = new DataConnection();
            dc.Serialize();

            //dc.CreateTableQuery();
            //dc.InsertData();
            dc.FetchData();

            Console.Read();
        }
    }
}
