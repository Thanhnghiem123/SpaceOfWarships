using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private int hp;
    private ObjectPooler objectPooler;
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (collision.CompareTag("Bullet") || collision.CompareTag("Bullet1") || collision.CompareTag("Bullet2") )
        {
            
            hp--;
            objectPooler.ReturnToPool(collision.gameObject);
            Debug.Log("hp: "+ hp);
            if(hp == 0)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                playerController.score += 1; // Increase score
                Destroy(gameObject);
            }
        }
    }
}
