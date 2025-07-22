using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public static event Action OnLevelInitialized;
    public static event Action OnGameWin;
    public static event Action OnGameOver;

    public static event Action<int> OnLevelChanged;
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnMovesChanged;
    public static event Action<int> OnLivesChanged;
    public static event Action<int> OnMatchesChanged;
    public static event Action<int, int, int> OnWinText;


    [Header("Game Data")]
    [SerializeField] private GameData gameData;

    private int totalLevels;

    private int currentRowCount;
    private int currentColumnCount;
    private int currentTotalClouds;
    private int currentLevel;
    private int currentLivesCount;
    private List<CloudData> currentClouds;
    private List<Cloud> selectedCards = new List<Cloud>();

    private int score;
    private int comboStreak;
    private int matchesCount;
    private int movesCount;

    public List<CloudData> CloudData => currentClouds;
    public int RowCount => currentRowCount;
    public int TotalClouds => currentTotalClouds;
    public int LivesCount => currentLivesCount;
    public int TotalLevels => totalLevels;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // GameInitialize();
        // InitializeLevel(currentLevel);
    }


    private void PrepareLevelState()
    {
        score = GameConstants.INT_ZERO;
        matchesCount = GameConstants.INT_ZERO;
        movesCount = GameConstants.INT_ZERO;
        comboStreak = GameConstants.INT_ZERO;
        InitializeLevel(currentLevel);
        EmitEvents();
        OnLevelInitialized?.Invoke();
    }

    private void EmitEvents()
    {
        OnLevelChanged?.Invoke(currentLevel);
        OnScoreChanged?.Invoke(score);
        OnMovesChanged?.Invoke(movesCount);
        OnLivesChanged?.Invoke(currentLivesCount);
        OnMatchesChanged?.Invoke(matchesCount);
    }

    private void InitializeLevel(int levelIndex)
    {
        currentRowCount = gameData.levels[levelIndex].row;
        currentColumnCount = gameData.levels[levelIndex].col;
        currentTotalClouds = gameData.levels[levelIndex].matchCount * GameConstants.INT_TWO;
        currentLivesCount = gameData.levels[levelIndex].lives;
        currentClouds = new List<CloudData>(gameData.levels[levelIndex].clouds);

    }

    private IEnumerator CheckMatch(Cloud a, Cloud b)
    {
        yield return new WaitForSeconds(GameConstants.FLOAT_ZERO_POINT_FIVE);

        if (a.BackName == b.BackName)
        {
            Debug.Log("Match found!");
            a.MarkAsMatched();
            b.MarkAsMatched();

            comboStreak++;
            matchesCount++;
            score += comboStreak * GameConstants.INT_TEN;
            AudioManager.Instance.PlayCloudMatchSound();
            OnScoreChanged?.Invoke(score);
            OnMatchesChanged?.Invoke(matchesCount);

            if (matchesCount == gameData.levels[currentLevel].matchCount)
            {
                OnGameWin?.Invoke();
                OnWinText?.Invoke(currentLevel + GameConstants.INT_ZERO, movesCount, score);
                selectedCards.Clear();
                AudioManager.Instance.PlayGameWinSound();
                Debug.Log("Game Win!");
            }
        }
        else
        {
            yield return new WaitForSeconds(GameConstants.FLOAT_ZERO_POINT_FIVE);
            StartCoroutine(a.Flip(false));
            StartCoroutine(b.Flip(false));

            comboStreak = GameConstants.INT_ZERO;
            currentLivesCount--;
            AudioManager.Instance.PlayCloudMismatchSound();
            OnLivesChanged?.Invoke(currentLivesCount);

            if (currentLivesCount == GameConstants.FLOAT_ZERO)
            {
                OnGameOver.Invoke();
                selectedCards.Clear();
                AudioManager.Instance.PlayGameOverSound();
                Debug.Log("Game Over!");
            }
        }
    }

    #region Public Methods

    public void GameInitialize()
    {
        currentLevel = PlayerPrefs.GetInt(GameConstants.CURRENTLEVELKEY, GameConstants.INT_ZERO);
        totalLevels = gameData.levels.Count;
        Debug.Log($"Total levels: {totalLevels}");
        PrepareLevelState();
    }

    public void OnCloudSelected(Cloud cloud)
    {
        if (selectedCards.Contains(cloud)) return;

        selectedCards.Add(cloud);
        movesCount++;
        AudioManager.Instance.PlayCloudFlipSound();
        OnMovesChanged?.Invoke(movesCount);
        if (selectedCards.Count == GameConstants.INT_TWO)
        {
            StartCoroutine(CheckMatch(selectedCards[GameConstants.INT_ZERO], selectedCards[GameConstants.INT_ONE]));
            selectedCards.Clear();
        }
    }

    public void OnNextLevel()
    {
        if (totalLevels <= currentLevel + GameConstants.FLOAT_ONE) return;

        currentLevel++;
        PlayerPrefs.SetInt(GameConstants.CURRENTLEVELKEY, currentLevel);
        PlayerPrefs.Save();
        PrepareLevelState();
    }

    public void OnRestartLevel()
    {
        PrepareLevelState();
    }

    #endregion

}
