﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BoSi_Reminder
{
    internal interface ISerializable
    {
        string Filename { get; }
    }

      
        internal static class SerializeManager
        {
            private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            public static readonly string DirPath = Path.Combine(AppData, "BoSi_Reminder");
            public static string CreateAndGetPath(string filename)
            {
                if (!Directory.Exists(DirPath))
                    Directory.CreateDirectory(DirPath);

                return Path.Combine(DirPath, filename);
            }

            public static void Serialize<TObject>(TObject obj) where TObject : ISerializable
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    var filename = CreateAndGetPath(obj.Filename);

                    using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(fs, obj);
                    }
                }
                catch (Exception e)
                {
                LogWriter.LogWrite("Serialize method");
                LogWriter.LogWrite(e.Message);
                throw;

                }
            }

            public static TObject Deserialize<TObject>(string filename) where TObject : ISerializable
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                    {
                        return (TObject)formatter.Deserialize(fs);
                    }
                }
                catch (Exception e)
                {
                LogWriter.LogWrite("Deserialize method");
                LogWriter.LogWrite(e.Message);
                    throw;
                }
            }
        }
    }


