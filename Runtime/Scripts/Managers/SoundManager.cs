using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        // List of audio sources
        [SerializeField] private List<AudioSource> audioSource = new(); // List of audio sources.
        [SerializeField] private List<AudioSource> rateSources = new();
        [SerializeField] private AudioSource colorSource;

        public List<AudioSource> AudioSources
        {
            get => audioSource;
            set => audioSource = value;
        }
        [SerializeField] private List<AudioClip> colorSounds = new();
        [SerializeField] private AudioSource finishMusic;
        [SerializeField] private int index;

        [SerializeField] private bool hasStopped;
        private bool canPlayMissionSound;


        private void Start()
        {
            canPlayMissionSound = true;
            MissionSound(canPlayMissionSound);
        }

        public void MissionSound(bool isAttached)
        {
            StartCoroutine(HandleMissionSoundTimer(isAttached));
        }

        private IEnumerator HandleMissionSoundTimer(bool canPlaySound)
        {
            yield return new WaitForSecondsRealtime(2.2f);
            if (canPlaySound)
            {
                audioSource[4].Play();
            }
        }


        public void PlayRateSound()
        {
            StartCoroutine(RateWithDelay());
        }

        public void PlayFinishMusicSource() => finishMusic.Play();
        public void PlayStarsSound() => audioSource[3].Play();
        
        private float lastPlayTime = -Mathf.Infinity;       // Track the last play time
        public void PlayWrongSound()
        {
            // Check if the cooldown time has passed
            if (Time.time >= lastPlayTime + 3)
            {
                audioSource[5].Play();
                lastPlayTime = Time.time; // Update the last play time
            }
        }


        public void PlayFinishGameSound() =>  audioSource[6].Play();

        public void PlayCorrectSnapSounds() => audioSource[2].Play();
        
        
        private IEnumerator RateWithDelay()
        {
            yield return new WaitForSecondsRealtime(0.7f);
            if (index == 3 && !hasStopped)
            {
                index = 1;
                rateSources[1].volume = 1;
                hasStopped = true;
            }

            rateSources[index].Play();
            index++;
        }

        public void SetColorSound(string skinColor)
        {
            switch (skinColor)
            {
                case "Blue Frog":
                    colorSource.clip = colorSounds[0];
                    colorSource.Play();
                    break;
                case "Green Frog":
                    colorSource.clip = colorSounds[1];
                    colorSource.Play();
                    break;
                case "Orange Frog":
                    colorSource.clip = colorSounds[2];
                    colorSource.Play();
                    break;
                case "Pink Frog":
                    colorSource.clip = colorSounds[3];
                    colorSource.Play();
                    break;
                case "Purple Frog":
                    colorSource.clip = colorSounds[4];
                    colorSource.Play();
                    break;
                case "Yellow Frog":
                    colorSource.clip = colorSounds[5];
                    colorSource.Play();
                    break;
            }
        }
    }
}