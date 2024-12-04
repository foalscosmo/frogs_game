using System.Collections.Generic;
using Frog;
using UnityEngine;

namespace Managers
{
    public class FrogManager : MonoBehaviour
    {
        // Reference to the duck spawner
        [SerializeField] private FrogSpawner frogSpawner; // Reference to the duck spawner.
        
        // List of snapped ducks
        [SerializeField] private List<DragObject> snappedFrogs; // List of ducks that are snapped.
        
        // Property to access snapped ducks
        public List<DragObject> SnappedFrogs => snappedFrogs; // Accessor to the list of snapped ducks.
        
        // Property to access duck spawner
        public FrogSpawner FrogSpawner => frogSpawner; // Accessor to the duck spawner.
        
    }
}