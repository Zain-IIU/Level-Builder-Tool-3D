[System.Serializable]
// ReSharper disable once CheckNamespace
public class TileMap 
{
     public Transforms tileTransforms;
     public string tileTag;

    public void SetTileValues(TileInfo newData)
    {
        tileTag = newData.tileTag;
        tileTransforms = newData.tileTransforms;
    }

    
}
