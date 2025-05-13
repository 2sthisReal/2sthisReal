using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene
{
    public class PauseUI : BaseUI
    {
        [SerializeField] private Button breakButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private List<Image> skillImageList;
        [SerializeField] private List<TextMeshProUGUI> skillNameList;
        protected override GameState GetUIState()
        {
            return GameState.Pause;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            List<SkillConfig> selectedSkillList = GameManager.Instance.skillManager.GetAll();
            for(int i = 0; i < skillImageList.Count; i++)
            {
                if(selectedSkillList.Count > i)
                {
                    skillImageList[i].sprite = selectedSkillList[i].skillIcon;
                    skillNameList[i].SetText(selectedSkillList[i].skillName);
                }
                else
                {
                    skillNameList[i].SetText("");
                }
            }
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            this.gameObject.SetActive(false);
            breakButton.onClick.AddListener(
                () =>
                {
                    this.gameObject.SetActive(false);
                    //GameManager.Instance.ChangeState(GameState.InGame);
                });
            homeButton.onClick.AddListener(
                () =>
                {
                    this.gameObject.SetActive(false);
                    GameManager.Instance.ChangeState(GameState.MainMenu);
                });
            exitButton.onClick.AddListener(
                () =>
                {
                    Application.Quit();
                });
        }
    }
}