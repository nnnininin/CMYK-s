using Manager;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Util.Input
{
    public class PauseInput : MonoBehaviour
    {
        [SerializeField]
        private TimeScaleManager timeScaleManager;
        
        [SerializeField]
        private InputActionMapManager inputActionMapManager;
        
        public Subject<Unit> OnPause { get; } = new();
        public Subject<Unit> OnResume { get; } = new();
        
        public void OnPauseInput(InputAction.CallbackContext context)
        {
            switch (context)
            {
                case { phase: InputActionPhase.Performed }:
                    Pause();
                    break;
            }
        }
        
        public void OnResumeInput(InputAction.CallbackContext context)
        {
            switch (context)
            {
                case { phase: InputActionPhase.Performed }:
                    Resume();
                    break;
            }
        }

        private void Pause()
        {
            inputActionMapManager.OutGameEnable();
            timeScaleManager.StopTime();
            OnPause.OnNext(Unit.Default);
        }

        private void Resume()
        {
            inputActionMapManager.OutGameDisable();
            timeScaleManager.ResumeTime();
            OnResume.OnNext(Unit.Default);
        }
    }
}
