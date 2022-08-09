using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


// ReSharper disable once CheckNamespace
public class LevelDesignTool : MonoBehaviour
{
    private enum State
    {
        Tiling,
        Props
    } 
    [SerializeField] private State designState;
    [SerializeField] private LevelSaveSystem saveSystem;
    [SerializeField] private TileInfo[] groundTilePrefabs;
    [SerializeField] private GameObject prop;
    private readonly List<Transform> _storedTiles = new List<Transform>();
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask handlerLayer;
    [SerializeField] private LayerMask tileLayer;
    [Range(0, 3)] [SerializeField] private float heightAdjuster;

    [SerializeField] private Transform pointer;

    #region Private Fields
    private bool _hasPutFirstTile;
    private RaycastHit _hit;
    private Ray _ray;
    private int _curPrefabIndex;
    private Vector3 _newPosition;
    private bool _gotProp;
    private GameObject _propInfo;
    #endregion
    
    
    private void Update()
    {
        if (Camera.main is { }) _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        StatePattern();
                 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            designState = designState == State.Props ? State.Tiling : State.Props;
        }

        if (!Input.GetKeyDown(KeyCode.R)) return;
        var transformLocalRotation = _storedTiles[_storedTiles.Count - 1].transform.GetChild(0).localRotation;
        transformLocalRotation = Quaternion.Euler(0, transformLocalRotation.eulerAngles.y + 90, 0);
        _storedTiles[_storedTiles.Count - 1].transform.GetChild(0).localRotation = transformLocalRotation;

        if (_storedTiles.Count == 0) return;

        var localTransform = _storedTiles[_storedTiles.Count - 1].localPosition;
        localTransform.y = heightAdjuster;
        _storedTiles[_storedTiles.Count - 1].localPosition = localTransform;
    }
    
    private void StatePattern()
    {
        switch (designState)
        {
            case State.Tiling:
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    _curPrefabIndex++;
                    _curPrefabIndex %= groundTilePrefabs.Length;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject()) return;       
                    RaycastHandles();
                    RaycastTiles();
                    if (_hasPutFirstTile) return;
                    _hasPutFirstTile = true;
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
  private void SpawnBaseModel()
              {
                  if (!Physics.Raycast(_ray, out _hit, 200, groundLayer)) return;
      
                  pointer.gameObject.SetActive(true);
                  _hit.collider.enabled = false;
                  _newPosition = _hit.point;
                  _newPosition.y = heightAdjuster;
                  var levelElement = Instantiate(groundTilePrefabs[_curPrefabIndex], transform, true);
                  print("base added");
                  var transform2 = levelElement.transform;
                  transform2.localPosition = _newPosition;
                  _storedTiles.Add(levelElement.GetComponent<Transform>());
                  pointer.DOMove(_storedTiles[_storedTiles.Count - 1].position + new Vector3(0, 1, 0), .1f);
              }
      
              private void RaycastHandles()
              {
                  if (!Physics.Raycast(_ray, out _hit, 200,handlerLayer)) return;
      
                  var newPos = _hit.transform.GetComponentInParent<TileInfo>().transform.localPosition;
                  newPos.y = heightAdjuster;
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

             
              private void RaycastProps()
              {
                  if (!Physics.Raycast(_ray, out _hit, 200,tileLayer)) return;

                  if (Input.GetMouseButtonDown(0))
                      _gotProp = !_gotProp;
                  
                  if (!_gotProp)
                  {
                      _gotProp = true;
                      _propInfo=Instantiate(prop, transform, true);
                      _storedTiles.Add(_propInfo.GetComponent<Transform>());
                      print("Prop Added");
                  }

                  _propInfo.transform.position = _hit.point;
              }

              private void RaycastTiles()
              {
                  if (!Physics.Raycast(_ray, out _hit, 200,tileLayer)) return;

                  var temp = _hit.collider.transform;
                  if (_storedTiles.Contains(temp.parent))
                  {
                      print("Found");
                      _storedTiles.Remove(temp.parent);
                      _storedTiles.Add(temp.parent);
                  }
                  pointer.DOMove(_storedTiles[_storedTiles.Count - 1].position + new Vector3(0, 1, 0), .1f);
              }
              private void SpawnNewElementAt(Vector3 newPos)
              {
                  var levelElement = Instantiate(groundTilePrefabs[_curPrefabIndex], transform, true);
                  print("new element added");
                  var transform2 = levelElement.transform;
                  transform2.localPosition = newPos;
                  _storedTiles.Add(levelElement.GetComponent<Transform>());
                  pointer.DOMove(_storedTiles[_storedTiles.Count - 1].position + new Vector3(0, 1, 0), .1f);
              }
    
    

    #endregion
    
    public void SetTileIndex(int value) => _curPrefabIndex = value;

    public void SaveLevel()
    {
        foreach (var tile in _storedTiles.Where(tile => tile.GetComponent<TileInfo>()))
        {
            tile.GetComponent<TileInfo>().DisableHandler();
        }
        var prefabObject = Instantiate(gameObject);
        saveSystem.SaveLevel(prefabObject);
    }

    
   
}
