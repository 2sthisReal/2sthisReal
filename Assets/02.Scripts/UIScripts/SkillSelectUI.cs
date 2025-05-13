using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene 
{
    public class SkillSelectUI : BaseUI
    {
        [SerializeField] private Button skillCancelButton;
        [SerializeField] private List<Button> RandomSkillButton;
        [SerializeField] private List<TextMeshProUGUI> RandomSkillTooltip;
        private List<SkillConfig> skillConfig;
        protected override GameState GetUIState()
        {
            return GameState.SkillSelect;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            this.gameObject.SetActive(false);
            skillConfig = new List<SkillConfig>(RandomSkillButton.Count);
            for (int i = 0; i < RandomSkillButton.Count; i++)
            {
                int index = i;
                RandomSkillButton[index].onClick.AddListener(
                    () =>
                    {
                        if(skillConfig.Count > index)
                        {
                            GameManager.Instance?.skillManager.AddSkill(skillConfig[index]);
                        }
                        UIManager.instance.SkillSelectUISetActive(false);
                    });
            }
            skillCancelButton.onClick.AddListener(
                () =>
                {
                    UIManager.instance.SkillSelectUISetActive(false);
                });
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SkillManager skillManager = GameManager.Instance.skillManager;
            skillConfig = skillManager.GetRandomSkills();
            Debug.Log(skillConfig.Count);
            for (int i = 0; i < skillConfig.Count; i++)
            {
                if (i < RandomSkillButton.Count)
                {
                    RandomSkillButton[i].image.sprite = skillConfig[i].skillIcon;
                    RandomSkillTooltip[i].SetText(skillConfig[i].skillName);
                }
            }
        }


        protected override void Start()
        {
            base.Start();
        }
    }
}