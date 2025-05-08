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
        { GameState.MainMenu, "MainScene" },
        { GameState.InGame, "StageScene" }
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

    public void LoadSceneForState(GameState state, Action onLoaded = null)
    {
        if (!sceneByState.ContainsKey(state))
        {
            Debug.LogWarning($"No scene mapped for state: {state}");
            return;
        }

        string sceneName = sceneByState[state];
        StartCoroutine(LoadSceneAsync(sceneName, () =>
        {
            GameManager.Instance.ChangeState(state);
            onLoaded?.Invoke();
        }));
    }

    public void LoadScene(string sceneName, Action onLoaded = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, onLoaded));
    }

    private IEnumerator LoadSceneAsync(string sceneName, Action onLoaded)
    {
        // 로딩 UI가 있다면 여기서 활성화
        // 예시)
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
