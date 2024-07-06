using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    public float speed = 2.0f;
    private ObjectPooler objectPooler;
    private float minX, maxX, minY, maxY;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;

        // Calculate screen boundaries
        Camera cam = Camera.main;
        Vector3 screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        minX = -screenBounds.x;
        maxX = screenBounds.x;
        minY = -screenBounds.y;
        maxY = screenBounds.y;
    }

    void Update()
    {
        // Move the enemy downwards
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Check if the enemy is out of screen bounds and return it to the pool
        if (transform.position.x < minX || transform.position.x > maxX || transform.position.y < minY || transform.position.y > maxY)
        {
            objectPooler.ReturnToPool(gameObject);
        }
    }
}
