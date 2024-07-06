using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    [SerializeField] public GameObject bulletBossTag;
    [SerializeField] private Transform bulletBossPoint;
    [SerializeField] private float speed = 6f;
    private float padding = 0.5f;
    private float minX, maxX;
    private float minY, maxY;
    private bool movingRight = true;
    private float fireRate = 0.4f, nextFire = 0.8f;

    private ObjectPooler objectPooler;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        Vector3 screen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, transform.position.y));
        minX = -screen.x + padding;
        maxX = screen.x - padding;

        // B?t ??u coroutine ?? xu?t hi?n boss sau 3 giây và di chuy?n xu?ng
        StartCoroutine(AppearAndMoveDown());

        Camera cam = Camera.main;
        Vector3 screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        minX = -screenBounds.x ;
        maxX = screenBounds.x ;
        minY = -screenBounds.y ;
        maxY = screenBounds.y ;
    }

    private IEnumerator AppearAndMoveDown()
    {
        // Ch? 6 giây tr??c khi xu?t hi?n
        yield return new WaitForSeconds(6.0f);

        // ??t v? trí ban ??u c?a Boss
        transform.position = new Vector3(transform.position.x, maxY+2, transform.position.z);

        // Di chuy?n xu?ng y=3.5
        while (transform.position.y > maxY-1.5)
        {
            transform.Translate(Vector3.up * (speed - 4) * Time.deltaTime);
            yield return null; // ??i khung hình ti?p theo
        }

        // ??t v? trí cu?i cùng t?i y=4.5
        transform.position = new Vector3(transform.position.x, (float)(maxY - 1.5), transform.position.z);

        // B?t ??u b?n ??n và di chuy?n ngang
        StartCoroutine(MoveAndShoot());
    }

    private IEnumerator MoveAndShoot()
    {
        while (true)
        {
            // Di chuy?n ngang
            Vector2 moveBoss = movingRight ? Vector2.right * speed * Time.deltaTime : Vector2.left * speed * Time.deltaTime;
            transform.Translate(moveBoss);

            // Ki?m tra v? trí hi?n t?i và thay ??i h??ng n?u c?n thi?t
            if (transform.position.x >= maxX)
            {
                movingRight = false;
            }
            else if (transform.position.x <= minX)
            {
                movingRight = true;
            }

            // Ki?m tra th?i gian ?? b?n viên ??n ti?p theo
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                fireBulletBoss();
            }

            yield return null; // ??i khung hình ti?p theo
        }
    }

    void fireBulletBoss()
    {
        objectPooler.SpawnFromPool(bulletBossTag.tag, bulletBossPoint.position, bulletBossPoint.rotation);
    }
}
