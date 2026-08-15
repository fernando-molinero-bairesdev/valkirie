using Valkirie.Core.Events;

namespace Valkirie.Core.Objectives
{
    // Covers most cases without a bespoke effect class: anything already listening via
    // GameEventListener (UI, audio, a level's own scripted sequence) can react to an
    // objective completing. Write a dedicated ObjectiveEffect only when this isn't enough.
    [UnityEngine.CreateAssetMenu(menuName = "Valkirie/Objectives/Effects/Raise Game Event")]
    public class RaiseGameEventEffect : ObjectiveEffect
    {
        public GameEvent eventToRaise;

        public override void Apply() => eventToRaise.Raise();
    }
}
