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
    [SerializeField] private Button reloadButton;
    
    public static UiManager Instance;

    private Coroutine _warningRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    private void OnEnable()
    {
        reloadButton.onClick.AddListener(ReloadGame);
        LevelManager.LevelCompleted += ShowEndScreen;
    }

    private void OnDisable()
    {
        reloadButton.onClick.RemoveListener(ReloadGame);
        LevelManager.LevelCompleted -= ShowEndScreen;
    }

    private void Start()
    {
    }

    private void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }

    private void ShowEndScreen()
    {
        DOVirtual.DelayedCall(1, () => endScreen.SetActive(true));
    }


}
