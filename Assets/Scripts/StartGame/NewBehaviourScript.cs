using UnityEngine.SceneManagement;
using UnityEngine;
using JetBrains.Annotations;
using UnityEngine.UI;
using Unity.VisualScripting;

public class NewBehaviourScript : MonoBehaviour
{
    public Text text;
    public GameObject menu;
    public void StartGame()
    {
        SceneManager.LoadScene(1); // ?i?u h??ng t?i scene m?i
        Time.timeScale = 1;
    }

    public void MenuGame()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        int lastSceneIndex = PlayerPrefs.GetInt("LastSceneIndex", 0);
        SceneManager.LoadScene(lastSceneIndex); // N?p l?i scene khi b?t ??u trò ch?i
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Menu()
    {
        Time.timeScale = 0f;
        menu.SetActive(true);
    }

    public void Continue()
    {
        GameObject player = GameObject.Find("Player"); // Giả sử "Player" là tên của GameObject.

        // Lấy component PlayerController từ GameObject.
        PlayerController playerController = player.GetComponent<PlayerController>();

        // Kiểm tra nếu playerController không null trước khi gọi hàm.
        if (playerController != null)
        {
            // Gọi hàm Resume() từ PlayerController.
            Debug.Log("aaaaaaaaaaaaaaa" +playerController.Resume());
        
        }
            menu.SetActive(false);
        Time.timeScale = 1f;
        
    }

    void Start()
    {

        int score = PlayerPrefs.GetInt("Score", 0);
        text.text = "" + score;
    }
    
}
