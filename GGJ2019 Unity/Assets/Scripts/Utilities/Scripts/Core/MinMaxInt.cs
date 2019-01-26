using UnityEngine;
using System.Collections;

// [Variables and functions related to stats with a minimum and maximum, 
// with various methods of incrementing and decrementing current stat. Useful for health, shields, etc]
[System.Serializable]
public class MinMaxInt
{
	public int _min, _max, _value;
	
	public int min
	{
		get { return _min; }
		set { _min = value; }
	}
	
	public int max
	{
		get { return _max; }
		set { _max = value; }
	}
	
	public virtual int value
	{
		get { return _value; }
		set { _value = Mathf.Clamp(value, _min, _max); }
	}
	
	public float percentage
	{
		get { return (float)_value / (float)_max; }
	}	
	
	public MinMaxInt()
	{
		min = 0;
		max = 100;
		value = max;
	}
	
	public MinMaxInt(int min, int max, int _value = -999)
	{
		this.min = min;
		this.max = max;
		value = _value;
	}

    public virtual int Random()
    {
        return UnityEngine.Random.Range(min, max);
    }

	/// <summary>
	/// Sets value to minimum.
	/// </summary>
	public virtual void SetToMin() {
		value = _min;
	}
	/// <summary>
	/// Sets value to maximum.
	/// </summary>
	public virtual void SetToMax() {
		value = _max;
	}
	
	public virtual void SetToPercent(float percent) {
		float temp = (float)max * percent;
		value = (int)temp;
	}

    public virtual void MatchValues(MinMaxFloat newMinMax)
    {
        min = (int)newMinMax.min;
        max = (int)newMinMax.max;
        value = (int)newMinMax.value;
    }

    public virtual void MatchValues(MinMaxInt newMinMax)
    {
        min = newMinMax.min;
        max = newMinMax.max;
        value = newMinMax.value;
    }
    /// <summary>
    /// Add another MinMax variable's values to this one
    /// </summary>
    /// <param name="newMinMax"></param>
    public virtual void Add(MinMaxInt newMinMax)
    {
        max += newMinMax.max;
        value += newMinMax.value;
    }
    /// <summary>
    /// Remove another MinMax variable's values from this one
    /// </summary>
    /// <param name="newMinMax"></param>
    public virtual void Remove(MinMaxInt newMinMax)
    {
        max += newMinMax.max;
        value += newMinMax.value;
    }

}
/// <summary>
/// MinMaxInt that also includes EventHandlers.
/// </summary>
[System.Serializable]
public class MinMaxEventInt : MinMaxInt 
{

	public delegate void EventHandler(MinMaxInt minMaxInt);
	public delegate void EventHandlerInt(int value);
	public delegate void EventHandlerFloat(float value);
	
	public EventHandler OnValueChanged;
	public EventHandler OnValueIncrease;	
	public EventHandler OnValueDecrease;		
	public EventHandler OnValueMax;
	public EventHandler OnValueMin;
	public EventHandler OnValueAboveHalf;
	public EventHandler OnValueBelowHalf;
	
//	public EventHandlerInt OnValueAbove;
//	public EventHandlerInt OnValueBelow;
//	public EventHandlerFloat OnValueAbovePercent; 
//	public EventHandlerFloat OnValueBelowPercent;
	public EventHandlerInt InvokeIfMinimum(int minimum, EventHandlerInt handler)
	{
		return v =>
		{
			if(v >= minimum) handler(v);
		};
	}
	
	public override int value
	{
		get { 
			return _value; 
		}
		set 
		{ 
			if(this.value == value) return;
			
			int oldValue = _value;
			float oldPercentage = percentage;
			
			_value = Mathf.Clamp(value, _min, _max); 
			
			float newPercentage = percentage;
			
			if(oldPercentage < 0.5f && newPercentage > 0.5f && OnValueAboveHalf != null) OnValueAboveHalf(this);
			if(oldPercentage > 0.5f && newPercentage < 0.5f && OnValueBelowHalf != null) OnValueBelowHalf(this);

			if(OnValueChanged != null) OnValueChanged(this);
			if(_value > oldValue) 
			{
				if(OnValueIncrease != null) OnValueIncrease(this);
			}
			else if(_value < oldValue)
			{
				if(OnValueDecrease != null) OnValueDecrease(this);
			}			
			if(_value == max && OnValueMax != null) OnValueMax(this);
			if(_value == min && OnValueMin != null) OnValueMin(this);
			
			
		}
	}
	
	public MinMaxEventInt() {
		min = 0;
		max = 100;
		value = 0;
	}
	
	public MinMaxEventInt(int min, int max, int value)
	{
		this.min = min;
		this.max = max;
		_value = Mathf.Clamp(value, min, max);
	}
	
}