using System;
using System.Threading;

namespace GMTK2024.Core
{
    using UnityEngine;
    using TMPro;
    using DG.Tweening;
    using Cysharp.Threading.Tasks;

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FadeTextEffect : MonoBehaviour, IUIEffect
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private float delayBetweenFades = 0.5f;

        private CancellationTokenSource cancellationTokenSource;

        private void Awake()
        {
            cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        public void StartEffect()
        {
            cancellationTokenSource = new CancellationTokenSource();
            StartFadeEffect(cancellationTokenSource.Token).Forget();
        }

        public void StopEffect()
        {
            cancellationTokenSource.Cancel();
        }

        private async UniTaskVoid StartFadeEffect(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    // Fade in
                    await textMeshProUGUI.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine).AsyncWaitForCompletion();

                    // Wait for delay
                    await UniTask.Delay((int)(delayBetweenFades * 1000), cancellationToken: cancellationToken);

                    // Fade out
                    await textMeshProUGUI.DOFade(0f, fadeDuration).SetEase(Ease.InOutSine).AsyncWaitForCompletion();

                    // Wait for delay
                    await UniTask.Delay((int)(delayBetweenFades * 1000), cancellationToken: cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
        }
    }

}