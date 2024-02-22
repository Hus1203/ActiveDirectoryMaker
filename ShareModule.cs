using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserMaking
{
    public partial class ShareModule
    {
        public DataGridView dataGridView1;
        public ShareModule(DataGridView dataGridView)
        {
            dataGridView1 = dataGridView;
        }

        public void ShareFolder(string folderPath, string shareName)
        {
            // Создайте объект ManagementClass, представляющий класс Win32_Share
            ManagementClass managementClass = new ManagementClass("Win32_Share");

            // Получите все уже существующие общие ресурсы
            ManagementObjectCollection shares = managementClass.GetInstances();

            foreach (ManagementObject share in shares)
            {
                if (string.Equals((string)share["Name"], shareName, StringComparison.OrdinalIgnoreCase))
                {
                    // Общий ресурс с таким именем уже существует, попробуем удалить его
                    share.Delete();
                    break;
                }
            }

            // Создайте входной параметры для метода Create
            ManagementBaseObject inParams = managementClass.GetMethodParameters("Create");

            // Задайте входные параметры
            inParams["Path"] = folderPath;
            inParams["Name"] = shareName;
            inParams["Type"] = 0x0; // Disk Drive

            // Вызовите метод Create и получите возвращаемый результат
            ManagementBaseObject outParams = managementClass.InvokeMethod("Create", inParams, null);

            //if ((uint)(outParams.Properties["ReturnValue"].Value) != 0)
            //{
            //    throw new Exception("Ошибка при попытке поделиться папкой.");
            //}
        } // Метод расшаривания папок
        public void GrantFolderAccess()
        {
            string baseDirectory = "C:\\public";

            if(dataGridView1.DataSource == null)
            {
                MessageBox.Show("Откройте файл с пользователями и повторите попытку");
                return;
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                string groupName = row.Cells["Группа"].Value.ToString();
                string login = $"{row.Cells["Фамилия"].Value.ToString()}{row.Cells["Инициалы"].Value.ToString()}";

                string groupDirectoryPath = Path.Combine(baseDirectory, groupName);
                string userDirectoryPath = Path.Combine(groupDirectoryPath, login);

                if (Directory.Exists(userDirectoryPath))
                {
                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                    {
                        UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, login);
                        if (user != null)
                        {
                            DirectorySecurity directorySecurity = Directory.GetAccessControl(userDirectoryPath);
                            SecurityIdentifier sid = user.Sid; // Получаем Sid пользователя
                            NTAccount ntAccount = (NTAccount)sid.Translate(typeof(NTAccount)); // Получаем NTAccount из Sid

                            directorySecurity.AddAccessRule(new FileSystemAccessRule(ntAccount, FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                            Directory.SetAccessControl(userDirectoryPath, directorySecurity);
                            ShareFolder(userDirectoryPath, login);
                        }
                        else
                        {
                            MessageBox.Show($"Пользователь {login} не найден в Active Directory.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Директория {userDirectoryPath} не найдена.");
                }
            } 
        }

    }
}
