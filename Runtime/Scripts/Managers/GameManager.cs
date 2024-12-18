using System;
using System.Collections;
using System.Text.RegularExpressions;
using com.appidea.MiniGamePlatform.CommunicationAPI;
using DG.Tweening;
using Frog;
using Hint;
using Particle;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private FrogManager frogManager; // Reference to the DuckManager.
        [SerializeField] private BucketManager bucketManager; // Reference to the BucketManager.
        [SerializeField] private ColorManager colorManager; // Reference to the ColorManager.
        [SerializeField] private SoundManager soundManager; // Reference to the SoundManager.
        [SerializeField] private ParticleManager particleManager; // Reference to the ParticleManager.
        [SerializeField] private UiManager uiManager; // Reference to the UiManager.
        [SerializeField] private HintManager hintManager; // Reference to the HintManager.
        [SerializeField] private LevelManager levelManager; // Reference to the LevelManager.
        private MatchColorFrogsEntryPoint _entryPoint;
        [SerializeField] private Button exitButton;

        private void OnEnable()
        {
            TouchSimulation.Enable();
            bucketManager.BucketSpawner.OnBucketsSpawn += frogManager.FrogSpawner.SetActiveFrogsWithDelay;
            frogManager.FrogSpawner.OnFrogsSpawn += SetHintTimer;
            foreach (var dragged in frogManager.SnappedFrogs) dragged.OnFrogDrag += HandleDragging;
            foreach (var snapped in frogManager.SnappedFrogs) snapped.OnFrogSpawn += HandleFrogDraggable;
            foreach (var snapped in frogManager.SnappedFrogs) snapped.OnCorrectSnap += SetMissionFalse;
            foreach (var snapped in frogManager.SnappedFrogs) snapped.OnInCorrectSnap += soundManager.PlayWrongSound;
            foreach (var snapped in frogManager.SnappedFrogs) snapped.OnInCorrectSnap += HandleDragOnInCorrectSnap;
            foreach (var snapped in frogManager.SnappedFrogs) snapped.OnTransformChange += particleManager.EnableSnappedParticle;
            foreach (var snapped in frogManager.SnappedFrogs) snapped.OnCorrectSnap += CorrectEvent;
            foreach (var frog in frogManager.SnappedFrogs) frog.OnCorrectSnapWithSkin += soundManager.SetColorSound;
            levelManager.OnSnapIndexIncrease += FinishLevel;
            levelManager.OnLevelChange += GoToNextLevel;
           // levelManager.OnGameFinish += particleManager.HandleFinishParticle;
            levelManager.OnGameFinish += soundManager.PlayFinishGameSound;
           // levelManager.OnGameFinish += particleManager.OnFinishPlayParticle;
            levelManager.OnGameFinish += soundManager.PlayFinishMusicSource;
            levelManager.OnGameFinish += SetFinishForPackage;
        }

        // Unsubscribe from events when the object is disabled

        private void OnDisable()
        {
            TouchSimulation.Disable();
            bucketManager.BucketSpawner.OnBucketsSpawn -= frogManager.FrogSpawner.SetActiveFrogsWithDelay;
            frogManager.FrogSpawner.OnFrogsSpawn -= SetHintTimer;
            foreach (var dragged in frogManager.SnappedFrogs) dragged.OnFrogDrag -= HandleDragging;
            foreach (var snapped in frogManager.SnappedFrogs) snapped.OnFrogSpawn -= HandleFrogDraggable;
            foreach (var snapped in frogManager.SnappedFrogs) snapped.OnCorrectSnap -= SetMissionFalse;
            foreach (var snapped in frogManager.SnappedFrogs) snapped.OnInCorrectSnap -= soundManager.PlayWrongSound;
            foreach (var snapped in frogManager.SnappedFrogs) snapped.OnInCorrectSnap -= HandleDragOnInCorrectSnap;
            foreach (var snapped in frogManager.SnappedFrogs) snapped.OnTransformChange -= particleManager.EnableSnappedParticle;
            foreach (var snapped in frogManager.SnappedFrogs) snapped.OnCorrectSnap -= CorrectEvent;
            foreach (var frog in frogManager.SnappedFrogs) frog.OnCorrectSnapWithSkin -= soundManager.SetColorSound;
            levelManager.OnSnapIndexIncrease -= FinishLevel;
            levelManager.OnLevelChange -= GoToNextLevel;
            //levelManager.OnGameFinish -= particleManager.HandleFinishParticle;
            levelManager.OnGameFinish -= soundManager.PlayFinishGameSound;
            //levelManager.OnGameFinish -= particleManager.OnFinishPlayParticle;
            levelManager.OnGameFinish -= soundManager.PlayFinishMusicSource;
            levelManager.OnGameFinish -= SetFinishForPackage;
        }

        private void Awake()
        {
            Application.targetFrameRate = 120;
            exitButton.onClick.AddListener(SetExitOnButton);
        }

        private void SetMissionFalse()
        {
            soundManager.MissionSound(false);
            hintManager.SetHintObjectActive(false);
            soundManager.AudioSources[4].volume = 0;
            uiManager.SetMissionTextFalse();
        }

        private bool isCooldownActive = false;

        private void HandleDragging(int index)
        {
            if (isCooldownActive) return; 
            for (int i = 0; i < frogManager.SnappedFrogs.Count; i++)
            {
                // Disable canDrag for all frogs except the one at the specified index
                frogManager.SnappedFrogs[i].CanDrag = (i == index);
            }
        }

        private void CancelDragging()
        {
            // Disable dragging for all frogs
            foreach (var frog in frogManager.SnappedFrogs)
            {
                frog.CanDrag = false;
            }

            // Start cooldown
            StartCoroutine(DraggingCooldown());
        }

        private IEnumerator DraggingCooldown()
        {
            isCooldownActive = true;

            // Wait for 0.3 seconds
            yield return new WaitForSeconds(0.3f);

            // Enable dragging for all frogs after cooldown
            foreach (var frog in frogManager.SnappedFrogs)
            {
                frog.CanDrag = true;
            }

            isCooldownActive = false;
        }
        
        
        
        private int spawnCounter;
        private void HandleFrogDraggable()
        {
            if (spawnCounter < 3)
            {
                spawnCounter++;
                if (spawnCounter != 3) return;
                foreach (var spawned in frogManager.SnappedFrogs) spawned.CanDrag = true;
            }
        }
        private void SetHintTimer()
        {
            hintManager.ActivateTimer();
            
            hintManager.HoldStartHint();
        }

        private void CorrectEvent()
        {
            levelManager.IncrementSnapIndex();
            soundManager.PlayCorrectSnapSounds();
            hintManager.HintIndexHandler();
            foreach (var drag in frogManager.SnappedFrogs) drag.CanDrag = true;
            CancelDragging();
        }

        private void HandleDragOnInCorrectSnap()
        {
            foreach (var drag in frogManager.SnappedFrogs) drag.CanDrag = true;
            CancelDragging();
        }

        private void FinishLevel()
        {
            soundManager.PlayStarsSound();
            hintManager.StartTimer = false;
            bucketManager.BucketSpawner.DisableAllBuckets();
            frogManager.FrogSpawner.DisableFrogs();
            uiManager.FillProgressBar(levelManager.LevelIndex);
            if (levelManager.LevelIndex < 4) soundManager.PlayRateSound();
        }
        
        private void GoToNextLevel()
        {
            StartCoroutine(SetNewLevel());
        }

        private IEnumerator SetNewLevel()
        {
            yield return new WaitForSecondsRealtime(1f);
            bucketManager.BucketSpawner.SpawnBuckets();
            frogManager.FrogSpawner.SpawnFrogsToStartPoint();
            colorManager.SetColorToObjects();
        }

        public void SetEntryPoint(MatchColorFrogsEntryPoint entryPoint)
        {
            _entryPoint = entryPoint;
        }
        private void SetFinishForPackage()
        {
            StartCoroutine(FinishAfterFireworks());
        }
        private IEnumerator FinishAfterFireworks()
        {
            yield return new WaitForSecondsRealtime(5f);
            _entryPoint.InvokeGameFinished();
        }

        private void SetExitOnButton()
        {
            _entryPoint.InvokeGameFinished();
        }
    }
}
