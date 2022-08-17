using UnityEngine;
// ReSharper disable once CheckNamespace

    public class TileInfo : MonoBehaviour
    {
        [SerializeField] private GameObject handlers;
        public Material transparent;

        public GameObject mesh;

        public void DisableHandler()
        {
            handlers.SetActive(false);
        }
        public void Preview(){
            print("transparentr");
            mesh.GetComponent<MeshRenderer>().sharedMaterial = transparent;
            mesh.GetComponent<BoxCollider>().enabled = false;
        }
    }
