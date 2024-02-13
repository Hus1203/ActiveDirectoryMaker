using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMaking
{
    [Serializable]
    public class UserList
    { 
        // Группа,Фамилия,Имя,Отчество,Инициалы
        public UserList(string Group, string Surname, string Name, string Lastname, string Initials) {

            Группа = Group;
            Фамилия = Surname;
            Имя = Name;
            Отчество = Lastname;
            Инициалы = Initials;
        }

        public UserList() { }

        [DisplayName("Группа")]
        public string Группа { get; set; }
        [DisplayName("Фамилия")]
        public string Фамилия { get; set; }
        [DisplayName("Имя")]
        public string Имя { get; set; }
        [DisplayName("Отчество")]
        public string Отчество { get; set;}
        [DisplayName("Инициалы")] 
        public string Инициалы { get; set;}




        //public void Updatinfo(string Group, string Surname, string Name, string Lastname, string Initials)
        //{
        //    Группа = Group;
        //    Фамилия = Surname;
        //    Имя = Name;
        //    Отчество = Lastname;
        //    Инициалы = Initials;
        //}

    }
}
