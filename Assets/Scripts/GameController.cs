using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public bool isTesting = false;
    [Range(0,50)]
    public float startingDistance = 10;
    [Range(0,5)]
    public float startingNoise = 1;

    public Transform catsParent;
    public GameObject playerPrefab;

    private List<Transform> _cats = new List<Transform>();
    private Vector3 _playerPosition;

    private GameObject currentPlayer;

    void Awake() {
        currentPlayer = Instantiate(playerPrefab, transform);
        currentPlayer.GetComponent<PlayerBehavior>().setGameController(this);
        updatePlayerPosition();
        for(int i = 0; i < catsParent.transform.childCount; i++) {
            Transform cat = catsParent.GetChild(i);
            cat.gameObject.GetComponent<CatAI>().setTarget(currentPlayer.transform);
            cat.position = catStartingPosition(cat.position.y);
            _cats.Add(cat);
        }
    }

    // Start is called before the first frame update
    void Start()
    {  

        // updatePlayerPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(isTesting && Input.GetButtonDown("Jump")) {
            updatePlayerPosition();
            StartCoroutine(DisperseCats());
        }
    }

    public void setPlayerView(PlayerBehavior newPlayer) {

    }


    IEnumerator DisperseCats() {
        foreach(Transform cat in _cats) {
            cat.Translate(directionFromPlayer(cat) * getNoisyDistance(), Space.World);
        }
        yield return null;
    }

    Vector3 catStartingPosition(float catPositionY) {
        float angle = Random.Range(0,360);
        float radius = getNoisyDistance();

        Vector3 offset = new Vector3(Mathf.Sin(angle), catPositionY, Mathf.Cos(angle)).normalized * radius;
        return _playerPosition + offset;
    }

    float getNoisyDistance() {
        return startingDistance + Random.Range(0, startingNoise * 2) - startingNoise;
    }

    Vector3 directionFromPlayer(Transform cat) {
        //Only consider the horizontal direction between the cat and the cat's target (the player)
        updatePlayerPosition();
        return (cat.position - _playerPosition).normalized;
    }

    private void updatePlayerPosition() {
        if(currentPlayer && _cats.Count > 0) {
            Transform cat = _cats[0];
            _playerPosition = new Vector3(currentPlayer.transform.position.x, cat.position.y, currentPlayer.transform.position.z);
        }
    }

    private void OnDrawGizmos() {
        if(isTesting) {
            Gizmos.color = Color.red;
            foreach(Transform cat in _cats) {
                Gizmos.DrawLine(cat.position, cat.position + directionFromPlayer(cat) * startingDistance);
            }
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_playerPosition, startingDistance);
        }
    }

}
