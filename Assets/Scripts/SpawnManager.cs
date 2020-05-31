using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] powerups;

    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerupContainer;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        print("Starting " + Time.time);
    }

    public void StartSpawning()
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

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}