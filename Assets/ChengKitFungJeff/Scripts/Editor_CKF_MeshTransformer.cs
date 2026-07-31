#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CKF_MeshTransformer))]
public class EditorMeshTransformer : Editor
{
    SerializedProperty targetMesh,
        originalMesh,
        newMesh,
        curEuler,
        curTranslate,
        curScale,
        modified,
        savePath;
    [InitializeOnLoadMethod]
    static void Init() {
        EditorApplication.delayCall += InitMesh;
    }
    static void InitMesh() {
        if (Application.isPlaying) return;
        Debug.Log("Loaded CKF_MeshTransformer");
        foreach (var mt in GameObject.FindObjectsOfType<CKF_MeshTransformer>())
        {
            mt.Init();
        }
    }

    private void OnEnable() {
        targetMesh = serializedObject.FindProperty("thisMesh");
        originalMesh = serializedObject.FindProperty("originalMesh");
        newMesh = serializedObject.FindProperty("newMesh");
        curEuler = serializedObject.FindProperty("euler");
        curTranslate = serializedObject.FindProperty("translate");
        curScale = serializedObject.FindProperty("scale");
        modified = serializedObject.FindProperty("modified");
        savePath = serializedObject.FindProperty("savePath");
    }
    override public void OnInspectorGUI() {
        Mesh uiMesh = (Mesh)EditorGUILayout.ObjectField("Target Mesh", (Mesh)targetMesh.boxedValue, typeof(Mesh), true);
        if (uiMesh != (Mesh)targetMesh.boxedValue) {
            targetMesh.boxedValue = uiMesh;
            curEuler.vector3Value = Vector3.zero;
            curTranslate.vector3Value = Vector3.zero;
            curScale.vector3Value = Vector3.one;
            modified.boolValue = false;
        }
        if (uiMesh)
        {
            if (uiMesh.isReadable)
            {
                Mesh modifyMesh = (Mesh)targetMesh.boxedValue;
                if (!modified.boolValue)
                {
                    originalMesh.ClearArray();
                    int _i = 0;
                    foreach (Vector3 vert in modifyMesh.vertices)
                    {
                        originalMesh.InsertArrayElementAtIndex(_i);
                        originalMesh.GetArrayElementAtIndex(_i).vector3Value = vert;
                        _i++;
                    }
                    modified.boolValue = true;
                }
                Vector3 uiEuler = EditorGUILayout.Vector3Field("Euler", curEuler.vector3Value);
                if (!uiEuler.Equals(curEuler.vector3Value))
                {
                    Vector3 degrees = (uiEuler - curEuler.vector3Value) * Mathf.Deg2Rad;
                    float cosX = Mathf.Cos(degrees.x), sinX = Mathf.Sin(degrees.x), cosY = Mathf.Cos(degrees.y), sinY = Mathf.Sin(degrees.y), cosZ = Mathf.Cos(degrees.z), sinZ = Mathf.Sin(degrees.z);
                    Vector3[] dupVertice = new Vector3[uiMesh.vertexCount];
                    newMesh.ClearArray();
                    for (int _i = 0; _i < dupVertice.Length; _i++)
                    {
                        dupVertice[_i] = new Vector3(cosZ * uiMesh.vertices[_i].x - sinZ * uiMesh.vertices[_i].y, cosZ * uiMesh.vertices[_i].y + sinZ * uiMesh.vertices[_i].x, uiMesh.vertices[_i].z);
                        dupVertice[_i] = new Vector3(dupVertice[_i].x, cosX * dupVertice[_i].y - sinX * dupVertice[_i].z, cosX * dupVertice[_i].z + sinX * dupVertice[_i].y);
                        dupVertice[_i] = new Vector3(cosY * dupVertice[_i].x + sinY * dupVertice[_i].z, dupVertice[_i].y, cosY * dupVertice[_i].z - sinY * dupVertice[_i].x);
                        newMesh.InsertArrayElementAtIndex(_i);
                        newMesh.GetArrayElementAtIndex(_i).vector3Value = dupVertice[_i];
                    }
                    modifyMesh.SetVertices(dupVertice);
                    modifyMesh.RecalculateNormals();
                    modifyMesh.RecalculateBounds();
                    curEuler.vector3Value = uiEuler;

                }
                Vector3 uiScale = EditorGUILayout.Vector3Field("Scale", curScale.vector3Value);
                if (!uiScale.Equals(curScale.vector3Value))
                {
                    Vector3 scale = new Vector3(uiScale.x == 0 || curScale.vector3Value.x == 0 ? 1 : uiScale.x / curScale.vector3Value.x,
                        uiScale.y == 0 || curScale.vector3Value.y == 0 ? 1 : uiScale.y / curScale.vector3Value.y,
                        uiScale.z == 0 || curScale.vector3Value.z == 0 ? 1 : uiScale.z / curScale.vector3Value.z);
                    Vector3[] dupVertice = new Vector3[uiMesh.vertexCount];
                    newMesh.ClearArray();
                    for (int _i = 0; _i < dupVertice.Length; _i++)
                    {
                        dupVertice[_i] = Vector3.Scale(uiMesh.vertices[_i], scale);
                        newMesh.InsertArrayElementAtIndex(_i);
                        newMesh.GetArrayElementAtIndex(_i).vector3Value = dupVertice[_i];
                    }
                    modifyMesh.SetVertices(dupVertice);
                    modifyMesh.RecalculateNormals();
                    modifyMesh.RecalculateBounds();
                    curScale.vector3Value = uiScale;

                }
                Vector3 uiTranslate = EditorGUILayout.Vector3Field("Translate", curTranslate.vector3Value);
                if (!uiTranslate.Equals(curTranslate.vector3Value))
                {
                    Vector3 translation = uiTranslate - curTranslate.vector3Value;
                    Vector3[] dupVertice = new Vector3[uiMesh.vertexCount];
                    newMesh.ClearArray();
                    for (int _i = 0; _i < dupVertice.Length; _i++) {
                        dupVertice[_i] = uiMesh.vertices[_i] + translation;
                        newMesh.InsertArrayElementAtIndex(_i);
                        newMesh.GetArrayElementAtIndex(_i).vector3Value = dupVertice[_i];
                    }
                    modifyMesh.SetVertices(dupVertice);
                    modifyMesh.RecalculateBounds();
                    curTranslate.vector3Value = uiTranslate;
                }

                if (GUILayout.Button("Apply transform"))
                {
                    curEuler.vector3Value = Vector3.zero;
                    curTranslate.vector3Value = Vector3.zero;
                    curScale.vector3Value = Vector3.one;
                }

                if (GUILayout.Button("Reset"))
                {
                    Vector3[] savedVertices = new Vector3[originalMesh.arraySize];
                    for (int _i = 0; _i < originalMesh.arraySize; _i++) savedVertices[_i] = originalMesh.GetArrayElementAtIndex(_i).vector3Value;
                    modifyMesh.SetVertices(savedVertices);
                    modifyMesh.RecalculateBounds();
                    curEuler.vector3Value = Vector3.zero;
                    curTranslate.vector3Value = Vector3.zero;
                    curScale.vector3Value = Vector3.one;
                }
                savePath.stringValue = EditorGUILayout.TextField("Save path", savePath.stringValue);
                EditorGUI.BeginDisabledGroup(savePath.stringValue == "");
                if(GUILayout.Button("Create Mesh Asset"))
                {
                    Mesh newMesh = new()
                    {
                        name = modifyMesh.name,
                        vertices = modifyMesh.vertices,
                        triangles = modifyMesh.triangles,
                        subMeshCount = modifyMesh.subMeshCount,
                        boneWeights = modifyMesh.boneWeights,
                        bindposes = modifyMesh.bindposes,
                        uv = modifyMesh.uv,
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
                        normals = modifyMesh.normals,
                        tangents = modifyMesh.tangents,
                        bounds = modifyMesh.bounds,
                    };
                    for (int i = 0; i < newMesh.subMeshCount; i++)
                        newMesh.SetSubMesh(i, modifyMesh.GetSubMesh(i));
                    newMesh.name = modifyMesh.name;
                    AssetDatabase.CreateAsset(newMesh, savePath.stringValue);
                }
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUI.HelpBox(GUILayoutUtility.GetRect(18, 18, "TextField"), "Mesh cannot be written. Check Inspector > Model > Read/Write ", MessageType.Error);
            }
        }
        else
        {
            uiMesh = null;
            if (((CKF_MeshTransformer)target).TryGetComponent(out MeshFilter mf))
                { uiMesh = mf.sharedMesh; }
            if (((CKF_MeshTransformer)target).TryGetComponent(out MeshFilter smr))
                { uiMesh = smr.sharedMesh; }
            if (uiMesh != null)
            {
                if (GUILayout.Button("Get Main mesh"))
                {
                    targetMesh.boxedValue = uiMesh;
                    curEuler.vector3Value = Vector3.zero;
                    curTranslate.vector3Value = Vector3.zero;
                    curScale.vector3Value = Vector3.one;
                    modified.boolValue = false;
                }
            }
            else {
                EditorGUI.HelpBox(GUILayoutUtility.GetRect(18, 18, "TextField"), "Object has no mesh.",MessageType.Info);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif