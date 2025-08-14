using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action<string> Warn;
    public static event Action<int> ScoreChanged;
    public static event Action<int> HighScoreChanged;
    public static event Action LevelCompleted;

    public static int HighScore
    {
        get => PlayerPrefs.GetInt("HighScore", 0);
        set => PlayerPrefs.SetInt("HighScore", value);
    }
        

    public static void NotifyScoreChanged(int score)
    {
        if (score > HighScore)
        {
            HighScore = score;
            HighScoreChanged?.Invoke(HighScore);
        }
        ScoreChanged?.Invoke(score);
    }

    public static void NotifyLevelCompleted()
    {
        LevelCompleted?.Invoke();
    }

    public static void NotifyWarning(string warning)
    {
        Warn?.Invoke(warning);
    }
}
