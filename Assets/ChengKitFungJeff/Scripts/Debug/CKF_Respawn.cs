using UnityEngine;

public class CKF_Respawn : MonoBehaviour
{
    public KeyCode key;
    private Rigidbody rb;
    private Vector3 startPos;

    private void Awake()
    {
        if(GetComponent<Rigidbody>() is Rigidbody RB) rb = RB;
        startPos = rb == null? transform.position : rb.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (rb == null)
            {
                rb.position = startPos;
            }
            else
            {
                transform.position = startPos;
            }
        }
    }
}
