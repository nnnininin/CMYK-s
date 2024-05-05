using UnityEngine;

namespace UI.ActionScene
{
    public class SoundManager:MonoBehaviour
    {
        [SerializeField] private AudioSource bgm;
        [SerializeField] private AudioSource deathSound;
        [SerializeField] private AudioSource colorLevelUpSound;
        [SerializeField] private AudioSource levelUpSound;
        [SerializeField] private AudioSource playerDamageSound;
        [SerializeField] private AudioSource clearSound;
        
        public void Awake()
        {
            bgm.Play();
        }
        
        public void PlayDeathSound()
        {
            deathSound.Play();
        }
        
        public void PlayColorLevelUpSound()
        {
            colorLevelUpSound.Play();
        }
        
        public void PlayLevelUpSound()
        {
            levelUpSound.Play();
        }
        
        public void PlayPlayerDamageSound()
        {
            playerDamageSound.Play();
        }
        public void PlayClearSound()
        {
            clearSound.Play();
        }
    }
}