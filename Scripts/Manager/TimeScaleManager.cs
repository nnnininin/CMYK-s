using UnityEngine;

namespace Manager
{
    public class TimeScaleManager : MonoBehaviour
    {
        //0~200の間で設定する
        [SerializeField, Range(0, 100)]
        private int timeScaleValue;
        private int _preTimeScaleValue;

        private void Start()
        {
            SetTimeScale();
        }

        private void Update()
        {
            UpdateTimeScale();
        }

        private void SetTimeScale()
        {
            _preTimeScaleValue = timeScaleValue;
            Time.timeScale = timeScaleValue / 100f;
        }

        private void UpdateTimeScale()
        {
            if (timeScaleValue == _preTimeScaleValue)
                return;
            SetTimeScale();
        }
        
        public void ResumeTime()
        {
            timeScaleValue = 100;
        }
        
        public void StopTime()
        {
            timeScaleValue = 0;
        }
    }
}
