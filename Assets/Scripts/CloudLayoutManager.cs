using UnityEngine;
using UnityEngine.UI;

public class CloudLayoutManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject cloudParent;
    [SerializeField] private GameObject cloudPrefab;

    
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
        InstantiateClouds();
    }

    private void SetupGrid()
    {
        GridLayoutGroup grid = cloudParent.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            grid.constraintCount = GameManager.Instance.rowCount;
        }
    }

    private void InstantiateClouds()
    {
        int total = GameManager.Instance.totalClouds;

        for (int i = 0; i < total; i++)
        {
            GameObject cloud = Instantiate(cloudPrefab, cloudParent.transform);
            cloud.name = "Cloud_" + i;
        }

        Debug.Log($"{total} clouds instantiated.");
    }

}
