using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject coverImage;
    [SerializeField] Button playButton;
     [SerializeField] Button howToPlayButton;
    [SerializeField] GameObject chooseLevelUI;

    public void HowToPlayButton() {
        SceneManager.LoadScene("HowToPlayScene");
    }

    public void PlayButton() {
        coverImage.SetActive(false);
        playButton.gameObject.SetActive(false);
        howToPlayButton.gameObject.SetActive(false);
        chooseLevelUI.SetActive(true);
    }

    public void EasyButton() {
        SceneManager.LoadScene("GameScene");
    }
    public void MediumButton() {
        GlobalGameManager.Instance.fieldSize = 70;
        GlobalGameManager.Instance.obstacleCount = 40;
        GlobalGameManager.Instance.numberOfCollectablesPerType = 3;
        GlobalGameManager.Instance.enemySpawnInterval = 2f;
        GlobalGameManager.Instance.enemySpawnRadius = 10f;
        GlobalGameManager.Instance.randomFinishScale = Random.Range(1, 6);
        GlobalGameManager.Instance.randomPlayerScale = Random.Range(1, 6);
        GlobalGameManager.Instance.orthographicSize = 10f;
        GlobalGameManager.Instance.enemySpeed = 5f;
        GlobalGameManager.Instance.playerSpeed = 7f;
        GlobalGameManager.Instance.triangleEnemyProbability = 0.5;
        SceneManager.LoadScene("GameScene");
    }
    public void HardButton() {
        GlobalGameManager.Instance.fieldSize = 100;
        GlobalGameManager.Instance.obstacleCount = 80;
        GlobalGameManager.Instance.numberOfCollectablesPerType = 2;
        GlobalGameManager.Instance.enemySpawnInterval = 2f;
        GlobalGameManager.Instance.enemySpawnRadius = 8f;
        GlobalGameManager.Instance.randomFinishScale = Random.Range(1, 10);
        GlobalGameManager.Instance.randomPlayerScale = Random.Range(1, 10);
        GlobalGameManager.Instance.orthographicSize = 13f;
        GlobalGameManager.Instance.enemySpeed = 7f;
        GlobalGameManager.Instance.playerSpeed = 10f;
        GlobalGameManager.Instance.triangleEnemyProbability = 0.8;
        SceneManager.LoadScene("GameScene");
    }
}
