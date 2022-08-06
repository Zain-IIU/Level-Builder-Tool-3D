using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// ReSharper disable once CheckNamespace
   public class LevelSaveSystem : MonoBehaviour
   {
       private const string SAVE_PATH = "/levelElements";
       
        public static void SaveLevel<T>(T obj,string key)
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + SAVE_PATH;
            Directory.CreateDirectory(path);
            var fileStream = new FileStream(path + key, FileMode.Append);
            formatter.Serialize(fileStream,obj);
            fileStream.Close();
        }

        public static T Load<T>(string key)
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + SAVE_PATH;

            T data = default;
            if (File.Exists(path + key))
            {
                var fileStream = new FileStream(path + key, FileMode.Open);
                data = (T)formatter.Deserialize(fileStream);
                fileStream.Close();
            }
            else
            {
                print("File Doesn't Exists");
            }
            return data;
        }
}
