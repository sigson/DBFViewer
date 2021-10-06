using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DBFViewer
{
    class DBFMaster
    {
        public static void DBFRead(string pathToDBFFile, ref DataSet dataSet)
        {
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=directoryPath;Extended Properties=dBASE IV;User ID=Admin;Password=;";
            using (OleDbConnection con = new OleDbConnection(constr))
            {
                var sql = "select * from " + pathToDBFFile;
                OleDbCommand cmd = new OleDbCommand(sql, con);
                con.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dataSet);
            }
        }
    }
}
