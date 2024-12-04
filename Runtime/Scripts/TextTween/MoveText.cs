using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TextTween
{
    public class MoveText : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float movementDistance;
        [SerializeField] private CanvasScaler canvasScaler;
        [SerializeField] private float moveDuration = 0.5f;

        private void OnEnable()
        {
            MoveTextTween();
        }

        private void MoveTextTween()
        {
            // Calculate the resolution scale to adjust movement distance based on screen resolution
            float resolutionScale = Screen.height / canvasScaler.referenceResolution.y;
            float adjustedDistance = movementDistance * resolutionScale;

            // Get the current position
            Vector3 startPosition = rectTransform.anchoredPosition;
            Vector3 endPosition = new Vector3(startPosition.x, startPosition.y - adjustedDistance, startPosition.z);

            // Start the movement
            StartCoroutine(MoveOverTime(startPosition, endPosition, moveDuration));
        }

        private System.Collections.IEnumerator MoveOverTime(Vector3 startPosition, Vector3 endPosition, float duration)
        {
            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            rectTransform.anchoredPosition = endPosition;
        }
    }
}
