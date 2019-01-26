using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenFlash : MonoBehaviour {

    [SerializeField]
    private Image flashImage;

    public void Reset()
    {
        flashImage.color = Color.clear;
        flashImage.DOKill();
    }

    public void StartFlashEffect(Color color, float duration, int loops = 1)
    {
        if (flashImage == null)
        {
            return;
        }

        flashImage.color = Color.clear;
        flashImage.DOColor(color, duration).SetLoops(loops, LoopType.Yoyo).SetEase(Ease.Flash).OnComplete(()=> flashImage.color = Color.clear);
    }

    public void StartFadeEffect(Color color, float duration)
    {
        flashImage.color = Color.clear;
        flashImage.DOColor(color, duration);
    }

    public void StartFadeFromBlack(float duration)
    {
        flashImage.color = Color.black;
        flashImage.DOColor(Color.clear, duration);
    }

}
