using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 20f;
    private Vector3 velocity;

    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float dx = Input.GetAxisRaw("Horizontal");
        float dy = Input.GetAxisRaw("Vertical");
        velocity = transform.forward * dy + transform.right * dx;
        velocity = transform.TransformDirection(velocity.normalized * speed);
    }

    void FixedUpdate() {
        rigidbody.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
    }
}
