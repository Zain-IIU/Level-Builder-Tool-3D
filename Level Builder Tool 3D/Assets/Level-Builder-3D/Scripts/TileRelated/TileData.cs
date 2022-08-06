
// ReSharper disable once CheckNamespace

[System.Serializable]
    public class TileData
    {
        public static int DataCounter=0;
        public float[] tilePos;

        public string tileTag;

        public TileData(TileInfo tile)
        {
            tileTag = tile.tileScriptable.elementInfo[DataCounter++].tileTag;
            
        
        }
        

    }
