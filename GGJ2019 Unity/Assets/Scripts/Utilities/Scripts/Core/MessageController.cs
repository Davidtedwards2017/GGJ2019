using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Utilites
{
    public class MessageController : Singleton<MessageController>
    {

        public delegate void MessageDelegate(params object[] args);
        private Dictionary<string, List<MessageDelegate>> eventDictonary = new Dictionary<string, List<MessageDelegate>>();

        public static void SendMessage(string eventName, params object[] args)
        {
            if (Instance == null)
            {
                return;
            }

            bool shouldCleanup = false;
            List<MessageDelegate> listeners;
            if (!Instance.eventDictonary.TryGetValue(eventName, out listeners))
            {
                return;
            }

            foreach (var listener in listeners)
            {
                if (listener == null || listener.Target.ToString() == "null")
                {
                    shouldCleanup = true;
                    continue;
                }

                listener.Invoke(args);
            }

            if (shouldCleanup)
            {
                Instance.Cleanup(listeners);
            }
        }

        public static void StartListening(string eventName, MessageDelegate listener)
        {
            List<MessageDelegate> listeners = Instance.eventDictonary.SafeGetOrInitialize(eventName);
            listeners.AddIfUnique(listener);
        }

        public static void StopListening(string eventName, MessageDelegate listener)
        {
            List<MessageDelegate> listeners;
            if (!Instance.eventDictonary.TryGetValue(eventName, out listeners))
            {
                return;
            }

            listeners.SafeRemove(listener);
        }

        private void Cleanup()
        {
            foreach (var eventName in eventDictonary.Keys)
            {
                Cleanup(eventDictonary[eventName]);
            }
        }

        private void Cleanup(List<MessageDelegate> collection)
        {
            collection = collection.Where(i => i != null && i.Target.ToString() != "null").ToList();
        }

    }
}