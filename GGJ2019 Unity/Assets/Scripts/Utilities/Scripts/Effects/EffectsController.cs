using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

public enum FlashType
{
    Foreground,
    Background
}

public class EffectsController : Singleton<EffectsController> {
    
    [SerializeField]
    private ScreenFlash ForegroundFlashEffectCtrl;

    [SerializeField]
    private ScreenFlash BackgroundFlashEffectCtrl;

    public void PlayScreenFlash(FlashType flashType, Color color, float duration, int loops = 1)
    {
        switch (flashType)
        {
            case FlashType.Background:
                Instance.BackgroundFlashEffectCtrl.StartFlashEffect(color, duration, loops);
                break;
            case FlashType.Foreground:
                Instance.ForegroundFlashEffectCtrl.StartFlashEffect(color, duration, loops);
                break;
        }
    }

    public void Reset()
    {
        Instance.ForegroundFlashEffectCtrl.Reset();
        Instance.BackgroundFlashEffectCtrl.Reset();
    }

    public void StartFade(Color color, float duration)
    {
        Instance.ForegroundFlashEffectCtrl.StartFadeEffect(color, duration);
    }        
    
    public void StartFadeFromBlack(float duration)
    {
        Instance.ForegroundFlashEffectCtrl.StartFadeFromBlack(duration);
    }

    public void StopTimeEffect(float duration)
    {
        StartCoroutine(StopTimeCoroutine(duration));
    }

    private IEnumerator StopTimeCoroutine(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

    public void PlaySlowDownEffect(SlowDownEffectData data)
    {
        StartCoroutine(PerformTimeScaleTween(data));
    }    

    public IEnumerator PerformTimeScaleTween(SlowDownEffectData data)
    {
        yield return DOTween.To(() => Time.timeScale, x => Time.timeScale = x, data.Amount, data.InDuration).SetEase(data.EaseIn).SetUpdate(true).WaitForCompletion();
        yield return new WaitForSecondsRealtime(data.holdDuration);
        yield return DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, data.OutDuration).SetEase(data.EaseOut).SetUpdate(true).WaitForCompletion();
    }

    private IEnumerator PerformTimeScaleTween(float amount, float duration, Ease ease, Action callback = null)
    {
        yield return DOTween.To(() => Time.timeScale, x => Time.timeScale = x, amount, duration).SetEase(ease).SetUpdate(true).WaitForCompletion();
    }

    public void PlaySlowDownEffect(float to, float from, float inDuration, float holdDuration, float outDuration)
    {
        StartCoroutine(SlowDownEffect(to, from, inDuration, holdDuration, outDuration));
    }

    private IEnumerator SlowDownEffect(float to, float from, float inDuration, float holdDuration, float outDuration)
    {
        TweenSpeed(inDuration, from, to);
        yield return new WaitForSeconds(inDuration + holdDuration);
        TweenSpeed(outDuration, to, from);
    }

    private void TweenSpeed(float duration, float start, float end)
    {
        DOTween.To(value => Time.timeScale = value, start, end, duration);//.OnUpdate(() => { Debug.Log(Time.timeScale); }); 
    }


}
