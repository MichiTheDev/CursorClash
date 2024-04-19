using System.Collections.Generic;
using CartoonFX;
using UnityEngine;

namespace MichiTheDev
{
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance { private set; get; }
        
        private static Dictionary<string, ParticleSystem> _particles = new();

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            ParticleSystem[] particles = Resources.LoadAll<ParticleSystem>("Particles");
            foreach(ParticleSystem particle in particles)
            {
                _particles.Add(particle.name, particle);
            }
            DontDestroyOnLoad(gameObject);
        }

        public static void SpawnParticle(string name, Vector3 location, CFXR_Effect.ClearBehavior clearBehavior = CFXR_Effect.ClearBehavior.Destroy)
        {
            ParticleSystem particleSystem = Instantiate(_particles[name], location, Quaternion.identity);
            particleSystem.GetComponent<CFXR_Effect>().clearBehavior = clearBehavior;
        }
    }
}
