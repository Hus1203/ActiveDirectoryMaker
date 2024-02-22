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
using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security.Principal;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Drawing2D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using PowerShellToolsPro.Packager;

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
            ProcessDataInADModule();
        }

        public void ProcessDataInADModule()
        {
            ADModule adModule = new ADModule(dataGridView1);
            ShareModule shareModule = new ShareModule(dataGridView1);
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


        //public bool OuExists(string ouName)
        //{
        //    using (DirectoryEntry root = new DirectoryEntry("LDAP://dc=mydomain,dc=com"))
        //    {
        //        foreach (DirectoryEntry child in root.Children)
        //        {
        //            if (child.Name == "OU=" + ouName)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
        //public void CreateOu(string oug)
        //{
        //    using (DirectoryEntry root = new DirectoryEntry("LDAP://dc=mydomain,dc=com"))
        //    {
        //        using (DirectoryEntry newOu = root.Children.Add("OU=" + oug, "OrganizationalUnit"))
        //        {
        //            newOu.CommitChanges();
        //        }
        //    }
        //}

        //public void CreateGroups()
        //{
        //    var groups = new HashSet<string>();

        //    foreach (DataGridViewRow row in dataGridView1.Rows)
        //    {
        //        string groupName = row.Cells["Группа"].Value.ToString();
        //        gr = $"SG_{groupName}";

        //        //Создаем OU для группы, если он еще не существует
        //        if (!OuExists(gr))
        //        {
        //            CreateOu(gr);

        //            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "mydomain.com", "OU=" + gr + ",DC=mydomain,DC=com"))
        //            {

        //                GroupPrincipal newGroup = new GroupPrincipal(context)
        //                {
        //                    Name = gr,
        //                    Description = "Описание группы",
        //                };
        //                newGroup.Save();

        //            }
        //        }
        //    }
        //}

        //public bool UserOuExists(string login, string gr)
        //{
        //    using (DirectoryEntry group = new DirectoryEntry("LDAP://mydomain.com/OU=" + gr + ",dc=mydomain,dc=com"))
        //    {
        //        foreach (DirectoryEntry child in group.Children)
        //        {
        //            if (child.Name == "OU=" + login)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    //MessageBox.Show("Такой пользователь существует");
        //    return false;
        //}
        //public void CreateUserOu(string login, string gr)
        //{
        //    using (DirectoryEntry root = new DirectoryEntry("LDAP://dc=mydomain,dc=com"))
        //    {
        //        using (DirectoryEntry parentOu = root.Children.Find("OU=" + gr))
        //        {
        //            using (DirectoryEntry newOu = parentOu.Children.Add("OU=" + login, "OrganizationalUnit"))
        //            {
        //                newOu.CommitChanges();
        //            }
        //        }
        //    }
        //}

        //private bool UserExists(string login)
        //{
        //    using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "mydomain.com"))
        //    {
        //        UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, login);
        //        return user != null;
        //    }
        //}
        //public void CreateUser()
        //{

        //    foreach (DataGridViewRow row in dataGridView1.Rows)
        //    {
                
        //        string groupName = row.Cells["Группа"].Value.ToString();

        //        oug = row.Cells["Группа"].Value.ToString();
        //        //group = $"OU={oug}"; //<-- сама группа
        //        sn = row.Cells["Фамилия"].Value.ToString();
        //        gname = $"{row.Cells["Имя"].Value} {row.Cells["Отчество"].Value}"; // Name + Patronymic
        //        init = row.Cells["Инициалы"].Value.ToString();
        //        fullName = $"{sn} {gname}";
        //        gr = $"SG_{oug}"; // <-- по идее это паттерн названия группы 
        //        login = $"{sn}{init}"; // WIN-2000 Login (Surname + Initials)
        //        normLogin = $"{login}{domain}"; // Standard login

        //        if (!UserExists(login))
        //        {
        //            if (!UserOuExists(login, gr))
        //            {
        //                CreateUserOu(login, gr);
        //            }

        //            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "mydomain.com", "OU=" + login + ",OU=" + gr + ",DC=mydomain,DC=com"))
        //            {
        //                UserPrincipal newUser = new UserPrincipal(context)
        //                {
        //                    Name = fullName,
        //                    SamAccountName = login,
        //                    Surname = sn,
        //                    Enabled = true,
        //                    UserPrincipalName = normLogin,
        //                    AccountExpirationDate = null,
        //                    GivenName = gname,
        //                    DisplayName = fullName,
        //                };

        //                newUser.SetPassword("aaaAAA111");
        //                newUser.Save();


        //                using (PrincipalContext groupContext = new PrincipalContext(ContextType.Domain, "mydomain.com", "OU=" + gr + ",DC=mydomain,DC=com"))
        //                {
        //                    GroupPrincipal group = GroupPrincipal.FindByIdentity(groupContext, IdentityType.Name, gr);
        //                    if (group != null)
        //                    {
        //                        group.Members.Add(newUser);
        //                        group.Save();
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("Группа не найдена!");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    MessageBox.Show("пользователи созданы!");
        //}


        private void Add_button_Click(object sender, EventArgs e)
        {
            if(dataGridView1.DataSource == null)
            {
                MessageBox.Show("Нет данных!");
                return;
            }
            else
            {
                ADModule admodule = new ADModule(dataGridView1);
                admodule.CreateGroups();
                admodule.CreateUser();
                MessageBox.Show("Группы и пользователи успешно созданы!");
            }
        } // Вывод групп в листбокс
        private void Clear_button_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        } // Отчистка табла

        private void Add_GroupRule_button_Click(object sender, EventArgs e) // Выдача разрешений пользователям групп
        {
            CheckBox[] checkboxes = { FullControl, Modify, ReadAndExecute, ListDirectory, Read, Write };

            GrantWriteAccessToUserFolders(checkboxes);
        }

        public Boolean CheckBoxe(CheckBox[] checkboxes)
        {
            var selectedGroupsCount = checkedListBox1.CheckedItems.Count;
            var selectedCheckboxesCount = checkboxes.Count(cb => cb.Checked);


            if (selectedGroupsCount == 0 || selectedCheckboxesCount == 0)
            {
                MessageBox.Show("Пожалуйста, выберите хотя бы одну группу и один чекбокс для назначения прав доступа.");
                return true;
            }
            else
            {
                return false;
            }
        }   // Проверка чекбоксов
        public void GrantWriteAccessToUserFolders(CheckBox[] checkboxes)
        {
            string baseDirectory = "C:\\public";

            if(CheckBoxe(checkboxes)) // Проверка 
            {
                return;
            }

            foreach (var groupItem in checkedListBox1.CheckedItems) // достаем группы из листбокса
            {
                string selectedGroup = groupItem.ToString();

                foreach (string groupDirectoryPath in Directory.GetDirectories(baseDirectory))
                {
                    string groupName = new DirectoryInfo(groupDirectoryPath).Name;

                    if (groupName == selectedGroup)
                    {
                        foreach (string userDirectoryPath in Directory.GetDirectories(groupDirectoryPath))
                        {
                            string userLogin = new DirectoryInfo(userDirectoryPath).Name;

                            using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                            {
                                UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userLogin);
                                if (user != null)
                                {
                                    DirectorySecurity directorySecurity = Directory.GetAccessControl(userDirectoryPath);

                                    // Удаляем все существующие правила, кроме разрешения на запись
                                    AuthorizationRuleCollection rules = directorySecurity.GetAccessRules(true, true, typeof(NTAccount));

                                    foreach (FileSystemAccessRule rule in rules)
                                    {
                                        directorySecurity.RemoveAccessRule(rule);
                                    }

                                    foreach (CheckBox checkbox in checkboxes) // Плохая реализация, но имеет место быть
                                    {
                                        if (checkbox.Checked)
                                        {
                                            if (checkbox.Name == "Read")
                                            {
                                                directorySecurity.AddAccessRule(new FileSystemAccessRule(user.Sid, FileSystemRights.Read, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                            }
                                            else if (checkbox.Name == "Write")
                                            {
                                                directorySecurity.AddAccessRule(new FileSystemAccessRule(user.Sid, FileSystemRights.Write, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                            }
                                            else if (checkbox.Name == "FullControl")
                                            {
                                                directorySecurity.AddAccessRule(new FileSystemAccessRule(user.Sid, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                            }
                                            else if (checkbox.Name == "Modify")
                                            {
                                                directorySecurity.AddAccessRule(new FileSystemAccessRule(user.Sid, FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                            }
                                            else if (checkbox.Name == "ReadAndExecute")
                                            {
                                                directorySecurity.AddAccessRule(new FileSystemAccessRule(user.Sid, FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                            }
                                            else if (checkbox.Name == "ListDirectory")
                                            {
                                                directorySecurity.AddAccessRule(new FileSystemAccessRule(user.Sid, FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                                            }
                                            Directory.SetAccessControl(userDirectoryPath, directorySecurity);
                                        }
                                    }

                                }
                                else
                                {
                                    MessageBox.Show($"Пользователь {userLogin} не найден в Active Directory.");
                                }
                            }
                        }
                    }
                }
            }

            MessageBox.Show("Права успешно установлены для всех пользовательских папок.");
        }   // Метод выдачи разрешений


        private void DeleteGroup_button_Click(object sender, EventArgs e) // Удаление директорий в Active directory
        {
            DialogResult dialogResult = MessageBox.Show("Вы уверенны что хотите удалить все группы?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (dialogResult == DialogResult.Yes)
            {
                DeletingAD delAD = new DeletingAD();
                delAD.DeleteAllGroups();
                MessageBox.Show("Группы успешно дезинтегрированны!");
            }
            else
            {
                return;
            }
        }

        public void CreateShares()
        {
            string baseDirectory = "C:\\\\public";
            if (dataGridView1 == null)
            {
                MessageBox.Show("Сначала загрузите данные");
                return;
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string groupName = row.Cells["Группа"].Value.ToString();
                    string userLogin = $"{row.Cells["Фамилия"].Value.ToString()}{row.Cells["Инициалы"].Value.ToString()}";

                    string groupDirectoryPath = Path.Combine(baseDirectory, groupName);
                    string userDirectoryPath = Path.Combine(groupDirectoryPath, userLogin);

                    if (!Directory.Exists(groupDirectoryPath))
                    {
                        Directory.CreateDirectory(groupDirectoryPath);
                    }
                    else
                    {
                        Console.WriteLine($"Директория {groupDirectoryPath} уже существует.");
                        // Вывод предупреждения или выполнение других действий
                    }

                    if (!Directory.Exists(userDirectoryPath))
                    {
                        Directory.CreateDirectory(userDirectoryPath);
                    }
                    else
                    {
                        Console.WriteLine($"Директория {userDirectoryPath} уже существует.");
                        // Вывод предупреждения или выполнение других действий
                    }
                }
                MessageBox.Show("well doneово");
            }
        } // Метод создания директорий

        private void MakeDir_Button(object sender, EventArgs e) // Слздание директорий
        {
            // GrantFolderAccess();
            CreateShares();

        }

        private void MakeSMBShare_Button(object sender, EventArgs e)
        {
            ShareModule shareModule = new ShareModule(dataGridView1);
            shareModule.dataGridView1 = dataGridView1;
            shareModule.GrantFolderAccess();
        }

        private void Modify_CheckedChanged(object sender, EventArgs e)
        {
            if (Modify.Checked)
            {
                ReadAndExecute.Checked = true;
                ReadAndExecute.Enabled = false;

                ListDirectory.Checked = true;
                ListDirectory.Enabled = false;

                Read.Checked = true;
                Read.Enabled = false;

                Write.Checked = true;
                Write.Enabled = false;
            }
            else
            {
                ReadAndExecute.Checked = false;
                ReadAndExecute.Enabled = true;

                ListDirectory.Checked = false;
                ListDirectory.Enabled = true;

                Read.Checked = false;
                Read.Enabled = true;

                Write.Checked = false;
                Write.Enabled = true;
            }
        } // Логика отображения чекбоксов
    }
}
