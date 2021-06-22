using System;
using System.IO;

namespace Helper
{
    public class HelperClass
    {
        public void CreateFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                File.Create(path);
            }
            else
            {
                File.Create(path);
            }
        }
    }
}
