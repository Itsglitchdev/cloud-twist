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

    private int score;
    private int matchesCount;
    private int movesCount;


    public List<CloudData> CloudData => currentClouds;
    public int RowCount => currentRowCount;
    public int TotalClouds => currentTotalClouds;
    public int LivesCount => currentLivesCount;


    private Cloud firstSelected;
    private Cloud secondSelected;
    public bool IsBusy { get; private set; }

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

    public void OnCloudSelected(Cloud selected)
    {
        if (firstSelected == null)
        {
            firstSelected = selected;
            return;
        }

        if (secondSelected == null && selected != firstSelected)
        {
            secondSelected = selected;
            StartCoroutine(CheckMatch());
        }
    }
    
    private IEnumerator CheckMatch()
    {
        IsBusy = true;

        yield return new WaitForSeconds(0.5f); 

        if (firstSelected.BackName == secondSelected.BackName)
        {
            firstSelected.MarkAsMatched();
            secondSelected.MarkAsMatched();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(firstSelected.Flip(false));
            StartCoroutine(secondSelected.Flip(false));
        }

        firstSelected = null;
        secondSelected = null;
        IsBusy = false;
    }
}
