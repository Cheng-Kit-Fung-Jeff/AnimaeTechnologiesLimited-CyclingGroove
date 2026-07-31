using System.Collections.Generic;
using UnityEngine;

public class CKF_ParticleCast : MonoBehaviour
{
    HashSet<Collider> selfCollider;
    public List<Transform> includeTransform = new();
    HashSet<Collider> hashsetIncludeColliders = new();
    public bool includeTerrain = true;
    HashSet<Collider> hashsetIncludeTerrain = new();
    public bool ignoreHitIn = true;
    public float sphereSize = 0.1f;
    public bool checkFloor = true;
    public float limitAngle = 60;
    private float cosLimitAngle = 0;

    public void setLimitAngle(float angle) { limitAngle = angle; setCosLimitAngle(angle); }
    public void setCosLimitAngle(float angle) { cosLimitAngle = -Mathf.Cos(angle * Mathf.Deg2Rad); }

    public enum State
    {
        idle,
        trying,
        success,
        interupt, // interupt double as badFloor
        badFloor,
        fail,
    }
    [ReadonlyField] public State state = State.idle;
    private Vector3 particlePosition,  particleVelocity;
    public Vector3 ParticlePosition { get => particlePosition; }
    public Vector3 ParticleVelocity { get => particleVelocity; }
    [ReadonlyField] public Vector3 initPosition, initVelocity;
    private float checkDuration, timeMult;
    public float CheckDuration { get => checkDuration; }
    private Transform parentTransform;
    public void AddIncludeTransform(Transform tr)
    {
        foreach (var col in Fn.GetComponentsInAll<Collider>(tr))
            hashsetIncludeColliders.Add(col);
    }
    public void Awake()
    {
        selfCollider = new(Fn.GetComponentsInAll<Collider>(transform));
        foreach (var tr in includeTransform)
            foreach (var col in Fn.GetComponentsInAll<Collider>(tr))
                hashsetIncludeColliders.Add(col);
        foreach (var col in FindObjectsByType<TerrainCollider>(FindObjectsSortMode.None))
            hashsetIncludeTerrain.Add(col);

        setCosLimitAngle(limitAngle);
    }

    public void Update()
    {

        if (checkDuration > 0)
        {
            float dt = timeMult == 1? Time.deltaTime: timeMult * Time.deltaTime;
            Vector3 nextParticlePosition = particlePosition + dt * particleVelocity + 0.5f * dt * dt * Physics.gravity;
            particleVelocity += dt * Physics.gravity;
            
            Vector3 checkDirection = nextParticlePosition - particlePosition;
            particleVelocity += Physics.gravity * dt;
            HashSet<Collider> hitIn = null;
            if (ignoreHitIn) { hitIn = new(Physics.OverlapSphere(particlePosition, sphereSize)); }
            RaycastHit[] hits = Physics.SphereCastAll(particlePosition, sphereSize, checkDirection, checkDirection.magnitude);
            foreach (RaycastHit hit in hits)
            {
                if (selfCollider.Contains(hit.collider)) continue;
                if (ignoreHitIn && hitIn.Contains(hit.collider)) continue;
                if (hashsetIncludeColliders.Contains(hit.collider) || includeTerrain && hashsetIncludeTerrain.Contains(hit.collider))
                {
                    if(hit.point != default)
                    {
                        nextParticlePosition = hit.point;
                    }
                    if (checkFloor)
                    {
                        if (checkFloor && (hit.point == default || Vector3.Dot(hit.normal, Physics.gravity.normalized) > cosLimitAngle))
                        {
                            state = State.badFloor;
                        }
                        else
                        {
                            state = State.success;
                        }
                    }
                    else state = State.success;
                }
                else
                {
                    state = State.interupt;
                }
                checkDuration = 0;
            }
            if (parentTransform != null)
            {
                parentTransform.position += nextParticlePosition - particlePosition;
                if(state != State.trying)
                {
                    parentTransform = null;
                }
            }
            particlePosition = nextParticlePosition;
            
            checkDuration -= Time.deltaTime;
        }
        else if (state == State.trying)
        {
            state = State.fail;
            parentTransform = null;
        }
        
    }

