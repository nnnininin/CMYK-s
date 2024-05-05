using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private SaveLoadManager saveLoadManager;

        public void LoadScene(string sceneName)
        {
            //GameDataをES3に保存
            saveLoadManager.SaveGameData();
            //シーンをロード
            SceneManager.LoadScene(sceneName);
        }
    }
}