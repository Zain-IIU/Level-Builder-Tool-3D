using UnityEngine;

// ReSharper disable once CheckNamespace
    public class TileInfo : MonoBehaviour
    {
        public Transforms tileTransforms;
        public string tileTag;
        public LevelTilingInformation tileScriptable;
        public void UpdatePos(Transforms newTransform,string tag)
        {
            tileTransforms = newTransform;
            tileTag = tag;
        }
    }
