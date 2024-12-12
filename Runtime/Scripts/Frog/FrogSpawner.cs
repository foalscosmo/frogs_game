using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Frog
{
    public class FrogSpawner : MonoBehaviour
    {
        [SerializeField] private Frogs frogs;
        [SerializeField] private List<Transform> frogSpawnPoints;
        [SerializeField] private List<Animator> animators = new();
        [SerializeField] private List<DragObject> dragObjects = new();
        private static readonly int IdleJump = Animator.StringToHash("IdleJump");
        public event Action OnFrogsSpawn;
        [SerializeField] private LevelManager levelManager;
        
        private void Awake()
        {
            SpawnFrogsToStartPoint();
            StartCoroutine(RandomJumpRoutine());
        }
        

        public void SetActiveFrogsWithDelay()
        {
            StartCoroutine(SetActiveFrogsCoroutine(0.35f));
        }

        private IEnumerator SetActiveFrogsCoroutine(float delayBetweenFrogs)
        {
            for (var index = 0; index < frogs.ActiveFrogs.Count; index++)
            {
                var frog = frogs.ActiveFrogs[index];
                frog.transform.SetParent(null);
                frog.SetActive(true);
                frog.transform.position = frogSpawnPoints[index].position;
                dragObjects[index].SkeletonMeshRenderer.sortingOrder = 5;
                if(index < frogs.ActiveFrogs.Count -1)
                {
                     yield return new WaitForSeconds(delayBetweenFrogs);
                }
            }

            OnFrogsSpawn?.Invoke();
            
        }
        
        private IEnumerator RandomJumpRoutine()
        {
            yield return new WaitForSeconds(10f); // Initial delay after spawning
            while (true)
            {
                List<int> freeFrogs = new List<int>();

                // Gather indices of frogs that are not attached
                for (int i = 0; i < dragObjects.Count; i++)
                {

                    if (!dragObjects[i].IsSnapped)
                    {
                        if (!dragObjects[i].IsDragging)
                        {
                            freeFrogs.Add(i);
                        }
                    }
                }
                
                if (freeFrogs.Count > 0)
                {
                    int randomIndex = freeFrogs[UnityEngine.Random.Range(0, freeFrogs.Count)];
                    animators[randomIndex].SetTrigger(IdleJump);
                }

                yield return new WaitForSeconds(7f); // Wait before triggering the next jump
            }
        }
        
        public void DisableFrogs()
        {
            if(levelManager.LevelIndex == 4) return;
            StartCoroutine(DisableFrogWithDelay());
        }

        private IEnumerator DisableFrogWithDelay()
        {
            yield return new WaitForSeconds(0.9f);
            foreach (var frog in frogs.ActiveFrogs)
            {
                frog.gameObject.SetActive(false);
            }
            foreach(var mesh in dragObjects) mesh.DisableMeshAfterLeaving();
        }

        public void SpawnFrogsToStartPoint()
        {
            for (var index = 0; index < frogs.ActiveFrogs.Count; index++)
            {
                frogs.ActiveFrogs[index].transform.position = frogSpawnPoints[index].position;
            }
        }
    }
}