using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace _DataPersistance
{
    public class ProgressionController: Singleton<ProgressionController>
    {
        bool initialized = false;
        private const string ProgressPrefName = "Progress";
        private const string ProgressFileName = "/progress.dat";

        public StatSerializer Serializer
        {
            get { throw new NotImplementedException(); }
        }
        
        private Progress<Stat> _Progress;
        public Progress<Stat> CurrentProgress
        {
            get
            {
                if(_Progress == null)
                {
                    _Progress = new Progress<Stat>();
                }

                return _Progress;
            }
            set
            {
                _Progress = value;
            }
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(ProgressPrefName);
            CurrentProgress = null;
        }

        private void TryToInitialize()
        {
            if (initialized)
            {
                return;
            }

            Debug.Log("Initializing Progress controller");
            initialized = true;
        }

        public void LoadProgress()
        {
            TryToInitialize();
            Debug.Log("Loading Progress....");

            var data = PlayerPrefs.GetString(ProgressPrefName);
            if(!string.IsNullOrEmpty(data))
            {
                var jobj = JObject.Parse(data);
                CurrentProgress = Progress<Stat>.FromJObject(jobj, Serializer);
            }
        }

        public void SaveProgress()
        {
            Debug.Log("Saving Progress....");

            var jobj = CurrentProgress.ToJObject();
            var dataToSave = jobj.ToString();
            
            PlayerPrefs.SetString(ProgressPrefName, dataToSave);
        }

        public T GetStat<T>() where T : Stat
        {
            var stats = CurrentProgress.GetAll<T>();
            return (stats.Any()) ? stats.First() : null;
        }

        public T GetStat<T>(Predicate<T> query) where T: Stat
        {
            return CurrentProgress.Get(query);
        }

        public bool StatMatch<T>(Predicate<T> query) where T: Stat
        {
            return CurrentProgress.GetAll<T>().Any(s => s != null && query.Invoke(s));
        }

        public List<T> GetStats<T>() where T : Stat
        {
            return CurrentProgress.GetAll<T>().Cast<T>().ToList();
        }

        public List<T> GetStats<T>(Predicate<T> query) where T : Stat
        {
            return CurrentProgress.GetAll<T>().Where(s => s != null && query.Invoke(s)).Cast<T>().ToList();
        }

        public void AddStat<T>(T stat) where T: Stat
        {
            CurrentProgress.AddStat(stat);
        }
    }
}