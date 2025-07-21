using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudLayoutManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject cloudParent;
    [SerializeField] private GameObject cloudPrefab;

    private GridLayoutGroup grid;

    void Awake()
    {
        grid = cloudParent.GetComponent<GridLayoutGroup>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnLevelDataReady += InitializeLayout;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnLevelDataReady -= InitializeLayout;
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
        for (int i = 0; i < cloudDataList.Count; i++)
        {
            GameObject cloud = Instantiate(cloudPrefab, cloudParent.transform);
            cloud.name = "Cloud_" + i;
            Cloud cloudComponent = cloud.GetComponent<Cloud>();
            cloudComponent.Setup(cloudDataList[i]);

            yield return null; 
        }
        grid.enabled = false;  

        Debug.Log($"{cloudDataList.Count} clouds instantiated.");
    }



}
