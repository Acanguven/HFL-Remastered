using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;

namespace HFL_Remastered
{
    public static class Connection
    {
        public static async Task<bool> login(string username, string password, string hwid)
        {
            Properties.Settings.Default.token = "";
            var values = new Dictionary<string, string>();
            values.Add("username", username);
            values.Add("password", password);
            values.Add("hwid", HWID.Generate());
            var content = new FormUrlEncodedContent(values);

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
                    var httpResponseMessage = await client.PostAsync("http://handsfreeleveler.com:4446/api/remotelogin", content);

                    if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        String data = await httpResponseMessage.Content.ReadAsStringAsync();
                        dynamic res = JsonConvert.DeserializeObject<object>(data);

                        if (IsPropertyExists(res, "err"))
                        {
                            throw new Exception((string)res.err);
                        }
                        else { 
                            if (IsPropertyExists(res, "userData"))
                            {
                                App.Client = JsonConvert.DeserializeObject<User>(data);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Server not responding to your request.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Application.Current.Shutdown();
                    return false;
                }
            }
        }


        public static async Task<bool> updateCheck()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://remote.handsfreeleveler.com/");
            client.DefaultRequestHeaders.Accept.Clear();

            // HTTP GET
            try
            {
                HttpResponseMessage response = await client.GetAsync("version.txt");
                if (response.IsSuccessStatusCode)
                {
                    String data = await response.Content.ReadAsStringAsync();
                    if (data == App.version)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("Cant check for updates terminating.");
                    Application.Current.Shutdown();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Cant check for updates terminating.");
                Application.Current.Shutdown();
            }
            return false;
        }

        public static bool IsPropertyExists(dynamic dynamicObj, string property)
        {
            try
            {
                var value = dynamicObj[property].Value;
                return true;
            }
            catch (RuntimeBinderException)
            {

                return false;
            }

        }
    }
}
