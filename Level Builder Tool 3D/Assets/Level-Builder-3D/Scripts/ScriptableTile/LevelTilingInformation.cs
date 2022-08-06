using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
    [CreateAssetMenu(fileName = "Level Design", menuName = "Tiling Information", order = 0)]
    public class LevelTilingInformation : ScriptableObject
    {
        public List<TileMap> elementInfo = new List<TileMap>();
    }
