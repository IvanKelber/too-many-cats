using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public bool isTesting = false;

    // Players
    public GameObject playerPrefab;
    [Range(0,50)]
    public float playerRadius = 20;
    [Range(1,10)]
    public int numPlayers = 1;
    private List<PlayerBehavior> _players = new List<PlayerBehavior>();
    private int _playerIndex = 0;
    private Vector3 _playerPosition;


    // Cats
    public GameObject catPrefab;
    [Range(0,50)]
    public float catRadius = 10;
    [Range(1,10)]
    public int numCats = 1;  
    private List<CatAI> _cats = new List<CatAI>();

    void Awake() {
        spawnPlayers(numPlayers);
        updatePlayerPosition();
        spawnCats(numCats);
    }

    // Update is called once per frame
    void Update()
    {
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
        float currentAngle = Random.Range(0,360);
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
        float catDistance = 360.0f/numberOfCats;
        float currentAngle = Random.Range(0,360);
        for(int i = 0; i < numberOfCats; i++) {
            float radAngle = currentAngle * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Sin(radAngle)*catRadius, 2, Mathf.Cos(radAngle)*catRadius);
            GameObject catObj = Instantiate(catPrefab, position, Quaternion.identity, transform);
            CatAI cat = catObj.GetComponent<CatAI>();
            cat.setTarget(_players[_playerIndex].transform);
            _cats.Add(cat);
            currentAngle += catDistance;
        }
    }

    void updateCats() {
        foreach(CatAI cat in _cats) {
           cat.setTarget(_players[_playerIndex].transform);
        }
    }

    Vector3 directionFromPlayer(Transform cat) {
        //Only consider the horizontal direction between the cat and the cat's target (the player)
        updatePlayerPosition();
        return (cat.position - _playerPosition).normalized;
    }

    private void updatePlayerPosition() {
        if(_cats.Count > 0) {
            Transform cat = _cats[0].transform;
            Vector3 playerPosition = _players[_playerIndex].transform.position;
            _playerPosition = new Vector3(playerPosition.x, cat.position.y, playerPosition.z);
        }
    }

    private void OnDrawGizmos() {
        if(isTesting) {
            Gizmos.color = Color.red;
            foreach(CatAI cat in _cats) {
                Gizmos.DrawLine(cat.transform.position, cat.transform.position + directionFromPlayer(cat.transform) * catRadius);
            }
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_playerPosition, catRadius);
        }
    }

}
