using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene
{
    public class StageClearUI : BaseUI
    {
        protected override GameState GetUIState()
        {
            return GameState.StageClear;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
        }
    }
}