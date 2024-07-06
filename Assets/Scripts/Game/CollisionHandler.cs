using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Enemy
public class CollisionHandler : MonoBehaviour
{
    private ObjectPooler objectPooler;
    void Start()
    {
        objectPooler = ObjectPooler.Instance;

    }
    

    // Enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();

        Debug.Log(collision.gameObject.name);
        if (collision.CompareTag("Bullet")  )
        {
            
            playerController.score += 1; // Increase score
            // L?y ??i t??ng k? thù và ??n
            GameObject enemyObject = gameObject;
            GameObject bulletObject = collision.gameObject;

            // Tr? c? hai v? pool
            objectPooler.ReturnToPool(enemyObject);
            objectPooler.ReturnToPool(bulletObject);
            
        }
        if(collision.CompareTag("Bullet1"))
        {
            playerController.score += 1; // Increase score
            objectPooler.ReturnToPool(gameObject);
            
        }
        
        if (collision.CompareTag("Shield") && PlayerController.hasShield == true)
        {
            objectPooler.ReturnToPool(gameObject);
        }
        if (collision.CompareTag("Player"))
        {
            
            Time.timeScale = 0;
            Invoke("GameOver", 0f);
        }

        if(true)
        {
            PlayerPrefs.SetInt("Score", playerController.score);
            PlayerPrefs.Save();
        }
         

    }

    void GameOver()
    {
        SceneManager.LoadScene(6);
    }
    
}
