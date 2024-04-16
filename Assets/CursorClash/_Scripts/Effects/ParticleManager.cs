using System.Collections.Generic;
using UnityEngine;

namespace MichiTheDev
{
    public class ParticleManager : MonoBehaviour
    {
        private static Dictionary<string, ParticleSystem> _particles = new();

        private void Awake()
        {
            ParticleSystem[] particles = Resources.LoadAll<ParticleSystem>("Particles");
            foreach(ParticleSystem particle in particles)
            {
                _particles.Add(particle.name, particle);
            }
            
            DontDestroyOnLoad(gameObject);
        }

        public static void SpawnParticle(string name, Vector2 location, bool destroy = true)
        {
            Instantiate(_particles[name], location, Quaternion.identity);
        }
    }
}
