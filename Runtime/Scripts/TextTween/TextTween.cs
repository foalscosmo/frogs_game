using System;
using DG.Tweening;
using UnityEngine;

namespace TextTween
{
    public class TextTween : MonoBehaviour
    {
        private void OnEnable()
        {
            ShakeText();
        }

        private void ShakeText()
        {
            transform.DOScale(1f, 0.5f);
        }
    }
}
