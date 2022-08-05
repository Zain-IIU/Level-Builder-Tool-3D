using UnityEngine;


    public class TileInfo : MonoBehaviour
    {
        public Transforms tileTransforms;
        public string tileTag;

        public void SetTileValues(Tile newData)
        {
            tileTag = newData.tileTag;
            tileTransforms = newData.tileTransforms;
        }
        public void UpdatePos(Transforms newTransform,string tag)
        {
            tileTransforms = newTransform;
            tileTag = tag;
        }
    }
