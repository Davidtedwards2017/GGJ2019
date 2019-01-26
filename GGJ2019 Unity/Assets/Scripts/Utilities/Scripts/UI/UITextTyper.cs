using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITextTyper : MonoBehaviour {

    public Color DefaultColor = Color.black;
	private Text UITextElement;
	public string TargetText;
	public float TypeSpeed;
	// Use this for initialization
	void Start () {
		UITextElement = GetComponent<Text>();
	}
	

	public void SetUIText(string text)
	{
		SetUIText(text, DefaultColor);
	}

	public void SetUIText(string text, Color color)
	{
		if(UITextElement == null || 
		   !UITextElement.enabled ||
		   UITextElement.text.Equals(text))
		{
			return;
		}
		
		UITextElement.text = "";
		UITextElement.color = color;
		TargetText = text;
		StopCoroutine("AddText");
		StopCoroutine("RemoveText");
		StartCoroutine("AddText");
	}

	public void ClearText()
	{
		StopCoroutine("AddText");
		StartCoroutine("RemoveText");
	}
	
	IEnumerator AddText() 
	{
		foreach(char letter in TargetText.ToCharArray())
		{
			UITextElement.text += letter;
			yield return new WaitTimeScaleIndependent(TypeSpeed);
		}
	}

	IEnumerator RemoveText() 
	{
		string text = UITextElement.text;
		while(!string.IsNullOrEmpty(text))
		{
			text = text.Remove(UITextElement.text.Length -1);
			UITextElement.text = text;
			yield return new WaitTimeScaleIndependent(TypeSpeed);
		}
	}
}
