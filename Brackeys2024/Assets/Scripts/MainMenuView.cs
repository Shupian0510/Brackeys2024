using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GMTK2024.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;
using GMTK2024.Sound;

namespace GMTK2024.Core
{
    public class MainMenuView : DropdownWindowView
    {
        [SerializeField] private WaveZoomTextEffect titleTextEffect;
        [SerializeField] private FadeTextEffect guideTextEffect;
        [SerializeField] private Button _startButton;
        
        [Inject] private ISoundManager _soundManager;
        
        
        public override void OnShow()
        {
            _startButton.interactable = true;
            
            titleTextEffect.StartEffect();
            guideTextEffect.StartEffect();
            
            _soundManager.PlayMusic(Music.MainMenu, true, 1f);
        }

        public override void OnHide()
        {
            _startButton.interactable = false;
            
            titleTextEffect.StopEffect();
            guideTextEffect.StopEffect();
            
            
        }

        public override void Initialize()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
        }

        private void OnStartButtonClicked()
        {
            UIManager.Instance.ShowUI<SelectStageView>();
        }
        
        
    }
}