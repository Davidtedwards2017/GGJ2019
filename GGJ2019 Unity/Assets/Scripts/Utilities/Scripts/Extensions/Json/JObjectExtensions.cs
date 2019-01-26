using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JObjectExtensions
{
    public static T SafeGetValue<T>(this JObject jobj, string propertyName)
    {
        if (jobj == null) return default(T);

        var prop = jobj[propertyName];
        if (prop == null) return default(T);

        return prop.Value<T>();
    }

    public static void SafeUpdate(this JObject jobj, string propertyName, string value)
    {
        jobj.Merge(
            new JObject()
            {
                { propertyName, value }
            });
    }
}
