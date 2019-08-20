using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPlayer : MonoBehaviour
{
    public LayerMask playerMask;
    public float distance = 1000;
    [Range(1, 10)]
    public float fieldOfViewScale = 1;

    private PlayerBehavior _spottedPlayer;
    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerBehavior selected = selectPlayer();
        if(_spottedPlayer != null) {
            _spottedPlayer.deselect();
        } 
        if(selected != null) {
            selected.select();
        }
        _spottedPlayer = selected;

        if(Input.GetButtonDown("Jump") && _spottedPlayer != null) {
            _spottedPlayer.targetSelf();
            _spottedPlayer = null;
        }
    }

    PlayerBehavior selectPlayer() {
        Collider[] others = Physics.OverlapSphere(transform.position, distance, playerMask);
        float closestAngle = 180f;
        Collider closestPlayer = null;
            foreach(Collider other in others) {
                if(other.transform.position != transform.position) {
                    Vector3 dirToOther = (other.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(dirToOther, transform.forward);
                    if(angle < _camera.fieldOfView/(2*fieldOfViewScale) && angle < closestAngle) {
                        closestAngle = angle;
                        closestPlayer = other;
                    }
                }
            }
        if(closestPlayer != null) {
            return closestPlayer.gameObject.GetComponent<PlayerBehavior>();
        }
        return null;
    }

    private void OnDrawGizmos() {
        // Gizmos.color = Color.cyan;
        // if(_camera != null) {
        //     float fieldBound = (_camera.fieldOfView/(2*fieldOfViewScale) + transform.eulerAngles.y) * Mathf.Deg2Rad ;
        //     Vector3 left = new Vector3(transform.forward.x * Mathf.Sin(-fieldBound), transform.forward.y, transform.forward.z * Mathf.Cos(-fieldBound));
        //     Vector3 right = new Vector3(transform.forward.x * Mathf.Sin(fieldBound), transform.forward.y, transform.forward.z * Mathf.Cos(fieldBound));

        //     Gizmos.DrawLine(transform.position, left*distance);
        //     Gizmos.DrawLine(transform.position, right*distance);
        // }
    }
}
