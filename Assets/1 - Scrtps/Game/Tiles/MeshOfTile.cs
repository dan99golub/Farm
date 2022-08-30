using UnityEngine;

namespace DefaultNamespace.Game
{
    public class MeshOfTile : MonoBehaviour
    {
        public MeshRenderer Mesh;

        public Material SharedMaterial
        {
            get => Mesh.sharedMaterial;
            set => Mesh.sharedMaterial = value;
        }
    }
}