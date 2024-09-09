using Cysharp.Threading.Tasks;

namespace GMTK2024.Core
{
    public interface IWindowView
    {
        UniTask Show();
        UniTask Hide();
        
        void OnShow();
        
        void OnHide();

        void Initialize();
    }
}