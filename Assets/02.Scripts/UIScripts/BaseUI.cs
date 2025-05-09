using UnityEngine;

namespace SWScene
{
    public abstract class BaseUI : MonoBehaviour
    {
        protected UIManager uiManager;

        public virtual void Init(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        protected abstract GameState GetUIState();
        public void SetActive(GameState state)
        {
            this.gameObject.SetActive(GetUIState() == state);
        }
    }

}