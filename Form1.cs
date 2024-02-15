using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using System.Security.AccessControl;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security.Principal;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace UserMaking
{
    public partial class Form1 : Form
    {

        List<UserList> userlist;

        public string oug;
        string group;
        string domain = "MYDOMAIN";
        string sn;
        string gname;
        string init;
        string fullName;
        string gr;
        string login;
        string normLogin;

        
        public Form1()
        {
            InitializeComponent();
            userlist = new List<UserList>();
        }

        public void LoadCSVIntoListBox()
        {
            if (dataGridView1.DataSource != null)
            {
                checkedListBox1.Items.Clear();
                List<string> uniqueGroups = new List<string>();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string gr = row.Cells["Группа"].Value.ToString();

                    if (!uniqueGroups.Contains(gr))
                    {
                        uniqueGroups.Add(gr);
                        checkedListBox1.Items.Add(gr);
                    }
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e) // Загрузка файла
        {
            ReadCSV readCSV = new ReadCSV();
            readCSV.LoadCSV();
            LoadCSVIntoListBox();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ReadCSV readCSV = new ReadCSV();
            readCSV.SaveCSV();
            //if (dataGridView1.DataSource != null)
            //{
            //    using (var writer = new StreamWriter("D:\\Users.csv", false, Encoding.GetEncoding("windows-1251")))
            //    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //    {
            //        csv.WriteRecords(userlist);
            //        MessageBox.Show("Сохраненно");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Вы пытаетесь сохарнить пустой файл!");
            //    return;
            //}

        } // Сохранение файла


        public bool OuExists(string ouName)
        {
            using (DirectoryEntry root = new DirectoryEntry("LDAP://dc=mydomain,dc=com"))
            {
                foreach (DirectoryEntry child in root.Children)
                {
                    if (child.Name == "OU=" + ouName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void CreateOu(string oug)
        {
            using (DirectoryEntry root = new DirectoryEntry("LDAP://dc=mydomain,dc=com"))
            {
                using (DirectoryEntry newOu = root.Children.Add("OU=" + oug, "OrganizationalUnit"))
                {
                    newOu.CommitChanges();
                }
            }
        }

        public void CreateGroups()
        {
            var groups = new HashSet<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string groupName = row.Cells["Группа"].Value.ToString();
                gr = $"SG_{groupName}";

                //Создаем OU для группы, если он еще не существует
                if (!OuExists(gr))
                {
                    CreateOu(gr);

                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "mydomain.com", "OU=" + gr + ",DC=mydomain,DC=com"))
                    {

                        GroupPrincipal newGroup = new GroupPrincipal(context)
                        {
                            Name = gr,
                            Description = "Описание группы",
                        };
                        newGroup.Save();

                    }
                }
            }
        }

        public bool UserOuExists(string login, string gr)
        {
            using (DirectoryEntry group = new DirectoryEntry("LDAP://mydomain.com/OU=" + gr + ",dc=mydomain,dc=com"))
            {
                foreach (DirectoryEntry child in group.Children)
                {
                    if (child.Name == "OU=" + login)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void CreateUserOu(string login, string gr)
        {
            using (DirectoryEntry root = new DirectoryEntry("LDAP://dc=mydomain,dc=com"))
            {
                using (DirectoryEntry parentOu = root.Children.Find("OU=" + gr))
                {
                    using (DirectoryEntry newOu = parentOu.Children.Add("OU=" + login, "OrganizationalUnit"))
                    {
                        newOu.CommitChanges();
                    }
                }
            }
        }
        public void CreateUser()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                
                string groupName = row.Cells["Группа"].Value.ToString();

                oug = row.Cells["Группа"].Value.ToString();
                //group = $"OU={oug}"; //<-- сама группа
                sn = row.Cells["Фамилия"].Value.ToString();
                gname = $"{row.Cells["Имя"].Value} {row.Cells["Отчество"].Value}"; // Name + Patronymic
                init = row.Cells["Инициалы"].Value.ToString();
                fullName = $"{sn} {gname}";
                gr = $"SG_{oug}"; // <-- по идее это паттерн названия группы 
                login = $"{sn}{init}"; // WIN-2000 Login (Surname + Initials)
                normLogin = $"{login}{domain}"; // Standard login


                if (!UserOuExists(login, gr))
                {
                    CreateUserOu(login, gr);
                }

                using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "mydomain.com", "OU=" + login + ",OU=" + gr + ",DC=mydomain,DC=com"))
                {
                    UserPrincipal newUser = new UserPrincipal(context)
                    {
                        Name = fullName,
                        SamAccountName = login,
                        Surname = sn,
                        Enabled = true,
                        UserPrincipalName = normLogin,
                        AccountExpirationDate = null,
                        GivenName = gname,
                        DisplayName = fullName,
                    };
                    
                    newUser.SetPassword("aaaAAA111");
                    newUser.Save();


                    using (PrincipalContext groupContext = new PrincipalContext(ContextType.Domain, "mydomain.com", "OU=" + gr + ",DC=mydomain,DC=com"))
                    {
                        GroupPrincipal group = GroupPrincipal.FindByIdentity(groupContext, IdentityType.Name, gr);
                        if (group != null)
                        {
                            group.Members.Add(newUser);
                            group.Save();
                        }
                        else
                        {
                            MessageBox.Show("Группа не найдена!");
                        }
                    }
                }
            }
            MessageBox.Show("пользователи созданы!");
        }


        /// Кнопки
        
        private void Add_button_Click(object sender, EventArgs e)
        {
            if(dataGridView1.DataSource == null)
            {
                MessageBox.Show("Нет данных!");
                return;
            }
            else
            {
                CreateGroups();
                CreateUser();
                MessageBox.Show("Группы и пользователи успешно созданы!");
            }
        }
        private void Clear_button_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }

        private void Add_PersonRule_button_Click(object sender, EventArgs e)
        {
            
        }
        private void Add_GroupRule_button_Click(object sender, EventArgs e)
        {
            FullControl.Tag = "FullControl";
            ReadData.Tag = "ReadData";
            CreateFiles.Tag = "CreateFiles";
            Delete.Tag = "Delete";

            //List<CheckBox> selectedPermissions = new List<CheckBox>();
            //foreach (Control control in checkedListBox1.Controls)
            //{
            //    if (control is CheckBox checkBox && checkBox.Checked)
            //    {
            //        selectedPermissions.Add(checkBox);
            //    }
            //}

            //List<string> selectedGroups = new List<string>();
            //foreach (object item in checkedListBox1.SelectedItems)
            //{
            //    selectedGroups.Add(item.ToString());
            //}

            //foreach (string selectedGroup in selectedGroups)
            //{
            //    string groupPath = "LDAP://SERVER1,dc=mydomain,dc=com";

            //    using (DirectoryEntry group = new DirectoryEntry(groupPath))
            //    {
            //        foreach (CheckBox permissionCheckbox in selectedPermissions)
            //        {
            //            string permission = permissionCheckbox.Text;


            //        }
            //        group.CommitChanges();
            //    }
            //}

        }


        /// Блок удаления директорий

        public void DeleteAllGroups()
        {
            using (DirectoryEntry root = new DirectoryEntry("LDAP://dc=mydomain,dc=com"))
            {
                DeleteChildEntries(root);
            }
        }

        private void DeleteChildEntries(DirectoryEntry parent)
        {
            foreach (DirectoryEntry child in parent.Children)
            {
                DeleteChildEntries(child);

                try
                {
                    if (DirectoryEntry.Exists(child.Path))
                    {
                        child.DeleteTree();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при удалении объекта: " + ex.Message);
                }
            }

            try
            {
                if (DirectoryEntry.Exists(parent.Path))
                {
                    parent.DeleteTree();
                    parent.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при удалении объекта: " + ex.Message);
            }
        }

        private void DeleteGroup_button_Click(object sender, EventArgs e)
        {
            DeleteAllGroups();
        }

       
    }
}
