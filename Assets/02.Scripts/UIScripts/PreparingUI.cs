using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWScene
{
    public class PreparingUI : BaseUI
    {
        protected override GameState GetUIState()
        {
            return GameState.Preparing;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
        }
    }
}