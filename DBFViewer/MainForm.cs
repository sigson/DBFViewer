using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBFViewer
{
    public partial class MainForm : Form
    {
		Encoding UsedEncoding;
		string OpenedFile;
        Form AboutForm;
        public MainForm()
        {
            InitializeComponent();
			EncodingName.Text = "utf-8";
			UsedEncoding = Encoding.UTF8;
            AboutForm = new About();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
			try
			{
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.Filter = "dBASE files (*.dbf)|*.dbf";
				ofd.ShowDialog();

				if (ofd.FileName.Length > 0)
				{
					OpenedFile = ofd.FileName;
					DataTable dt = ParseDBF.ReadDBF(ofd.FileName, UsedEncoding);
					DBFDataGrid.DataSource = dt;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message + "\r\r" + ex.StackTrace, "Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        private void EncodingName_SelectedIndexChanged(object sender, EventArgs e)
        {
			try
			{
				UsedEncoding = (EncodingName.Text == "utf-8" ? Encoding.UTF8 : UsedEncoding);
				UsedEncoding = (EncodingName.Text == "ASCII" ? Encoding.ASCII : UsedEncoding);
				UsedEncoding = (EncodingName.Text == "DOS" ? Encoding.GetEncoding(866) : UsedEncoding);
				UsedEncoding = (EncodingName.Text == "utf-32" ? Encoding.UTF32 : UsedEncoding);
				UsedEncoding = (EncodingName.Text == "CP1251(ansi cyrillic)" ? Encoding.GetEncoding(1251) : UsedEncoding);
				DataTable dt = ParseDBF.ReadDBF(OpenedFile, UsedEncoding);
				DBFDataGrid.DataSource = dt;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message + "\r\r" + ex.StackTrace, "Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DBFDataGrid.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = Path.GetFileNameWithoutExtension(new FileInfo(OpenedFile).Name) + ".csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = DBFDataGrid.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[DBFDataGrid.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += "\"" + DBFDataGrid.Columns[i].HeaderText.ToString().Trim() + "\",";
                            }
                            outputCsv[0] += columnNames.Substring(0, columnNames.Length - 1);

                            for (int i = 1; (i - 1) < DBFDataGrid.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += "\"" + DBFDataGrid.Rows[i - 1].Cells[j].Value.ToString().Trim() + "\",";
                                }
                                outputCsv[i] = outputCsv[i].Substring(0, outputCsv[i].Length - 1);
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm.Show();
        }
    }
}
