using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI playerScoreUIText;
    public TextMeshProUGUI winnerScoreUIText;
    public TextMeshProUGUI loserScoreUIText;
    private readonly string[] tags = new string[] {"Add1Modifier", "Substract1Modifier", "Hole"};

    void UpdateScore(int points) {
        score += points;
        playerScoreUIText.SetText(score.ToString());
        winnerScoreUIText.SetText(score.ToString());
        loserScoreUIText.SetText(score.ToString());
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (tags.Contains(collision.gameObject.tag))
        {
            UpdateScore(10);
        }
    }
}
