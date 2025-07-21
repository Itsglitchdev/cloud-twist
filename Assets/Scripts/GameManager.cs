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

    public int rowCount;
    public int columnCount;
    public int levelCount;
    public int totalClouds;
    private int currentLevel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        { 
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rowCount = gameData.levels[currentLevel].row;
        totalClouds = gameData.levels[currentLevel].matchCount * 2;
        OnLevelDataReady?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }
    

}
