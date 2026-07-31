using UnityEngine;

public class CKF_MeshTransformer : MonoBehaviour
{
    [SerializeField] Mesh thisMesh;
    [SerializeField] Vector3[] originalMesh,
        newMesh;
    [SerializeField] Vector3 euler = Vector3.zero,
        translate = Vector3.zero,
        scale = Vector3.one;
    [SerializeField] bool modified = false;
#if UNITY_EDITOR
    [SerializeField] string savePath;
#endif
    public void Awake()
    {
        Init();
    }

    public void Init() {
        if (thisMesh && modified && newMesh.Length > 0)
        {
            thisMesh.SetVertices(newMesh);
            thisMesh.RecalculateNormals();
            thisMesh.RecalculateBounds();
        }
    }
}
