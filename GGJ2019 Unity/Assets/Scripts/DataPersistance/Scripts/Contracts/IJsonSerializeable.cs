using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public interface IJsonSerializeable
{
    JObject ToJObject();
    void PopulateFromJObject(JToken token);
}
