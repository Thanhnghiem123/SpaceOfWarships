using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 7.0f;
    public GameObject bulletPrefab;
    public GameObject bullet1Prefab;
    public GameObject bullet;
    public Transform bulletSpawnPoint;
    public float fireRate = 0.5f;
    private float nextFire = 1.0f;

    private ObjectPooler objectPooler;
    private float minX, maxX, minY, maxY;
    private float padding = 0.5f;

    public GameObject shieldPrefab;
    private GameObject currentShield = null;
    internal static bool hasShield = false;
    public int bulletExtra = 0;
    public int bulletType = 0;

    private float shieldDuration = 5f; // Thời gian tồn tại của khiên, có thể điều chỉnh
    private Coroutine shieldCoroutine;
    private Collider2D playerCollider;
    private int currentSceneIndex;

    // Biến cho AudioSource và tệp nhạc
    private AudioSource audioSource;
    public AudioClip shootSound;

    public Text text;
    public int score = 0;

    private NewBehaviourScript newBehaviourScript;
    private bool isPaused = false;

    public bool Pause()
    {
        isPaused = true;
        return isPaused;

    }
    public bool Resume()
    {
        isPaused = false;
        return isPaused;
    }


    void Start()
    {
        Debug.Log("PlayerController Start");
        bulletExtra = 0;
        bulletType = 0;
        ChangeBulletType();

        Camera cam = Camera.main;
        Vector3 screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        minX = -screenBounds.x + padding;
        maxX = screenBounds.x - padding;
        minY = -screenBounds.y + padding;
        maxY = screenBounds.y - padding;

        objectPooler = ObjectPooler.Instance;
        playerCollider = GetComponent<Collider2D>();

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("LastSceneIndex", currentSceneIndex);
        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY") && PlayerPrefs.HasKey("PlayerZ"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");
            transform.position = new Vector3(x, y, z);
        }
        Debug.Log("ObjectPooler instance: " + objectPooler);

        // Gán AudioSource và AudioClip
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = shootSound;

        if (PlayerPrefs.HasKey("Score"))
        {
            score = PlayerPrefs.GetInt("Score");
            Debug.Log("SCore: " + score);
        }

        GameObject gameController = GameObject.FindWithTag("UI");
        if (gameController != null)
        {
            newBehaviourScript = gameController.GetComponent<NewBehaviourScript>();
        }
    }

    void Update()
    {
        text.text = "SCORE " + score;

        Vector2 moveDirection = Vector2.zero;

        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // Convert the touch position to movement direction
                moveDirection = touch.deltaPosition.normalized;
            }
        }
        else
        {
            // Fallback to keyboard input for testing in the editor
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            moveDirection = new Vector2(moveHorizontal, moveVertical);
        }

        // Apply movement
        Vector2 newPosition = moveDirection * speed * Time.deltaTime;
        transform.Translate(newPosition);

        // Clamp the position
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;

        

        if(Input.GetKey(KeyCode.Space) && Time.timeScale != 0)
        {
            if ( Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                if (bulletExtra == 0)
                {
                    FireBullet();
                }
                else if (bulletExtra == 1)
                {
                    FireBullet1();
                }
                else if (bulletExtra >= 2)
                {
                    FireBullet2();
                }
            }
        }
        


        if(Input.GetKeyDown(KeyCode.Escape) )
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                newBehaviourScript.Menu();
            }
            else 
            {
                newBehaviourScript.Continue();
            }
        }
    }

    void FireBullet()
    {
        objectPooler.SpawnFromPool(bullet.tag, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        PlayShootSound();
    }

    void FireBullet1()
    {
        objectPooler.SpawnFromPool(bullet.tag, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Quaternion spawnRotation1 = Quaternion.Euler(0, 0, bulletSpawnPoint.rotation.eulerAngles.z + 45f);
        Quaternion spawnRotation2 = Quaternion.Euler(0, 0, bulletSpawnPoint.rotation.eulerAngles.z - 45f);
        objectPooler.SpawnFromPool(bullet.tag, bulletSpawnPoint.position, spawnRotation1);
        objectPooler.SpawnFromPool(bullet.tag, bulletSpawnPoint.position, spawnRotation2);
        PlayShootSound();
    }

    void FireBullet2()
    {
        objectPooler.SpawnFromPool(bullet.tag, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Quaternion spawnRotation1 = Quaternion.Euler(0, 0, bulletSpawnPoint.rotation.eulerAngles.z + 45f);
        Quaternion spawnRotation2 = Quaternion.Euler(0, 0, bulletSpawnPoint.rotation.eulerAngles.z - 45f);
        Quaternion spawnRotation3 = Quaternion.Euler(0, 0, bulletSpawnPoint.rotation.eulerAngles.z + 22.5f);
        Quaternion spawnRotation4 = Quaternion.Euler(0, 0, bulletSpawnPoint.rotation.eulerAngles.z - 22.5f);
        objectPooler.SpawnFromPool(bullet.tag, bulletSpawnPoint.position, spawnRotation1);
        objectPooler.SpawnFromPool(bullet.tag, bulletSpawnPoint.position, spawnRotation2);
        objectPooler.SpawnFromPool(bullet.tag, bulletSpawnPoint.position, spawnRotation3);
        objectPooler.SpawnFromPool(bullet.tag, bulletSpawnPoint.position, spawnRotation4);
        PlayShootSound();
    }

    void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Speed"))
        {
            GameObject speedObj = collision.gameObject;
            speed = speed * 2;
            objectPooler.ReturnToPool(speedObj);
            Invoke("ResetSpeed", 5f);
        }
        else if (collision.CompareTag("BulletExtra"))
        {
            GameObject bulletExtraObj = collision.gameObject;
            bulletExtra++;
            objectPooler.ReturnToPool(bulletExtraObj);
        }
        else if (collision.CompareTag("BulletType"))
        {
            GameObject bulletTypeObj = collision.gameObject;
            bulletType++;
            ChangeBulletType();
            objectPooler.ReturnToPool(bulletTypeObj);
        }
        else if (collision.CompareTag("Shield"))
        {
            GameObject shieldObj = collision.gameObject;
            hasShield = true;
            ActivateShield();
            objectPooler.ReturnToPool(shieldObj);
        }
    }

    void ResetSpeed()
    {
        speed = 5.0f;
    }

    void ChangeBulletType()
    {
        
        if (bulletType == 0)
        {
                bullet = bulletPrefab;
        }
        if (bulletType == 1)
        {
                bullet = bullet1Prefab;
        }

    }

    void ActivateShield()
    {
        if (currentShield != null)
        {
            objectPooler.ReturnToPool(currentShield);
        }

        currentShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        Collider2D shieldCollider = currentShield.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCollider, shieldCollider);
        currentShield.transform.SetParent(transform);

        FloatingObject floatingObject = currentShield.GetComponent<FloatingObject>();
        if (floatingObject != null)
        {
            floatingObject.SetSpeed(0f);
        }

        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }

        shieldCoroutine = StartCoroutine(ShieldDuration());
    }

    IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(shieldDuration);

        if (currentShield != null)
        {
            objectPooler.ReturnToPool(currentShield);
            currentShield = null;
            hasShield = false;
        }
    }

    void OnApplicationQuit()
    {
        bulletExtra = 0;
        bulletType = 0;
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }

    void OnEnable()
    {
        SceneManager.sceneUnloaded += SaveScoreOnSceneChange;
    }

    void OnDisable()
    {
        SceneManager.sceneUnloaded -= SaveScoreOnSceneChange;
    }

    void SaveScoreOnSceneChange(Scene scene)
    {
        
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
        Debug.Log("Score saved: " + score);
    }

}
