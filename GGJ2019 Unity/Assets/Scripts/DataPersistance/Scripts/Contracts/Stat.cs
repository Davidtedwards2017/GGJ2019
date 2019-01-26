using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _DataPersistance
{
    [Serializable]
    public abstract class Stat : IJsonSerializeable
    {
        public string Name
        {
            get { return GetType().Name; }
        }

        public virtual JObject ToJObject()
        {
            return new JObject()
            {
                { "name", Name }
            };
        }

        public abstract void PopulateFromJObject(JToken token);
    }
}