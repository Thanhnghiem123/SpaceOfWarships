using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemTags; // Danh sách các tag c?a v?t ph?m
    [SerializeField] private float spawnInterval = 4f; // Kho?ng th?i gian gi?a các l?n spawn
    private float minX, maxX;
    private float padding = 0.5f; // Kho?ng cách v?i rìa màn hình
    private ObjectPooler objectPooler;

    // Start is called before the first frame update
    void Start()
    {
        // Tính toán ph?m vi di chuy?n d?a trên camera chính
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        minX = -screenBounds.x + padding;
        maxX = screenBounds.x - padding;

        objectPooler = ObjectPooler.Instance; // L?y instance c?a ObjectPooler

        // B?t ??u vòng l?p spawn v?t ph?m
        StartCoroutine(SpawnItem());
    }

    private IEnumerator SpawnItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomItem();
        }
    }

    private void SpawnRandomItem()
    {
        if (itemTags.Count == 0)
        {
            Debug.LogWarning("No items to spawn");
            return;
        }

        // Ch?n m?t tag v?t ph?m ng?u nhiên t? danh sách
        int randomIndex = Random.Range(0, itemTags.Count);
        string itemTag = itemTags[randomIndex].tag;

        // Ch?n m?t v? trí ng?u nhiên trong kho?ng minX và maxX
        float spawnX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, transform.position.z);

        // Spawn v?t ph?m t? pool t?i v? trí spawn
        objectPooler.SpawnFromPool(itemTag, spawnPosition, Quaternion.identity);
    }
}
