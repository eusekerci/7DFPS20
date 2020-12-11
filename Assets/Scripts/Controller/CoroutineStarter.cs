using System.Collections;
using UnityEngine;

public class CoroutineStarter : MonoBehaviour
{
    private static CoroutineStarter _slave;
    static CoroutineStarter()
    {
        _slave = new GameObject("CoroutineStarter").AddComponent<CoroutineStarter>();
        DontDestroyOnLoad(_slave.gameObject);
    }

    public static Coroutine Run(IEnumerator function)
    {
        return _slave.StartCoroutine(function);
    }
}