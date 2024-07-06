using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public float speed = 2.0f;
    public float fireRate = 0.5f;
    private float nextFire = 0.0f;
    private ObjectPooler objectPooler;
    public GameObject bulletEnemyPrefab; // Prefab c?a ??n k? thù
    public Transform bulletSpawnPoint;
    private float minX, maxX, minY, maxY, padding = 1f;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;

        // Ki?m tra xem bulletSpawnPoint ?ã ???c gán ch?a
        if (bulletSpawnPoint == null)
        {
            Debug.LogError("bulletSpawnPoint ch?a ???c gán trong Inspector");
        }

        // Ki?m tra xem bulletEnemyPrefab có null không
        if (bulletEnemyPrefab == null)
        {
            Debug.LogError("bulletEnemyPrefab ch?a ???c gán trong Inspector");
        }

        // Tính toán gi?i h?n màn hình
        Camera cam = Camera.main;
        Vector3 screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        minX = -screenBounds.x - padding;
        maxX = screenBounds.x + padding;
        minY = -screenBounds.y - padding;
        maxY = screenBounds.y + padding;
    }

    void Update()
    {
        // Di chuy?n enemy xu?ng d??i
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Ki?m tra th?i gian ?? b?n viên ??n ti?p theo
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            FireBullet();
        }

        // Ki?m tra n?u enemy ra kh?i gi?i h?n màn hình và tr? v? pool
        if (transform.position.x < minX || transform.position.x > maxX || transform.position.y < minY || transform.position.y > maxY)
        {
            objectPooler.ReturnToPool(gameObject);
        }
    }

    void FireBullet()
    {
        // Ki?m tra xem bulletSpawnPoint ?ã ???c gán ch?a
        if (bulletSpawnPoint != null)
        {
            // Ki?m tra xem bulletEnemyPrefab có null không tr??c khi spawn ??n
            if (bulletEnemyPrefab != null)
            {
                objectPooler.SpawnFromPool(bulletEnemyPrefab.tag, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            }
            else
            {
                Debug.LogError("bulletEnemyPrefab ch?a ???c gán trong Inspector");
            }
        }
        else
        {
            Debug.LogError("bulletSpawnPoint ch?a ???c gán trong Inspector");
        }
    }
}
