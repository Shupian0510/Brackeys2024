using Cysharp.Threading.Tasks;

namespace GMTK2024.Core
{
    public interface ILoadTransitionHandler
    {
        UniTask StartTransition();
        void UpdateTransition(float progress);
        UniTask EndTransition();
    }
}