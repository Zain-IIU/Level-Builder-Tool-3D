using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
   public class LevelSaveSystem : MonoBehaviour
   {
       [SerializeField] private string prefabSavePath;
       private static int counter = 0;
       public  void SaveLevel(GameObject levelParent)
       {
           counter++;
           var customPath = prefabSavePath;
           var localPath = customPath + levelParent.name +counter + ".prefab";
           localPath=AssetDatabase.GenerateUniqueAssetPath(localPath);

           PrefabUtility.SaveAsPrefabAssetAndConnect(levelParent, localPath, InteractionMode.UserAction);
       }
   }
