﻿using System;
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

        static EventManager()
        {
            _listeners = new Dictionary<Type, List<IEventListenerBase>>();
        }

        /// <summary>
        /// Register a listener to a specific "EventType" event
        /// </summary>
        /// <typeparam name="EventType"></typeparam>
        /// <param name="listener"></param>
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

        /// <summary>
        /// Unsubscribe listener from event
        /// </summary>
        /// <typeparam name="EventType"></typeparam>
        /// <param name="listener"></param>
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

        /// <summary>
        /// Broadcast an event through al of its listeners
        /// </summary>
        /// <typeparam name="EventType"></typeparam>
        /// <param name="newEvent"></param>
        public static void Trigger<EventType>(EventType newEvent) where EventType : struct 
        {
            if (!_listeners.TryGetValue(typeof(EventType), out List<IEventListenerBase> listeners))
                return;

            for (int i = 0; i < listeners.Count; i++)
            {
                (listeners[i] as IEventListener<EventType>).OnEvent(newEvent);
            }
        }
    }

    // Helpers for easy listener registration and deregistration
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