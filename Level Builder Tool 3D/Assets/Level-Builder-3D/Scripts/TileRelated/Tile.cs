using UnityEngine;

[System.Serializable]
public class Tile 
{
     public Transforms tileTransforms;
     public string tileTag;

    public void SetTileValues(TileInfo newData)
    {
        tileTag = newData.tileTag;
        tileTransforms = newData.tileTransforms;
    }

    
}
