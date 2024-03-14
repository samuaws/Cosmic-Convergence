using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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
    public GameObject waveEndedUI;
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI enemyStatsText;
    public TextMeshProUGUI hudWave;
    private int totalEnemies;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }
    void Update()
    {
        enemyStatsText.text = (totalEnemies - activeEnemies.Count).ToString() + "/" + totalEnemies.ToString();
        hudWave.text = "Wave " + currentWave.ToString();
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        while (true)
        {
            currentWave++;
            Debug.Log("Wave " + currentWave + " started!");

            isWaveInProgress = true;

            if (currentWave == 1) // First wave only spawns melee enemies
            {
                SpawnFirstWave();
            }
            else // Subsequent waves spawn both melee and ranged enemies
            {
                SpawnEnemies();
            }

            yield return new WaitUntil(() => AllEnemiesDestroyed());
            isWaveInProgress = false;
            int nextwave = currentWave + 1;
            waveNumberText.text = nextwave.ToString();
            waveEndedUI.SetActive(true);
            yield return new WaitForSeconds(timeBetweenWaves);
            waveEndedUI.SetActive(false);
        }
    }

    void SpawnFirstWave()
    {
        activeEnemies.Clear(); // Clear the list before spawning new enemies
        int numSpawnPoints = Mathf.Min(currentWave + 6, spawnPoints.Count); // Determine number of spawn points for this wave
        totalEnemies = numSpawnPoints;
        enemyStatsText.gameObject.SetActive(true);
        for (int i = 0; i < numSpawnPoints; i++)
        {
            GameObject enemy = Instantiate(meleeEnemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            activeEnemies.Add(enemy); // Add spawned enemy to the list
        }
    }

    void SpawnEnemies()
    {
        activeEnemies.Clear(); // Clear the list before spawning new enemies
        bool spawnRangedEnemy = true; // Flag to alternate between ranged and non-ranged enemies

        int numSpawnPoints = Mathf.Min(currentWave + 6, spawnPoints.Count); // Determine number of spawn points for this wave
        totalEnemies = numSpawnPoints;
        for (int i = 0; i < numSpawnPoints; i++)
        {
            for (int j = 0; j < enemiesPerWave; j++)
            {
                GameObject enemyPrefab = spawnRangedEnemy ? rangedEnemyPrefab : meleeEnemyPrefab;
                GameObject enemy = Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
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
