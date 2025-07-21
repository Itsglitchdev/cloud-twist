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

    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitializedPanels()
    {
        startPanel.SetActive(true);
        inGamePanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
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
    }

}
