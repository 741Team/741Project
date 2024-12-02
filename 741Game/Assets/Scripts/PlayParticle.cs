using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayParticles()
    {
        particle.Play();
    }

    private void OnEnable()
    {
        particle = GetComponent<ParticleSystem>();
        PlayParticles();
    }
}
