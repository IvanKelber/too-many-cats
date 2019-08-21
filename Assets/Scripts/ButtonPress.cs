using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{

    public LayerMask acceptableMask;
    [Range(1,100)]
    public int raycastResolution = 1;
    public float raycastHeight = 3;
    public AudioClip soundFX;
    private AudioSource _audioSource;

    private int _oldRaycastResolution;
    private bool _pressed = false;
    private bool _soundPlaying = false;
    private Collider _collider;
    private Vector3[] _corners;
    private List<Vector3> _raycasts = new List<Vector3>();
    private Vector3 _defaultPosition;
    private Vector3 _pressedPosition;

    // Start is called before the first frame update
    void Start()
    {
        _defaultPosition = transform.localScale;
        _pressedPosition = new Vector3(_defaultPosition.x, _defaultPosition.y/2, _defaultPosition.z);
        print(_defaultPosition);
        print(_pressedPosition);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = soundFX;
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

        interpolateRaycasts();
    }

    // Update is called once per frame
    void Update()
    {
        bool newlyPressed = checkForCollisions();
        if(newlyPressed != _pressed) {
            if(newlyPressed) {
                onCollisionEnter();
            } else {
                onCollisionExit();
            }
        }

        // For updating the raycast resolution during gameplay.
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
        // Raycasts on edges:
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
        // Raycasts on diagonals
        for(int i = 0; i < _corners.Length; i++) {
            Vector3 cornerFrom = _corners[i];
            Vector3 cornerTo = _corners[(i+2) % _corners.Length];

            float distance = Vector3.Distance(cornerFrom, cornerTo);
            float step = distance / raycastResolution;

            Vector3 direction = (cornerTo - cornerFrom).normalized;
            for(int j = 0; j < raycastResolution; j++) {
                Vector3 stepCast = cornerFrom + direction * step * j;
                _raycasts.Add(stepCast);
            }
        }

    }

    IEnumerator AnimateButtonDown() {
        float percentage = (transform.localScale.y - _defaultPosition.y)/(_pressedPosition.y - _defaultPosition.y);
        print("Percentage: " + percentage);
        transform.localScale = Vector3.Lerp(_pressedPosition, _defaultPosition, percentage);
        yield return null;
    }

     IEnumerator AnimateButtonUp() {
        float percentage = (transform.localScale.y - _pressedPosition.y)/(_defaultPosition.y - _pressedPosition.y);
        print("Percentage: " + percentage);
        transform.localScale = Vector3.Lerp(_defaultPosition, _pressedPosition, percentage);
        yield return null;
    }

    private void animateButtonDown() {
        transform.localScale = Vector3.Lerp(_defaultPosition, _pressedPosition, Time.deltaTime);
    }

    
    private void animateButtonUp() {
        transform.localScale = Vector3.Lerp(_pressedPosition, _defaultPosition, Time.deltaTime);
    }

    private void onCollisionEnter() {
        print("On collision enter");
        _pressed = true;
        if(!_soundPlaying) {
            StartCoroutine(PlaySound());
            StartCoroutine(AnimateButtonDown());
            // animateButtonDown();
            print(transform.localScale);
        }
    }

    private void onCollisionExit() {
        print("On collision exit");
        _pressed = false;
        StartCoroutine(AnimateButtonUp());

        // animateButtonUp();
        print(transform.localScale);
    }

    IEnumerator PlaySound() {
        print("playing sound");
        _soundPlaying = true;
        _audioSource.Play();
        yield return new WaitForSeconds(_audioSource.clip.length);
        _soundPlaying = false;
    }

    private void OnDrawGizmos() {
        if(_raycasts != null) {
            Gizmos.color = Color.red;
            foreach(Vector3 corner in _raycasts) {
                Gizmos.DrawLine(corner, corner + Vector3.up * raycastHeight);
            } 
        }
    }
    


}


