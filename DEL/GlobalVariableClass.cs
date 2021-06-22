using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DEL
{
    public class GlobalVariableClass
    {

        private string connectionString;
        private string fileCreatePath;

        public string ConnectionString
        {
            get{ return connectionString; }
            set{ connectionString = value; }
        }
        public string FileCreatePath
        {
            get { return fileCreatePath; }
            set { fileCreatePath = value; }
        }

    }
}
