using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAI : MonoBehaviour
{   
    public TargetedPlayer targetedPlayer;

    [SerializeField]
    public float speed = 5.0f;
    public LayerMask targetLayerMask;
    public Material[] possibleMaterials;
    public Camera playerCam;

    private Animator _animator;
    private bool inView = false;
    private bool reachedTarget = false;
    private Rigidbody _body;
    private SkinnedMeshRenderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        if(possibleMaterials.Length > 0) {
            Component[] renderers = GetComponentsInChildren(typeof(SkinnedMeshRenderer));
            if(renderers.Length == 1) {
                _renderer = renderers[0] as SkinnedMeshRenderer;
                _renderer.material = possibleMaterials[UnityEngine.Random.Range(0,possibleMaterials.Length)];
            } else {
                throw new System.IndexOutOfRangeException("Why is there more or less than 1 SkinnedMeshRenderer: " + renderers.Length);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = targetedPlayer.GetPosition();
        Vector3 targetPosition = new Vector3(target.x, transform.position.y, target.z);
        Vector3 targetDirection = (targetPosition - transform.position).normalized;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = rotation;

        reachedTarget = CollisionRay(targetDirection);
        inView = _renderer.IsVisibleFrom(targetedPlayer.cameraHelper.camera);
        bool walking = !reachedTarget && !inView;
        if(walking && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Cat_Sit")) {
            _body.MovePosition(transform.position + targetDirection * speed * Time.deltaTime);
        }
        _animator.SetBool("isWalking", walking);
    }

    bool CollisionRay(Vector3 direction) {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, 3, targetLayerMask)) {
            //To do other stuff in here.
            // Like some animation
            return true;
        }
        return false;
    }

    public void setTargetedPlayer(TargetedPlayer targetedPlayer) {
        this.targetedPlayer = targetedPlayer;
    }

    public Vector3 getTarget() {
        return targetedPlayer.GetPosition();
    }

}
