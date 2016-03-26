using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HFL_Remastered
{
    public static class Localization
    {
        public static void SetLanguageDictionary(Window obj)
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri("..\\Resources\\StringResources.en-us.xaml", UriKind.Relative);
                    break;
                case "tr-TR":
                    dict.Source = new Uri("..\\Resources\\StringResources.tr-tr.xaml", UriKind.Relative);
                    break;
            }
            if (dict.Source == null)
            {
                dict.Source = new Uri("..\\Resources\\StringResources.en-us.xaml", UriKind.Relative);
            }
            obj.Resources.MergedDictionaries.Add(dict);
        }

        static Localization()
        {
            //bla bla here
        }
    }
}
