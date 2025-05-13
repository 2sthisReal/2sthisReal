using UnityEngine;
using UnityEngine.SceneManagement;

namespace SWScene
{
    public abstract class BaseUI : MonoBehaviour
    {
        protected UIManager uiManager;

        protected virtual void Start()
        {

        }
        protected virtual void Awake()
        {

        }

        public virtual void Init(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        protected virtual void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
        }

        protected virtual void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        protected abstract GameState GetUIState();
        public void SetActive(GameState state)
        {
            this.gameObject.SetActive(GetUIState() == state);
        }

        protected virtual void OnSceneLoaded()
        {

        }

        protected virtual void Update()
        {

        }
    }

}