
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;



// ReSharper disable once CheckNamespace
public class TestScript : MonoBehaviour
    {
        [SerializeField] private TileInfo groundTilePrefab;
        [SerializeField] private string tileName;
        private  List<TileInfo> _tiles = new List<TileInfo>();
       private Vector3 _newPosition;
       [SerializeField] private LayerMask groundLayer;
       [SerializeField] private LayerMask handlerLayer;
       [SerializeField] private bool hasPutFirstTile;
        private RaycastHit _hit;
        private RaycastHit _hitHandler;
        private Ray _ray;
            
              private void Update()
             {
                 if (Input.GetKeyDown(KeyCode.R))
                 {
                     var transformLocalRotation = _tiles[_tiles.Count - 1].transform.localRotation;
                     transformLocalRotation = Quaternion.Euler(0, transformLocalRotation.eulerAngles.y + 90, 0);
                     _tiles[_tiles.Count - 1].transform.localRotation = transformLocalRotation;

                 }
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
      
              
              [ContextMenu("Save Data")]
              public void SaveLevel()
              {
                  LevelSaveSystem.SaveLevel(this.gameObject);
                  //SaveLoadSystem.SaveLoadSystem.Save();
              }
              
      
              #endregion
      
              #region Generate Level
              
              [ContextMenu("Generate Data")]
              public void GenerateLevel()
              {
                  //SaveLoadSystem.SaveLoadSystem.Load();
              }
      
              #endregion
              
              #region Raycasting to spawn Object
      
              private void SpawnBaseModel()
              {
                  if (!Physics.Raycast(_ray, out _hit, 200, groundLayer)) return;
      
                  _hit.collider.enabled = false;
                  _newPosition = _hit.point;
                  _newPosition.y = 0;
                  var levelElement = Instantiate(groundTilePrefab, this.transform, true);
                  var transform2 = levelElement.transform;
                  transform2.localPosition = _newPosition;
                  levelElement.tileTagInScene = tileName;
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
                  var levelElement = Instantiate(groundTilePrefab, this.transform, true);
                  var transform2 = levelElement.transform;
                  transform2.localPosition = newPos;
                  levelElement.tileTagInScene = tileName;
                  _tiles.Add(levelElement.GetComponent<TileInfo>());
              }
              #endregion
      
              private void OnDestroy()
              {
                  DOTween.KillAll();
              }
        
    }
