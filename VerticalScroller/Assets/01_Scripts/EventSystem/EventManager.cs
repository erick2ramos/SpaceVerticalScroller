using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseSystems.EventSystem
{
    public interface IEventListenerBase { }
    public interface IEventListener<EventType> : IEventListenerBase 
    {
        void OnEvent(EventType eventType);
    }

    public static class EventManager
    {
        static Dictionary<Type, List<IEventListenerBase>> _listeners;

        public static void AddListener<EventType>(IEventListener<EventType> listener) where EventType : struct
        {
            Type type = typeof(EventType);

            if (!_listeners.ContainsKey(type))
            {
                _listeners[type] = new List<IEventListenerBase>();
            }

            if (!ListenerExists(type, listener))
            {
                _listeners[type].Add(listener);
            }
        }

        public static void RemoveListener<EventType>(IEventListener<EventType> listener) where EventType : struct
        {
            Type type = typeof(EventType);

            if (!_listeners.ContainsKey(type)) return;

            for (int i = 0; i < _listeners[type].Count; i++)
            {
                if (_listeners[type][i] == listener)
                {
                    _listeners[type].Remove(listener);

                    if (_listeners[type].Count == 0)
                    {
                        _listeners.Remove(type);
                    }

                    return;
                }
            }
        }

        private static bool ListenerExists(Type type, IEventListenerBase listener)
        {
            List<IEventListenerBase> result;

            if (!_listeners.TryGetValue(type, out result))
                return false;

            bool exists = false;

            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] == listener)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }
    }

    // Helpers for easy listener registration
    public static class EventRegister
    {
        public static void EventStartListening<EventType>(this IEventListener<EventType> caller) where EventType : struct
        {
            EventManager.AddListener<EventType>(caller);
        }

        public static void EventStopListening<EventType>(this IEventListener<EventType> caller) where EventType : struct
        {
            EventManager.RemoveListener<EventType>(caller);
        }
    }
}