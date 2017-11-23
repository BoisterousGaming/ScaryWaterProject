using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHandler : MonoBehaviour
{
    ArrayList mAllObjects = new ArrayList();
    public EnvironmentManager _environmentManager = null;
    public ParticleSystem _Firefly;

    //public static ArrayList GetComponentsOfType<T>(GameObject objects) where T : class
    //{
    //    Component[] tArrOfObjs = objects.GetComponents(typeof(Component));
    //    ArrayList tObjs = new ArrayList();
    //    T tComponent;
    //    for (int i = 0; i < tArrOfObjs.Length; ++i)
    //    {
    //        tComponent = tArrOfObjs[i] as T;
    //        if (tComponent != null)
    //            tObjs.Add(tComponent);
    //    }
    //    return tObjs;
    //}
}
