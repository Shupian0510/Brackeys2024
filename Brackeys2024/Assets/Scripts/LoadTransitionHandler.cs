using System;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UniTask = Cysharp.Threading.Tasks.UniTask;

namespace GMTK2024.Core
{
    public class LoadTransitionHandler : MonoBehaviour, ILoadTransitionHandler
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI loadingText;

        private CancellationTokenSource cancellationTokenSource;

        private void Start()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
            gameObject.SetActive(false);
        }

        public async UniTask StartTransition()
        {
            gameObject.SetActive(true);
            
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            // Start screen fade in
            canvasGroup.alpha = 0f;
            await canvasGroup.DOFade(1f, 1f).SetEase(Ease.InOutQuad).ToUniTask(cancellationToken: cancellationTokenSource.Token);
        }

        public void UpdateTransition(float progress)
        {
            if (loadingText != null)
            {
                // Update the loading text with 1-3 dots repeatedly
                int dotCount = Mathf.CeilToInt(progress * 3) % 4; // Cycle through 0 to 3 dots
                loadingText.text = "Loading" + new string('.', dotCount);
            }
        }

        public async UniTask EndTransition()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            // Start screen fade out
            await canvasGroup.DOFade(0f, 1f).SetEase(Ease.InOutQuad).ToUniTask(cancellationToken: cancellationTokenSource.Token);
            
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }
    }

}