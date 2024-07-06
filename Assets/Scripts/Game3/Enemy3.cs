using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public float speed = 2.0f;
    public float fireRate = 0.5f;
    private float nextFire = 0.0f;
    private ObjectPooler objectPooler;
    public GameObject bulletEnemyPrefab; // Prefab c?a ??n k? th�
    public Transform bulletSpawnPoint;
    private float minX, maxX, minY, maxY, padding = 1f;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;

        // Ki?m tra xem bulletSpawnPoint ?� ???c g�n ch?a
        if (bulletSpawnPoint == null)
        {
            Debug.LogError("bulletSpawnPoint ch?a ???c g�n trong Inspector");
        }

        // Ki?m tra xem bulletEnemyPrefab c� null kh�ng
        if (bulletEnemyPrefab == null)
        {
            Debug.LogError("bulletEnemyPrefab ch?a ???c g�n trong Inspector");
        }

        // T�nh to�n gi?i h?n m�n h�nh
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

        // Ki?m tra th?i gian ?? b?n vi�n ??n ti?p theo
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            FireBullet();
        }

        // Ki?m tra n?u enemy ra kh?i gi?i h?n m�n h�nh v� tr? v? pool
        if (transform.position.x < minX || transform.position.x > maxX || transform.position.y < minY || transform.position.y > maxY)
        {
            objectPooler.ReturnToPool(gameObject);
        }
    }

    void FireBullet()
    {
        // Ki?m tra xem bulletSpawnPoint ?� ???c g�n ch?a
        if (bulletSpawnPoint != null)
        {
            // Ki?m tra xem bulletEnemyPrefab c� null kh�ng tr??c khi spawn ??n
            if (bulletEnemyPrefab != null)
            {
                objectPooler.SpawnFromPool(bulletEnemyPrefab.tag, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            }
            else
            {
                Debug.LogError("bulletEnemyPrefab ch?a ???c g�n trong Inspector");
            }
        }
        else
        {
            Debug.LogError("bulletSpawnPoint ch?a ???c g�n trong Inspector");
        }
    }
}
