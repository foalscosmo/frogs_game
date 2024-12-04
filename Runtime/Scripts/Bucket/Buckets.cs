using System.Collections.Generic;
using UnityEngine;

namespace Bucket
{
    public class Buckets : MonoBehaviour
    {
        [SerializeField] private List<GameObject> activeBuckets = new();
        public List<GameObject> ActiveBuckets => activeBuckets;

    }
}