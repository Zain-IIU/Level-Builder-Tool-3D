using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


// ReSharper disable once CheckNamespace
public class TestScript : MonoBehaviour
    {
        [SerializeField] private TileInfo groundTilePrefab;
        [SerializeField] private Vector2 tilesRandomizer;
        [SerializeField] private string tileName;
        private  List<TileInfo> _tiles = new List<TileInfo>();
       private Vector3 _newPosition;
       [SerializeField] private GameObject testObject;
       [SerializeField] private LayerMask groundLayer;
       [SerializeField] private LayerMask handlerLayer;
       [SerializeField] private bool hasPutFirstTile;
        private RaycastHit _hit;
        private RaycastHit _hitHandler;
        private Ray _ray;
        /*     
              private void Update()
             {
                 if (Input.GetMouseButtonDown(0))
                 {
                     if (Camera.main is { }) _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                     RaycastHandles();
                     if (hasPutFirstTile) return;
                     hasPutFirstTile = true;
                     SpawnBaseModel();
                 }
             }
      
             
             #region Design Level
      
              public void DrawLevel()
              {
                  var randomX = Random.Range(-tilesRandomizer.x, tilesRandomizer.x);
                  var randomZ = Random.Range(-tilesRandomizer.y, tilesRandomizer.y);
      
                  var levelElement = Instantiate(groundTilePrefab);
                  scriptableObject.elementInfo.Add(null);
                  levelElement.gameObject.SetActive(false);
                  levelElement.transform.DOMove(Vector3.zero, 0);
                  levelElement.transform.DOMoveX(randomX, 0);
                  levelElement.transform.DOMoveZ(randomZ, 0).OnComplete(() =>
                  {
                      levelElement.gameObject.SetActive(true);
                      levelElement.tileTag = tileName;
                      var transform1 = levelElement.transform;
                      levelElement.tileTransforms.SetTransforms(transform1.localPosition, transform1.localScale,
                          transform1.localRotation);
                      _tiles.Add(levelElement.GetComponent<TileInfo>());
                  });
              }
              [ContextMenu("Save Data")]
              public void SaveLevel()
              {
                  var curIndex = scriptableObject.elementInfo.Count - _tiles.Count;
                  
                  print(curIndex);
                   foreach (var curTile in _tiles)
                  {
                      try
                      {
                          scriptableObject.elementInfo[curIndex++].SetTileValues(curTile);
                      }
                      catch (Exception e)
                      {
                          Camera.main.backgroundColor=Color.red;
                          throw;
                      }
                      
                  }
              }
              
      
              #endregion
      
              #region Generate Level
              
              [ContextMenu("Generate Data")]
              public void GenerateLevel()
              {
                  if (scriptableObject.elementInfo.Count == 0)
                  {
                      print("No Data to generate");
                      Camera.main.backgroundColor=Color.green;
                      return;
                  }
      
                  foreach (var elementInfo in scriptableObject.elementInfo)
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
              
              #region Raycasting to spawn Object
      
              private void SpawnBaseModel()
              {
                  if (!Physics.Raycast(_ray, out _hit, 200, groundLayer)) return;
      
                  _hit.collider.enabled = false;
                  _newPosition = _hit.point;
                  _newPosition.y = 0;
                  var levelElement = Instantiate(groundTilePrefab);
                  scriptableObject.elementInfo.Add(null);
                  var transform2 = levelElement.transform;
                  transform2.localPosition = _newPosition;
                  levelElement.tileTag = tileName;
                  var transform1 = transform2;
                  levelElement.tileTransforms.SetTransforms(transform1.localPosition, transform1.localScale,
                      transform1.localRotation);
                  _tiles.Add(levelElement.GetComponent<TileInfo>());
              }
      
              private void RaycastHandles()
              {
                  if (!Physics.Raycast(_ray, out _hitHandler, 200,handlerLayer)) return;
      
                  var newPos = _hitHandler.transform.GetComponentInParent<TileInfo>().transform.localPosition;
                  switch (_hitHandler.transform.tag)
                  {
                      case "Forward":
                          newPos.z += 2;
                          SpawnNewElementAt(newPos);
                          break;
                      case "Backward":
                          newPos.z -= 2;
                          SpawnNewElementAt(newPos);
                          break;
                      case "Left":
                          newPos.x -= 2;
                           SpawnNewElementAt(newPos);
                           break;
                      case "Right":
                          newPos.x += 2;
                          SpawnNewElementAt(newPos);
                           break;
                  }
              }
      
              private void SpawnNewElementAt(Vector3 newPos)
              {
                  var levelElement = Instantiate(groundTilePrefab);
                  scriptableObject.elementInfo.Add(null);
                  var transform2 = levelElement.transform;
                  transform2.localPosition = newPos;
                  levelElement.tileTag = tileName;
                  var transform1 = transform2;
                  levelElement.tileTransforms.SetTransforms(transform1.localPosition, transform1.localScale,
                      transform1.localRotation);
                  _tiles.Add(levelElement.GetComponent<TileInfo>());
              }
              #endregion
      
              private void OnDestroy()
              {
                  DOTween.KillAll();
              }*/
    }
