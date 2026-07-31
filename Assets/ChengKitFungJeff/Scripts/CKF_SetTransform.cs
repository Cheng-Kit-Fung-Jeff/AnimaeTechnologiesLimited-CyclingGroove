using UnityEngine;

public class CKF_SetTransform : MonoBehaviour
{
    public void SetPositionX(float value)
    {
        transform.position = new Vector3(value, transform.position.y, transform.position.z);
    }
    public void SetPositionY(float value)
    {
        transform.position = new Vector3(transform.position.x, value, transform.position.z);
    }
    public void SetPositionZ(float value)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, value);
    }

    public void SetPositionXY(Vector3 value)
    {
        transform.position = new Vector3(value.x, value.y, transform.position.z);
    }
    public void SetPositionXZ(Vector3 value)
    {
        transform.position = new Vector3(value.x, transform.position.y, value.z);
    }
    public void SetPositionYZ(Vector3 value)
    {
        transform.position = new Vector3(transform.position.x, value.y, value.z);
    }

    public void SetPosition(Vector3 value)
    {
        transform.position = value;
    }

    public void SetLocalPositionX(float value)
    {
        transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
    }
    public void SetLocalPositionY(float value)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, value, transform.localPosition.z);
    }
    public void SetLocalPositionZ(float value)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value);
    }

    public void SetLocalPositionXY(Vector3 value)
    {
        transform.localPosition = new Vector3(value.x, value.y, transform.localPosition.z);
    }
    public void SetLocalPositionXZ(Vector3 value)
    {
        transform.localPosition = new Vector3(value.x, transform.localPosition.y, value.z);
    }
    public void SetLocalPositionYZ(Vector3 value)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, value.y, value.z);
    }

    public void SetLocalPosition(Vector3 value)
    {
        transform.localPosition = value;
    }

    public void SetEulerX(float value)
    {
        transform.rotation = Quaternion.Euler(value, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
    public void SetEulerY(float value)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, value, transform.rotation.eulerAngles.z);
    }
    public void SetEulerZ(float value)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, value);
    }

    public void SetLocalEulerX(float value)
    {
        transform.localRotation = Quaternion.Euler(value, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }
    public void SetLocalEulerY(float value)
    {
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, value, transform.localRotation.eulerAngles.z);
    }
    public void SetLocalEulerZ(float value)
    {
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, value);
    }
}
