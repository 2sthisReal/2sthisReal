using System.Linq;
using UnityEngine;

namespace Jang
{
    public class StagePresetManager : MonoBehaviour
    {
        [SerializeField] StagePreset[] presets; // 스테이지 프리셋들

        void Awake()
        {   
            // Resources/StagePresets/ 경로에 존재하는 모든 StagePreset 파일 불러오기
            presets = Resources.LoadAll<StagePreset>("StagePresets");
        }

        // 랜덤한 프리셋 반환
        public StagePreset GetRandomPreset()
        {
            int idx = Random.Range(0, presets.Length);

            return presets[idx];
        }
    }
}
