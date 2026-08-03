using System;
using System.Collections.Generic;
using UnityEngine;

public class CKF_ScanCast : MonoBehaviour
{
    public float frequency = 10;
    float delay, curDelay = 0;
    public Quaternion rotate;
    [MinMaxField(-10, 10)] public Vector2 rangeY = new(-5,5);
    [MinMaxField(-10, 10)] public Vector2 rangeX = new(-5,5);
    [ReadonlyField] [MinMaxField(-10, 10)] public Vector2 rangeAutoY; //if terrain is hit adjust Y;
    public float maxViewDistance = 10;
    public Vector3 origin;
    private Vector3 transformedOrigin;

    public void SetFrequency(float frequency) {
        this.frequency = frequency;
        delay = frequency == 0 ? float.PositiveInfinity : 1 / frequency;
        curDelay = Mathf.Min(curDelay, delay);
    }
    public bool ignoreTerrain = true;
    private HashSet<Collider> selfCollider = new(), terrianColliders = new();
    private float uncheckedDeltaTime = 0;

    [ReadonlyField] public List<HitProfile> hitProfiles = new();
    [Serializable]
    public class HitProfile
    {
        public static HashSet<Transform> transforms = new();
        public Transform transform;
        public Vector3 point;
        public Vector3 worldPoint;
        public Vector3 velocity = Vector3.zero;
        public HitProfile(Collider collider, Vector3 point)
        {
            transform = collider.transform;
            transforms.Add(transform);
            this.point = transform.InverseTransformPoint(point);
            worldPoint = point;
        }
    }
    public bool showGizmos = false;

    private void removeHitProfileAt(int index)
    {
        HitProfile.transforms.Remove(hitProfiles[index].transform);
        hitProfiles.RemoveAt(index);
    }


    private void Awake()
    {
        SetFrequency(frequency);
        foreach(var col in Fn.GetComponentsInAll<Collider>(transform))
            selfCollider.Add(col);

        foreach (var col in FindObjectsByType<TerrainCollider>(FindObjectsSortMode.None))
            terrianColliders.Add(col);
        rangeAutoY = rangeY;
    }

    private void Update()
    {
        uncheckedDeltaTime += Time.deltaTime;
        if (curDelay > Time.deltaTime)
        {
            curDelay -= Time.deltaTime;
        }
        else
        {
            curDelay -= Time.deltaTime;
            while (curDelay <= 0)
                curDelay += delay;

            transformedOrigin = transform.TransformPoint(rotate * origin);
            Vector3 nextDirection, worldDirection;
            float invUncheckedDeltaTime = 1 / uncheckedDeltaTime;
            Quaternion invRotation = Quaternion.Inverse(rotate);

            for (int i = hitProfiles.Count; i-- > 0;)
            {
                if(hitProfiles[i].transform == null) { removeHitProfileAt(i); continue; }
                Vector3 transformedHit = hitProfiles[i].transform.TransformPoint(hitProfiles[i].point);
                worldDirection = transformedHit - transformedOrigin;
                nextDirection = invRotation * transform.InverseTransformDirection(worldDirection);
                if (nextDirection.z <= 0)
                {
                    removeHitProfileAt(i);
                }
                else if (nextDirection.x < rangeX.x * nextDirection.z
                    || nextDirection.x > rangeX.y * nextDirection.z
                    || nextDirection.y < rangeAutoY.x * nextDirection.z
                    || nextDirection.y > rangeAutoY.y * nextDirection.z)
                {
                    removeHitProfileAt(i);
                }
                else if (Physics.Raycast(transformedOrigin, worldDirection, out RaycastHit hit, maxViewDistance))
                {
                    if (hit.transform == hitProfiles[i].transform)
                    {
                        hitProfiles[i].velocity = invUncheckedDeltaTime * (transformedHit - hitProfiles[i].worldPoint);
                        hitProfiles[i].worldPoint = transformedHit;
                    }
                    else
                    {
                        removeHitProfileAt(i);
                    }
                }
                else
                {
                    removeHitProfileAt(i);
                }
            }

            nextDirection = rotate * new Vector3(UnityEngine.Random.Range(rangeX.x, rangeX.y), UnityEngine.Random.Range(rangeAutoY.x, rangeAutoY.y), 1);
            {
                if (showGizmos) Debug.DrawRay(transformedOrigin, transform.TransformDirection(nextDirection),Color.green,1f);
                if (Physics.Raycast(transformedOrigin, transform.TransformDirection(nextDirection), out RaycastHit hit, maxViewDistance))
                {
                    if (terrianColliders.Contains(hit.collider))
                    {
                        if(ignoreTerrain)
                            rangeAutoY.x = Mathf.Min(nextDirection.y, rangeY.y);
                    }
                    else if (!selfCollider.Contains(hit.collider))
                    {
                        if (!HitProfile.transforms.Contains(hit.collider.transform))
                        {
                            hitProfiles.Add(new(hit.collider, hit.point));
                        }
                    }
                }
            }
            if (ignoreTerrain)
            {
                nextDirection = rotate * new Vector3(UnityEngine.Random.Range(rangeX.x, rangeX.y), UnityEngine.Random.Range(rangeY.x, rangeAutoY.x), 1);
                if (showGizmos) Debug.DrawRay(transformedOrigin, transform.TransformDirection(nextDirection), Color.blue, 1f);
                if (Physics.Raycast(transformedOrigin, transform.TransformDirection(nextDirection), out RaycastHit hit, maxViewDistance))
                {
                    if (!terrianColliders.Contains(hit.collider))
                    {
                        rangeAutoY.x = nextDirection.y;
                    }
                }
                
            }
            uncheckedDeltaTime = 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.TransformPoint(rotate * origin), transform.TransformPoint(rotate * (origin + new Vector3(rangeX.x, rangeY.x, 1))));
        Gizmos.DrawLine(transform.TransformPoint(rotate * origin), transform.TransformPoint(rotate * (origin + new Vector3(rangeX.y, rangeY.x, 1))));
        Gizmos.DrawLine(transform.TransformPoint(rotate * origin), transform.TransformPoint(rotate * (origin + new Vector3(rangeX.x, rangeY.y, 1))));
        Gizmos.DrawLine(transform.TransformPoint(rotate * origin), transform.TransformPoint(rotate * (origin + new Vector3(rangeX.y, rangeY.y, 1))));
        if (rangeAutoY != default)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.TransformPoint(rotate * origin), transform.TransformPoint(rotate * (origin + new Vector3(rangeX.x, rangeAutoY.x, 1))));
            Gizmos.DrawLine(transform.TransformPoint(rotate * origin), transform.TransformPoint(rotate * (origin + new Vector3(rangeX.y, rangeAutoY.x, 1))));
            Gizmos.DrawLine(transform.TransformPoint(rotate * origin), transform.TransformPoint(rotate * (origin + new Vector3(rangeX.x, rangeAutoY.y, 1))));
            Gizmos.DrawLine(transform.TransformPoint(rotate * origin), transform.TransformPoint(rotate * (origin + new Vector3(rangeX.y, rangeAutoY.y, 1))));
        }
    }
}
