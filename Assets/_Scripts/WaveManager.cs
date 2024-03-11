using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public GameObject rangedEnemyPrefab; // The prefab of the ranged enemy to spawn
    public GameObject meleeEnemyPrefab; // The prefab of the melee (non-ranged) enemy to spawn
    public List<Transform> spawnPoints; // The points where enemies will spawn
    public float timeBetweenWaves = 10f; // Time between each wave
    public int enemiesPerWave = 5; // Number of enemies to spawn per wave
    public float timeBetweenEnemies = 1f; // Time between each enemy spawn within a wave

    private int currentWave = 0; // Current wave number
    private bool isWaveInProgress = false; // Flag to track if a wave is currently in progress
    private List<GameObject> activeEnemies = new List<GameObject>(); // List to store active enemies

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        while (true)
        {
            currentWave++;
            Debug.Log("Wave " + currentWave + " started!");

            isWaveInProgress = true;
            SpawnEnemies();
            yield return new WaitUntil(() => AllEnemiesDestroyed());
            isWaveInProgress = false;

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemies()
    {
        activeEnemies.Clear(); // Clear the list before spawning new enemies
        bool spawnRangedEnemy = true; // Flag to alternate between ranged and non-ranged enemies
        foreach (Transform spawnPoint in spawnPoints)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                GameObject enemyPrefab = spawnRangedEnemy ? rangedEnemyPrefab : meleeEnemyPrefab;
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                activeEnemies.Add(enemy); // Add spawned enemy to the list
                spawnRangedEnemy = !spawnRangedEnemy; // Switch between ranged and non-ranged enemies
            }
        }
    }

    bool AllEnemiesDestroyed()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null) // Check if enemy still exists
                return false;
        }
        return true;
    }

    // You can use this method to remove destroyed enemies from the list
    public void RemoveEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
    }

    public bool IsWaveInProgress()
    {
        return isWaveInProgress;
    }

    public int GetCurrentWaveNumber()
    {
        return currentWave;
    }
}
