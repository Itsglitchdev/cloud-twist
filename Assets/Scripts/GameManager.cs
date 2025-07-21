using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action OnLevelDataReady;

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
        GameInitialize();
        // InitializeLevel(currentLevel);
    }

    private void GameInitialize()
    {
        currentLevel = 0;
        score = 0;
        matchesCount = 0;
        movesCount = 0;
        totalLevels = gameData.levels.Count;
        InitializeLevel(currentLevel);
    }

    private void InitializeLevel(int levelIndex)
    {
        currentRowCount = gameData.levels[levelIndex].row;
        currentColumnCount = gameData.levels[levelIndex].col;
        currentTotalClouds = gameData.levels[levelIndex].matchCount * 2;
        currentLivesCount = gameData.levels[levelIndex].lives;
        currentClouds = new List<CloudData>(gameData.levels[levelIndex].clouds);

        OnLevelDataReady?.Invoke();
    }

    public void OnCloudSelected(Cloud cloud)
    {
        if (selectedCards.Contains(cloud)) return;

        selectedCards.Add(cloud);

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
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(a.Flip(false));
            StartCoroutine(b.Flip(false));
        }
    }

}
