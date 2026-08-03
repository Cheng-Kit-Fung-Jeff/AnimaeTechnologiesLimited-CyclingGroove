using System.Collections.Generic;
using UnityEngine;

public class CKF_SeparateMesh : MonoBehaviour
{
    public bool enable = true;
    public List<MeshProfile> meshProfiles;
    [System.Serializable]
    public class MeshProfile
    {
        public string meshName, objectName;
        public List<Range> ranges;
        public LayerMask layer;
        [System.Serializable]
        public class Range
        {
            public Transform target;
            public float radius;
        }
    }
    public bool AddCreator = true;
    private SkinnedMeshRenderer selfSMR;
    public bool showGizmos;

    private void Awake()
    {
        if (!enable) return;
        selfSMR = GetComponent<SkinnedMeshRenderer>();

        foreach (MeshProfile profile in meshProfiles)
        {
            GameObject newGB = new(profile.objectName);
            newGB.transform.position = transform.position;
            newGB.transform.parent = transform.parent;
            newGB.layer = Log2int(profile.layer);

            SkinnedMeshRenderer newSMR = newGB.AddComponent<SkinnedMeshRenderer>();
            newSMR.bones = selfSMR.bones;
            newSMR.rootBone = selfSMR.rootBone;
            newSMR.sharedMesh = CreateMesh(profile.meshName, selfSMR.sharedMesh, profile);
            newSMR.materials = selfSMR.materials;
            if (AddCreator)
            {
                CKF_CreateObject newCO = newGB.AddComponent<CKF_CreateObject>();
                newCO.data = newSMR.sharedMesh;
            }
            
        }

        selfSMR.gameObject.SetActive(false) ;

        int Log2int(int num)
        {
            int res = 0;
            while (num > 1)
            {
                num >>= 1;
                res++;
            }

            return res;
        }
    }

    public Mesh CreateMesh(string name, Mesh modifyMesh, MeshProfile mp)
    {
        List<Vector3> newVertices = new();
        List<Vector2> newUV = new();
        List<BoneWeight> newBoneWeights = new();
        List<int> newTriangeles = new();

        Dictionary<int, int> vertMap = new();
        for (int i = 0; i < modifyMesh.vertices.Length; i++)
        {
            if(!IsIn(i, modifyMesh.vertices, mp)) continue;
            newVertices.Add(modifyMesh.vertices[i]);
            newUV.Add(modifyMesh.uv[i]);
            newBoneWeights.Add(modifyMesh.boneWeights[i]);
            vertMap.Add(i, newVertices.Count - 1);
        }
        for (int i = 0; i < modifyMesh.triangles.Length; i+=3)
        {
            if (!vertMap.ContainsKey(modifyMesh.triangles[i])) continue;
            if (!vertMap.ContainsKey(modifyMesh.triangles[i+1])) continue;
            if (!vertMap.ContainsKey(modifyMesh.triangles[i+2])) continue;
            newTriangeles.Add(vertMap[modifyMesh.triangles[i]]);
            newTriangeles.Add(vertMap[modifyMesh.triangles[i + 1]]);
            newTriangeles.Add(vertMap[modifyMesh.triangles[i + 2]]);
        }
        

        static bool IsIn(int v, Vector3[] vertices, MeshProfile mp)
        {
            foreach(var r in mp.ranges)
                if (Vector3.Distance(vertices[v],r.target.localPosition) < r.radius) return true;
            return false;
        }

        /*Debug.Log(
            "vertices: " + modifyMesh.vertices.Length
            + "\nvertexCount: " + modifyMesh.vertexCount
            + "\ntriangles: " + modifyMesh.triangles.Length / 3
            + "\nUV: " + modifyMesh.uv.Length
            + "\nUV2: " + modifyMesh.uv2.Length
            + "\nUV3: " + modifyMesh.uv3.Length
            + "\nUV4: " + modifyMesh.uv4.Length
            + "\nUV5: " + modifyMesh.uv5.Length
            + "\nUV6: " + modifyMesh.uv6.Length
            + "\nUV7: " + modifyMesh.uv7.Length
            + "\nUV8: " + modifyMesh.uv8.Length
            + "\ncolors: " + modifyMesh.colors.Length
            + "\ncolors32: " + modifyMesh.colors32.Length
            + "\nboneWeights: " + modifyMesh.boneWeights.Length
            + "\nbindposes: " + modifyMesh.bindposes.Length
            + "\nnewVertices: " + newVertices.Count
            + "\nnewTriangles: " + newTriangeles.Count / 3
            );*/

        Mesh newMesh = new()
        {
            name = name,
            vertices = newVertices.ToArray(),
            triangles = newTriangeles.ToArray(),
            subMeshCount = modifyMesh.subMeshCount,
            boneWeights = newBoneWeights.ToArray(),
            bindposes = modifyMesh.bindposes,
            uv = newUV.ToArray(),
            uv2 = modifyMesh.uv2,
            uv3 = modifyMesh.uv3,
            uv4 = modifyMesh.uv4,
            uv5 = modifyMesh.uv5,
            uv6 = modifyMesh.uv6,
            uv7 = modifyMesh.uv7,
            uv8 = modifyMesh.uv8,
            colors = modifyMesh.colors,
            colors32 = modifyMesh.colors32,
            vertexBufferTarget = modifyMesh.vertexBufferTarget,
            hideFlags = modifyMesh.hideFlags,
            indexFormat = modifyMesh.indexFormat,
            indexBufferTarget = modifyMesh.indexBufferTarget,
            /*normals = modifyMesh.normals,
            tangents = modifyMesh.tangents,
            bounds = modifyMesh.bounds,//*/
        };

        /*for (int i = 0; i < newMesh.subMeshCount; i++)
            newMesh.SetSubMesh(i, modifyMesh.GetSubMesh(i));*/
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        newMesh.RecalculateTangents();
        return newMesh;
        //return default;
    }
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.white;
        foreach (var mp in meshProfiles)
            foreach(var r in mp.ranges)
                Gizmos.DrawWireSphere(r.target.position, r.radius);
    }
}
