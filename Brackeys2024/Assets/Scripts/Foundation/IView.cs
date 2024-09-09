namespace GMTK2024.Foundation
{
    public interface IView<T> where T : IViewModel
    {
        void Initialize(T viewModel);
    }
}