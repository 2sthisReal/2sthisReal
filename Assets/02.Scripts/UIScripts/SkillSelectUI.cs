using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene 
{
    public class SkillSelectUI : BaseUI
    {
        [SerializeField] private Button skillSelectButton;
        [SerializeField] private Button skillCancelButton;
        [SerializeField] private Button RandomSkill0;
        [SerializeField] private Button RandomSkill1;
        [SerializeField] private Button RandomSkill2;
        protected override GameState GetUIState()
        {
            return GameState.SkillSelect;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            skillSelectButton.onClick.AddListener(
                () => 
                {
                    this.SetActive(GameState.InGame);
                    //GameManager.Instance.ChangeState(GameState.InGame); 
                });
            skillCancelButton.onClick.AddListener(
                () =>
                {
                    Application.Quit();
                    GameManager.Instance.ChangeState(GameState.InGame);
                });
        }
    }
}