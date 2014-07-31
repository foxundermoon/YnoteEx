using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnoteEx
{
    class Ynote
    {
        YnoteUser user;
        public MyWebClient WebClient;
        ILoger _Log;
        public ILoger Log
        {
            set 
            {
                _Log = value;
                if(null != WebClient)
                {
                    WebClient.Log = value;
                }

            }
        }
        public Ynote()
        {
           
        }

        public void Init()
        {
             user = new YnoteUser("fox@vvfox.com","232381204");
            WebClient = new MyWebClient(user);

        }

        public bool Login()
        {
            return WebClient.Login();
        }

        public bool CreatNote()

    }
}
