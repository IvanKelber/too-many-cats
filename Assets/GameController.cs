using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public bool isTesting = false;
    [Range(0,50)]
    public float startingDistance = 10;
    public float startingNoise = 1;

    public Transform catsParent;

    private List<Transform> _cats = new List<Transform>();
    private Vector3 _playerPosition;

    // Start is called before the first frame update
    void Start()
    {  
        startingNoise = Mathf.Clamp(0, startingDistance/2, startingNoise);
        for(int i = 0; i < catsParent.transform.childCount; i++) {
            Transform cat = catsParent.GetChild(i);
            cat.position = catStartingPosition();
            _cats.Add(cat);
            if(_playerPosition == null) {
                Vector3 position = cat.GetComponent<CatAI>().target.position;
                _playerPosition = new Vector3(position.x, cat.position.y, position.z);
            }
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

    Vector3 catStartingPosition() {
        float angle = Random.Range(0,360);
        float radius = startingDistance + Random.Range(0, startingNoise * 2) - startingNoise;

        Vector3 offset = new Vector3(Mathf.Cos(angle), _playerPosition.y, Mathf.Sin(angle)).normalized * radius;
        print(_playerPosition + offset);
        print( (_playerPosition + offset).magnitude);
        return _playerPosition + offset;
    }

    Vector3 directionFromPlayer(Transform cat) {
        //Only consider the horizontal direction between the cat and the cat's target (the player)
        return (cat.position - _playerPosition).normalized;
    }

    private void OnDrawGizmos() {
        //print(_cats.Count);
        Gizmos.color = Color.red;
        foreach(Transform cat in _cats) {
            Gizmos.DrawLine(cat.position, cat.position + directionFromPlayer(cat) * startingNoise);
        }
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_playerPosition, startingDistance);
    }
}
