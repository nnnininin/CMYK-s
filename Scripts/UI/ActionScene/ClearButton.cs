using Scene;
using UnityEngine;

namespace UI.ActionScene
{
    public class ClearButton: MonoBehaviour
    {
        [SerializeField] 
        private SceneLoader sceneLoader;
        
        private const string NextSceneName = "StartScene";
        
        public void OnClickLoad()
        {
            sceneLoader.LoadScene(NextSceneName);
        }
    }
}