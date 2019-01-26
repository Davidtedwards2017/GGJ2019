using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace _DataPersistance
{
    [Serializable]
    public class Progress<Y> where Y : IJsonSerializeable
    {
        public Dictionary<string, List<Y>> Stats = new Dictionary<string, List<Y>>();

        public void AddStat<T>(T stat) where T : Y
        {
            var type = stat.GetType().Name;
            
            var collection = GetStatType(type);
            collection.Add(stat);            
        }

        public JObject ToJObject()
        {
            var jObj = new JObject();
            foreach(var entry in Stats)
            {
                var statCollection = new JArray();
                foreach(var foo in entry.Value)
                {
                    statCollection.Add(foo.ToJObject());
                }

                jObj.Add(entry.Key, statCollection);
            }

            return jObj;
        }

        public static Progress<Y> FromJObject(JObject jObj, StatSerializer Serializer)
        {
            var progress = new Progress<Y>();

            foreach(var property in jObj.Properties())
            {
                var type = Serializer.GetStatType(property.Name);;
                if (type == null) continue;

                var statEntries = new List<Y>();

                foreach (var jarrayEntry in (JArray)property.Value)
                {
                    var instance = (Y) Activator.CreateInstance(type);
                    instance.PopulateFromJObject(jarrayEntry);
                    statEntries.Add(instance);
                }

                progress.Stats.Add(property.Name, statEntries);
            }

            return progress;
        }

        public T Get<T>() where T : Y
        {
            return Get<T>(query => true);
        }

        public T Get<T>(Predicate<T> query) where T : Y
        {
            Type type = typeof(T);
            var key = type.Name;

            var collection = GetStatType(key);
            return (T)collection.FirstOrDefault(entry => entry != null && query((T)entry));
        }

        public IEnumerable<T> GetAll<T>() where T : Y
        {
            Type type = typeof(T);
            var key = type.Name;

            return GetStatType(key).Where(s => s != null).Cast<T>();
        }

        private List<Y> GetStatType(string statType)
        {
            if (!Stats.ContainsKey(statType))
            {
                Stats.Add(statType, new List<Y>());
            }

            return Stats[statType];
        }
    }
}