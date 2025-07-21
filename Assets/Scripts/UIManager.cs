using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("All Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private GameObject loadingObject;
    [SerializeField] private GameObject cloudHolderObject;

    [Header("Button Reference")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartButton;

    [Header("Text Reference")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI matchesText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;


    // Start is called before the first frame update
    void Start()
    {
        InitializedPanels();
        ButtonEventsHandler();
    }

    void OnEnable()
    {
        GameManager.OnLevelChanged += UpdateLevel;
        GameManager.OnScoreChanged += UpdateScore;
        GameManager.OnMovesChanged += UpdateMoves;
        GameManager.OnLivesChanged += UpdateLives;
        GameManager.OnMatchesChanged += UpdateMatches;
        GameManager.OnWinText += ShowWinText;

        GameManager.OnGameWin += OnGameWin;
        GameManager.OnGameOver += OnGameOver;

        CloudLayoutManager.OnCloudsReady += OnCloudsReady;
    }

    void OnDisable()
    {
        GameManager.OnLevelChanged -= UpdateLevel;
        GameManager.OnScoreChanged -= UpdateScore;
        GameManager.OnMovesChanged -= UpdateMoves;
        GameManager.OnLivesChanged -= UpdateLives;
        GameManager.OnMatchesChanged -= UpdateMatches;
        GameManager.OnWinText -= ShowWinText;

        GameManager.OnGameWin -= OnGameWin;
        GameManager.OnGameOver -= OnGameOver;

        CloudLayoutManager.OnCloudsReady -= OnCloudsReady;
    }

    void InitializedPanels()
    {
        startPanel.SetActive(true);
        inGamePanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        loadingObject.SetActive(false);
        cloudHolderObject.SetActive(false);
    }

    void ButtonEventsHandler()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    void OnStartButtonClicked()
    {
        startPanel.SetActive(false);
        inGamePanel.SetActive(true);
        loadingObject.SetActive(true);

        GameManager.Instance.GameInitialize();
    }

    void OnCloudsReady()
    {
        loadingObject.SetActive(false);
        cloudHolderObject.SetActive(true);
    }

    void OnGameWin()
    {
        inGamePanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }

    void OnNextLevelButtonClicked()
    {
        gameWinPanel.SetActive(false);
        inGamePanel.SetActive(true);
        loadingObject.SetActive(true);
        cloudHolderObject.SetActive(false);

        GameManager.Instance.OnNextLevel();
    }

    void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        inGamePanel.SetActive(false);
    }

    void OnRestartButtonClicked()
    {
        gameOverPanel.SetActive(false);
        inGamePanel.SetActive(true);
        loadingObject.SetActive(true); 
        cloudHolderObject.SetActive(false);

        GameManager.Instance.OnRestartLevel();
    }

    void UpdateLevel(int levelIndex)
    {
        levelText.text = $"Level {levelIndex + 1}";
    }

    void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    void UpdateMoves(int moves)
    {
        movesText.text = $"Moves: {moves}";
    }

    void UpdateLives(int lives)
    {
        livesText.text = $"Lives: {lives}";
    }

    void UpdateMatches(int matches)
    {
        matchesText.text = $"Matches: {matches}";
    }

    void ShowWinText(int level, int moves, int score)
    {
        winText.text = $"Level {level} Complete!\nYou did it in {moves} moves with a score of {score}. Great job!";
    }



}
