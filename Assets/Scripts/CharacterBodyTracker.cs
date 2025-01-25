using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTracker : MonoBehaviour
{

    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    private int emissionRate = 300;

    private void Start()
    {
        particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>());
    }
    // Start is called before the first frame update
    public void BadHit()
    {
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            emissionRate -= 100;
            ParticleSystem.EmissionModule emission = particleSystem.emission;
            emission.rateOverTime = emissionRate;
            if (emissionRate == 0)
            {
                GameOver();
            }
        }
    }

    public void HealthyHit()
    {
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            emissionRate = (emissionRate == 300) ? 300 : emissionRate + 100;

            ParticleSystem.EmissionModule emission = particleSystem.emission;
            emission.rateOverTime = emissionRate;
        }
    }
    public void GameOver()
    {
        Debug.Log("ohno");
    }
}
