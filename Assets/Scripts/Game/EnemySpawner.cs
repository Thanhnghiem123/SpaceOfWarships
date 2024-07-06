using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import th? vi?n ?? qu?n lý các scene

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyTags; // Danh sách các tag cho các lo?i k? thù khác nhau
    public float spawnRate = 2.0f;
    public float spawnRangeX = 8.0f;

    private ObjectPooler objectPooler;
    private bool spawning = true; // Bi?n theo dõi tr?ng thái t?o k? thù

    void Start()
    {
        objectPooler = ObjectPooler.Instance; // L?y instance c?a ObjectPooler

        InvokeRepeating("SpawnEnemy", spawnRate, spawnRate);
        Invoke("StopSpawning", 25f); // D?ng t?o k? thù sau 50 giây
    }

    void SpawnEnemy()
    {
        if (!spawning) return; // Ki?m tra n?u ?ang d?ng t?o k? thù

        if (enemyTags.Count == 0)
        {
            Debug.LogWarning("No enemy tags specified for spawning.");
            return;
        }

        // Ch?n ng?u nhiên m?t tag t? danh sách
        string randomTag = enemyTags[Random.Range(0, enemyTags.Count)].tag;

        Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), transform.position.y, transform.position.z);

        // Spawn ??i t??ng t? object pooler
        objectPooler.SpawnFromPool(randomTag, spawnPosition, transform.rotation);
    }

    void StopSpawning()
    {
        spawning = false; // C?p nh?t tr?ng thái ?? d?ng t?o k? thù
        CancelInvoke("SpawnEnemy"); // H?y vi?c g?i l?p l?i c?a SpawnEnemy

        StartCoroutine(CheckEnemiesRemaining()); // B?t ??u ki?m tra s? l??ng k? thù còn l?i
    }

    // Kiem tra con enemy nao khong
    IEnumerator CheckEnemiesRemaining()
    {
        while (true)
        {
            // ??m s? l??ng k? thù trong c?nh
            int enemyCount = CountEnemies();
            Debug.Log(enemyCount);
            // Ki?m tra n?u không còn k? thù nào
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
                yield break; // Thoát coroutine
            }

            // ??i 1 giây tr??c khi ki?m tra l?i
            yield return new WaitForSeconds(2.0f);
        }
    }

    // Dem so enemy
    int CountEnemies()
    {
        // ??m t?t c? các ??i t??ng có tag là k? thù
        int count = 0;
        foreach (GameObject tag in enemyTags)
        {
            count += GameObject.FindGameObjectsWithTag(tag.tag).Length;
        }
        return count;
    }

    

}
