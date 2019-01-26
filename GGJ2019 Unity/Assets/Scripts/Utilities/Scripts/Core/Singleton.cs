using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{ 
    private static T instance;

    //Returns the instance of this singleton.
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                //if (instance == null)
                //{
                //    Logger.Error("An instance of " + typeof(T) +
                //       " is needed in the scene, but there is none.");
                //}
            }

            return instance;
        }
    }
}
