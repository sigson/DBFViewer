using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace DBFViewer
{
    class DBFMaster
    {
        public static void DBFRead(string pathToDBFFile, ref DataSet dataSet)
        {
            string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + new FileInfo(pathToDBFFile).DirectoryName + ";Extended Properties=dBASE IV;User ID=Admin;Password=;";
            using (OleDbConnection con = new OleDbConnection(constr))
            {
                var sql = "select * from " + pathToDBFFile;
                OleDbCommand cmd = new OleDbCommand(sql, con);
                con.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dataSet);
                //ds
            }
        }
    }
}
