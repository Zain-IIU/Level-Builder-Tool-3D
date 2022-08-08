using UnityEngine;
// ReSharper disable once CheckNamespace

    public class TileInfo : MonoBehaviour
    {
        [SerializeField] private GameObject handlers;

        public void DisableHandler()
        {
            handlers.SetActive(false);
        }
    }
