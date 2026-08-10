using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Fn
{
    public static HashSet<T> HashIntersetLowFail<T>(List<HashSet<T>> x, int failThreshold) {
        HashSet<T> ret = null;
        int fails = 0;
        foreach (HashSet<T> h in x)
            if (fails > failThreshold) return null;
            else if (h.Count == 0) fails++;
            else if (ret == null) ret = new(h);
            else if (ret.Count == 0) return ret;
            else ret.IntersectWith(h);
        return ret;
    }

    public static float FloatRate(float fromValue, float toValue, float rate) => fromValue * (1 - rate) + toValue * rate;
    public static float FloatRatio(float fromValue, float toValue, float value) => (value - fromValue) / (toValue - fromValue);
    public static float FloatLerp(float fromValue, float toValue, float step) => fromValue == toValue ? fromValue
        : fromValue < toValue ? Mathf.Min(fromValue + step, toValue) : Mathf.Max(fromValue - step, toValue);

    public static float NormalisedAngle(float angle) => angle - 360 * (float)Math.Floor((angle + 180) / 360);
    public static Vector3 NormalisedAngle(Vector3 euler) => new(NormalisedAngle(euler.x), NormalisedAngle(euler.y), NormalisedAngle(euler.z));

    //assume unnormalised
    public static float AngleD(Vector3 from, Vector3 to, Vector3 up) {
        float angle = Mathf.Rad2Deg * Mathf.Acos(Mathf.Clamp(Vector3.Dot(from.normalized, to.normalized), -1, 1));
        return Vector3.Dot(Vector3.Cross(from, to), up) < 0 ? angle : -angle;
    }
    public static float AngleAtVectorD(Vector3 from, Vector3 to, Vector3 up)
        => AngleD(VectorToNormalDirection(from, up), VectorToNormalDirection(to, up), up);

    public static float Angle2CCD(Vector2 from, Vector2 to) // assume center at origin
    {
        float distFrom = from.sqrMagnitude, distTo = to.sqrMagnitude, distFromTo = (from - to).sqrMagnitude;
        float res = Mathf.Rad2Deg * Mathf.Acos(Mathf.Clamp((distFrom + distTo - distFromTo) / (2 * Mathf.Sqrt(distFrom * distTo)), -1, 1));
        //x(A)y(B)+y(A)x(H)+x(B)y(H)-x(A)y(H)-y(A)x(B)-y(B)x(H)
        //x(A)y(B)-y(A)x(B)
        float det = from.x * to.y - from.y * to.x;
        return (from.x * to.y - from.y * to.x) < 0 ? 360 - res : res;
    }
    public static Vector2 Rotate2CCD(Vector2 point, float angle)// assume center at origin
    {
        float sinA = Mathf.Sin(angle), cosA = Mathf.Cos(angle);
        return new(cosA * point.x + sinA * point.y, cosA * point.y - sinA * point.x);
    }


    public static Vector3 RayFarPointOnSphere(Ray ray, Vector3 spherePos, float sphereRadius)
    {
        Vector3 dRaySpherePos = spherePos - ray.origin;
        float dotRayVectorPos = Vector3.Dot(ray.direction, dRaySpherePos),
            det = dotRayVectorPos * dotRayVectorPos + sphereRadius * sphereRadius - dRaySpherePos.sqrMagnitude;
        if (det < 0) return new Vector3(-1, -1, -1);
        return ray.direction * (dotRayVectorPos + Mathf.Pow(det, 0.5f)) + ray.origin;
    }
    public static Vector3 PointOnSphere(Vector3 point, Vector3 spherePos, float sphereRadius)
    {
        if (point.Equals(spherePos)) return Vector3.zero;
        return (point - spherePos).normalized * sphereRadius + spherePos;
    }

    public static Vector3 VectorOnNormalDirection(Vector3 vector, Vector3 normal)
        => Vector3.Dot(vector, normal) / normal.sqrMagnitude * normal;
    public static Vector3 VectorToNormalDirection(Vector3 vector, Vector3 normal)
        => vector - VectorOnNormalDirection(vector, normal);

    public static Vector3 PointOnPlaneNormalDirection(Vector3 point, Vector3 planePoint, Vector3 normal)
    {
        Vector3 normalnormal = normal.normalized;
        return Vector3.Dot(point - planePoint, normalnormal) * normalnormal;
    }

    public static Vector3 PointToPlane(Vector3 point, Vector3 planePoint, Vector3 normal)
        => point - PointOnPlaneNormalDirection(point, planePoint, normal);

    public static Vector3 MirrorNormal(Vector3 vector, Vector3 normal)
        => vector - 2 * VectorOnNormalDirection(vector, normal);

    public static Vector3 MirrorPlane(Vector3 point, Vector3 planePoint, Vector3 normal)
        => point - 2 * PointOnPlaneNormalDirection(point, planePoint, normal);


    //end assume unnormalised
    //assume normalised
    public static float AngleAtVectorND(Vector3 from, Vector3 to, Vector3 up)
        => AngleD(VectorToNormalDirectionN(from, up), VectorToNormalDirectionN(to, up), up);

    public static Vector3 VectorOnNormalDirectionN(Vector3 vector, Vector3 normal)
        => Vector3.Dot(vector, normal) * normal;

    public static Vector3 VectorToNormalDirectionN(Vector3 vector, Vector3 normal)
        => vector - VectorOnNormalDirectionN(vector, normal);

    public static Vector3 PointOnPlaneNormalDirectionN(Vector3 point, Vector3 planePoint, Vector3 normal)
        => Vector3.Dot(point - planePoint, normal) * normal;

    public static Vector3 PointToPlaneN(Vector3 point, Vector3 planePoint, Vector3 normal)
        => point - PointOnPlaneNormalDirectionN(point, planePoint, normal);

    public static Vector3 MirrorNormalN(Vector3 vector, Vector3 normal)
        => vector - 2 * VectorOnNormalDirectionN(vector, normal);

    public static Vector3 MirrorPlaneN(Vector3 point, Vector3 planePoint, Vector3 normal)
        => point - 2 * PointOnPlaneNormalDirectionN(point, planePoint, normal);

    //end assume normalised

    public static float PointOnLineRatio(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        => Vector3.Dot(point - lineStart, lineEnd - lineStart) / (lineEnd - lineStart).sqrMagnitude;

    public static void SphereIntersectLine(Vector3 pointA, Vector3 pointB, Vector3 center, float radius, out Vector3 res1, out Vector3 res2)
    {   // res1 is negative intersection, res2 is positive intersection
        pointB -= pointA;
        center -= pointA;

        float scale = Vector3.Dot(center, pointB) / pointB.sqrMagnitude;

        float det = radius * radius - (center - scale * pointB).sqrMagnitude;
        if (det < 0)
        {
            res1 = Vector3.negativeInfinity;
            res2 = Vector3.negativeInfinity;
            return;
        }
        float dscale = Mathf.Sqrt(det) / pointB.magnitude;
        float check = scale - dscale;
        res1 = 0 <= check && check <= 1 ? check * pointB + pointA : Vector3.negativeInfinity;
        check = scale + dscale;
        res2 = 0 <= check && check <= 1 ? check * pointB + pointA : Vector3.negativeInfinity;
    }

    public static void SphereIntersectLine(Vector3 pointA, Vector3 pointB, Vector3 center, float radius, out float lerpA, out float lerpB)
    {
        pointB -= pointA;
        center -= pointA;

        float scale = Vector3.Dot(center, pointB) / pointB.sqrMagnitude;

        float det = radius * radius - (center - scale * pointB).sqrMagnitude;
        if (det < 0)
        {
            lerpA = float.NegativeInfinity;
            lerpB = float.NegativeInfinity;
            return;
        }
        float dscale = Mathf.Sqrt(det) / pointB.magnitude;
        lerpA = scale - dscale;
        lerpB = 1 - scale - dscale;
    }

    public static Quaternion GetDQuat(Quaternion curQuat, Quaternion preQuat)
    {
        Quaternion ret = curQuat * Quaternion.Inverse(preQuat);
        //Debug.Log("[GetDQuat] pret: " + ret);
        ret = ret.Equals(Quaternion.identity) ? Quaternion.identity :
            Quaternion.AngleAxis(2 * Mathf.Acos(Mathf.Clamp(ret.w, -1, 1))
            * Mathf.Rad2Deg, new Vector3(ret.x, ret.y, ret.z).normalized);//*/
        if (ret.w < 0) ret = new(-ret.x, -ret.y, -ret.z, -ret.w);
        //Debug.Log("[GetDQuat] ret: " + ret);
        return ret;
    }

    public static void SetSoftJointLimit(ConfigurableJoint joint, float? limit, float? bounciness, float? contactDist)
    {
        joint.linearLimit = new()
        {
            limit = limit == null ? joint.linearLimit.limit : (float)limit,
            bounciness = bounciness == null ? joint.linearLimit.bounciness : (float)bounciness,
            contactDistance = contactDist == null ? joint.linearLimit.contactDistance : (float)contactDist
        };
    }
    public static void SetSoftJointLimitSpring(ConfigurableJoint joint, float? spring, float? damper)
    {
        joint.linearLimitSpring = new()
        {
            spring = spring == null ? joint.linearLimitSpring.spring : (float)spring,
            damper = damper == null ? joint.linearLimitSpring.damper : (float)damper
        };
    }

    public static void SetSoftJointAngularXLimitSpring(ConfigurableJoint joint, float? spring, float? damper)
    {
        joint.angularXLimitSpring = new()
        {
            spring = spring == null ? joint.linearLimitSpring.spring : (float)spring,
            damper = damper == null ? joint.linearLimitSpring.damper : (float)damper
        };
    }

    public static void SetSoftJointAngularYZLimitSpring(ConfigurableJoint joint, float? spring, float? damper)
    {
        joint.angularYZLimitSpring = new()
        {
            spring = spring == null ? joint.linearLimitSpring.spring : (float)spring,
            damper = damper == null ? joint.linearLimitSpring.damper : (float)damper
        };
    }

    public static void SetJointLimit(HingeJoint joint, float? min, float? max, float? bounciness, float? minBounceVelocity) {
        joint.limits = new()
        {
            min = min == null ? joint.limits.min : (float)min,
            max = max == null ? joint.limits.max : (float)max,
            bounciness = bounciness == null ? joint.limits.bounciness : (float)bounciness,
            bounceMinVelocity = minBounceVelocity == null ? joint.limits.bounceMinVelocity : (float)minBounceVelocity
        };
    }

    public static float[][] MatrixTp(float[][] m)
    {
        if (m.Length == 0 || m[0].Length == 0) return Array.Empty<float[]>();
        float[][] res = new float[m[0].Length][];
        for (int i = 0; i < res.Length; ++i)
        {
            res[i] = new float[m.Length];
            for (int j = 0; j < m.Length; ++j)
                res[i][j] = m[j][i];
        }
        return res;
    }

    public static float[][] MatrixMul(float[][] a, float[][] b)
    {
        if (a.Length == 0 || a[0].Length == 0 || b.Length == 0 || b[0].Length == 0 || a[0].Length != b.Length) return Array.Empty<float[]>();

        float[][] res = new float[a.Length][];

        for (int i = 0; i < a.Length; ++i)
        {
            res[i] = new float[b[0].Length];
            for (int j = 0; j < b[0].Length; ++j)
            {
                for (int k = 0; k < b.Length; ++k)
                    res[i][j] += a[i][k] * b[k][j];
            }
        }
        return res;
    }

    public static float[][] MatrixMulTp(float[][] m)
    {
        if (m.Length == 0 || m[0].Length == 0) return Array.Empty<float[]>();

        float[][] res = new float[m.Length][];
        for (int i = 0; i < res.Length; ++i)
            res[i] = new float[res.Length];
        for (int i = 0; i < res.Length; ++i)
            for (int j = i; j < res.Length; ++j)
            {
                for (int k = 0; k < m[0].Length; ++k)
                    res[i][j] += m[i][k] * m[j][k];
                if(i != j)
                    res[j][i] = res[i][j];
            }
        return res;
    }

    public static float[][] MatrixTpMul(float[][] m)
    {
        if (m.Length == 0 || m[0].Length == 0) return Array.Empty<float[]>();

        float[][] res = new float[m[0].Length][];
        for (int i = 0; i < res.Length; ++i)
            res[i] = new float[res.Length];
        for (int i = 0; i < res.Length; ++i)
            for (int j = i; j < res.Length; ++j)
            {
                for (int k = 0; k < m.Length; ++k)
                    res[i][j] += m[k][i] * m[k][j];
                if (i != j)
                    res[j][i] = res[i][j];
            }
        return res;
    }

    public static float[][] MatrixInverse(float[][] m, float zero = 0.0009765625f)
    {
        if (m.Length == 0 || m.Length != m[0].Length) return Array.Empty<float[]>();

        bool IsZero(float v) => Mathf.Abs(v) < zero;

        if (m.Length > 2)
        {
            float[][] res = new float[m.Length][], mcp = new float[m.Length][];
            for (int i = 0; i < m.Length; ++i)
            {
                res[i] = new float[m.Length];
                res[i][i] = 1;
                mcp[i] = new float[m.Length];
                for (int j = 0; j < m.Length; ++j)
                {
                    mcp[i][j] = m[i][j];
                }
            }
            for (int i = m.Length - 1; i > 0; --i)
            {
                float[] baseRow = mcp[i];
                if (IsZero(baseRow[i]))
                {
                    int pt = i;
                    do
                    {
                        if (pt == 0) return Array.Empty<float[]>();
                        --pt;
                    }
                    while (IsZero(mcp[pt][i]));
                    baseRow = mcp[pt];
                    mcp[pt] = mcp[i];
                    mcp[i] = baseRow;
                    var temp = res[pt];
                    res[pt] = res[i];
                    res[i] = temp;
                }
                float imul = 1 / baseRow[i];

                for (int j = 0; j < m.Length; ++j)
                {
                    res[i][j] *= imul;
                }
                for (int j = 0; j < i; ++j)
                {
                    mcp[i][j] *= imul;

                }
                baseRow = mcp[i];
                for (int _i = 0; _i < i; ++_i)
                {
                    for (int j = 0; j < m.Length; ++j)
                        res[_i][j] -= mcp[_i][i] * res[i][j];
                    for (int j = 0; j < i; ++j)
                        mcp[_i][j] -= mcp[_i][i] * baseRow[j];

                }
            }
            if (IsZero(mcp[0][0]))
                return Array.Empty<float[]>();
            {
                float imul = 1 / mcp[0][0];
                for (int j = 0; j < m.Length; ++j)
                    res[0][j] *= imul;
            }
            for (int i = 0; i < m.Length; ++i)
                for (int j = 0; j < i; ++j)
                {
                    if (IsZero(mcp[i][j])) continue;
                    for (int _j = 0; _j < m.Length; ++_j)
                        res[i][_j] -= mcp[i][j] * res[j][_j]; 
                }
            return res;
        }
        else if (m.Length == 2)
        {
            float det = m[0][0] * m[1][1] - m[1][0] * m[0][1];
            if (det != 0)
            {
                det = 1 / det;
                return new float[2][] { new float[2] { m[1][1] * det, -m[0][1] * det }, new float[2] { -m[1][0] * det, m[0][0] * det } };
            }
        }
        else if (m.Length == 1) return m[0][0] == 0 ? Array.Empty<float[]>() : new float[1][] { new float[1] { 1 } };
        ;
        return Array.Empty<float[]>();
    }

    public static float[] LinearRegression(Func<float, float, float>[] model, float[] x, float[] y)
    {
        float[][] X = new float[model.Length][];
        float[][] Y = new float[1][];
        Y[0] = new float[model.Length]; 
        for (int i = 0; i < X.Length; i++)
        {
            X[i] = new float[x.Length];
            Y[0][i] = y[i];
            Func<float, float, float> cur = model[i];
            for (int j = 0; j < x.Length; j++)
                X[i][j] = cur(x[j], y[j]);
        }

        float[] res = new float[model.Length];

        float[][] temp = MatrixTpMul(X);
        PrintMatrix(temp);
        temp = MatrixInverse(temp);
        PrintMatrix(temp);
        temp = MatrixMul(X, temp);
        PrintMatrix(temp);
        temp = MatrixMul(Y, temp);
        PrintMatrix(temp);

        X = MatrixMul(Y, MatrixMul(X, MatrixInverse(MatrixTpMul(X))));

        return res;
        // ((_Xtp*_X).inv()*_Xtp*_y).tp() = _y.tp()*_X*(_Xtp*_X).inv()
        /*
         def LinearRegression(_df, model, x = "x", y = "y", offset = 0): #model is a tuple of functions
    #from pandas import DataFrame as DF
    _x = None
    _y = None
    if type(_df) == DF:
        _x = _df[x].tolist()
        _y = _df[y].tolist()
    elif type(_df) == dict:
        _x,_y = tuple(zip(*_df.items()))
    elif type(_df) == tuple or type(_df) == list:
        _x = _df[0 if x == "x" else x]
        _y = _df[1 if x == "y" else y]
    _y = Matrix((_y,)).tp()
    _X = Matrix(tuple(tuple(_f(_v) for _f in model) for _v in _x) if offset == 0 else
        tuple(tuple(_f(_v) for _f in model) for _v in _x for _v in (_v+offset,)))
    _Xtp = _X.tp()
    return ((_Xtp*_X).inv()*_Xtp*_y).tp()
         */
    }

    public class PID
    {
        public float p, i, d;
        float preError = 0, errorIntegral = 0;
        public PID(float p, float i, float d)
        {
            this.p = p;
            this.i = i;
            this.d = d;
        }
        public float Update(float error, float dt)
        {
            errorIntegral += 0.5f * (error + preError) * dt;
            float errorDerivative = (error - preError) / dt;
            preError = error;
            return p * error + i * errorIntegral + d * errorDerivative;
        }
    }

    public class PD4
    {
        public float p, d;
        Vector4 preError = Vector4.zero;
        public PD4(float p, float d)
        {
            this.p = p;
            this.d = d;
        }
        public Vector4 Update(Vector4 error, float dt)
        {
            Vector4 errorDerivative = (error - preError) / dt;
            preError = error;
            return p * error + d * errorDerivative;
        }
    }

    public class PD3 {
        public float p, d;
        Vector3 preError = Vector3.zero;
        public PD3(float p, float d)
        {
            this.p = p;
            this.d = d;
        }
        public Vector3 Update(Vector3 error, float dt)
        {
            Vector3 errorDerivative = (error - preError) / dt;
            preError = error;
            return p * error + d * errorDerivative;
        }
    }

    public class PD2
    {
        public float p, d;
        Vector2 preError = Vector2.zero;
        public PD2(float p, float d)
        {
            this.p = p;
            this.d = d;
        }
        public Vector2 Update(Vector2 error, float dt)
        {
            Vector2 errorDerivative = (error - preError) / dt;
            preError = error;
            return p * error + d * errorDerivative;
        }
    }

    public class PD
    {
        public float p, d;
        float preError = 0;
        public PD(float p, float d)
        {
            this.p = p;
            this.d = d;
        }
        public float Update(float error, float dt)
        {
            float errorDerivative = (error - preError) / dt;
            preError = error;
            return p * error + d * errorDerivative;
        }
    }

    private static readonly Dictionary<int, ColliderContactMask> MapColliderContactMask = new();
    private class ColliderContactMask {
        public HashSet<int> provideContactKeys = new(), hasModifiableContactKeys = new();
        public ColliderContactMask() { }
    }
    public static void AddProvideContact(Collider collider, int key) {
        int colliderID = collider.GetInstanceID();
        if (!MapColliderContactMask.ContainsKey(colliderID)) MapColliderContactMask[colliderID] = new();
        MapColliderContactMask[colliderID].provideContactKeys.Add(key);
        collider.providesContacts = true;
    }
    public static void RemoveProvideContact(Collider collider, int key)
    {
        int colliderID = collider.GetInstanceID();
        if (!MapColliderContactMask.ContainsKey(colliderID) || MapColliderContactMask[colliderID].provideContactKeys.Count == 0) return;
        if (MapColliderContactMask[colliderID].provideContactKeys.Remove(key))
            if (MapColliderContactMask[colliderID].provideContactKeys.Count == 0)
                collider.providesContacts = false;
    }
    public static void AddHasModifiableContact(Collider collider, int key)
    {
        int colliderID = collider.GetInstanceID();
        if (!MapColliderContactMask.ContainsKey(colliderID)) MapColliderContactMask[colliderID] = new();
        MapColliderContactMask[colliderID].hasModifiableContactKeys.Add(key);
        collider.hasModifiableContacts = true;
    }
    public static void RemoveHasModifiableContact(Collider collider, int key)
    {
        int colliderID = collider.GetInstanceID();
        if (!MapColliderContactMask.ContainsKey(colliderID) || MapColliderContactMask[colliderID].hasModifiableContactKeys.Count == 0) return;
        if (MapColliderContactMask[colliderID].hasModifiableContactKeys.Remove(key))
            if (MapColliderContactMask[colliderID].hasModifiableContactKeys.Count == 0)
                collider.hasModifiableContacts = false;
    }

    private static readonly Dictionary<int, DisableMask> MapDisableMask = new();
    private class DisableMask
    {
        public HashSet<int> disableKeys = new();
        public DisableMask() { }
    }
    public static void AddDisable(MonoBehaviour com, int key)
    {
        int comID = com.GetInstanceID();
        if (!MapDisableMask.ContainsKey(comID)) MapDisableMask[comID] = new();
        MapDisableMask[comID].disableKeys.Add(key);
        com.enabled = false;
    }
    public static void AddDisable(GameObject com, int key)
    {
        int comID = com.GetInstanceID();
        if (!MapDisableMask.ContainsKey(comID)) MapDisableMask[comID] = new();
        MapDisableMask[comID].disableKeys.Add(key);
        com.SetActive(false);
    }
    public static void RemoveDisable(MonoBehaviour com, int key)
    {
        int comID = com.GetInstanceID();
        if (!MapDisableMask.ContainsKey(comID) || MapDisableMask[comID].disableKeys.Count == 0) return;
        if (MapDisableMask[comID].disableKeys.Remove(key) && MapDisableMask[comID].disableKeys.Count == 0)
            com.enabled = true;
    }
    public static void RemoveDisable(GameObject com, int key)
    {
        int comID = com.GetInstanceID();
        if (!MapDisableMask.ContainsKey(comID) || MapDisableMask[comID].disableKeys.Count == 0) return;
        if (MapDisableMask[comID].disableKeys.Remove(key) && MapDisableMask[comID].disableKeys.Count == 0)
            com.SetActive(false);
    }

    public static readonly Dictionary<int, string> mapShortMonth = new()
    {
        { 1, "Jan" },
        { 2, "Feb" },
        { 3, "Mar" },
        { 4, "Apr" },
        { 5, "May" },
        { 6, "Jun" },
        { 7, "Jul" },
        { 8, "Aug" },
        { 9, "Sep" },
        { 10, "Oct" },
        { 11, "Nov" },
        { 12, "Dec" }
    };

    public static string TimeString(DateTime dateTime)
        => $"{dateTime.Year.ToString()}-{dateTime.Month.ToString().PadLeft(2, '0')}-{dateTime.Day.ToString().PadLeft(2, '0')}_T{dateTime.Hour.ToString().PadLeft(2, '0')}{dateTime.Minute.ToString().PadLeft(2, '0')}{dateTime.Second.ToString().PadLeft(2, '0')}{dateTime.Millisecond.ToString().PadLeft(3, '0')}";

    public static List<T> GetComponentsInAll<T>(Transform parent) {
        List<T> buffer = new();
        List<Transform> queue = new() { parent };
        while (queue.Count > 0)
        {
            Transform next = queue[^1];
            queue.RemoveAt(queue.Count - 1);
            buffer.AddRange(next.GetComponents<T>());
            foreach (Transform child in next)
                queue.Add(child);
        }
        return buffer;
    }

    [Serializable]
    public abstract class AbstractSerialised {
    }

    [Serializable]
    public class BoolSerialised : AbstractSerialised
    {
        [SerializeField]
        public bool data;
        public BoolSerialised(bool data) { this.data = data; }
    }

    [Serializable]
    public class IntSerialised : AbstractSerialised
    {
        [SerializeField]
        public int data;
        public IntSerialised(int data = 0) { this.data = data; }
    }

    [Serializable]
    public class FloatSerialised : AbstractSerialised
    {
        [SerializeField]
        public float data;
        public FloatSerialised(float data = 0) { this.data = data; }
    }

    [Serializable]
    public class StringSerialised : AbstractSerialised
    {
        [SerializeField]
        public string data;
        public StringSerialised(string data = "") { this.data = data; }
    }
    [Serializable]
    public class ListSerialised {
        [SerializeReference]
        public readonly List<AbstractSerialised> data;
        public ListSerialised(List<AbstractSerialised> data) { this.data = data; }
    }

    [Serializable]
    public class DictionarySerialised : ISerializationCallbackReceiver
    {
        public Dictionary<string, AbstractSerialised> data;
        [SerializeField] public readonly List<string> key;
        [SerializeReference] public readonly List<AbstractSerialised> value;
        public DictionarySerialised(Dictionary<string, AbstractSerialised> data)
        {
            this.data = data;
            key = new(data.Keys);
            value = new(data.Values);
        }
        public void OnBeforeSerialize()
        {
            key.AddRange(data.Keys);
            value.AddRange(data.Values);
        }
        public void OnAfterDeserialize()
        {
            for (int i = 0; i < key.Count; i++)
                data.Add(key[i], value[i]);
        }

    }

    [Serializable]
    public class SingleSerialised<T>
    {
        [SerializeField]
        public T data;
        public SingleSerialised(T data = default) { this.data = data; }
    }

    private readonly static Dictionary<Type, object> predicatedSortedList = new();
    public static List<T> PrepareBinaryInsert<T, P>(List<T> list, Func<T, P> predicate) where P : IComparable
    {
        if (!predicatedSortedList.ContainsKey(typeof(T))) predicatedSortedList.Add(typeof(T), new Dictionary<List<T>, (List<P>, Func<T, P>)>());
        var predication = (Dictionary<List<T>, (List<P>, Func<T, P>)>)predicatedSortedList[typeof(T)];
        predication[list] = new(new(), predicate);
        return list;
    }
    public static int BinaryInsert<T, P>(List<T> list, T ele) where P : IComparable
    {
        var predication = (Dictionary<List<T>, (List<P> predicatedList, Func<T, P> predicate)>)predicatedSortedList[typeof(T)];
        List<P> predicatedList = predication[list].predicatedList;
        var predicate = predication[list].predicate;
        P predicatedEle = predicate(ele);
        object predicatedObj = (object)predicatedEle;
        if (list.Count == 0 || predicatedList[^1].CompareTo(predicatedObj) <= 0)
        {
            list.Add(ele);
            predicatedList.Add(predicatedEle);
            return 0;
        }
        int check, comparedCheck = predicatedList[0].CompareTo(predicatedObj);
        if (comparedCheck > 0)
        {
            list.Insert(0, ele);
            predicatedList.Insert(0, predicatedEle);
            return 0;
        }
        if (comparedCheck == 0)
        {
            check = 1;
            while (check < list.Count && predicatedList[check].CompareTo(predicatedObj) == 0) ++check;
            list.Insert(check, ele);
            predicatedList.Insert(check, predicatedEle);
            return check;
        }
        int a = 0, b = list.Count;
        while (a < b)
        {
            check = (a + b) >> 1;
            comparedCheck = predicatedList[check].CompareTo(predicatedObj);
            if (comparedCheck == 0)
            {
                ++check;
                while (check < list.Count && predicatedList[check].CompareTo(predicatedObj) == 0) ++check;
                list.Insert(check, ele);
                predicatedList.Insert(check, predicatedEle);
                return check;
            }
            if (comparedCheck > 0)
            {
                b = check;
            }
            else
            {
                a = check + 1;
            }
        }
        list.Insert(a, ele);
        predicatedList.Insert(a, predicatedEle);
        return 0;
    }

    public static void BinaryInsertRemovePredicatedAtIndex<T, P>(List<T> list, int index)
    {
        var predication = (Dictionary<List<T>, (List<P> predicatedList, Func<T, P>)>)predicatedSortedList[typeof(T)];
        if (!predication.ContainsKey(list)) return;
        predication[list].predicatedList.RemoveAt(index);
    }

    public static int BinaryInsertIndex(List<float> list, float value)
    {
        if (list.Count == 0) return 0;
        if (value < list[0])
        {
            return 0;
        }
        else if (value >= list[^1])
        {
            return list.Count;
        }
        else if(value == list[0])
        {
            for (int i = 1; i < list.Count;i++)
            {
                if (list[i] == value) continue;
                return i;
            }
            return list.Count;
        }

        int a = 0, b = list.Count, check;

        while (a < b)
        {
            check = (a + b) >> 1;
            if (value > list[check])
            {
                a = check + 1;
            }
            else if (value < list[check])
            {
                b = check;
            }
            else
            {
                for (; ++check < list.Count;)
                {
                    if (list[check] == value) continue;
                    return check;
                }
                return list.Count;
            }
        }

        return a;
    }

    public enum On
    {
        Update,
        FixedUpdate,
    }

    public static bool InvokeAnd(Func<bool> f, bool Default = true)
    {
        foreach (var e in f.GetInvocationList())
            if (!((Func<bool>)e).Invoke()) return false;
        return Default;
    }

    public static bool InvokeOr(Func<bool> f, bool Default = false)
    {
        foreach (var e in f.GetInvocationList())
            if (((Func<bool>)e).Invoke()) return true;
        return Default;
    }

    public static T InvokeReduce<T>(Func<T> f, Func<T, T, T> operation, T result = default)
    {
        foreach (var e in f.GetInvocationList())
            result = operation(((Func<T>)e).Invoke(), result);
        return result;
    }

    public static bool IsTypeInTypeOfType<T>(Type compare)
        => typeof(T).IsGenericType && typeof(T).GenericTypeArguments[0] == compare;
    public static bool IsTypeInTypeOfType<T>(T _, Type compare)
        => typeof(T).IsGenericType && typeof(T).GenericTypeArguments[0] == compare;

    public static Type[] GenericTypeArguments<T>()
        => typeof(T).IsGenericType ? typeof(T).GenericTypeArguments : null;
    public static Type[] GenericTypeArguments<T>(T _)
        => typeof(T).IsGenericType ? typeof(T).GenericTypeArguments : null;

    public static bool IsDefinitionTypeOfType<T>(Type compare)
        => typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == compare;
    public static bool IsDefinitionTypeOfType<T>(T _, Type compare)
        => typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == compare;

    public static byte[] GetBytesFromBase<T>(T data)
        =>
        data == null ? null :
        data is List<string> STRINGLIST ? FromStringstoBytes(STRINGLIST, Encoding.UTF8.GetBytes) :
        data is string[] STRINGARR ? FromStringstoBytes(STRINGARR, Encoding.UTF8.GetBytes) :
        data is string STRING ? Encoding.UTF8.GetBytes(STRING) :
        data is List<double> DOUBLELIST ? FromBasestoBytes(DOUBLELIST) :
        data is double[] DOUBLEARR ? FromBasestoBytes(DOUBLEARR) :
        data is double DOUBLE  ? BitConverter.GetBytes(DOUBLE) :
        data is List<ulong> ULONGLIST ? FromBasestoBytes(ULONGLIST) :
        data is ulong[] ULONGARR ? FromBasestoBytes(ULONGARR) :
        data is ulong ULONG ? BitConverter.GetBytes(ULONG) :
        data is List<long> LONGLIST ? FromBasestoBytes(LONGLIST) :
        data is long[] LONGARR ? FromBasestoBytes(LONGARR) :
        data is long LONG ? BitConverter.GetBytes(LONG) :
        data is List<float> FLOATLIST ? FromBasestoBytes(FLOATLIST) :
        data is float[] FLOATARR ? FromBasestoBytes(FLOATARR) :
        data is float FLOAT? BitConverter.GetBytes(FLOAT) :
        data is List<uint> UINTLIST ? FromBasestoBytes(UINTLIST) :
        data is uint[] UINTARR ? FromBasestoBytes(UINTARR) :
        data is uint UINT? BitConverter.GetBytes(UINT) :
        data is List<int> INTLIST ? FromBasestoBytes(INTLIST) :
        data is int[] INTARR ? FromBasestoBytes(INTARR) :
        data is int INT ? BitConverter.GetBytes(INT) :
        data is List<char> CHARLIST ? FromBasestoBytes(CHARLIST) :
        data is char[] CHARARR ? FromBasestoBytes(CHARARR) :
        data is char CHAR? BitConverter.GetBytes(CHAR) :   
        data is List<ushort> USHORTLIST ? FromBasestoBytes(USHORTLIST) :
        data is ushort[] USHORTARR ? FromBasestoBytes(USHORTARR) :
        data is ushort USHORT ? BitConverter.GetBytes(USHORT) :
        data is List<short> SHORTLIST ? FromBasestoBytes(SHORTLIST) :
        data is short[] SHORTARR ? FromBasestoBytes(SHORTARR) :
        data is short SHORT ? BitConverter.GetBytes(SHORT) :
        data is List<bool> BOOLLIST ? BOOLLIST.ConvertAll(e => BitConverter.GetBytes(e)[0]).ToArray() :
        data is bool[] BOOLARR ? FromBasestoBytes(BOOLARR) :
        data is bool BOOL ? BitConverter.GetBytes(BOOL) :
        data is List<byte> BYTELIST ? BYTELIST.ToArray() :
        data is byte[] ? (byte[])(object)data:
        data is byte BYTE ? new byte[1] {BYTE} :
        data is List<sbyte> SBYTELIST ? SBYTELIST.ConvertAll(e=>(byte)e).ToArray() :
        data is sbyte[] SBYTEARR ? FromBasestoBytes(SBYTEARR) :
        data is sbyte SBYTE ? new byte[1] {(byte)SBYTE} :
        null;
    public static T FromBytesToBase<T>(byte[] data)
        => (T)(object)(
        typeof(T) == typeof(IEnumerable<string>) ? FromBytesToStrings(data) :
        typeof(T) == typeof(List<string>) ? FromBytesToStrings(data).ToList() :
        typeof(T) == typeof(string[]) ? FromBytesToStrings(data).ToArray() :
        typeof(T) == typeof(string) ? Encoding.UTF8.GetString(data) :
        typeof(T) == typeof(IEnumerable<double>) ? FromBytesToIEnumerable<double>(data) :
        typeof(T) == typeof(List<double>) ? FromBytesToList<double>(data) :
        typeof(T) == typeof(double[]) ? FromBytesToArray<double>(data) :
        typeof(T) == typeof(double) ? BitConverter.ToDouble(data) :
        typeof(T) == typeof(IEnumerable<ulong>) ? FromBytesToIEnumerable<ulong>(data) :
        typeof(T) == typeof(List<ulong>) ? FromBytesToList<ulong>(data) :
        typeof(T) == typeof(ulong[]) ? FromBytesToArray<ulong>(data) :
        typeof(T) == typeof(ulong) ? BitConverter.ToUInt64(data) :
        typeof(T) == typeof(IEnumerable<long>) ? FromBytesToIEnumerable<long>(data) :
        typeof(T) == typeof(List<long>) ? FromBytesToList<long>(data) :
        typeof(T) == typeof(long[]) ? FromBytesToArray<long>(data) :
        typeof(T) == typeof(long) ? BitConverter.ToInt64(data) :
        typeof(T) == typeof(IEnumerable<float>) ? FromBytesToIEnumerable<float>(data) :
        typeof(T) == typeof(List<float>) ? FromBytesToList<float>(data) :
        typeof(T) == typeof(float[]) ? FromBytesToArray<float>(data) :
        typeof(T) == typeof(float) ? BitConverter.ToSingle(data) :
        typeof(T) == typeof(IEnumerable<uint>) ? FromBytesToIEnumerable<uint>(data) :
        typeof(T) == typeof(List<uint>) ? FromBytesToList<uint>(data) :
        typeof(T) == typeof(uint[]) ? FromBytesToArray<uint>(data) :
        typeof(T) == typeof(uint) ? BitConverter.ToUInt32(data) :
        typeof(T) == typeof(IEnumerable<int>) ? FromBytesToIEnumerable<int>(data) :
        typeof(T) == typeof(List<int>) ? FromBytesToList<int>(data) :
        typeof(T) == typeof(int[]) ? FromBytesToArray<int>(data) :
        typeof(T) == typeof(int) ? BitConverter.ToInt32(data) :
        typeof(T) == typeof(IEnumerable<char>) ? FromBytesToIEnumerable<char>(data) :
        typeof(T) == typeof(List<char>) ? FromBytesToList<char>(data) :
        typeof(T) == typeof(char[]) ? FromBytesToArray<char>(data) :
        typeof(T) == typeof(char) ? BitConverter.ToChar(data) :
        typeof(T) == typeof(IEnumerable<ushort>) ? FromBytesToIEnumerable<ushort>(data) :
        typeof(T) == typeof(List<ushort>) ? FromBytesToList<ushort>(data) :
        typeof(T) == typeof(ushort[]) ? FromBytesToArray<ushort>(data) :
        typeof(T) == typeof(ushort) ? BitConverter.ToUInt16(data) :
        typeof(T) == typeof(IEnumerable<short>) ? FromBytesToIEnumerable<short>(data) :
        typeof(T) == typeof(List<short>) ? FromBytesToList<short>(data) :
        typeof(T) == typeof(short[]) ? FromBytesToArray<short>(data) :
        typeof(T) == typeof(short) ? BitConverter.ToInt16(data) :
        typeof(T) == typeof(IEnumerable<bool>) ? FromBytesToIEnumerable<bool>(data) :
        typeof(T) == typeof(List<bool>) ? FromBytesToList<bool>(data) :
        typeof(T) == typeof(bool[]) ? FromBytesToArray<bool>(data) :
        typeof(T) == typeof(bool) ? BitConverter.ToBoolean(data) :
        typeof(T) == typeof(IEnumerable<byte>) ? data.AsEnumerable():
        typeof(T) == typeof(List<byte>) ? data.ToList() :
        typeof(T) == typeof(byte[]) ? data :
        typeof(T) == typeof(IEnumerable<sbyte>) ? FromBytesToIEnumerable<sbyte>(data) :
        typeof(T) == typeof(List<sbyte>) ? FromBytesToList<sbyte>(data) :
        typeof(T) == typeof(sbyte[]) ? FromBytesToArray<sbyte>(data) :
        (typeof(T) == typeof(byte)) || (typeof(T) == typeof(sbyte)) ? data[0] :
        null
        );

    public static byte[] FromStringstoBytes(IEnumerable<string> strs, Func<string, byte[]> encoder)
    {
        List<byte> buffer = new();
        foreach (string str in strs) {
            buffer.AddRange(BitConverter.GetBytes(str.Length));
            buffer.AddRange(encoder(str));
        }
        return buffer.ToArray();
    }
    public static byte[] FromBasestoBytes<T>(IEnumerable<T> Ts)
    {
        List<byte> buffer = new();
        foreach (T t in Ts)
            buffer.AddRange(GetBytesFromBase(t));
        return buffer.ToArray();
    }
    public static IEnumerable<string> FromBytesToStrings(byte[] data) {
        int pointer = 0;
        while (pointer < data.Length)
        {
            int count = BitConverter.ToInt32(data, pointer);
            pointer += 4;
            yield return Encoding.UTF8.GetString(data, pointer, count);
            pointer += count;
        }
    }

    private static IEnumerable<T> FromBytesToIEnumerable<T>(byte[] data)
    {
        int size = byteSize[typeof(T)];

        if (typeof(T) == typeof(double))
            for (int pointer = 0; pointer < data.Length; pointer += size)
                yield return (T)(object)BitConverter.ToDouble(data, pointer);
        else if (typeof(T) == typeof(ulong))
            for (int pointer = 0; pointer < data.Length; pointer += size)
                yield return (T)(object)BitConverter.ToUInt64(data, pointer);
        else if (typeof(T) == typeof(long))
            for (int pointer = 0; pointer < data.Length; pointer += size)
                yield return (T)(object)BitConverter.ToInt64(data, pointer);
        else if (typeof(T) == typeof(float))
            for (int pointer = 0; pointer < data.Length; pointer += size)
                yield return (T)(object)BitConverter.ToSingle(data, pointer);
        else if (typeof(T) == typeof(uint))
            for (int pointer = 0; pointer < data.Length; pointer += size)
                yield return (T)(object)BitConverter.ToUInt32(data, pointer);
        else if (typeof(T) == typeof(int))
            for (int pointer = 0; pointer < data.Length; pointer += size)
                yield return (T)(object)BitConverter.ToInt64(data,pointer);
        else if (typeof(T) == typeof(char))
            for (int pointer = 0; pointer < data.Length; pointer += size)
                yield return (T)(object)BitConverter.ToChar(data, pointer);
        else if (typeof(T) == typeof(ushort))
            for (int pointer = 0; pointer < data.Length; pointer += size)
                yield return (T)(object)BitConverter.ToUInt16(data, pointer);
        else if (typeof(T) == typeof(short))
            for (int pointer = 0; pointer < data.Length; pointer += size)
                yield return (T)(object)BitConverter.ToInt16(data, pointer);
        else if (typeof(T) == typeof(bool))
            for (int pointer = 0; pointer < data.Length; pointer += size)
                yield return (T)(object)BitConverter.ToBoolean(data, pointer);
        else if (typeof(T) == typeof(byte))
            for (int pointer = 0; pointer < data.Length; pointer += size)
                yield return (T)(object) data[pointer];
        else if (typeof(T) == typeof(sbyte))
            for (int pointer = 0; pointer < data.Length; pointer += size)
                yield return (T)(object)(sbyte)data[pointer];
    }

    private static List<T> FromBytesToList<T>(byte[] data)
    {
        int shift = byteShift[typeof(T)], size = byteSize[typeof(T)], pointer = 0;
        List<T> res = new(data.Length >> shift);

        if (typeof(T) == typeof(double))
            for (; pointer < data.Length; pointer += size)
                res.Add((T)(object)BitConverter.ToDouble(data, pointer));
        else if (typeof(T) == typeof(ulong))
            for (; pointer < data.Length; pointer += size)
                res.Add((T)(object)BitConverter.ToUInt16(data, pointer));
        else if (typeof(T) == typeof(long))
            for (; pointer < data.Length; pointer += size)
                res.Add((T)(object)BitConverter.ToInt16(data, pointer));
        else if (typeof(T) == typeof(float))
            for (; pointer < data.Length; pointer += size)
                res.Add((T)(object)BitConverter.ToSingle(data, pointer));
        else if (typeof(T) == typeof(uint))
            for (; pointer < data.Length; pointer += size)
                res.Add((T)(object)BitConverter.ToUInt32(data, pointer));
        else if (typeof(T) == typeof(int))
            for (; pointer < data.Length; pointer += size)
                res.Add((T)(object)BitConverter.ToInt32(data, pointer));
        else if (typeof(T) == typeof(char))
            for (; pointer < data.Length; pointer += size)
                res.Add((T)(object)BitConverter.ToChar(data, pointer));
        else if (typeof(T) == typeof(ushort))
            for (; pointer < data.Length; pointer += size)
                res.Add((T)(object)BitConverter.ToUInt16(data, pointer));
        else if (typeof(T) == typeof(short))
            for (; pointer < data.Length; pointer += size)
                res.Add((T)(object)BitConverter.ToInt16(data, pointer));
        else if (typeof(T) == typeof(bool))
            for (; pointer < data.Length; ++pointer)
                res.Add((T)(object)BitConverter.ToBoolean(data, pointer));
        else if (typeof(T) == typeof(byte))
            for (; pointer < data.Length; ++pointer)
                res.Add((T)(object)data[pointer]);
        else if (typeof(T) == typeof(sbyte))
            for (; pointer < data.Length; ++pointer)
                res.Add((T)(object)(sbyte)data[pointer]);
        return res;
    }

    private static T[] FromBytesToArray<T>(byte[] data)
    {
        int shift = byteShift[typeof(T)], size = byteSize[typeof(T)], pointer = 0, iter = 0;
        T[] res = new T[data.Length >> shift];

        if (typeof(T) == typeof(double))
            for (; pointer < data.Length; pointer += size)
                res[iter++] = (T)(object)BitConverter.ToDouble(data, pointer);
        else if (typeof(T) == typeof(ulong))
            for (; pointer < data.Length; pointer += size)
                res[iter++] = (T)(object)BitConverter.ToUInt16(data, pointer);
        else if (typeof(T) == typeof(long))
            for (; pointer < data.Length; pointer += size)
                res[iter++] = (T)(object)BitConverter.ToInt64(data, pointer);
        else if (typeof(T) == typeof(float))
            for (; pointer < data.Length; pointer += size)
                res[iter++] = (T)(object)BitConverter.ToSingle(data, pointer);
        else if (typeof(T) == typeof(uint))
            for (; pointer < data.Length; pointer += size)
                res[iter++] = (T)(object)BitConverter.ToUInt32(data, pointer);
        else if (typeof(T) == typeof(int))
            for (; pointer < data.Length; pointer += size)
                res[iter++] = (T)(object)BitConverter.ToInt32(data, pointer);
        else if (typeof(T) == typeof(char))
            for (; pointer < data.Length; pointer += size)
                res[iter++] = (T)(object)BitConverter.ToChar(data, pointer);
        else if (typeof(T) == typeof(ushort))
            for (; pointer < data.Length; pointer += size)
                res[iter++] = (T)(object)BitConverter.ToUInt16(data, pointer);
        else if (typeof(T) == typeof(short))
            for (; pointer < data.Length; pointer += size)
                res[iter++] = (T)(object)BitConverter.ToInt16(data, pointer);
        else if (typeof(T) == typeof(bool))
            for (; pointer < data.Length; ++pointer)
                res[iter++] = (T)(object)BitConverter.ToBoolean(data, pointer);
        else if (typeof(T) == typeof(byte))
            for (; pointer < data.Length; ++pointer)
                res[iter++] = (T)(object)data[pointer];
        else if (typeof(T) == typeof(sbyte))
            for (; pointer < data.Length; ++pointer)
                res[iter++] = (T)(object)(sbyte)data[pointer];
        return res;
    }

    public static readonly Dictionary<Type, int> byteSize = new()
    {
        {typeof(decimal), 16},
        {typeof(double), 8},
        {typeof(ulong), 8},
        {typeof(long), 8},
        {typeof(uint), 4},
        {typeof(int), 4},
        {typeof(float), 4},
        {typeof(char), 2},
        {typeof(ushort), 2},
        {typeof(short), 2},
        {typeof(bool), 1},
        {typeof(byte), 1},
        {typeof(sbyte), 1},
    };
    public static readonly Dictionary<Type, int> byteShift = new()
    {
        {typeof(decimal), 4},
        {typeof(double), 3},
        {typeof(ulong), 3},
        {typeof(long), 3},
        {typeof(uint), 2},
        {typeof(int), 2},
        {typeof(float), 2},
        {typeof(char), 1},
        {typeof(ushort), 1},
        {typeof(short), 1},
        {typeof(bool), 0},
        {typeof(byte), 0},
        {typeof(sbyte), 0},
    };

    public static void GenericReflection(Type t) {
        
        if (t.IsArray) {
            Debug.Log("Array");
            GenericReflection(t.GetElementType());
        }
        else if (t.IsGenericType)
        {
            bool isBase = true;
            Type tDef = t.GetGenericTypeDefinition();
            if (tDef == typeof(List<>)) {isBase = false; Debug.Log("List"); }
            
            if(isBase) Debug.Log("likely base");
            foreach (Type tt in t.GenericTypeArguments)
                GenericReflection(tt);
        }
        else {
            Debug.Log(t);
        }
    }

    public static bool ContainsReservedSymbols(string str) {
        foreach (char c in str)
            if ("<>:\"/\\|?*".Contains(c)) return true;
        return false;
    }

    public static RaycastHit defaultRayCastHit = new();


    private static readonly HashSet<char> charBuffer = new();
    public static bool StringDuplicates(string check, int max, out HashSet<char> lastElements)
    {
        lastElements = charBuffer;
        int dup = 0;
        charBuffer.Clear();
        foreach(char c in check)
        {
            if (charBuffer.Add(c))
            {
                dup++;
                if(dup > max)
                {
                    charBuffer.Remove(c);
                    return true;
                }
            }
        }
        return false;
    }

    public static HashSet<char> StringCountDuplicates(string check)
    {
        charBuffer.Clear();
        foreach (char c in check)
            charBuffer.Add(c);
        return charBuffer;
    }

    public class NestedToken
    {
        private bool valid = false;
        public bool Valid { get => valid; }
        
        public Dictionary<string, NestedToken> branch;

        public bool Contains(string key) { return branch.ContainsKey(key); }
        public NestedToken Get(string key) { return branch.ContainsKey(key) ? branch[key]: null; }

        public void Add(string key)
        {
            string[] tokens = key.Split(); // splits strictly space
            NestedToken cur = this;
            for(int i = 0; i < tokens.Length;)
            {
                cur.branch ??= new();
                if (!cur.branch.ContainsKey(tokens[i]))
                    cur.branch.Add(tokens[i], new()); 
                cur = cur.branch[tokens[i]];
                if(++i == tokens.Length)
                    cur.valid = true;
            }
        }

        public int FindBest(string[] tokens, int start, NestedToken t , out string result)
        {
            result = "";
            if (start >= tokens.Length) return -1;
            string curToken = tokens[start], curTokens = "";
            int next = -1;
            NestedToken curNT = this;
            while (curNT.branch != null && curNT.branch.ContainsKey(curToken))
            {
                curNT = curNT.branch[curToken];
                curTokens += curToken + " ";
                if(curNT.Valid)
                {
                    result = curTokens[..^1];
                    next = start+1;
                }
                if (++start >= tokens.Length) break;
                curToken = tokens[start];
            }
            return next;
        }
    }

    public static void PrintMatrix<T>(T[][] m)
    {
        if (m.Length == 0 || m[0].Length == 0) { Debug.Log("[]\n"); return; }
        int[] maxes = new int[m[0].Length];
        string[][] strings = new string[m.Length][];

        for (int j = 0; j < m[0].Length; ++j)
            maxes[j] = 0;

        for (int i = 0; i < m.Length; ++i)
        {
            strings[i] = new string[m[i].Length];
            for (int j = 0; j < m[i].Length; ++j)
            {
                strings[i][j] = m[i][j].ToString();
                if (strings[i][j].Length > maxes[j])
                    maxes[j] = strings[i][j].Length;
            }
        }
        string res = "";
        for (int i = 0; i < m.Length; ++i)
        {
            res += "[";
            for (int j = 0; j < m.Length;)
            {
                if(maxes[j] - strings[i][j].Length > 0)
                    res += new string(' ', maxes[j] - strings[i][j].Length);
                res += strings[i][j];
                if (++j < m.Length) res += ',';
            }
            res += "]\n";
        }
        Debug.Log(res);
    }
    
    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void i() {
        byte[] tex = new byte[0]; 
        List<byte[]> texes = new(), nt = null;
        Tuple<byte[]> data = new(new byte[0]);//

        Debug.Log(IsTypeInTypeOfType(tex, typeof(byte)));
        Debug.Log(IsTypeInTypeOfType(texes, typeof(byte[])));
        Debug.Log(IsTypeInTypeOfType(data, typeof(byte[])));
        Debug.Log(IsDefinitionTypeOfType(tex, typeof(List<>)));
        Debug.Log(IsDefinitionTypeOfType(texes, typeof(List<>)));
        Debug.Log(IsDefinitionTypeOfType(nt, typeof(List<>)));
        Debug.Log(IsDefinitionTypeOfType(data, typeof(List<>)));
        Debug.Log(IsDefinitionTypeOfType(data, typeof(Tuple<>)));//
        
        List<string> ss = new () { "Hello,","I am Cheng Kit Fung Jeff,","also known as CKF."};
        foreach (var line in FromBytesToBase<IEnumerable<string>>(GetBytesFromBase(ss))) Debug.Log(line);
        List<double> ds = new() { 54.1451, 0.00131, 155 };
        string res = "";
        foreach (var line in FromBytesToBase<List<double>>(GetBytesFromBase(ds))) res +=","+line;
        Debug.Log(res);
        res = "";
        foreach (var line in FromBytesToBase<double[]>(GetBytesFromBase(ds))) res += "," + line;
        Debug.Log(res);
        res = "";
        List<bool> bs = new() { true, false, false, true, false, true, true, false};
        foreach (var line in FromBytesToBase<bool[]>(GetBytesFromBase(bs))) res += line;
        foreach (var line in FromBytesToBase<List<byte>>(GetBytesFromBase(bs))) res += line;
        Debug.Log(res);
        res = "";
        List<sbyte> sbts = new() { 40, 0, -15, 85, -32, 10, 0, -21 };
        foreach (var line in FromBytesToBase<sbyte[]>(GetBytesFromBase(sbts))) res += ","+line;
        Debug.Log(res.Substring(1));//
        CKF_DataManager dm = GameObject.FindAnyObjectByType<CKF_Root>().gameObject.AddComponent<CKF_DataManager>();
        Debug.Log(dm.GetPath(CKF_DataManager.PathType.data));
        Debug.Log(dm.GetPath(CKF_DataManager.PathType.sharedScene));
        Debug.Log(dm.GetPath(CKF_DataManager.PathType.sharedWorld));
        Debug.Log(dm.GetPath(CKF_DataManager.PathType.worlds));
        Debug.Log(dm.GetPath(CKF_DataManager.PathType.global));

        GenericReflection(typeof((string, (uint, ulong)[], (byte, long, double, List<(int, int[],(float,string),bool)>,sbyte), ushort)));
    }//*/
    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void i()
    {
        for (int testsize = 4; testsize < 100; testsize <<= 1)
        { 
            float[] test = (new int[testsize]).Select(c => UnityEngine.Random.Range(0, 10f)).ToArray();
            string deb = "";

            List<float> res = new();

            foreach (float i in test)
            {
                res.Insert(BinaryInsertIndex(res, i), i);
            }
            deb = "";
            foreach (float i in res)
            {
                deb += i + ", ";
            }
            Debug.Log(deb);
        }
    }*/
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void i()
    {
        /*for (int testsize = 4; testsize < 6; ++testsize)
        { 
            float[][] test = new float[testsize][];
            for (int i = 0; i < testsize; ++i)
            {
                test[i] = new float[testsize];
                for (int j = 0; j < testsize; ++j)
                {
                    test[i][j] = UnityEngine.Random.Range(0, testsize);
                }
            }
            PrintMatrix(test);
            PrintMatrix(MatrixMul(test, test));
            PrintMatrix(MatrixMul(MatrixTp(test), test));
            PrintMatrix(MatrixTpMul(test));
            PrintMatrix(MatrixMul(test, MatrixTp(test)));
            PrintMatrix(MatrixMulTp(test));

            Debug.Log("Begin MatrixInverse");
            float[][] inv = MatrixInverse(test);
            Debug.Log("End MatrixInverse");
            PrintMatrix(MatrixMulTp(inv));
            PrintMatrix(MatrixMul(test, inv));
            PrintMatrix(MatrixMul(inv, test));
        }*/

        int testSize = 20;
        float[] x = new float[testSize], y = new float[testSize], r;

        Func<float, float, float>[] model = new Func<float, float, float>[3]
        { static (float x, float y) => 1, static (float x, float y) => x, static (float x, float y) => x * x };

        for (int i = 0; i < testSize; ++i)
        {
            x[i] = UnityEngine.Random.Range(0.0f, 10.0f);
            y[i] = 3*x[i]*x[i]-7 * x[i] + 5;
        }
        r = LinearRegression(model, x, y);
        string deb = "";
        for (int i = 0; i < r.Length;)
        {
            deb += r[i];
            ++i;
            if (i != r.Length)
            {
                deb += ',';
            }
        }
        Debug.Log(deb);
    }//*/
}