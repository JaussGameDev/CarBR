using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particles;
    [SerializeField] private List<ParticleSystem> particlesBoost;
    [SerializeField] private List<ParticleSystem> particlesExplosion;

    void Awake()
    {
        particles.AddRange(GetComponentsInChildren<ParticleSystem>());
        foreach (ParticleSystem particle in particles)
        {
            if (particle.name == "boost") particlesBoost.Add(particle);
            if (particle.name == "explosion") particlesExplosion.Add(particle);
            particle.Stop();
        }

    }

    // Update is called once per frame
    public List<ParticleSystem> GetBoosts()
    {
        return particlesBoost;
    }
    public List<ParticleSystem> GetExplosions()
    {
        return particlesExplosion;
    }
    public List<ParticleSystem> GetParticles()
    {
        return particlesBoost;
    }
}