    public void BeginCast(Vector3 origin, Vector3 velocity, float ignoreDistance, float checkDuration, float timeMult = 1)
    {
        state = State.trying;
        particlePosition = origin;
        particleVelocity = velocity;
        initVelocity = velocity;
        this.checkDuration = checkDuration;
        if (ignoreDistance > 0)
        {
            float cur, sqrIgnoreDistance = ignoreDistance * ignoreDistance, sqrParticleVelocity = particleVelocity.sqrMagnitude, sqrGravity = Physics.gravity.sqrMagnitude,
                dotGV = Vector3.Dot(Physics.gravity, particleVelocity);
            float next = ignoreDistance;
            int solve = 8;
            do
            {
                cur = next;
                float sqrCur = cur * cur;
                next = cur - (sqrCur * (sqrParticleVelocity + cur * dotGV + sqrCur * sqrGravity) - sqrIgnoreDistance)
                / (cur * (2 * sqrParticleVelocity + 3 * cur * dotGV + 4 * sqrCur * sqrGravity));
            } while (Mathf.Abs(next - cur) > 0.001f && --solve > 0);
            next = Mathf.Abs(next);
            this.checkDuration -= next;
            particlePosition += next * (particleVelocity + 0.5f * next * Physics.gravity);
            initPosition = particlePosition;
            particleVelocity += next * Physics.gravity;
            //Debug.Log("ignoreDistance: " + ignoreDistance + "\nnext: " + next+ "\norigin: "+ origin + "\ninitPosition :"+ initPosition);
        }
        
        this.timeMult = timeMult;
        parentTransform = null;
        // |dt * particleVelocity + 0.5f * dt * dt * Physics.gravity|  = ignoreDistance
        // (dt * particleVelocity.x+ 0.5f * dt * dt * Physics.gravity.x) ^ 2 + ... = ignoreDistance * ignoreDistance
        // dt * dt (particleVelocity.x * particleVelocity.x + dt * Physics.gravity.x * particleVelocity.x + dt * dt * Physics.gravity.x * Physics.gravity.x)
        // dt * dt (|particleVelocity|^2 + dt * dot(Physics.gravity, particleVelocity) + dt * dt * |Physics.gravity|^2)
    }

    public void BeginCast(Transform parentTransform, Vector3 origin, Vector3 velocity, float ignoreDistance, float checkDuration, float timeMult = 1)
    {
        state = State.trying;
        particlePosition = origin;
        particleVelocity = velocity;
        initVelocity = velocity;
        this.checkDuration = checkDuration;
        this.timeMult = timeMult;
        this.parentTransform = parentTransform;

        if (ignoreDistance > 0)
        {
            float cur, sqrIgnoreDistance = ignoreDistance * ignoreDistance, sqrParticleVelocity = particleVelocity.sqrMagnitude, sqrGravity = Physics.gravity.sqrMagnitude,
                dotGV = Vector3.Dot(Physics.gravity, particleVelocity);
            float next = ignoreDistance;
            int solve = 8;
            do
            {
                cur = next;
                float sqrCur = cur * cur;
                next = cur - (sqrCur * (sqrParticleVelocity + cur * dotGV + sqrCur * sqrGravity) - sqrIgnoreDistance)
                / (cur * (2 * sqrParticleVelocity + 3 * cur * dotGV + 4 * sqrCur * sqrGravity));
            } while (Mathf.Abs(next - cur) > 0.001f && --solve > 0);
            next = Mathf.Abs(next);
            this.checkDuration -= next;
            Vector3 dP = next * (particleVelocity + 0.5f * next * Physics.gravity);
            particlePosition += dP;
            initPosition = particlePosition;
            parentTransform.position += dP;
            particleVelocity += next * Physics.gravity;
        }

    }

    public bool showGizmos = false;
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        if (state != State.idle)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(particlePosition, sphereSize);
        }
    }

    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    public static void EnableBackfaceQuery()
    {
        Physics.queriesHitBackfaces = true;
    }*/
}
