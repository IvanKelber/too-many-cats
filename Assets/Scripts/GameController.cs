using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public bool isTesting = false;
    [Range(0,50)]
    public float catRadius = 10;
    [Range(0,50)]
    public float playerRadius = 20;
    [Range(0,5)]
    public float startingNoise = 1;

    public Transform catsParent;
    public GameObject playerPrefab;
    public GameObject catPrefab;
    [Range(1,10)]
    public int numPlayers = 1;

    private List<Transform> _cats = new List<Transform>();
    private int _currentPlayerIndex;

    private Vector3 _playerPosition;

    private GameObject _currentPlayer;

    private List<Transform> _players = new List<Transform>();

    void Awake() {
        spawnPlayers(numPlayers);
        // _currentPlayer = Instantiate(playerPrefab, transform);
        // _currentPlayer.GetComponent<PlayerBehavior>().setGameController(this);
        // updatePlayerPosition();
        // for(int i = 0; i < catsParent.transform.childCount; i++) {
        //     Transform cat = catsParent.GetChild(i);
        //     cat.gameObject.GetComponent<CatAI>().setTarget(_currentPlayer.transform);
        //     cat.position = catStartingPosition(cat.position.y);
        //     _cats.Add(cat);
        // }
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
        newPlayer.GetComponent<PlayerBehavior>().turnOn();
        newPlayer.deselect();
        _currentPlayer.GetComponent<PlayerBehavior>().turnOff();
        _currentPlayer = newPlayer.gameObject;
        updatePlayerPosition();
        updateCats();
    }

    private void spawnPlayers(int numberOfPlayers) {
        float playerDistance = 360.0f/numberOfPlayers;
        print(playerDistance);
        float currentAngle = 0;
        for(int i = 0; i < numberOfPlayers; i++) {
            float radAngle = currentAngle * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Sin(radAngle)*playerRadius, 2, Mathf.Cos(radAngle)*playerRadius);
            GameObject player = Instantiate(playerPrefab, position, Quaternion.identity, transform);
            _players.Add(player.transform);
            currentAngle += playerDistance;
        }
        
    }

    private void spawnCats(int numberOfCats) {

    }

    void updateCats() {
        foreach(Transform cat in _cats) {
            cat.GetComponent<CatAI>().setTarget(_currentPlayer.transform);
        }
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
        return catRadius + Random.Range(0, startingNoise * 2) - startingNoise;
    }

    Vector3 directionFromPlayer(Transform cat) {
        //Only consider the horizontal direction between the cat and the cat's target (the player)
        updatePlayerPosition();
        return (cat.position - _playerPosition).normalized;
    }

    private void updatePlayerPosition() {
        if(_currentPlayer && _cats.Count > 0) {
            Transform cat = _cats[0];
            _playerPosition = new Vector3(_currentPlayer.transform.position.x, cat.position.y, _currentPlayer.transform.position.z);
        }
    }

    private void OnDrawGizmos() {
        if(isTesting) {
            Gizmos.color = Color.red;
            foreach(Transform cat in _cats) {
                Gizmos.DrawLine(cat.position, cat.position + directionFromPlayer(cat) * catRadius);
            }
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_playerPosition, catRadius);
        }
    }

}
