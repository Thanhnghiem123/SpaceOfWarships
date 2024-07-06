using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public Text transitionText; // Reference ??n UI Text ?? hi?n th? "Qua màn"

    void Start()
    {
        if (transitionText != null)
        {
            transitionText.gameObject.SetActive(false); // ?n ban ??u khi không c?n hi?n th?
        }
        else
        {
            Debug.LogError("Transition Text is not assigned in the inspector.");
        }
    }

    IEnumerator ShowTransitionText()
    {
        if (transitionText == null)
        {
            Debug.LogError("Transition Text is not assigned in the inspector.");
        }
        else
        {
            transitionText.gameObject.SetActive(true);
            float duration = 1.0f; // Th?i gian hi?n th? "Qua màn"
            float timer = 0.0f;

            // Phóng to ch? "Qua màn"
            while (timer < duration)
            {
                float scale = Mathf.Lerp(1.0f, 1.5f, timer / duration); // T? l? phóng to t? 1.0 lên 1.5
                transitionText.transform.localScale = new Vector3(scale, scale, 1.0f);

                timer += Time.deltaTime;
                yield return null;
            }
            // ??i 1 giây tr??c khi bi?n m?t
            yield return new WaitForSeconds(1.0f);

            // Bi?n m?t ch? "Qua màn"
            transitionText.gameObject.SetActive(false);
        }
        

        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            PlayerPrefs.SetFloat("PlayerX", playerPosition.x);
            PlayerPrefs.SetFloat("PlayerY", playerPosition.y);
            PlayerPrefs.SetFloat("PlayerZ", playerPosition.z);
        }
        // Chuy?n sang scene m?i
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1) // Ki?m tra xem có th? chuy?n sang scene ti?p theo không
        {
            int nextSceneIndex = currentSceneIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
        if (player != null)
        {
            PlayerController p = player.GetComponent<PlayerController>();
            if (p != null)
            {
                p.bulletExtra = 0;
                p.bullet = p.bulletPrefab;
            }
        }
    }

    public void StartLevelTransition()
    {
        StartCoroutine(ShowTransitionText());
    }
}
