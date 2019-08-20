using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public bool isTesting = false;

    public TargetedPlayer targetedPlayer;

    // Players
    public GameObject playerPrefab;
    [Range(0,50)]
    public float playerRadius = 20;
    [Range(1,10)]
    public int numPlayers = 1;
    private List<PlayerBehavior> _players = new List<PlayerBehavior>();

    // Cats
    public GameObject catPrefab;
    [Range(0,50)]
    public float catRadius = 10;
    [Range(1,10)]
    public int numCats = 1;  
    private List<CatAI> _cats = new List<CatAI>();

    void Awake() {
        spawnPlayers(numPlayers);
        spawnCats(numCats);
        if(_players.Count > 0) {
            setTargetedPlayer(_players[0]);
        } else {
            throw new System.NullReferenceException("No player behaviors in scene to target.");
        }
    }

    // Spawns a group of static players that are playerRadius away from the center.
    private void spawnPlayers(int numberOfPlayers) {
        float playerDistance = 360.0f/numberOfPlayers;
        float currentAngle = Random.Range(0,360);
        for(int i = 0; i < numberOfPlayers; i++) {
            float radAngle = currentAngle * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Sin(radAngle)*playerRadius, 2, Mathf.Cos(radAngle)*playerRadius);
            GameObject player = Instantiate(playerPrefab, position, Quaternion.identity, transform);
            PlayerBehavior playerBehavior = player.GetComponent<PlayerBehavior>();
            _players.Add(playerBehavior);
            currentAngle += playerDistance;
        }
    }
    
    // Sets the targetedPlayer object to the current player.  
    // Also ensures that the targetedPlayer's game controller is set for spotplayer to work properly.
    private void setTargetedPlayer(PlayerBehavior newPlayer) {
        targetedPlayer.SetNewPlayer(newPlayer);
    }

    // Spawns cats in a random location catRadius units away from the initial targeted player
    private void spawnCats(int numberOfCats) {
        float catDistance = 360.0f/numberOfCats;
        float currentAngle = Random.Range(0,360);
        for(int i = 0; i < numberOfCats; i++) {
            float radAngle = currentAngle * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Sin(radAngle)*catRadius, 2, Mathf.Cos(radAngle)*catRadius);
            GameObject catObj = Instantiate(catPrefab, position, Quaternion.identity, transform);
            CatAI cat = catObj.GetComponent<CatAI>();
            cat.setTargetedPlayer(targetedPlayer);
            _cats.Add(cat);
            currentAngle += catDistance;
        }
    }

    //Uses the height of the starting transform so that the resulting vector is flat.
    private Vector3 normalizedDirection(Vector3 start, Vector3 end) {
        float height = start.y;
        return (new Vector3(end.x, height, end.z) - start).normalized;
    }

    private void OnDrawGizmos() {
        if(isTesting) {
            Gizmos.color = Color.red;
            Vector3 playerPosition = targetedPlayer.GetPosition();
            foreach(CatAI cat in _cats) {
                Gizmos.DrawLine(cat.transform.position, cat.transform.position + normalizedDirection(cat.transform.position, playerPosition) * catRadius);
            }
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(playerPosition, catRadius);
        }
    }



}
