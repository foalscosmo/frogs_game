using System;
using System.Collections;
using UnityEngine;

namespace Bucket
{
    public class BucketSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints; // Array of spawn points for buckets
        [SerializeField] private Buckets buckets; // Reference to the Buckets script
        public event Action OnBucketsSpawn; // Event triggered when buckets are spawned

        private void Awake()
        {
            foreach (var bucket in buckets.ActiveBuckets) bucket.SetActive(false); // Deactivate all buckets on awake
        }
        
        private void Start()
        {
            SpawnBuckets(); 
        }

        public void SpawnBuckets()
        {
            StartCoroutine(SpawnBucketsWithDelay());
        }

        private IEnumerator SpawnBucketsWithDelay()
        {
            for (var i = 0; i < spawnPoints.Length; i++)
            {
                yield return new WaitForSecondsRealtime(0.2f);
                buckets.ActiveBuckets[i].transform.position = spawnPoints[i].position;
                buckets.ActiveBuckets[i].gameObject.SetActive(true); 
                if (i == 2) OnBucketsSpawn?.Invoke(); 
            }
        }

        public void DisableAllBuckets()
        {
            StartCoroutine(DisableBucketsWithDelay());
        }

        private IEnumerator DisableBucketsWithDelay()
        {
            yield return new WaitForSeconds(0.9f); // Wait for a short delay before disabling buckets
            foreach (var bucket in buckets.ActiveBuckets)
            {
                bucket.gameObject.SetActive(false); // Deactivate each bucket
            }
        }
    }
}