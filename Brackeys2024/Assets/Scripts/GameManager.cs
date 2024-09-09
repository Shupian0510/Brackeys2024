using System;
using System.Collections.Generic;
using GMTK2024.Foundation;
using GMTK2024.Interactions;
using GMTK2024.Players;
using UniRx;
using UnityEngine;
using Zenject;

namespace GMTK2024.Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private MainMenuView mainMenuView;
        [SerializeField] private PauseMenuView pauseMenuView;
        [SerializeField] private NoteView noteView;
        
        private bool _isGamePaused;
        private bool _isShowingNote;

        private void Start()
        {
            UIManager.Instance.AddWindow(mainMenuView);
            UIManager.Instance.ShowUI<MainMenuView>();
            
            UIManager.Instance.AddWindow(pauseMenuView);
            UIManager.Instance.AddWindow(noteView);
            

            StageManager.Instance.Initialize();
        }

        public void ResumeGame()
        {
            _isGamePaused = false;
            UIManager.Instance.HideUI<PauseMenuView>();
            StageManager.Instance.Resume();
        }

        public void PauseGame()
        {
            if (_isGamePaused)
            {
                ResumeGame();
                return;
            } 
            
            if (_isShowingNote)
            {
                HideNote();
                return;
            }
            
            UIManager.Instance.ShowUI<PauseMenuView>();
            StageManager.Instance.Pause();
            
            _isGamePaused = true;
        }

        public void ShowNote(PropReadable propReadable)
        {
            var noteView = UIManager.Instance.GetUI<NoteView>();
            noteView.SetNoteContent(propReadable);
            
            UIManager.Instance.ShowUI<NoteView>();

            _isShowingNote = true;
            
            StageManager.Instance.Pause();
        }
        
        public void HideNote()
        {
            var noteView = UIManager.Instance.GetUI<NoteView>();
            noteView.ResetNoteContent();
            
            UIManager.Instance.HideUI<NoteView>();

            _isShowingNote = false;
            
            StageManager.Instance.Resume();
        }
    }
}