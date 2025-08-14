using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private Button homeButton;
    
    public static UiManager Instance;

    private Coroutine _warningRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    private void OnEnable()
    {
        LevelManager.Warn += OnWarn;
        LevelManager.ScoreChanged += UpdateScore;
        LevelManager.LevelCompleted += ShowEndScreen;
    }

    private void OnDisable()
    {
        LevelManager.Warn -= OnWarn;
        LevelManager.ScoreChanged -= UpdateScore;
        LevelManager.LevelCompleted -= ShowEndScreen;
    }

    private void Start()
    {
        UpdateHighScore(LevelManager.HighScore);
    }

    private void OnWarn(string warning)
    {
        Warn(warning, 2);
    }

    public void Warn(string warning, float time)
    {
        if(_warningRoutine != null)
            StopCoroutine(_warningRoutine);
        _warningRoutine = StartCoroutine(ShowWarningRoutine(warning, time));
    }

    private IEnumerator ShowWarningRoutine(string warning, float duration)
    {
        warningText.text = warning;
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        warningText.gameObject.SetActive(false);
    }

    private void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }

    private void ShowEndScreen()
    {
        DOVirtual.DelayedCall(1, () => endScreen.SetActive(true));
    }

    private void UpdateHighScore(int score)
    {
        // highScoreText.text = score.ToString();
    }

    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

}
