using Bucket;
using UnityEngine;

namespace Managers
{
    public class BucketManager : MonoBehaviour
    {
        [SerializeField] private BucketSpawner bucketSpawner;
        public BucketSpawner BucketSpawner => bucketSpawner;
    }
}