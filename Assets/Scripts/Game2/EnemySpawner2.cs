using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner2 : MonoBehaviour
{
    public List<GameObject> enemyTags; // Danh sách các tag cho các lo?i k? thù khác nhau
    public float spawnRate = 2.0f;
    public float spawnRangeX = 8.0f;
    public float minDistanceBetweenEnemies = 3.0f; // Kho?ng cách t?i thi?u gi?a các enemy

    private ObjectPooler objectPooler;
    private bool spawning = true; // Bi?n theo dõi tr?ng thái t?o k? thù
    private List<Vector3> spawnPositions = new List<Vector3>(); // Danh sách v? trí c?a các k? thù ?ã spawn
    private GameObject boss;
    private int currentScene;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        objectPooler = ObjectPooler.Instance; // L?y instance c?a ObjectPooler

        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler instance not found.");
            return;
        }

        InvokeRepeating("SpawnEnemies", spawnRate, spawnRate);
        

        Debug.Log("Spawning started, will stop in 30 seconds.");
        currentScene = SceneManager.GetActiveScene().buildIndex;

        if(currentScene != 4)
        {
            Invoke("StopSpawning", 30f);
        }
    }

    private void Update()
    {
        if (boss == null && currentScene == 4)
            Invoke("StopSpawning", 0f); // D?ng t?o k? thù sau 20 giây
    }

    void SpawnEnemies()
    {
        if (!spawning) return;

        if (enemyTags.Count == 0)
        {
            Debug.LogWarning("No enemy tags specified for spawning.");
            return;
        }

        Debug.Log("Spawning enemies at: " + Time.time);

        foreach (GameObject enemyTag in enemyTags)
        {
            Vector3 spawnPosition;

                spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), transform.position.y, transform.position.z);
                spawnPositions.Add(spawnPosition);
                objectPooler.SpawnFromPool(enemyTag.tag, spawnPosition, transform.rotation);
                Debug.Log("Spawned enemy at position: " + spawnPosition);
        }
    }

    

    void StopSpawning()
    {
        spawning = false;
        CancelInvoke("SpawnEnemies");
        //Debug.Log("Spawning stopped at: " + Time.time);
        StartCoroutine(CheckEnemiesRemaining());
    }

    IEnumerator CheckEnemiesRemaining()
    {
        while (true)
        {
            int enemyCount = CountEnemies();
            Debug.Log("Enemy count: " + enemyCount);

            
            if (enemyCount == 0)
            {
                

                LevelTransition levelTransition = FindObjectOfType<LevelTransition>();
                if (levelTransition != null)
                {
                    levelTransition.StartLevelTransition();
                }
                else
                {
                    Debug.LogError("LevelTransition script is not found in the scene.");
                }
                yield break;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    int CountEnemies()
    {
        int count = 0;
        foreach (GameObject tag in enemyTags)
        {
            count += GameObject.FindGameObjectsWithTag(tag.tag).Length;
        }
        return count;
    }
}
