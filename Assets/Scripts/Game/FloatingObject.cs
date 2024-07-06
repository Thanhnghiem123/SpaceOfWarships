using Unity.VisualScripting;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Vector2 direction = Vector2.up; // H??ng di chuy?n
    private float minX, maxX, minY, maxY;
    private float padding = 0.5f; // Kho?ng c�ch gi?i h?n v?i r�a m�n h�nh


    // Start is called before the first frame update
    void Start()
    {
        // T�nh to�n ph?m vi di chuy?n d?a tr�n camera ch�nh
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        minX = -screenBounds.x + padding;
        maxX = screenBounds.x - padding;
        minY = -screenBounds.y + padding;
        maxY = screenBounds.y - padding;
    }

    // Update is called once per frame
    void Update()
    {
        // Di chuy?n v?t th?
        Vector2 move = direction * speed * Time.deltaTime;
        transform.Translate(move);

        // Gi?i h?n v?t th? trong ph?m vi m�n h�nh
        Vector3 pos = transform.position;
        if (pos.x < minX || pos.x > maxX)
        {
            direction.x = -direction.x; // ??o ng??c h??ng theo tr?c x
            pos.x = Mathf.Clamp(pos.x, minX, maxX); // Gi?i h?n v? tr� x
        }
        if (pos.y < minY || pos.y > maxY)
        {
            direction.y = -direction.y; // ??o ng??c h??ng theo tr?c y
            pos.y = Mathf.Clamp(pos.y, minY, maxY); // Gi?i h?n v? tr� y
        }
        transform.position = pos;
    }

    public void SetSpeed(float newspeed)
    {
        this.speed = newspeed;
    }
}
