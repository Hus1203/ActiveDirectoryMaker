﻿using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMaking
{
    public partial class DeletingAD
    {
        string dc1 = "mydomain";
        string dc2 = "com";
        public void DeleteAllGroups()
        {
            using (DirectoryEntry root = new DirectoryEntry($"LDAP://dc={dc1},dc={dc2}"))
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

    }
}
