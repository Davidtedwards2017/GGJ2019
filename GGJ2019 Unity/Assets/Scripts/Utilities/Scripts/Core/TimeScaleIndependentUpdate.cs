using UnityEngine;
using System.Collections;

public class TimeScaleIndependentUpdate : MonoBehaviour {

    public IPauseHandler PauseHandler;
	public bool pausedWhenGameIsPaused = true;

	float previousTimeSinceStartup;

	protected virtual void Awake()
	{
		previousTimeSinceStartup = Time.realtimeSinceStartup;
	}

	protected virtual void Update () 
	{
		float realTimeSinceStartup = Time.realtimeSinceStartup;
		deltaTime = realTimeSinceStartup - previousTimeSinceStartup;
		previousTimeSinceStartup = realTimeSinceStartup;

		if(deltaTime < 0)
		{
			deltaTime = 0;
		}

		if(pausedWhenGameIsPaused && IsGamePaused())
		{
			deltaTime = 0;
		}
	}

	public IEnumerator TimeScaleIndependentWaitForSeconds(float seconds)
	{
		float elapsedTime = 0;
		while(elapsedTime < seconds)
		{
			yield return null;
			elapsedTime += deltaTime;
		}
	}

	private bool IsGamePaused()
	{
		if(PauseHandler != null)
		{
		    return PauseHandler.IsGamePaused() || Time.timeScale == 0;
		}
    
		return false;
	}

	public IEnumerator WaitForRealSeconds(float time)
	{
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time)
		{
			yield return null;
		}
	}

    public float deltaTime { get; private set; }
}
