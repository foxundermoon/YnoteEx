using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnoteEx
{
    public class YnoteUser
    {
        public string Name{get;set;}
        public string Password{get;set;}
        public YnoteUser(string name, string password)
        {
            Name = name;
            Password = password;
        }
        public YnoteUser()
        { }
    }
}
