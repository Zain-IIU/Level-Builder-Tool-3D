using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
   public class LevelSaveSystem : MonoBehaviour
   {
       private static int counter = 0;
       public static void SaveLevel(GameObject levelParent)
       {
           counter++;
           var localPath = "Assets/Prefabs/" + levelParent.name +counter + ".prefab";
           localPath=AssetDatabase.GenerateUniqueAssetPath(localPath);

           PrefabUtility.SaveAsPrefabAssetAndConnect(levelParent, localPath, InteractionMode.UserAction);
       }
   }
