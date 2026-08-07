using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_PathController : MonoBehaviour, I_CKF_Reset
{
    public GameObject pathSprite,glowSprite;
    public Transform glowLayer;
    public List<Transform> startNodes;
    private HashSet<int> addedPaths = new();
    private readonly Dictionary<int, List<CKF_Path>> mapTransformToPaths = new();
    public CKF_BoundCapture cameraBounds;
    public UnityEvent<Vector3> setStartPosition = new();

    public void AddPath(CKF_Path p)
    {
        if (addedPaths.Contains(p.GetInstanceID())) return;
        addedPaths.Add(p.GetInstanceID());
        if (p.enterA)
        {
            if (!mapTransformToPaths.ContainsKey(p.nodeA.transform.GetInstanceID()))
                mapTransformToPaths[p.nodeA.transform.GetInstanceID()] = new();
            mapTransformToPaths[p.nodeA.transform.GetInstanceID()].Add(p);
        }
        if (p.enterB)
        {
            if (!mapTransformToPaths.ContainsKey(p.nodeB.transform.GetInstanceID()))
                mapTransformToPaths[p.nodeB.transform.GetInstanceID()] = new();
            mapTransformToPaths[p.nodeB.transform.GetInstanceID()].Add(p);
        }
    }
    public void RemovePath(CKF_Path p) // it shouldn't be needed
    {
        if (!addedPaths.Contains(p.GetInstanceID())) return;
        addedPaths.Remove(p.GetInstanceID());
        if (p.enterA)
        {
            mapTransformToPaths[p.nodeA.transform.GetInstanceID()].Remove(p);
        }
        if (p.enterB)
        {

            mapTransformToPaths[p.nodeB.transform.GetInstanceID()].Remove(p);
        }
    }

    private void Start()
    {
        if(startNodes.Count > 0)
        {
            Vector3 averagePosition = Vector3.zero;
            foreach (Transform node in startNodes)
            {
                averagePosition.Set(averagePosition.x + node.position.x,
                    averagePosition.y + node.position.y,
                    averagePosition.z + node.position.z
                    );
            }
            averagePosition /= startNodes.Count;
            setStartPosition?.Invoke(averagePosition);

            foreach (Transform node in startNodes) // if done on Awake then the ui position would not be updated correctly
                ExploreNode(node, 0);
        }
    }

    private readonly List<CKF_Path> pathBuffer = new();
    public void Explore(float amount)
    {
        pathBuffer.Clear();
        pathBuffer.AddRange(mapActivePath.Values);
        foreach (var path in pathBuffer)
        {
            path.Add(amount);
        }
    }

    public void ExploreNode(Transform node, float amount)
    {
        if(mapTransformToPaths.ContainsKey(node.GetInstanceID()))
            foreach (var path in mapTransformToPaths[node.GetInstanceID()])
                path.Enter(node, amount);
    }

    public readonly Dictionary<int, CKF_Path> mapActivePath = new();

    public void AddActive(CKF_Path path)
    {
        mapActivePath[path.GetInstanceID()] = path;
    }


    public void RemoveActive(CKF_Path path)
    {
        if (mapActivePath.ContainsKey(path.GetInstanceID())) mapActivePath.Remove(path.GetInstanceID());
    }

    public void SceneReset()
    {
        foreach (var path in addedPaths)
            ((CKF_Path)Resources.InstanceIDToObject(path)).PathReset();
    }
}
