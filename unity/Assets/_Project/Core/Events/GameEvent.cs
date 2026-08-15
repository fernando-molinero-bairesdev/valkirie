using System.Collections.Generic;
using UnityEngine;

namespace Valkirie.Core.Events
{
    // Payload-less event asset. Wire it into the Inspector via GameEventListener;
    // nothing that raises it needs to know who's listening.
    [CreateAssetMenu(menuName = "Valkirie/Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        private readonly List<GameEventListener> _listeners = new();

        public void Raise()
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnRaised();
        }

        public void Register(GameEventListener listener) => _listeners.Add(listener);
        public void Unregister(GameEventListener listener) => _listeners.Remove(listener);
    }

    // Generic payload variant. Unity can't serialize/expose an open generic in the
    // Inspector, so concrete closed subclasses (see StringGameEvent) are what you create
    // assets from; add more concrete subclasses as new payload shapes are needed.
    public abstract class GameEvent<T> : ScriptableObject
    {
        private readonly List<System.Action<T>> _listeners = new();

        public void Raise(T payload)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i]?.Invoke(payload);
        }

        public void Register(System.Action<T> listener) => _listeners.Add(listener);
        public void Unregister(System.Action<T> listener) => _listeners.Remove(listener);
    }

    [CreateAssetMenu(menuName = "Valkirie/Events/String Game Event")]
    public class StringGameEvent : GameEvent<string> { }
}
