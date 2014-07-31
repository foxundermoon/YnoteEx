using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace YnoteEx
{
    public partial class Form1 : Form , ILoger
    {
        Ynote Ynote;
        WebBrowser webBrowser1;
        CookieContainer container;
        TextBox textBoxLog;
        MyWebClient client;
        //string loginUrl = "http://account.youdao.com/login?back_url=http://note.youdao.com";
        string loginUrl = "http://reg.163.com/";
        public Form1()
        {
            InitializeComponent();
            init();
        }

        void init()
        {
            container = new CookieContainer();
            webBrowser1 = new WebBrowser();
            //webBrowser1.Url = new Uri(loginUrl);
            //panel1.Controls.Add(webBrowser1);
            //webBrowser1.Bounds = panel1.Bounds;
            textBoxLog = textBox4;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ynote = new Ynote();
            Ynote.Init();
            Ynote.Log = this;
            Ynote.Login();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using(var client = new WebClientEx())
            {
                client.CookieContainer = this.container;
                //textBox1.Text= client.DownloadString("http://note.youdao.com/");
                client.DownloadFile("http://note.youdao.com/",@"D:\youdao\youdao.com.txt");
                //using (var fs = File.Create(@"D:\youdao\youdao.com.txt"))
                //{
                    //fs.Write()
                //}

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://note.youdao.com/");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(loginUrl);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = WebRequest.CreateHttp(new Uri("http://note.youdao.com"));
            //request.CookieContainer = this.container;
            request.Headers.Set(HttpRequestHeader.Cookie,textBox1.Text);
            request.Accept = "*/*";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            container.Add( response.Cookies);
            Stream sm = response.GetResponseStream();
            FileStream fs = File.Create(@"D:\youdao\youdao.com.txt");
            StreamWriter sw = new StreamWriter(fs);
            StreamReader sr = new StreamReader(sm);
            sw.Write(sr.ReadToEnd());
            //fs.Close();
            //sm.Close();
            sw.Close();
            sr.Close();
        }

        public void i(string s)
        {
            textBoxLog.AppendText(s + "\n");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            client = new MyWebClient();
            client.Log = this;
            client.Login();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(client == null)
            {
                MessageBox.Show("请先登录！！！");
            }
            else
            {
                client.DoPostTest();
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string postUrl = textBox2.Text;
            string postData = textBox3.Text;

            i(client.DoPost(postUrl,postData));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBoxLog.ResetText();
        }
    }
}
