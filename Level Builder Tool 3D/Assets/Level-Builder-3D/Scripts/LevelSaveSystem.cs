using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
   public class LevelSaveSystem : MonoBehaviour
   {
       public static void SaveLevel(GameObject levelParent)
       {
           var localPath = "Assets/Prefabs" + levelParent.name + ".prefab";
           localPath=AssetDatabase.GenerateUniqueAssetPath(localPath);

           PrefabUtility.SaveAsPrefabAssetAndConnect(levelParent, localPath, InteractionMode.UserAction);
       }
   }
