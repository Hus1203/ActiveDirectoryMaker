using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace UserMaking
{
    public partial class ReadCSV 
    {
        List<UserList> userlist;

        public void ShowException()
        {
            MessageBox.Show("Невозможно загрузить содержимое этого файла.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void ReloadDataGridView()
        {
            Form1 form = (Form1)Application.OpenForms["Form1"];

            form.dataGridView1.DataSource = null;
            form.dataGridView1.DataSource = userlist;
            form.dataGridView1.Refresh();
            form.Show();
        }

        public void ReadingCSV(string Name)
        {
            userlist = new List<UserList>();

            using (var reader = new StreamReader(Name, Encoding.GetEncoding("windows-1251")))
            using (var file = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                try
                {
                    userlist = file.GetRecords<UserList>().ToList();
                }
                catch
                {
                    ShowException();
                }
                finally
                {
                   ReloadDataGridView();
                }
            }

        }
        public void LoadCSV()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var Name = openFileDialog.FileName;

                if (Name.Contains(".csv"))
                    //ReadCSV(Name);
                    ReadingCSV(Name);

                else
                    MessageBox.Show("Допустимые расширения:\n.csv", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

    }
}
