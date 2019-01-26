using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ScrollingCombatText : MonoBehaviour {

    Text textElement;
    public CanvasGroup canvasGroup;
    public float FadeDuration = 0.2f;

    public void Initialize(string text, float duration, Color color, Vector3 targetedRelativeOffset)
    {
        textElement = GetComponent<Text>();
        textElement.text = text;
        textElement.color = color;

        StartCoroutine(AnimationSequence(duration, targetedRelativeOffset));
    }

   private IEnumerator AnimationSequence(float duration, Vector3 targetedRelativeOffset)
   {
       transform.DOMove(targetedRelativeOffset, duration).SetRelative(true);
       yield return new WaitForSeconds(duration);
       canvasGroup.DOFade(0, FadeDuration);
       yield return new WaitForSeconds(FadeDuration);
       Destroy(gameObject);
   }

}
