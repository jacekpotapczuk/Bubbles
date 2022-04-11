using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text[] scoreTexts;
    
    private void OnEnable()
    {
        GameManager.OnScoreGain += UpdateScoreTexts;
    }
    
    private void OnDisable()
    {
        GameManager.OnScoreGain += UpdateScoreTexts;
    }

    private void UpdateScoreTexts(int score)
    {
        foreach (var scoreText in scoreTexts)
        {
            scoreText.text = score.ToString();
        }
    }
}
