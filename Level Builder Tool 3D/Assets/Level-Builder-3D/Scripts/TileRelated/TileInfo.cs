using UnityEngine;
using SaveLoadSystem;



// ReSharper disable once CheckNamespace
[RequireComponent(typeof(SaveableEntity))]
    public class TileInfo : MonoBehaviour,ISaveable
    {
        public string tileTagInScene;
        public SaveableEntity.Vector3Data tilePosInScene;
        
        #region interface methods

        public bool NeedsToBeSaved()
        {
            return true;
        }

        public bool NeedsReinstantiation()
        {
            return false;
        }

        public object SaveState()
        {
            return new TileData
            {
                tileTag = tileTagInScene,
                tilePos = tilePosInScene
            };

        }

        public void LoadState(object state)
        {
            var data =(TileData) state;
            this.tileTagInScene = data.tileTag;
            this.tilePosInScene = data.tilePos;
        }

        public void PostInstantiation(object state)
        {
            var data =(TileData) state;
        }

        public void GotAddedAsChild(GameObject obj, GameObject hisParent)
        {
            print("Not added as child");
        }

        #endregion

       
        [System.Serializable]
        struct TileData
        {
            public string tileTag;
            public SaveableEntity.Vector3Data tilePos;
            public SaveableEntity.Vector4Data tileRot;
        }
       
    }
