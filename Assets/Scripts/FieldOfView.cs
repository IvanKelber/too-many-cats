using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    public float viewRadius = 10.0f;
    [Range(0,360)]
    public float viewAngle = 110.0f;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    List<Transform> FindVisibleTargets() {
        List<Transform> visibleObjects = new List<Transform>();
        Collider[] objectsInRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for(int i = 0; i < objectsInRadius.Length; i++) {
            Transform target = objectsInRadius[i].transform;
            Vector3 direction = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, direction) < viewAngle/2) {
            RaycastHit hit;
                if(!Physics.Raycast(transform.position, direction, out hit, viewRadius, obstacleMask)) {
                    visibleObjects.Add(target);
                    
                    CatAI cat = objectsInRadius[i].gameObject.GetComponent<CatAI>();
                    if(cat != null) {
                        //cat.StopWalking();
                    }
                    
                }
            }
           
        }
        return visibleObjects;
    }

    void OnDrawGizmos() {
        List<Transform> visibleObjects = FindVisibleTargets();
        foreach(Transform t in visibleObjects) {
            Gizmos.DrawLine(transform.position, t.position);
        }
    }
}
