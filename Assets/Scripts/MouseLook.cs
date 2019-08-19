using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public enum RotationAxes {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }
    public RotationAxes axes = RotationAxes.MouseXAndY;

    public float horizScale = 9.0f;
    public float vertScale  = 7.0f;

    public Transform body;

    private static float maxVert = 45.0f;
    private static float minVert = -maxVert;

    private float _rotationX = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (axes == RotationAxes.MouseX) {
        // horizontal rotation here
            this.transform.Rotate(0, Input.GetAxis("Mouse X") * horizScale, 0);


        }
        else if (axes == RotationAxes.MouseY) {
        // vertical rotation here

            _rotationX -= Input.GetAxis("Mouse Y") * vertScale;
            _rotationX = Mathf.Clamp(_rotationX, minVert, maxVert);

            float rotationY = transform.localEulerAngles.y;

            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);

        }
        else {
        // both horizontal and vertical rotation here

            _rotationX -= Input.GetAxis("Mouse Y") * vertScale;
            _rotationX = Mathf.Clamp(_rotationX, minVert, maxVert);

            float delta = Input.GetAxis("Mouse X") * horizScale;
            float rotationY = transform.localEulerAngles.y + delta;

            
            // Rotate attached body
            if(body != null) {
                body.localEulerAngles = new Vector3(0, rotationY, 0);
                transform.localEulerAngles = new Vector3(_rotationX, body.localEulerAngles.y, 0);

            } else {
                transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
            }

        }      
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
    }
}
