using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace Frog
{
    public class DragObject : MonoBehaviour
    {
        [SerializeField] private Transform originalPosition;
        [SerializeField] private List<Transform> targetTransforms = new();
        [SerializeField] private MeshRenderer mesh;
        [SerializeField] private SkeletonMecanim skeleton;
        [SerializeField] private GameObject shadowObj;
        [SerializeField] private GameObject idleShadowObj;
        [SerializeField] private AudioSource frogSound;
        [SerializeField] private int frogIndex;
        private Vector2 offset;
        private Camera mainCamera;
        private Finger activeFinger;
        public bool IsSnapped { get; private set; }
        public bool IsDragging { get; private set; }
        public bool CanDrag { get; set; }
        public MeshRenderer SkeletonMeshRenderer { get; private set; }
        public delegate void SnapAction();
        public delegate void SnapActionWithSkin(string skinName);
        public event SnapAction OnCorrectSnap; 
        public event SnapActionWithSkin OnCorrectSnapWithSkin;
        public event Action OnInCorrectSnap;
        public event Action OnFrogSpawn;
        public event Action<Transform> OnTransformChange;

        public event Action<int> OnFrogDrag;
        
        private void Awake()
        {
            mainCamera = Camera.main;
            SkeletonMeshRenderer = skeleton.GetComponent<MeshRenderer>();
            EnhancedTouchSupport.Enable();
        }

        private void OnEnable()
        {
            IsSnapped = false;
            ETouch.Touch.onFingerDown += HandleFingerDown;
            ETouch.Touch.onFingerUp += HandleFingerUp;
            ETouch.Touch.onFingerMove += HandleFingerMove;
        }

        private void OnDisable()
        {
            ETouch.Touch.onFingerDown -= HandleFingerDown;
            ETouch.Touch.onFingerUp -= HandleFingerUp;
            ETouch.Touch.onFingerMove -= HandleFingerMove;
        }

        private void HandleFingerDown(Finger finger)
        {
            if(!CanDrag) return;
            if (IsSnapped || IsDragging || activeFinger != null) return;
            Vector2 touchPosition = mainCamera.ScreenToWorldPoint(finger.screenPosition);
            if (!IsTouchingObject(touchPosition)) return;
            OnFrogDrag?.Invoke(frogIndex);
            IsDragging = true;
            activeFinger = finger;
            offset = touchPosition - (Vector2)transform.position;
            shadowObj.SetActive(false);
            idleShadowObj.SetActive(false);
            transform.DOScale(1.2f, 0.2f);
            SkeletonMeshRenderer.sortingOrder = 7;
        }

        private void HandleFingerMove(Finger finger)
        {
            if (finger != activeFinger || !IsDragging) return;
            Vector2 touchPosition = mainCamera.ScreenToWorldPoint(finger.screenPosition);
            transform.position = touchPosition - offset;
        }

        private void HandleFingerUp(Finger finger)
        {
            if(!CanDrag) return;
            if (finger != activeFinger) return;

            
            IsDragging = false;
            activeFinger = null;
            transform.DOScale(1f, 0.2f);
            foreach (var target in targetTransforms)
            {
                var transform1 = transform;
                var position = transform1.position;
                var distanceToBucket = Vector2.Distance(new Vector2(position.x, position.y + 2.5f), target.position);
                if (!(distanceToBucket <= 3.5f) || gameObject.layer != target.gameObject.layer) continue;
                SnapToTarget(target);
                break;
            }

            if (IsSnapped) return;
            ReturnToOriginalPosition();
            OnInCorrectSnap?.Invoke();
        }
        
        private bool IsTouchingObject(Vector2 touchPosition)
        {
            var hit = Physics2D.OverlapPoint(touchPosition);
            return hit != null && hit.transform == transform;
        }

        private void SnapToTarget(Transform target)
        {
            var position = target.position;
            Vector3 targetPosition = new Vector3(position.x, position.y - 0.6f);
            SkeletonMeshRenderer.sortingOrder = 6;
            OnTransformChange?.Invoke(target);
            IsSnapped = true;
            transform.SetParent(target);
            transform.DOScale(0.63f, 0.2f);
            transform.DOMove(targetPosition, 0.3f).SetEase(Ease.OutBack);
            OnCorrectSnap?.Invoke();
            OnCorrectSnapWithSkin?.Invoke(skeleton.Skeleton.Skin.Name);
        }

        private void ReturnToOriginalPosition()
        {
            idleShadowObj.transform.localScale = Vector3.zero;
            idleShadowObj.SetActive(true);
            idleShadowObj.transform.DOScale(1f,0.35f);
            transform.DOMove(originalPosition.position, 0.3f).OnComplete(() =>
            {
                SkeletonMeshRenderer.sortingOrder = 5;
            });
        }
        

        //event on animation
        public void ActivateMeshAfterStart()
        {
          shadowObj.SetActive(true);
          mesh.enabled = true;
        }

        public void SetFrogSoundOnAnimation()
        {
            frogSound.Play();

        }

        public void CanDragObject()
        {
            OnFrogSpawn?.Invoke();
        }

        public void DisableMeshAfterLeaving()
        {
            mesh.enabled = false;
        } 
        
        // private void OnDrawGizmos()
        // {
        //     if (targetTransforms == null || targetTransforms.Count == 0) return;
        //
        //     foreach (var target in targetTransforms)
        //     {
        //         float distanceToBucket = Vector2.Distance(new Vector2(transform.position.x, transform.position.y+2.5f), target.position);
        //
        //         // Update Gizmo visualization
        //         Gizmos.color = distanceToBucket < 3.5f ? Color.green : Color.red; // Green if within snapping radius, red otherwise
        //         Gizmos.DrawWireSphere(target.position, 3.5f); // Visualize snap radius
        //         Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y+2.5f), target.position); // Draw a line to the target
        //     }
        // }
    }
}