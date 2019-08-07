using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public bool isTesting = false;
    [Range(0,20)]
    public float startingDistance = 10;
    public float startingNoise = 1;

    public Transform catsParent;

    private List<Transform> _cats = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {  
        startingNoise = Mathf.Clamp(0, startingDistance/2, startingNoise);
        for(int i = 0; i < catsParent.transform.childCount; i++) {
            _cats.Add(catsParent.GetChild(i));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(isTesting && Input.GetButtonDown("Jump")) {
            StartCoroutine(DisperseCats());
        }
    }

    IEnumerator DisperseCats() {
        foreach(Transform cat in _cats) {
            cat.Translate(directionFromPlayer(cat) * startingDistance, Space.World);
        }
        yield return null;
    }

    Vector3 directionFromPlayer(Transform cat) {
        CatAI catAI = cat.GetComponent<CatAI>();
        //Only consider the horizontal direction between the cat and the cat's target (the player)
        Vector3 playerPosition = new Vector3(catAI.target.position.x, cat.position.y, catAI.target.position.z);
        Vector3 direction = (cat.position - playerPosition).normalized;
        return direction;
    }

    private void OnDrawGizmos() {
        //print(_cats.Count);
        Gizmos.color = Color.red;
        foreach(Transform cat in _cats) {
            Gizmos.DrawLine(cat.position, cat.position + directionFromPlayer(cat) * startingNoise);
        }
    }
}
