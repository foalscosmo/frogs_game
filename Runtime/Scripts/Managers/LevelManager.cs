using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        // Snap index
        [SerializeField] private int snapIndex;
        
        // Level index
        [SerializeField] private int levelIndex;

        [SerializeField] private TextMeshProUGUI finishText;

        // Property to access level index
        public int LevelIndex => levelIndex;

        // Event triggered when snap index increases
        public event Action OnSnapIndexIncrease;

        // Event triggered when level changes
        public event Action OnLevelChange;
        public event Action OnGameFinish;

        // Increment snap index
        public void IncrementSnapIndex()
        {
            snapIndex++;
            if (snapIndex != 3) return;
            OnSnapIndexIncrease?.Invoke();
            levelIndex++;
            ChangeLevel();
            snapIndex = 0;
        }

        // Change level based on level index
        private void ChangeLevel()
        {
            switch (levelIndex)
            {
                case < 5:
                    OnLevelChange?.Invoke();
                    break;
                default:
                    StartCoroutine(Timer());
                    break;
            }
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSecondsRealtime(0.6f);
            finishText.gameObject.SetActive(true);
            OnGameFinish?.Invoke();

        }
    }
}