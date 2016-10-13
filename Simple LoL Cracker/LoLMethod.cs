using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SLOLC
{
            
    public class Objects : Dictionary<string, object>
    {
        public string type;

        public Objects()
        {
            this.type = null;
        }

        public Objects(string type)
        {
            this.type = type;
        }

        public string GetString(string key)
        {
            return (string)this[key];
        }
    }

    public class LoLMethod
    {
        private JavaScriptSerializer serializer = new JavaScriptSerializer(); // To handle the JavaScripts
        public string login; //The server to connect to

        /***********************************************************
         *https://lq.eun1.lol.riotgames.com EU Nordic'N'East #0
         *https://lq.br.lol.riotgames.com   Brasil #1
         *https://lq.la1.lol.riotgames.com  Latin America North #2
         *https://lq.la2.lol.riotgames.com  Latin America South #3
         *https://lq.na1.lol.riotgames.com  North America #4
         *https://lq.oc1.lol.riotgames.com  Oceaina #5
         *https://lq.ru.lol.riotgames.com   Russia #6
         *https://lq.tr.lol.riotgames.com   Turkey #7
         *https://lq.euw1.lol.riotgames.com EU West #8
         ***********************************************************/

        public string SetLogin(int region)
        {
            switch (region)
            {
                case 0:
                    login = "https://lq.eun1.lol.riotgames.com/";
                    break;
                case 1:
                    login = "https://lq.br.lol.riotgames.com/";
                    break;
                case 2:
                    login = "https://lq.la1.lol.riotgames.com/";
                    break;
                case 3:
                    login = "https://lq.la2.lol.riotgames.com/";
                    break;
                case 4:
                    login = "https://lq.na1.lol.riotgames.com/";
                    break;
                case 5:
                    login = "https://lq.oc1.lol.riotgames.com/";
                    break;
                case 6:
                    login = "https://lq.ru.lol.riotgames.com/";
                    break;
                case 7:
                    login = "https://lq.tr.lol.riotgames.com/";
                    break;
                case 8:
                    login = "https://lq.euw1.lol.riotgames.com/";
                    break;
            }
            return login;
        }

        public bool CheckAccount(string username, string password, string proxy)
        {

            try
            {
                StringBuilder sb = new StringBuilder();
                string payload = "user=" + username + "&password=" + password;
                string query = payload;
                WebProxy myproxy = new WebProxy(proxy, false);
                WebRequest con = WebRequest.Create(login + "login-queue/rest/queue/authenticate");
                con.Proxy = myproxy;
                con.Method = "POST";
                byte[] Bytes = Encoding.UTF8.GetBytes(query);
                con.ContentType = "application/x-www-form-urlencoded";
                con.ContentLength = Bytes.Length;
                Stream outputStream = con.GetRequestStream();
                outputStream.Write(Bytes, 0, Bytes.Length);
                WebResponse webresponse = con.GetResponse();
                Stream inputStream = webresponse.GetResponseStream();

                int c;
                while ((c = inputStream.ReadByte()) != -1)
                    sb.Append((char)c);

                Objects result = serializer.Deserialize<Objects>(sb.ToString());
                outputStream.Close();
                inputStream.Close();
                con.Abort();

                if (!result.ContainsKey("token"))
                {
                    while (sb.ToString() == null || !result.ContainsKey("token"))
                    {
                        try
                        {
                            sb.Clear();

                            con = WebRequest.Create(login + "login-queue/rest/queue/authToken/" + username.ToLower());
                            con.Method = "GET";
                            webresponse = con.GetResponse();
                            inputStream = webresponse.GetResponseStream();

                            int f;
                            while ((f = inputStream.ReadByte()) != -1)
                                sb.Append((char)f);

                            result = serializer.Deserialize<Objects>(sb.ToString());

                            inputStream.Close();
                            con.Abort();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }

                //label1.Text = "Status: You logged in successfully";
                //label1.ForeColor = Color.Green;

                return true;
            }
            catch
            {
                //label1.ForeColor = Color.Red;
                //label1.Text = "Status: Your username or password is incorrect!";
                return false;
            }
        }

    }
}
