using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK2024.Core
{
    public class PauseMenuView : DropdownWindowView
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button levelSelectButton;
        [SerializeField] private Button resetButton;
        
        [SerializeField] private WaveZoomTextEffect waveZoomTextEffect;

        [SerializeField] private TextMeshProUGUI stageName;
        
        public override void OnShow()
        {
            resumeButton.interactable = true;
            levelSelectButton.interactable = true;
            resetButton.interactable = true;
            
            waveZoomTextEffect.StartEffect();
        }

        public override void OnHide()
        {
            resumeButton.interactable = false;
            levelSelectButton.interactable = false;
            resetButton.interactable = false;
            
            waveZoomTextEffect.StopEffect();
        }

        public override void Initialize()
        {
            resumeButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ResumeGame();
            });
            
            levelSelectButton.onClick.AddListener(() =>
            {
                StageManager.Instance.LoadStageSelect();
            });
            
            resetButton.onClick.AddListener(() =>
            {
                StageManager.Instance.ReloadStage();
            });
        }
    }
}