using UnityEngine;
using UnityEngine.UI;

public class UIManager : Manager<UIManager>
{
    private const string infoString = "Level: {0}/{1}\nPlatforms: {2}/{3}";
    public const string Lose = "YOU LOSE";
    public const string Next = "NEXT LEVEL";
    public const string Win = "YOU WIN!\nCONGRATULATIONS!";

    [SerializeField]
    private ResultController resultPrefab = null;
    [SerializeField]
    private Text info = null;
    [SerializeField]
    private Toggle wallKicksToggle = null;

    public void ShowResult(string message)
    {
        ResultController result = Instantiate(resultPrefab, transform);
        result.Initialize(message);
    }

    private void Awake()
    {
        wallKicksToggle.onValueChanged.AddListener(delegate
        {
            LevelManager.Instance.isWallKicks = wallKicksToggle.isOn;
        });
    }

    private void Update()
    {
        info.text = string.Format(infoString, LevelManager.Instance.Level, LevelManager.maxLevel, LevelManager.Instance.PassedPlatforms, LevelManager.Instance.Platforms.Count);
    }
}