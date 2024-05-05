using UnityEngine;
using UnityEngine.InputSystem;

namespace Util.Input
{
    public class InputActionMapManager : MonoBehaviour
    {
        [SerializeField]
        private InputActionAsset inputAsset;

        private InputActionMap InGameMap { get; set; }
        private InputActionMap OutGameMap { get; set; }
        private InputActionMap InGamePauseMap{ get; set; }

        private const string InGame = "InGame";
        private const string OutGame = "OutGame";
        private const string InGamePause = "InGamePause";
        
        public void Awake()
        {
            InGameMap = inputAsset.FindActionMap(InGame);
            OutGameMap = inputAsset.FindActionMap(OutGame);
            InGamePauseMap = inputAsset.FindActionMap(InGamePause);
        }
        public void OnEnable()
        {
            OutGameDisable();
        }

        public void OnDisable()
        {
            OutGameEnable();
        }
        
        public void OutGameDisable()
        {
            InGameMap.Enable();
            OutGameMap.Disable();
            InGamePauseMap.Enable();
        }
        
        public void OutGameEnable()
        {
            InGameMap.Disable();
            OutGameMap.Enable();
            InGamePauseMap.Disable();
        }
        
        public void AllDisable()
        {
            InGameMap.Disable();
            OutGameMap.Disable();
            InGamePauseMap.Disable();
        }
    }
}
