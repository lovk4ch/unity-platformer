using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Manager<LevelManager>
{
    private const float maxDistance = 3.8f;
    public const float Gravity = 19.9f;
    public const int maxLevel = 9;

    private float levelLength;

    public int Level { get; set; }

    public bool isWallKicks;

    [SerializeField]
    private PlayerController playerPrefab = null;
    private PlayerController player = null;

    [SerializeField]
    private PlatformController platformPrefab = null;
    public int PassedPlatforms => player.lastPlatform + 1;
    public List<PlatformController> Platforms { get; private set; }

    private void Awake()
    {
        Platforms = new List<PlatformController>();
        GenerateLevel();
    }

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, new Vector3(-Level, Level + 1, 0f), Quaternion.identity);
    }

    public void Win()
    {
        if (Level < maxLevel)
        {
            UIManager.Instance.ShowResult(UIManager.Next);
            GenerateLevel();
        }
        else
            UIManager.Instance.ShowResult(UIManager.Win);
    }

    public void Lose()
    {
        UIManager.Instance.ShowResult(UIManager.Lose);
        SpawnPlayer();
    }

    public void GenerateLevel()
    {
        for (int i = 0; i < Platforms.Count; i++)
        {
            Destroy(Platforms[i].gameObject);
            Platforms.RemoveAt(i--);
        }

        var platform = Instantiate(platformPrefab, Vector3.zero, Quaternion.identity);
        platform.Expand(10);
        Platforms.Add(platform);

        Level++;
        levelLength = 25 + 25 * Level;
        float posX = platform.Length + Level;

        while (posX < levelLength)
        {
            platform = GeneratePlatform(ref posX);
            Platforms.Add(platform);
        }

        SpawnPlayer();
    }

    private PlatformController GeneratePlatform(ref float posX)
    {
        var height = Platforms[Platforms.Count - 1].transform.position.y;

        // calculate new platform's height
        var posY = Random.Range(height - maxDistance, height + maxDistance);
        if (!isWallKicks)
            posY *= Mathf.Min(1, Platforms[Platforms.Count - 1].Length / 4);
        posY = (float)Math.Round(posY, 1);
        var position = new Vector3(posX, posY, 0);

        // generate platform
        var platform = Instantiate(platformPrefab, position, Quaternion.identity);
        var blocksCount = 3 + (int)(Mathf.Sqrt(levelLength - posX) * Random.Range(0.1f, 1.1f));

        platform.Expand(blocksCount);
        posX += platform.Length + Level;
        return platform;
    }
}