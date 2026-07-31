using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-20000)]
public class CKF_SceneObjectBucket : MonoBehaviour
{
    public static CKF_SceneObjectBucket instance;
    public List<ComponentBucket> bucketComponentIDs = new();
    public List<ComponentsBucket> bucketComponentsIDs = new();
    [System.Serializable]
    public class ComponentBucket
    {
        public string key;
        public Component component;
    }
    [System.Serializable]
    public class ComponentsBucket
    {
        public string key;
        public List<Component> bucket = new();
    }


    public readonly Dictionary<string, Component> mapComponent = new();
    public readonly Dictionary<string, HashSet<int>> mapComponentsID = new();

    private void Awake()
    {
        instance = this;
        foreach (var v in bucketComponentsIDs)
        {
            mapComponentsID[v.key] = new();
            foreach (var m in v.bucket)
            {
                mapComponentsID[v.key].Add(m.GetInstanceID());
            }
        }
        foreach (var v in bucketComponentIDs)
        {
            mapComponent[v.key] = v.component;
        }
    }
    public void AddToComponent(string key, Component mono)
    {
        if (!mapComponentsID.ContainsKey(key)) mapComponentsID[key] = new();
        mapComponentsID[key].Add(mono.GetInstanceID());
    }
    public void AddToComponents(string key, Component mono)
    {
        if(!mapComponentsID.ContainsKey(key)) mapComponentsID[key] = new();
        mapComponentsID[key].Add(mono.GetInstanceID());
    }

    public bool ContainsInComponents(string key, Component mono)
    {
        return mapComponentsID.ContainsKey(key) && mapComponentsID[key].Contains(mono.GetInstanceID());
    }
    public Component GetInComponent(string key)
    {
        if(mapComponent.ContainsKey(key)) return mapComponent[key];
        return null;
    }


    public void OnDestroy()
    {
        if(instance == this) instance = null;
    }
}
