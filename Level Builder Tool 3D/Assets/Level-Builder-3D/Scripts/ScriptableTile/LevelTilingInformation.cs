using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(fileName = "Level Design", menuName = "Tiling Information", order = 0)]
    public class LevelTilingInformation : ScriptableObject
    {
        public List<Tile> elementInfo = new List<Tile>();
    }
