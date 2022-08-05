
    
    using UnityEngine;

    [System.Serializable]
    public class Transforms
    {
        [SerializeField] private Vector3 tilePos;
        [SerializeField] private Vector3 tileScale;
        [SerializeField] private Quaternion tileRot;

        public void SetTransforms(Vector3 pos,Vector3 scale,Quaternion rot)
        {
            tilePos = pos;
            tileScale = scale;
            tileRot = rot;
        }

        public Vector3 GetPos() => tilePos;
        public Vector3 GetScale() => tileScale;
        public Quaternion GetRot() => tileRot;
    }
