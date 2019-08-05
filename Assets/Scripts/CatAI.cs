using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAI : MonoBehaviour
{
    public Transform target;
    public float speed = 5.0f;
    public LayerMask targetLayerMask;

    private bool inView = false;
    private bool reachedTarget = false;
    private Rigidbody rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = (target.position - transform.position).normalized;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = rotation;

        reachedTarget = CollisionRay(targetDirection);
        inView = GetComponent<Renderer>().IsVisibleFrom(Camera.main);
        if(!reachedTarget && !inView) {
            rigidbody.MovePosition(transform.position + targetDirection * speed * Time.deltaTime);
        }
    }

    bool CollisionRay(Vector3 direction) {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, 2, targetLayerMask)) {
            //To do other stuff in here.
            // Like some animation
            return true;
        }
        return false;
    }

}
