using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] private string videoURL = "https://gdiezve.github.io/VideoHost/howtoplay.mp4";
    private VideoPlayer videoPlayer;

    void Awake() {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer) {
            videoPlayer.url = videoURL;
            videoPlayer.playOnAwake = false;
            videoPlayer.Prepare();

            videoPlayer.prepareCompleted += OnVideoPrepared;
        }
    }

    private void OnVideoPrepared(VideoPlayer source) {
        videoPlayer.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
