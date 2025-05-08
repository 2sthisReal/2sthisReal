using UnityEngine;

namespace Jang
{
    public class StagePresetManager : MonoBehaviour
    {
        [SerializeField] StagePreset[] presets;

        void Awake()
        {
            presets = Resources.LoadAll<StagePreset>("StagePresets");
        }

        public StagePreset GetRandomPreset()
        {
            int idx = Random.Range(0, presets.Length);

            return presets[idx];
        }
    }
}
