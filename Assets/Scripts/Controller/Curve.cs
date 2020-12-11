using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Curve")]
public class Curve : ScriptableObject
{
    private static bool _isInited = false;
    private static Curve _instance;
    public static Curve Instance
    {
        get
        {
            if (!_isInited) // This is way cheaper than null check
            {
                _instance = Resources.Load<Curve>("Curves");
                _isInited = true;
            }
            return _instance;
        }
    }

    public static void Tween(AnimationCurve curve, float duration, Action<float> perTickAction, Action postAction)
    {
        CoroutineStarter.Run(TweenCoroutine(curve, duration, perTickAction, postAction));
    }

    private static IEnumerator TweenCoroutine(AnimationCurve curve, float duration, Action<float> perTickAction, Action postAction)
    {
        for (float f = 0f; f < duration; f += Time.deltaTime)
        {
            perTickAction(curve.Evaluate(f / duration));
            yield return null;
        }
        postAction();
    }
}