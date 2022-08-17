using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// ReSharper disable once CheckNamespace
public class LevelDesignTool : MonoBehaviour
{
    public TextMeshProUGUI modeText;
    public GameObject propWindow;
    public Material transparentMat;
    public TileInfo previewModel;
    public GameObject previewModelObj;
    public GameObject tileWindow;
    private enum State
    {
        Tiling,
        Props,
    } 
    [SerializeField] private State designState;
    [SerializeField] private LevelSaveSystem saveSystem;
    [SerializeField] private TileInfo[] groundTilePrefabs;
    [SerializeField] private GameObject[] prop;
    [SerializeField] private GameObject[] previewProp;
    private readonly List<Transform> _storedTiles = new List<Transform>();
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask handlerLayer;
    [SerializeField] private LayerMask tileLayer;
    [Range(0, 3)] [SerializeField] private float heightAdjuster;

    [SerializeField] private Transform pointer;
    [SerializeField] private Transform propPointer;

    #region Private Fields
    private bool _hasPutFirstTile;
    private RaycastHit _hit;
    private Ray _ray;
    private int _curTilePrefabIndex;
    private int _curPropPrefabIndex;
    private Vector3 _newPosition;
    private bool _gotProp;
    private GameObject _propInfo;
    #endregion
    
    private void Awake() {
        SetUpMode();
    }
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

        
    }

    private void OnValidate()
    {
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
                propPointer.gameObject.SetActive(false);
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    _curTilePrefabIndex++;
                    _curTilePrefabIndex %= groundTilePrefabs.Length;
                }
                PreviewRaycastHandles();
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
                propPointer.gameObject.SetActive(true);
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
                  var levelElement = Instantiate(groundTilePrefabs[_curTilePrefabIndex], transform, true);
                  print("base added");
                  var transform2 = levelElement.transform;
                  transform2.localPosition = _newPosition;
                  _storedTiles.Add(levelElement.GetComponent<Transform>());
                  pointer.transform.parent = _storedTiles[_storedTiles.Count - 1];
                  pointer.DOLocalMove(new Vector3(0, 1, 0), .1f);
              }
                private void PreviewRaycastHandles(){
                     if (!Physics.Raycast(_ray, out _hit, 200,handlerLayer)) return;
      
                  var newPos = _hit.transform.GetComponentInParent<TileInfo>().transform.localPosition;
                  newPos.y = heightAdjuster;
                  switch (_hit.transform.tag) 
                  {
                      case "Forward":
                          newPos.z += 2;
                          previewModel.transform.position = newPos;
                          break;
                      case "Backward":
                          newPos.z -= 2;
                            previewModel.transform.position = newPos;
                          break;
                      case "Left":
                          newPos.x -= 2;
                          previewModel.transform.position = newPos;
                           break;
                      case "Right":
                          newPos.x += 2;
                          previewModel.transform.position = newPos;
                           break;
                  }
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

                  propPointer.transform.position = _hit.point;
                  if(previewModelObj){
                    previewModelObj.transform.parent = _hit.collider.transform;
                     previewModelObj.transform.position = _hit.point;
                  }
                  
                  if (!_gotProp && Input.GetMouseButton(0))
                  {
                      _gotProp = true;
                      _propInfo=Instantiate(prop[_curPropPrefabIndex], transform, true);
                      Vector3 scale = _propInfo.transform.localScale;
                      _propInfo.transform.localScale = Vector3.zero;
                      _propInfo.transform.DOScale(scale, .3f).SetEase(Ease.OutBounce);
                      previewModelObj.transform.DOScale(scale,.3f);
                      _storedTiles.Add(_propInfo.GetComponent<Transform>());
                      _propInfo.transform.parent = _hit.collider.transform;
                      _propInfo.transform.position = _hit.point;
                      //Destroy(previewModelObj);
                  }

                  if (Input.GetMouseButtonDown(0))
                      _gotProp = false;
              }

              private void RaycastTiles()
              {
                  if (!Physics.Raycast(_ray, out _hit, 200,tileLayer)) return;

                  var temp = _hit.collider.transform;
                  if (_storedTiles.Contains(temp.parent))
                  {
                      print("Found");
                      var parent = temp.parent;
                      _storedTiles.Remove(parent);
                      _storedTiles.Add(parent);
                  }
                  pointer.transform.parent = _storedTiles[_storedTiles.Count - 1];
                  pointer.DOLocalMove(new Vector3(0, 1, 0), .1f);
              }
              private void SpawnNewElementAt(Vector3 newPos)
              {
                  var levelElement = Instantiate(groundTilePrefabs[_curTilePrefabIndex], transform, true);
                  Vector3 scale = levelElement.transform.localScale;
                  levelElement.transform.localScale = Vector3.zero;
                  levelElement.transform.DOScale(scale, .2f);
                  print("new element added");
                  var transform2 = levelElement.transform;
                  transform2.localPosition = newPos;
                  _storedTiles.Add(levelElement.GetComponent<Transform>());
                  pointer.transform.parent = _storedTiles[_storedTiles.Count - 1];
                  pointer.DOLocalMove(new Vector3(0, 1, 0), .1f);
              }
    
    

    #endregion
    
    public void SetTileIndex(int value) {
         _curTilePrefabIndex = value;
         if(previewModel){
            Destroy(previewModel.gameObject);
         }
          previewModel =  Instantiate(groundTilePrefabs[_curTilePrefabIndex],transform,true);
          previewModel.Preview();
          //previewModel.GetComponentInChildren<MeshRenderer>().materials[0] = transparentMat;
           // previewModel.GetComponentInChildren<BoxCollider>().enabled = false;
          //previewModel.GetComponent<MeshRenderer>().materials[0].SetColor
         //previewModel = groundTilePrefabs[_curTilePrefabIndex];
    }
    
    public void SetPropIndex(int value) { 
        _curPropPrefabIndex = value;
        if(previewModelObj){
            Destroy(previewModelObj.gameObject);
        }
        previewModelObj = (GameObject) Instantiate(prop[_curPropPrefabIndex]);
        Collider[] colliders = previewModelObj.GetComponentsInChildren<Collider>();
        MeshRenderer[] renderers = previewModelObj.GetComponentsInChildren<MeshRenderer>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.sharedMaterial = transparentMat;
        }
        //previewModel.transform.parent = null;
       // previewModelObj.GetComponent<MeshRenderer>().sharedMaterial = transparentMat;
    }
    
    public void SaveLevel()
    {
        pointer.gameObject.SetActive(false);
        foreach (var tile in _storedTiles.Where(tile => tile.GetComponent<TileInfo>()))
        {
            tile.GetComponent<TileInfo>().DisableHandler();
        }
        var prefabObject = Instantiate(gameObject);
        saveSystem.SaveLevel(prefabObject);
    }

    public void ToggleMode(){
        if (designState == State.Props)
        {
            designState = State.Tiling;
            modeText.text = "Tiling Mode";
        }
        else
        {
            designState = State.Props;
            if(previewModel){
                Destroy(previewModel.gameObject);
            }
            modeText.text = "Props Mode";
        }
        SetUpMode();
    }
    public void SetUpMode(){
        if(designState == State.Props)
        {
            tileWindow.SetActive(false);
            propWindow.SetActive(true);
        }else{
            tileWindow.SetActive(true);
            propWindow.SetActive(false);
        }
    }   
}
