
namespace FacetedWorlds.Reversi.ViewModels
{
    public interface IPresentationServices
    {
        void Synchronize();
        bool IsInTrialMode { get; }
    }
}
