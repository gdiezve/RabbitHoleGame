using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerMenu : MonoBehaviour
{
    public GameObject keepTryingMenuUI;
    public void PlayButton() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    public void LoadMainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    } 

    public void Resume() {
        Time.timeScale = 1;
        keepTryingMenuUI.SetActive(false);
    }
}
