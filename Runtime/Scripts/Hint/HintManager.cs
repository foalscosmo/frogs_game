using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bucket;
using DG.Tweening;
using Frog;
using UnityEngine;

namespace Hint
{
    public class HintManager : MonoBehaviour
    {
        [SerializeField] private Buckets floats; 
        [SerializeField] private Transform hintObj; 
        [SerializeField] private List<DragObject> dragObjects = new(); 
        [SerializeField] private int hintIndex; 
        private Vector2 tPosition; 
        public bool StartTimer { get; set; } 
        private float afkTimer; 
        private const float AfkCheckInterval = 8f; 
        
        private void Update()
        {
            if (StartTimer)
            {
                afkTimer += Time.deltaTime;
                if (!(afkTimer >= AfkCheckInterval)) return;
                CheckAfk(); 
            }
            else 
            {
                afkTimer = 0; 
            }
        }
        
        private bool hasStarted;

        public void HoldStartHint()
        {
            if (hasStarted) return;
            StartCoroutine(SetHintOnStart());
            hasStarted = true;
        }

        private IEnumerator SetHintOnStart()
        {
            yield return new WaitForSecondsRealtime(1f);
            foreach (var t in dragObjects.Where(t => t.gameObject.layer == hintIndex))
            {
                var transform1 = t.transform;
                var position = transform1.position;
                hintObj.transform.position = position;
                tPosition = position;
                SetHintObjectActive(true);
            }

            StartCoroutine(HoldOnHintStart());
        }


        private void CheckAfk()
        {
            if (!StartTimer) return; 
            afkTimer = 0f; 
            foreach (var t in dragObjects.Where(t => t.gameObject.layer == hintIndex))
            {
                var transform1 = t.transform;
                var position = transform1.position;
                hintObj.transform.position = position;
                tPosition = position;
                SetHintObjectActive(true);
            }

            StartCoroutine(HoldOnHintStart());
        }

        private IEnumerator HoldOnHintStart()
        {
            yield return new WaitForSecondsRealtime(0.3f);
            foreach (var t in floats.ActiveBuckets) 
            {
                if (t.gameObject.layer == hintIndex) 
                {
                    hintObj.transform.DOMove(t.transform.position, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        hintObj.transform.position = tPosition;
                        StartCoroutine(HoldOnHintLast(t));
                    });
                }
            }
        }

        private IEnumerator HoldOnHintLast(GameObject frog)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            hintObj.transform.DOMove(frog.transform.position, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                SetHintObjectActive(false);
            });
        }

        public void SetHintObjectActive(bool isActive)
        {
            hintObj.gameObject.SetActive(isActive);
        }

        public void ActivateTimer()
        {
            StartTimer = true; 
        }
    
        public void HintIndexHandler()
        {
            afkTimer = 0; 

            foreach (var obj in dragObjects.Where(obj => !obj.IsSnapped))
            {
                hintIndex = obj.gameObject.layer; 
                break;
            }
        
            if (hintIndex > 8) 
            {
                hintIndex = 6;
            }
        }
    }
}
