using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner2 : MonoBehaviour
{
    public List<GameObject> enemyTags; // Danh s�ch c�c tag cho c�c lo?i k? th� kh�c nhau
    public float spawnRate = 2.0f;
    public float spawnRangeX = 8.0f;
    public float minDistanceBetweenEnemies = 3.0f; // Kho?ng c�ch t?i thi?u gi?a c�c enemy

    private ObjectPooler objectPooler;
    private bool spawning = true; // Bi?n theo d�i tr?ng th�i t?o k? th�
    private List<Vector3> spawnPositions = new List<Vector3>(); // Danh s�ch v? tr� c?a c�c k? th� ?� spawn
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
            Invoke("StopSpawning", 0f); // D?ng t?o k? th� sau 20 gi�y
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
