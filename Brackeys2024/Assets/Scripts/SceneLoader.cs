using System;
using Cysharp.Threading.Tasks;
using GMTK2024.Foundation;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GMTK2024.Core
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        [SerializeField] private LoadTransitionHandler _loadTransitionHandler;
        
        [Inject]
        private ZenjectSceneLoader _zenjectSceneLoader;
        
        public async UniTask LoadSceneAsync(StageSettings stageSettings, Action onLoaded = null)
        {
            await _loadTransitionHandler.StartTransition();

            // await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

            AsyncOperation loadOperation = _zenjectSceneLoader.LoadSceneAsync(stageSettings.SceneName, LoadSceneMode.Single, container =>
            {
                
            });

            while (!loadOperation.isDone)
            {
                _loadTransitionHandler.UpdateTransition(loadOperation.progress);
                await UniTask.Yield();
            }
            
            await UniTask.DelayFrame(3);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(stageSettings.SceneName));

            onLoaded?.Invoke();
            await _loadTransitionHandler.EndTransition();
        }
    }
}