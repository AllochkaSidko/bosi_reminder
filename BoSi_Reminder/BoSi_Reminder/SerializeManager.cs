using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BoSi_Reminder
{
    //інтерфейс, метод якого повертає назуву файлу, де буде серіалізований користувач
    internal interface ISerializable
    {
        string Filename { get; }
    }

      
        internal static class SerializeManager
        {
            //якщо немає такої директорії, то ми її створюємо і повертаємо повний шлях до файлу
            public static string CreateAndGetPath(string filename)
            {
                if (!Directory.Exists(StaticResources.DirPath))
                    Directory.CreateDirectory(StaticResources.DirPath);

                return Path.Combine(StaticResources.DirPath, filename);
            }

        //метод серіалізації
        public static void Serialize<TObject>(TObject obj) where TObject : ISerializable
            {

            //відбувається серіалізація об'єкту у бінарний формат і записується у файл
            //якщо файлу нема, то він попередньо створиться
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
                //запис помилки у лог-файл
                LogWriter.LogWrite("Serialize method",e);
            }
            }

        //метод десеріалізації
        public static TObject Deserialize<TObject>(string filename) where TObject : ISerializable, new()
            {
            //з бінарного формату десеріалізується назад в об'єкт
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
                LogWriter.LogWrite("Deserialize method",e);
            }

            return new TObject();
            }
        }
    }


