using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAI : MonoBehaviour
{
    public Transform target;
    public float speed = 5.0f;
    public LayerMask targetLayerMask;

    private Animator _animator;
    private bool inView = false;
    private bool reachedTarget = false;
    private Rigidbody _body;


    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 targetDirection = (targetPosition - transform.position).normalized;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = rotation;

        reachedTarget = CollisionRay(targetDirection);
        inView = GetComponent<Renderer>().IsVisibleFrom(Camera.main);
        bool walking = !reachedTarget && !inView;
        if(walking && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Cat_Sit")) {
            _body.MovePosition(transform.position + targetDirection * speed * Time.deltaTime);
        }
        _animator.SetBool("isWalking", walking);
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
