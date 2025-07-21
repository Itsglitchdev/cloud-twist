using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private const string CURRENTLEVELKEY = "CurrentLevel";

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

    public void GameInitialize()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        score = 0;
        matchesCount = 0;
        movesCount = 0;
        comboStreak = 0;
        totalLevels = gameData.levels.Count;
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
        currentTotalClouds = gameData.levels[levelIndex].matchCount * 2;
        currentLivesCount = gameData.levels[levelIndex].lives;
        currentClouds = new List<CloudData>(gameData.levels[levelIndex].clouds);

    }

    public void OnCloudSelected(Cloud cloud)
    {
        if (selectedCards.Contains(cloud)) return;

        selectedCards.Add(cloud);
        movesCount++;
        OnMovesChanged?.Invoke(movesCount);
        if (selectedCards.Count == 2)
        {
            StartCoroutine(CheckMatch(selectedCards[0], selectedCards[1]));
            selectedCards.Clear();
        }
    }


    private IEnumerator CheckMatch(Cloud a, Cloud b)
    {
        yield return new WaitForSeconds(0.5f);

        if (a.BackName == b.BackName)
        {
            Debug.Log("Match found!");
            a.MarkAsMatched();
            b.MarkAsMatched();

            comboStreak++;
            matchesCount++;
            score += comboStreak * 10;
            OnScoreChanged?.Invoke(score);
            OnMatchesChanged?.Invoke(matchesCount);

            if (matchesCount == gameData.levels[currentLevel].matchCount)
            {
                OnGameWin?.Invoke();
                OnWinText?.Invoke(currentLevel + 1, movesCount, score);
                selectedCards.Clear();
                Debug.Log("🎉 Game Win!");
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(a.Flip(false));
            StartCoroutine(b.Flip(false));

            comboStreak = 0;
            currentLivesCount--;
            OnLivesChanged?.Invoke(currentLivesCount);

            if (currentLivesCount == 0)
            {
                OnGameOver.Invoke();
                selectedCards.Clear();
                Debug.Log("❌ Game Over!");
            }
        }
    }

    public void OnNextLevel()
    {
        if (totalLevels <= currentLevel + 1) return;

        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.Save();
        score = 0;
        matchesCount = 0;
        movesCount = 0;
        comboStreak = 0;
        InitializeLevel(currentLevel);
        EmitEvents();
        OnLevelInitialized?.Invoke();
    }

    public void OnRestartLevel()
    {
        score = 0;
        matchesCount = 0;
        movesCount = 0;
        comboStreak = 0;
        InitializeLevel(currentLevel);
        EmitEvents();
        OnLevelInitialized?.Invoke();
    }


}
