using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private GameObject[] _players;
    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerupContainer;
    [SerializeField]
    public GameObject _playerContainer;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        print("Starting " + Time.time);
        SpawnPlayers();
    }

    public void StartEnemySpawn()
    {
        StartCoroutine(SpawnEnemyRoutine(1.5f));
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine(float waitTime)
    {
        yield return new WaitForSeconds(3);
        float randomXPosition = Random.Range(-9.1f, 10.1f);

        while (_stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, transform.position = new Vector3(randomXPosition, 5.9f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSecondsRealtime(3);

        int spawnTime = Random.Range(3, 8);
        float randomXPosition = Random.Range(-9.1f, 10.1f);

        while (_stopSpawning == false)
        {
            int randomPowerUp = Random.Range(0, 3);
            GameObject newPowerUp = Instantiate(powerups[randomPowerUp], new Vector3(randomXPosition, 9.9f, 0), Quaternion.identity);
            newPowerUp.transform.parent = _powerupContainer.transform;
            yield return new WaitForSecondsRealtime(spawnTime);
        }
    }

    public void SpawnPlayers()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Single_Player")
        {
            Vector3 playerOnePosition = new Vector3(-2.9f, -3.3f, 0);
            _players[0] = Instantiate(_playerPrefab, playerOnePosition, Quaternion.identity);
            _players[0].GetComponent<Player>()._playerId = 0;
            _players[0].transform.parent = _playerContainer.transform;
        }

        if (currentScene.name == "Co-Op_Mode")
        {
            Vector3 playerOnePosition = new Vector3(-8.9f, -3.3f, 0);
            _players[0] = Instantiate(_playerPrefab, playerOnePosition, Quaternion.identity);
            _players[0].GetComponent<Player>()._playerId = 0;
            _players[0].transform.parent = _playerContainer.transform;

            Vector3 playerTwoPosition = new Vector3(8.2f, -3.3f, 0);
            _players[1] = Instantiate(_playerPrefab, playerTwoPosition, Quaternion.identity);
            _players[1].GetComponent<Player>()._playerId = 1;
            _players[1].transform.parent = _playerContainer.transform;
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}