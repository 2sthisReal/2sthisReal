using System.Collections;
using Jang;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SWScene
{
    public class InGameUI : BaseUI
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private BaseUI pauseUI;
        [SerializeField] private BaseUI skillSelectUI;

        // InGameUI
        [SerializeField] private TextMeshProUGUI stageText;
        [SerializeField] private CanvasGroup stageUI;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Slider levelSlider;

        protected override GameState GetUIState()
        {
            return GameState.InGame;
        }

        protected override void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            StageManager.onStageStart += UpdateStageText;
            Player.OnExpChanged += UpdateLevelBar;
            Player.OnLevelChanged += UpdateLevelText;

            InitLevelUI();
        }

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "InGameScene")
            {
                pauseUI.gameObject.SetActive(false);
                skillSelectUI.gameObject.SetActive(false);

                

            }
        }

        protected override void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            StageManager.onStageStart -= UpdateStageText;
            Player.OnExpChanged -= UpdateLevelBar;
            Player.OnLevelChanged -= UpdateLevelText;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            pauseUI.Init(uiManager);
            skillSelectUI.Init(uiManager);
            pauseButton?.onClick.AddListener(
                () =>
                {
                    pauseUI.gameObject.SetActive(true);
                    skillSelectUI.gameObject.SetActive(false);
                    GameManager.Instance.PauseGame();
                    //GameManager.Instance.ChangeState(GameState.Pause); 
                });
        }
        void ShowSkillSelectUI()
        {
            pauseUI.gameObject.SetActive(false);
            skillSelectUI.gameObject.SetActive(true);
            GameManager.Instance.PauseGame();
        }

        void UpdateStageText(int currentStage)
        {
            stageText.text = currentStage.ToString();

            StartCoroutine(ShowStageTextUI());
        }

        void UpdateLevelBar(float maxExp, float exp)
        {
            levelSlider.value = exp / maxExp;
        }

        void UpdateLevelText(int level)
        {
            levelText.text = $"Lv.{level}";

            ShowSkillSelectUI();
        }

        IEnumerator ShowStageTextUI()
        {
            float fadeDuration = 0.5f;
            float visibleDuration = 3f;

            stageUI.alpha = 1;
            stageUI.gameObject.SetActive(true);

            yield return new WaitForSeconds(visibleDuration);

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                stageUI.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
                yield return null;
            }

            stageUI.alpha = 0;
            stageUI.gameObject.SetActive(false);
        }

        private void InitLevelUI()
        {
            levelSlider.value = 0f;
            levelText.text = "Lv.1";
        }
    }
}