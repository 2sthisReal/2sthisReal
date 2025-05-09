using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance {  get; private set; }

    private Dictionary<GameState, string> sceneByState = new Dictionary<GameState, string>()
    {
        { GameState.MainMenu, "MainMenuScene" },
        { GameState.InGame, "InGameScene" },
        { GameState.Victory, "VictoryScene" },
        { GameState.Preparing, "PreparingScene" },
        { GameState.StageClear, "StageClearScene" },
        { GameState.Pause, "PauseScene"},
        { GameState.GameOver, "GameOverScene" }
    };

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneForState(GameState state)
    {
        if (!sceneByState.TryGetValue(state, out string sceneName))
        {
            Debug.LogWarning($"No scene mapped for state: {state}");
            return;
        }

        StartCoroutine(LoadSceneAsync(sceneName, () =>
        {
            GameManager.Instance.SetState(state);
        }));
    }

    public void LoadScene(string sceneName, Action onLoaded = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, onLoaded));
    }

    private IEnumerator LoadSceneAsync(string sceneName, Action onLoaded)
    {
        // �ε� UI�� �ִٸ� ���⼭ Ȱ��ȭ
        // ����)
        // ShowLoadingUI();
        // yield return SceneManager.LoadSceneAsync(sceneName);
        // HideLoadingUI();

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            yield return null;
        }

        onLoaded?.Invoke();
    }
}
