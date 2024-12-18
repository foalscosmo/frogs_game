using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Particle
{
    public class ParticleManager : MonoBehaviour
    {
        [SerializeField] private GameObject snappedParticle; 
        [SerializeField] private List<ParticleSystem> snappedParticles = new();
       // [SerializeField] private List<GameObject> finishParticle = new();
       // [SerializeField] private GameObject[] objectsToActivate; 
       // [SerializeField] private AudioSource fireWokSound;
        private void Awake()
        {
            DisableSnappedParticle(); 
        }
        // public void OnFinishPlayParticle()
        // {
        //     StartCoroutine(ActivateObjectsWithDelay());
        // }
        //
        // private IEnumerator ActivateObjectsWithDelay()
        // {
        //     foreach (var obj in objectsToActivate)
        //     {
        //         var delay = Random.Range(0.1f, 1f);
        //         yield return new WaitForSeconds(delay);
        //         obj.SetActive(true);
        //         fireWokSound.Play();
        //     }
        // }
    
        // public void HandleFinishParticle()
        // {
        //     foreach (var particle in finishParticle) particle.SetActive(true);
        // }

        public void EnableSnappedParticle(Transform targetTransform)
        {
            snappedParticle.transform.position = targetTransform.position; 
            foreach(var particle in snappedParticles) particle.Play();
        }

        private void DisableSnappedParticle()
        {
            foreach(var particle in snappedParticles) particle.Stop();
        }
    }
}
