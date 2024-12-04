using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private List<Image> progressBarSlots = new();
        [SerializeField] private Sprite completeSlot;
        [SerializeField] private List<Animator> progressAnimators = new();
        [SerializeField] private TextMeshProUGUI missionText;
        private static readonly int Fill = Animator.StringToHash("Fill");

        public TextMeshProUGUI MissionText
        {
            get => missionText;
            set => missionText = value;
        }
        private void Start()
        {
            missionText.gameObject.SetActive(false);
            StartCoroutine(HandleMissionSoundTimer());
        }

        private IEnumerator HandleMissionSoundTimer()
        {
            yield return new WaitForSecondsRealtime(2.2f);
            missionText.gameObject.SetActive(true);
        }

        public void SetMissionTextFalse()
        {
            var position = missionText.transform.position;
            missionText.transform.DOMove(new Vector2
                (position.x, position.y +20f), 1.8f).OnComplete(() =>
            {
                missionText.gameObject.SetActive(false);
            });
        }

        public void FillProgressBar(int index)
        {
            progressBarSlots[index].sprite = completeSlot;
            progressAnimators[index].SetTrigger(Fill);
        }
    }
}