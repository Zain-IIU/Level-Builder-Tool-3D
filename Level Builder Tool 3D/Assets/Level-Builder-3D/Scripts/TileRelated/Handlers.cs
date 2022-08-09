using UnityEngine;


    public class Handlers : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Tile"))
            {
                gameObject.SetActive(false);
            }
        }
    }
