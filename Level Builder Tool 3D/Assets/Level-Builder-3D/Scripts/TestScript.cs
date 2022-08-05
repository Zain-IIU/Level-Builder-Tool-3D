using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


public class TestScript : MonoBehaviour
    {
        [SerializeField] private TileInfo groundTilePrefab;

        [SerializeField] private Vector2 tilesRandomizer;
        public LevelTilingInformation levelDesignScriptableObject;
        [SerializeField] private string tileName;
        private  List<TileInfo> _tiles = new List<TileInfo>();



        #region Design Level

        public void DrawLevel()
        {
            var randomX = Random.Range(-tilesRandomizer.x, tilesRandomizer.x);
            var randomZ = Random.Range(-tilesRandomizer.y, tilesRandomizer.y);

            var levelElement = Instantiate(groundTilePrefab);
            levelDesignScriptableObject.elementInfo.Add(null);
            levelElement.gameObject.SetActive(false);
            levelElement.transform.DOMove(Vector3.zero, 0);
            levelElement.transform.DOMoveX(randomX, 0);
            levelElement.transform.DOMoveZ(randomZ, 0).OnComplete(() =>
            {
                levelElement.gameObject.SetActive(true);
                levelElement.tileTag = tileName;
                var transform1 = levelElement.transform;
                print(transform1.localPosition);
                print(transform1.localRotation);
                levelElement.tileTransforms.SetTransforms(transform1.localPosition, transform1.localScale,
                    transform1.localRotation);
                _tiles.Add(levelElement.GetComponent<TileInfo>());
            });
        }
        [ContextMenu("Save Data")]
        public void SaveLevel()
        {

            var curIndex = levelDesignScriptableObject.elementInfo.Count - _tiles.Count;
           
            print(levelDesignScriptableObject.elementInfo[curIndex].tileTransforms);
             foreach (var curTile in _tiles)
            {
                print ("saving data");
                levelDesignScriptableObject.elementInfo[curIndex++].SetTileValues(curTile);
            }
        }
        

        #endregion

        #region Generate Level
        
        [ContextMenu("Generate Data")]
        public void GenerateLevel()
        {
            if (levelDesignScriptableObject.elementInfo.Count == 0)
            {
                print("No Data to generate");
                return;
            }

            foreach (var elementInfo in levelDesignScriptableObject.elementInfo)
            {
                var element = Instantiate(groundTilePrefab);
                element.gameObject.SetActive(false);
                element.transform.DOLocalMove(Vector3.zero, 0);
                element.UpdatePos(elementInfo.tileTransforms,elementInfo.tileTag);
                
                var transform1 = element.transform;
                
                
                var elementLocalTransform = transform1.localPosition;
                var elementLocalScale = transform1.localScale;
                var elementLocalRot = transform1.localRotation;

                elementLocalTransform = elementInfo.tileTransforms.GetPos();
                elementLocalScale = elementInfo.tileTransforms.GetScale();
                elementLocalRot = elementInfo.tileTransforms.GetRot();

                element.transform.localPosition = elementLocalTransform;
                element.transform.localScale = elementLocalScale;
                element.transform.localRotation = elementLocalRot;
                
                element.gameObject.SetActive(true);
            }
           
        }

        #endregion

        private void OnDestroy()
        {
            DOTween.KillAll();
        }
    }
