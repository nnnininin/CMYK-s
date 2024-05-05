using Player.Util;
using Scene;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ActionScene
{
    public class GameOverManager:MonoBehaviour
    {
        [Inject] private GlobalPlayerEventManager globalPlayerEventManager;  
        [SerializeField] private SceneLoader sceneLoader;
        
        private void Start()
        {
            globalPlayerEventManager.OnGlobalDeath.Subscribe(_ =>
            {
                sceneLoader.LoadScene("StartScene");
            }).AddTo(this);
        }
    }
}