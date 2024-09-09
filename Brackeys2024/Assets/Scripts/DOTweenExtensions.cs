using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace GMTK2024.Core
{
    public static class DOTweenExtensions
    {
        public static UniTask ToUniTask(this Tween tween, CancellationToken cancellationToken = default)
        {
            var completionSource = new UniTaskCompletionSource();

            tween.OnKill(() => completionSource.TrySetCanceled(cancellationToken));
            tween.OnComplete(() => completionSource.TrySetResult());
            tween.OnRewind(() => completionSource.TrySetCanceled(cancellationToken));

            return completionSource.Task.AttachExternalCancellation(cancellationToken);
        }
    }
}