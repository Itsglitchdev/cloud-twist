using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
public class GameData : ScriptableObject
{
    public List<levelData> levels = new List<levelData>();
}
[System.Serializable]
public class levelData
{
    public int level;
    public int row;
    public int col;
    public int matchCount;
    public int lives;
    public List<CloudData> clouds = new List<CloudData>();
}

[System.Serializable]
public class CloudData
{
    public CloudFrontData front;
    public CloudBackData back;

}
[System.Serializable]
public class CloudFrontData
{
    public CloudFront cloudFront;
    public Sprite imageFront;
}
[System.Serializable]
public class CloudBackData
{
    public CloudBack cloudBack;
    public Sprite imageBack;
}
