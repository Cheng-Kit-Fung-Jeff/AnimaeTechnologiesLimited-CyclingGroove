#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CKF_PhysicsApplier))]
public class EditorPhysicsApplier : Editor
{
    Transform thatTr;
    SerializedProperty thatRB;
    SerializedProperty setPosition;
    SerializedProperty setEuler;
    SerializedProperty setVelocity;
    SerializedProperty setAngularVelocity;
    SerializedProperty setImpulse;

    private void Awake()
    {
        thatTr = ((CKF_PhysicsApplier)target).transform;
        thatRB = serializedObject.FindProperty("thisRB");
        setPosition = serializedObject.FindProperty("setPosition");
        setEuler = serializedObject.FindProperty("setEuler");
        setVelocity = serializedObject.FindProperty("setVelocity");
        setAngularVelocity = serializedObject.FindProperty("setAngularVelocity");
        setImpulse = serializedObject.FindProperty("setImpulse");
        if (thatRB.boxedValue == null && ((CKF_PhysicsApplier)target).TryGetComponent(out Rigidbody rb))
            thatRB.boxedValue = rb;
        serializedObject.ApplyModifiedProperties();
    }

    override public void OnInspectorGUI() {
        thatRB.boxedValue = EditorGUILayout.ObjectField("Target RigidBody", (Rigidbody)thatRB.boxedValue, typeof(Rigidbody),true);
        if (thatRB.boxedValue == null){
            if (((CKF_PhysicsApplier)target).TryGetComponent(out Rigidbody rb))
            {
                if (GUILayout.Button("Get Main Rigidbody"))
                {
                    thatRB.boxedValue = rb;
                }
            }
            else
            {
                EditorGUI.HelpBox(GUILayoutUtility.GetRect(18, 18, "TextField"), "This object has no RigidBody", MessageType.Warning);
            }
        }
        if (GUILayout.Button("Get Position"))
        {
            setPosition.vector3Value = thatTr.position;
        }
        if (GUILayout.Button("Get Euler"))
        {
            setEuler.vector3Value = thatTr.eulerAngles;
        }
        Rigidbody targetRB = (Rigidbody)thatRB.boxedValue;
        if (targetRB)
        {
            if (GUILayout.Button("Get Velocity"))
            {
                setVelocity.vector3Value = targetRB.velocity;
            }
            if (GUILayout.Button("Get Angular Velocity"))
            {
                setAngularVelocity.vector3Value = targetRB.angularVelocity;
            }
        }
            setPosition.vector3Value = EditorGUILayout.Vector3Field("Set Position",setPosition.vector3Value);
            setEuler.vector3Value = EditorGUILayout.Vector3Field("Set Euler", setEuler.vector3Value);
        if (targetRB)
        {
            setVelocity.vector3Value = EditorGUILayout.Vector3Field("Set Velocity",setVelocity.vector3Value);
            setAngularVelocity.vector3Value = EditorGUILayout.Vector3Field("Set Angular Velocity",setAngularVelocity.vector3Value);
            setImpulse.vector3Value = EditorGUILayout.Vector3Field("Set Impulse", setImpulse.vector3Value);
        }
        if (GUILayout.Button("Apply Position")) {
            thatTr.position = setPosition.vector3Value;
        }
        if (GUILayout.Button("Apply Euler"))
        {
            thatTr.eulerAngles = setEuler.vector3Value;
        }
        if (targetRB){
            if (GUILayout.Button("Apply Velocity"))
            {
                targetRB.velocity = setVelocity.vector3Value;
            }
            if (GUILayout.Button("Apply Angular Velocity"))
            {
                targetRB.angularVelocity = setAngularVelocity.vector3Value;
            }
            if (GUILayout.Button("Apply Impulse"))
            {
                targetRB.AddForce(setImpulse.vector3Value, ForceMode.Impulse);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }

}
#endif