using TMPro;
using UnityEngine;

[DefaultExecutionOrder(ScriptsOrder.Managers)]
public class UIManager : MonoBehaviour
{
    public GameObject scoringPanel;
    public GameObject againPanel;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI RemainTimeText;
    public TextMeshProUGUI YourScoreText;
    
    void Awake()
    {
        GameManager.Instance.OnScoreUpdated += OnScoreUpdated;
        GameManager.Instance.OnGameStarted += OnGameStarted;
        GameManager.Instance.OnGameEnded += OnGameEnded;
        
        scoringPanel.SetActive(true);
        againPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnScoreUpdated -= OnScoreUpdated;
        GameManager.Instance.OnGameStarted -= OnGameStarted;
        GameManager.Instance.OnGameEnded -= OnGameEnded;
    }

    private int _score;
    private float _remainTime;
    public bool gameStarted;
    void OnGameStarted(float remainTime)
    {
        gameStarted = true;
        _remainTime = remainTime;
    }

    void OnGameEnded()
    {
        YourScoreText.text = "Your score: " + _score;
        scoringPanel.SetActive(false);
        againPanel.SetActive(true);
    }

    void OnScoreUpdated(int newScore)
    {
        _score = newScore;
        ScoreText.text = _score.ToString();
    }

    private void Update()
    {
        if (!gameStarted) return;
        _remainTime -= Time.deltaTime;
        RemainTimeText.text = $"{_remainTime:0.0}";
    }
}
