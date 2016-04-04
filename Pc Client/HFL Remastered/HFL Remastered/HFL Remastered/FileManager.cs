using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace HFL_Remastered
{
    public static class FileManager
    {
        public static bool checkPaths()
        {
            if (!checkBol()) setBol();
            if (!checkLol()) setLol();
            return checkBol() && checkLol();
        }

        public static bool checkBol()
        {
            if (Properties.Settings.Default.bolPath != "")
            {
                if (File.Exists(Properties.Settings.Default.bolPath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool checkLol()
        {
            if (Properties.Settings.Default.lolPath != "")
            {
                if (File.Exists(Properties.Settings.Default.lolPath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void setBol()
        {
            bool fileSet = false;
            while (!fileSet)
            {
                MessageBox.Show("Please select Bol Studio.exe");
                var dlg = new OpenFileDialog();
                dlg.Filter = "BoL Studio.exe" + "|" + "BoL Studio.exe";
                if (dlg.ShowDialog() == true)
                {
                    if (File.Exists(dlg.FileName))
                    {
                        fileSet = true;
                        Properties.Settings.Default.bolPath = dlg.FileName;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }

        public static void setLol()
        {
            bool fileSet = false;
            while (!fileSet)
            {
                MessageBox.Show("Please select lol.launcher.exe");
                var dlg = new OpenFileDialog();
                dlg.Filter = "lol.launcher" + "|" + "lol.launcher.exe";
                if (dlg.ShowDialog() == true)
                {
                    if (File.Exists(dlg.FileName))
                    {
                        fileSet = true;
                        Properties.Settings.Default.lolPath = dlg.FileName;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }

        public static void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }

            return objectOut;
        }

        public static bool checkRecovery()
        {
            if (File.Exists("smurfs.recovery") || File.Exists("smurfs.recovery"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void LockCamera()
        {
            try
            {
                var filePath = Path.GetDirectoryName(Properties.Settings.Default.lolPath) + "\\Config\\game.cfg";
                var lines = File.ReadAllLines(filePath);
                int index = 0;
                int lockPos = 0;
                int heightPos = 0;
                int widthPos = 0;
                int windowModeIndex = 0;
                foreach (string line in lines)
                {
                    if (line.Contains("CameraLockMode="))
                    {
                        lockPos = index;
                    }
                    if (line.Contains("Height=") && !line.Contains("ItemShopPrevResize"))
                    {
                        heightPos = index;
                    }
                    if (line.Contains("Width=") && !line.Contains("ItemShopPrevResize"))
                    {
                        widthPos = index;
                    }
                    if (line.Contains("WindowMode"))
                    {
                        windowModeIndex = index;
                    }
                    index++;
                }
                lines[lockPos] = "CameraLockMode=1";
                lines[heightPos] = "Height=480";
                lines[widthPos] = "Width=640";
                lines[windowModeIndex] = "WindowMode=1";
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
