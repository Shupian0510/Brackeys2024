using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GMTK2024.Foundation;

namespace GMTK2024.Core
{
    public class UIManager : Singleton<UIManager>
    {
        private IWindowView _currentWindow;
        private List<IWindowView> _windowList = new List<IWindowView>();

        public void AddWindow(IWindowView window)
        {
            _windowList.Add(window);
            window.Initialize();
        }

        public async void ShowUI<T>() where T : IWindowView
        {
            var window = _windowList.OfType<T>().FirstOrDefault();
            if (window == null)
            {
                UnityEngine.Debug.LogError($"No window of type {typeof(T)} found in the window list.");
                return;
            }

            if (_currentWindow != null && _currentWindow != (IWindowView)window)
            {
                _currentWindow.OnHide();
                await _currentWindow.Hide();
            }

            _currentWindow = window;
            await _currentWindow.Show();
            _currentWindow.OnShow();
        }

        public async void HideUI<T>() where T : IWindowView
        {
            var window = _windowList.OfType<T>().FirstOrDefault();
            if (window == null)
            {
                UnityEngine.Debug.LogError($"No window of type {typeof(T)} found in the window list.");
                return;
            }

            if (_currentWindow == (IWindowView)window)
            {
                _currentWindow.OnHide();
                await _currentWindow.Hide();
                _currentWindow = null;
            }
        }

        public T GetUI<T>() where T : IWindowView
        {
            var window = _windowList.OfType<T>().FirstOrDefault();
            if (window == null)
            {
                UnityEngine.Debug.LogError($"No window of type {typeof(T)} found in the window list.");
                return default(T);
            }

            return window;
        }
    }
}