using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace GMTK2024.Core
{
    public abstract class DropdownWindowView : MonoBehaviour, IWindowView
    {
        [SerializeField] private RectTransform windowRectTransform;
        [SerializeField] private float showDuration = 0.5f;
        [SerializeField] private float hideDuration = 0.3f;
        [SerializeField] private float bounceIntensity = 1.2f;

        private Vector3 hiddenPosition;
        private Vector3 visiblePosition;

        private void Awake()
        {
            hiddenPosition = new Vector3(windowRectTransform.anchoredPosition.x, Screen.height * 1.5f, 0);
            visiblePosition = new Vector3(windowRectTransform.anchoredPosition.x, 0, 0);

            windowRectTransform.anchoredPosition = hiddenPosition;
        }

        public async UniTask Show()
        {
            gameObject.SetActive(true);
            windowRectTransform.anchoredPosition = hiddenPosition;

            await windowRectTransform.DOAnchorPos(visiblePosition, showDuration)
                .SetEase(Ease.OutBounce)
                .SetEase(Ease.OutBack, bounceIntensity)
                .ToUniTask();
        }

        public async UniTask Hide()
        {
            await windowRectTransform.DOAnchorPos(hiddenPosition, hideDuration)
                .SetEase(Ease.InOutQuad)
                .ToUniTask();
            gameObject.SetActive(false);
        }

        public abstract void OnShow();
        public abstract void OnHide();
        public abstract void Initialize();
    }
}