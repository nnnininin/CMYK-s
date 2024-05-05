using UnityEngine;

namespace Util.Animation
{
    public class ParticleFPSController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        private float timeBetweenUpdates;
        private float skippedTime;

        [SerializeField, Range(1, 60)] private int fps;

        private void Start()
        {
            timeBetweenUpdates = 1f / fps;
            particle.Simulate(0.0f, true, true);
            particle.Play();
        }

        private void Update()
        {
            skippedTime += Time.deltaTime;

            if (!(skippedTime >= timeBetweenUpdates)) return;
            particle.Simulate(skippedTime, true, false);
            skippedTime = 0f;
        }
    }
}