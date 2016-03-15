using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsFreeLeveler
{
    public class Dictionary
    {
        public static English_Base English = new English_Base();
        public static Turkish_Base Turkish = new Turkish_Base();


        public class English_Base
        {
            public List<lang_def> words = new List<lang_def>();

            public English_Base()
            {
                words.Add(new lang_def("t1", "1. Start BoL Studio.exe"));
                words.Add(new lang_def("loginButton", "Login"));
                words.Add(new lang_def("registerButton", "Register"));
                words.Add(new lang_def("continue", "Continue"));
            }
        }

        public class Turkish_Base
        {
            public List<lang_def> words = new List<lang_def>();

            public Turkish_Base()
            {
                words.Add(new lang_def("t1", "1. Bol Studio.exe'yi başlat"));
                words.Add(new lang_def("loginButton", "Giriş Yap"));
                words.Add(new lang_def("registerButton", "Kayıt Ol"));
                words.Add(new lang_def("continue", "Devam et"));
            }
        }

        public string text(string prop)
        {
            if (Settings.language == "English")
            {
                return English.words.First(word => (word.name == prop)).content;
            }else if(Settings.language == "Türkçe"){
                return Turkish.words.First(word => (word.name == prop)).content;
            }
            else
            {
                return English.words.First(word => (word.name == prop)).content;
            }
        }
    }

    public class lang_def{
        public string name;
        public string content;

        public lang_def(string n, string c)
        {
            name = n;
            content = c;
        }
    }
}
