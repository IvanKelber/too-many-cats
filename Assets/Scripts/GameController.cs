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

    private Vector3 _playerPosition;

    private List<PlayerBehavior> _players = new List<PlayerBehavior>();
    private int _playerIndex = 0;

    void Awake() {
        spawnPlayers(numPlayers);

        updatePlayerPosition();
        for(int i = 0; i < catsParent.transform.childCount; i++) {
            Transform cat = catsParent.GetChild(i);
            cat.gameObject.GetComponent<CatAI>().setTarget(_players[_playerIndex].transform);
            cat.position = catStartingPosition(cat.position.y);
            _cats.Add(cat);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isTesting && Input.GetButtonDown("Jump")) {
            updatePlayerPosition();
            StartCoroutine(DisperseCats());
        }
    }

    public void setPlayerView(int newPlayerIndex) {
        PlayerBehavior newPlayer = _players[newPlayerIndex];
        newPlayer.turnOn();
        newPlayer.deselect();
        newPlayer.setGameController(this);
        _players[_playerIndex].turnOff();
        _playerIndex = newPlayerIndex;
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
            PlayerBehavior playerBehavior = player.GetComponent<PlayerBehavior>();
            playerBehavior.setIndex(i);
            _players.Add(playerBehavior);
            currentAngle += playerDistance;
        }
        _players[_playerIndex].turnOn();
        _players[_playerIndex].setGameController(this);
    }

    private void spawnCats(int numberOfCats) {

    }

    void updateCats() {
        foreach(Transform cat in _cats) {
            cat.GetComponent<CatAI>().setTarget(_players[_playerIndex].transform);
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
        if(_cats.Count > 0) {
            Transform cat = _cats[0];
            Vector3 playerPosition = _players[_playerIndex].transform.position;
            _playerPosition = new Vector3(playerPosition.x, cat.position.y, playerPosition.z);
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
