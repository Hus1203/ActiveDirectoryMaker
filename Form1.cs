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
using System.Diagnostics;

namespace UserMaking
{
    public partial class Form1 : Form
    {
        // Предусловие. В коде по прежнему присутсвует хардкодинг.
        List<UserList> userlist;

        public string oug;
        string group;
        string sn;
        string gname;
        string init;
        string fullName;
        string gr;
        string login;
        string normLogin;

        Conf conf = new Conf();
        string baseDirectory = Conf.baseDirectory;

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
        } // Загрузка из файла в листбокс

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadCSV readCSV = new ReadCSV();
            readCSV.LoadCSV();
            LoadCSVIntoListBox();
        } // Загрузка файла
        
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
            if (CheckBoxe(checkboxes)) // Проверка 
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

        private void DeleteGroup_button_Click(object sender, EventArgs e) 
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
        } // Удаление директорий в Active directory

        public void CreateShares()
        {     
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

        private void MakeDir_Button(object sender, EventArgs e) 
        {
            CreateShares();
        } // Создание директорий

        private void MakeSMBShare_Button(object sender, EventArgs e)
        {
            ShareModule shareModule = new ShareModule(dataGridView1);
            shareModule.dataGridView1 = dataGridView1;
            shareModule.GrantFolderAccess();
        } // Создание Шары


        /// Логика отображения чекбоксов
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
        }  

        private void ReadAndExecute_CheckedChanged(object sender, EventArgs e)
        {
            if (ReadAndExecute.Checked)
            {
                ListDirectory.Checked = true;
                ListDirectory.Enabled = false;

                Read.Checked = true;
                Read.Enabled = false;

            }
            else
            {
                ListDirectory.Checked = false;
                ListDirectory.Enabled = true;

                Read.Checked = false;
                Read.Enabled = true;

            }

        }

        private void FullControl_CheckedChanged(object sender, EventArgs e)
        {
            if (FullControl.Checked)
            {
                Modify.Checked = true;
                Modify.Enabled = false;

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
                Modify.Checked = false; 
                Modify.Enabled = true;

                ReadAndExecute.Checked = false;
                ReadAndExecute.Enabled = true;

                ListDirectory.Checked = false;
                ListDirectory.Enabled = true;

                Read.Checked = false;
                Read.Enabled = true;

                Write.Checked = false;
                Write.Enabled = true;
            }

        }
    }
}
