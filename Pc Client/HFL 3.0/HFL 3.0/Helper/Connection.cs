using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Runtime.Serialization;

namespace HFL_3._0
{
    public static class Connection
    {
        public static async Task<bool> login(string username, string password, string hwid, Awesomium.Core.JSObject callbackarg)
        {
            var values = new Dictionary<string, string>();
            values.Add("username", username);
            values.Add("password", password);
            values.Add("hwid", hwid);
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

                        string data = await httpResponseMessage.Content.ReadAsStringAsync();

                        var res = JSONSerializer<serviceRes>.DeSerialize(data);

                        if (res.err != null && res.userData == null)
                        {
                            callbackarg?.Invoke("call", callbackarg, false);
                            MessageBox.Show(res.err);
                            return false;
                        }
                        else
                        {
                            if (res.userData != null)
                            {
                                App.Client = JSONSerializer<User>.DeSerialize(data);
                                callbackarg?.Invoke("call", callbackarg, data);
                                LoginContract loginDetails = new LoginContract();
                                loginDetails.username = username;
                                loginDetails.password = password;
                                Storage.SerializeObject(loginDetails, "loginDetails.xml");
                                return true;
                            }
                            else
                            {
                                callbackarg?.Invoke("call", callbackarg, false);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Server not responding to your request.");
                        callbackarg?.Invoke("call", callbackarg, false);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    callbackarg?.Invoke("call", callbackarg, false);
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
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Cant check for updates terminating.");
                Process.GetCurrentProcess().Kill();
            }
            return false;
        }

        public static class JSONSerializer<TType> where TType : class
        {
            /// <summary>
            /// Serializes an object to JSON
            /// </summary>
            public static string Serialize(TType instance)
            {
                var serializer = new DataContractJsonSerializer(typeof(TType));
                using (var stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, instance);
                    return Encoding.Default.GetString(stream.ToArray());
                }
            }

            /// <summary>
            /// DeSerializes an object from JSON
            /// </summary>
            public static TType DeSerialize(string json)
            {
                using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(TType));
                    return serializer.ReadObject(stream) as TType;
                }
            }
        }

        [DataContract]
        public class serviceRes
        {
            [DataMember]
            public string message { get; set; }

            [DataMember]
            public string err { get; set; }

            [DataMember]
            public userData userData { get; set; }

            [DataMember]
            public forumData forumData { get; set; }
        }


        [Serializable]
        public class LoginContract
        {
            public string username { get; set; }
            public string password { get; set; }
        }
    }
}
