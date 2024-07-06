using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemTags; // Danh s�ch c�c tag c?a v?t ph?m
    [SerializeField] private float spawnInterval = 4f; // Kho?ng th?i gian gi?a c�c l?n spawn
    private float minX, maxX;
    private float padding = 0.5f; // Kho?ng c�ch v?i r�a m�n h�nh
    private ObjectPooler objectPooler;

    // Start is called before the first frame update
    void Start()
    {
        // T�nh to�n ph?m vi di chuy?n d?a tr�n camera ch�nh
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        minX = -screenBounds.x + padding;
        maxX = screenBounds.x - padding;

        objectPooler = ObjectPooler.Instance; // L?y instance c?a ObjectPooler

        // B?t ??u v�ng l?p spawn v?t ph?m
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

        // Ch?n m?t tag v?t ph?m ng?u nhi�n t? danh s�ch
        int randomIndex = Random.Range(0, itemTags.Count);
        string itemTag = itemTags[randomIndex].tag;

        // Ch?n m?t v? tr� ng?u nhi�n trong kho?ng minX v� maxX
        float spawnX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, transform.position.z);

        // Spawn v?t ph?m t? pool t?i v? tr� spawn
        objectPooler.SpawnFromPool(itemTag, spawnPosition, Quaternion.identity);
    }
}
