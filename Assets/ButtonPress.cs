using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{

    public LayerMask acceptableMask;
    [Range(1,100)]
    public int raycastResolution = 1;
    public float raycastHeight = 3;

    private int _oldRaycastResolution;
    private bool _pressed = false;
    private Collider _collider;
    private Vector3[] _corners;
    private List<Vector3> _raycasts;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        _corners = new Vector3[4];
        _oldRaycastResolution = raycastResolution;
        Bounds bounds = _collider.bounds;
        Vector3 bottomRight = bounds.min;
        Vector3 topLeft = bounds.max;

        //Top corners
        _corners[0] = topLeft; //Top left
        _corners[1] = new Vector3(bottomRight.x, topLeft.y, topLeft.z); // Top Right
        _corners[2] = new Vector3(bottomRight.x, topLeft.y, bottomRight.z); // Bottom Right
        _corners[3] = new Vector3(topLeft.x, topLeft.y, bottomRight.z); //Bottom Left

        _raycasts = new List<Vector3>();
        interpolateRaycasts();
    }

    // Update is called once per frame
    void Update()
    {
        _pressed = checkForCollisions();

        if(_pressed) {
            print("Button is currently depressed");
        }
        if(raycastResolution != _oldRaycastResolution) {
            _oldRaycastResolution = raycastResolution;
            _raycasts.Clear();
            interpolateRaycasts();
        }
    }

    private bool checkForCollisions() {
        foreach(Vector3 origin in _raycasts) {
            RaycastHit hit;
            if(Physics.Raycast(origin, Vector3.up, out hit, raycastHeight, acceptableMask)) {
                return true;
            }
        }
        return false;
    }

    private void interpolateRaycasts() {
        for(int i = 0; i < _corners.Length; i++) {
            Vector3 cornerFrom = _corners[i];
            Vector3 cornerTo = _corners[(i+1) % _corners.Length];

            float distance = Vector3.Distance(cornerFrom, cornerTo);
            float step = distance / raycastResolution;

            Vector3 direction = (cornerTo - cornerFrom).normalized;
            for(int j = 0; j < raycastResolution; j++) {
                Vector3 stepCast = cornerFrom + direction * step * j;
                _raycasts.Add(stepCast);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        foreach(Vector3 corner in _raycasts) {
            Gizmos.DrawLine(corner, corner + Vector3.up * raycastHeight);
        } 
    }
    


}


