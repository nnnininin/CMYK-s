using UnityEngine;

namespace UI.StartScene
{
    public class SoundManager:MonoBehaviour
    {
        [SerializeField] private AudioSource bgm;
        private void Awake()
        {
            bgm.Play();
        }
    }
}