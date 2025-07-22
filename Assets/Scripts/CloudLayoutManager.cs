using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudLayoutManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject cloudParent;
    [SerializeField] private GameObject cloudPrefab;
    
    public static event Action OnCloudsReady;
    private GridLayoutGroup grid;

    void Awake()
    {
        grid = cloudParent.GetComponent<GridLayoutGroup>();
    }

    private void OnEnable()
    {
        GameManager.OnLevelInitialized += InitializeLayout;
        GameManager.OnGameWin += DestroyAllClouds;
        GameManager.OnGameOver += DestroyAllClouds;
    }

    private void OnDisable()
    {
        GameManager.OnLevelInitialized -= InitializeLayout;
        GameManager.OnGameWin -= DestroyAllClouds;
        GameManager.OnGameOver -= DestroyAllClouds;
    }

    private void InitializeLayout()
    {
        SetupGrid();
        StartCoroutine(InstantiateCloudsCoroutine());
    }

    private void SetupGrid()
    {

        if (grid != null)
        {

            grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            grid.constraintCount = GameManager.Instance.RowCount;
        }
    }

    private IEnumerator InstantiateCloudsCoroutine()
    {
        List<CloudData> cloudDataList = GameManager.Instance.CloudData;
        grid.enabled = true;
        for (int i = GameConstants.INT_ZERO; i < cloudDataList.Count; i++)
        {
            GameObject cloud = Instantiate(cloudPrefab, cloudParent.transform);
            cloud.name = "Cloud_" + i;
            Cloud cloudComponent = cloud.GetComponent<Cloud>();
            cloudComponent.Setup(cloudDataList[i]);

            yield return null;
        }

        Debug.Log($"{cloudDataList.Count} clouds instantiated.");
        OnCloudsReady?.Invoke(); 
        yield return null;
        grid.enabled = false;
    }

    private void DestroyAllClouds()
    { 
        foreach (Transform child in cloudParent.transform)
        {
            Destroy(child.gameObject);
        }
    }


}
