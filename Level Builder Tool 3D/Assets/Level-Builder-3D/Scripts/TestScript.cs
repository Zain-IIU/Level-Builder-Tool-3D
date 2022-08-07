using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;



// ReSharper disable once CheckNamespace
public class TestScript : MonoBehaviour
    {
        private enum State
        {
            Tiling,
            Props
        }
           
        [SerializeField] private TileInfo[] groundTilePrefab;
        [SerializeField] private GameObject prop;
        [SerializeField] private string tileName;
        private  List<TileInfo> _tiles = new List<TileInfo>();
       private Vector3 _newPosition;
       [SerializeField] private LayerMask groundLayer;
       [SerializeField] private LayerMask handlerLayer;
       [SerializeField] private LayerMask tileLayer;
       [SerializeField] private bool hasPutFirstTile;
        private RaycastHit _hit;
        private Ray _ray;
        int curPrefabIndex;
        
        [SerializeField] private State designState;
              private void Update()
             {
                 if (Camera.main is { }) _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                 StatePattern();
                 
                 if (Input.GetKeyDown(KeyCode.Space))
                 {
                     designState = designState == State.Props ? State.Tiling : State.Props;
                 }

                
             }


              private void StatePattern()
              {
                  switch (designState)
                  {
                      case State.Tiling:
                          if (Input.GetKeyDown(KeyCode.R))
                          {
                              var transformLocalRotation = _tiles[_tiles.Count - 1].transform.GetChild(0).localRotation;
                              transformLocalRotation = Quaternion.Euler(0, transformLocalRotation.eulerAngles.y + 90, 0);
                              _tiles[_tiles.Count - 1].transform.GetChild(0).localRotation = transformLocalRotation;

                          }
                          if (Input.GetKeyDown(KeyCode.Z))
                          {
                              curPrefabIndex++;
                              curPrefabIndex %= groundTilePrefab.Length;
                          }
                          if (Input.GetMouseButtonDown(0))
                          {
                             
                              RaycastHandles();
                              if (hasPutFirstTile) return;
                              hasPutFirstTile = true;
                              SpawnBaseModel();
                          }

                          break;
                      case State.Props:
                          RaycastProps();
                          break;
                      default:
                          throw new ArgumentOutOfRangeException();
                  }
              }
              
             
             #region Design Level
      
              
              [ContextMenu("Save Data")]
              public void SaveLevel()
              {
                  var prefabObject = Instantiate(gameObject);
                  LevelSaveSystem.SaveLevel(prefabObject);
              }
              
      
              #endregion
      
              
              #region Raycasting to spawn Object
      
              private void SpawnBaseModel()
              {
                  if (!Physics.Raycast(_ray, out _hit, 200, groundLayer)) return;
      
                  _hit.collider.enabled = false;
                  _newPosition = _hit.point;
                  _newPosition.y = 0;
                  var levelElement = Instantiate(groundTilePrefab[curPrefabIndex], transform, true);
                  var transform2 = levelElement.transform;
                  transform2.localPosition = _newPosition;
                  levelElement.tileTagInScene = tileName;
                  _tiles.Add(levelElement.GetComponent<TileInfo>());
              }
      
              private void RaycastHandles()
              {
                  if (!Physics.Raycast(_ray, out _hit, 200,handlerLayer)) return;
      
                  var newPos = _hit.transform.GetComponentInParent<TileInfo>().transform.localPosition;
                  switch (_hit.transform.tag) 
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

              private bool gotProp;
              private GameObject propInfo;
              private void RaycastProps()
              {
                  if (!Physics.Raycast(_ray, out _hit, 200,tileLayer)) return;

                  if (Input.GetMouseButtonDown(0))
                      gotProp = !gotProp;
                  
                  if (!gotProp)
                  {
                      gotProp = true;
                      propInfo=Instantiate(prop, transform, true);
                  }

                  propInfo.transform.position = _hit.point;


              }
      
              private void SpawnNewElementAt(Vector3 newPos)
              {
                  var levelElement = Instantiate(groundTilePrefab[curPrefabIndex], transform, true);
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
