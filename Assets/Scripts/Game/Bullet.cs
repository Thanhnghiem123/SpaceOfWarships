using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    private ObjectPooler objectPooler;
    private float minX, maxX, minY, maxY, padding = 1f;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;

        // Calculate screen boundaries
        Camera cam = Camera.main;
        Vector3 screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        minX = -screenBounds.x - padding;
        maxX = screenBounds.x + padding;
        minY = -screenBounds.y - padding;
        maxY = screenBounds.y + padding;
    }

    void Update()
    {
        // Move the bullet upwards
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // Check if the bullet is out of screen bounds and return it to the pool
        if (transform.position.x < minX || transform.position.x > maxX || transform.position.y < minY || transform.position.y > maxY)
        {
            objectPooler.ReturnToPool(gameObject);
        }
    }
}
