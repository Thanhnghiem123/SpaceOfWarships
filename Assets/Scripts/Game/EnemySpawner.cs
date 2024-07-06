using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import th? vi?n ?? qu?n l� c�c scene

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyTags; // Danh s�ch c�c tag cho c�c lo?i k? th� kh�c nhau
    public float spawnRate = 2.0f;
    public float spawnRangeX = 8.0f;

    private ObjectPooler objectPooler;
    private bool spawning = true; // Bi?n theo d�i tr?ng th�i t?o k? th�

    void Start()
    {
        objectPooler = ObjectPooler.Instance; // L?y instance c?a ObjectPooler

        InvokeRepeating("SpawnEnemy", spawnRate, spawnRate);
        Invoke("StopSpawning", 25f); // D?ng t?o k? th� sau 50 gi�y
    }

    void SpawnEnemy()
    {
        if (!spawning) return; // Ki?m tra n?u ?ang d?ng t?o k? th�

        if (enemyTags.Count == 0)
        {
            Debug.LogWarning("No enemy tags specified for spawning.");
            return;
        }

        // Ch?n ng?u nhi�n m?t tag t? danh s�ch
        string randomTag = enemyTags[Random.Range(0, enemyTags.Count)].tag;

        Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), transform.position.y, transform.position.z);

        // Spawn ??i t??ng t? object pooler
        objectPooler.SpawnFromPool(randomTag, spawnPosition, transform.rotation);
    }

    void StopSpawning()
    {
        spawning = false; // C?p nh?t tr?ng th�i ?? d?ng t?o k? th�
        CancelInvoke("SpawnEnemy"); // H?y vi?c g?i l?p l?i c?a SpawnEnemy

        StartCoroutine(CheckEnemiesRemaining()); // B?t ??u ki?m tra s? l??ng k? th� c�n l?i
    }

    // Kiem tra con enemy nao khong
    IEnumerator CheckEnemiesRemaining()
    {
        while (true)
        {
            // ??m s? l??ng k? th� trong c?nh
            int enemyCount = CountEnemies();
            Debug.Log(enemyCount);
            // Ki?m tra n?u kh�ng c�n k? th� n�o
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
                yield break; // Tho�t coroutine
            }

            // ??i 1 gi�y tr??c khi ki?m tra l?i
            yield return new WaitForSeconds(2.0f);
        }
    }

    // Dem so enemy
    int CountEnemies()
    {
        // ??m t?t c? c�c ??i t??ng c� tag l� k? th�
        int count = 0;
        foreach (GameObject tag in enemyTags)
        {
            count += GameObject.FindGameObjectsWithTag(tag.tag).Length;
        }
        return count;
    }

    

}
