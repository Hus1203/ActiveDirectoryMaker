using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace UserMaking
{
    public partial class ADModule : Conf
    {
        //public static DataGridViewRow view { get; set; }
        // List<UserList> userlist;

       // string dc1 = "mydomain";
       // string dc2 = "com";

       // Conf conf = new Conf();

        public string oug;
        string group;
       // string domain = "mydomain.com";
        string sn;
        string gname;
        string init;
        string fullName;
        string gr;
        string login;
        string normLogin;

        public DataGridView dataGridView1;

        public ADModule(DataGridView dataGridView)
        {
            dataGridView1 = dataGridView;
        }

        public bool OuExists(string ouName)
        {
            using (DirectoryEntry root = new DirectoryEntry($"LDAP://dc={dc1},dc={dc2}"))
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

                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain, domain, "OU=" + gr + ",DC=" + dc1 + ",DC=" + dc2))
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
            using (DirectoryEntry group = new DirectoryEntry("LDAP://mydomain.com/OU=" + gr + ",dc=" + dc1 + ",dc=" + dc2 ))
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
            using (DirectoryEntry root = new DirectoryEntry($"LDAP://dc={dc1},dc={dc2}"))
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

        private bool UserExists(string login)
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, domain))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, login);
                return user != null;
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

                if (!UserExists(login))
                {
                    if (!UserOuExists(login, gr))
                    {
                        CreateUserOu(login, gr);
                    }

                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain, domain, "OU=" + login + ",OU=" + gr + ", DC = " + dc1 + ", DC = " + dc2))
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


                        using (PrincipalContext groupContext = new PrincipalContext(ContextType.Domain, "mydomain.com", "OU=" + gr + ", DC = " + dc1 + ", DC = " + dc2))
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
            }
            MessageBox.Show("пользователи созданы!");
        }





    }
}
