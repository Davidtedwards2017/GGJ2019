using UnityEngine;
using UnityEngine.Events;
using System.Collections;

// [Variables and functions related to stats with a minimum and maximum, 
// with various methods of incrementing and decrementing current stat. Useful for health, shields, etc]
[System.Serializable]
public class MinMaxFloat
{
    public float _min, _value, _max; // Exposed values to override clamping behavior if desired

    public float min
    {
        get { return _min; }
        set { _min = value; }
    }

    public virtual float value
    {
        get { return _value; }
        set { _value = Mathf.Clamp(value, _min, _max); }
    }

    public float max
    {
        get { return _max; }
        set { _max = value; }
    }

    public float percentage
    {
        get { return (_value - min) / (_max - min); }
    }

    public float randomValue
    {
        get { return Random.Range(_min, _max); }
    }

    public MinMaxFloat()
    {
        min = 0f;
        max = 1f;
        value = min;
    }

    public MinMaxFloat(float min, float max, float value)
    {
        this.min = min;
        this.max = max;
        this.value = value;
    }
    /// <summary>
    /// Sets value to minimum.
    /// </summary>
    public virtual void SetToMin()
    {
        value = _min;
    }
    /// <summary>
    /// Sets value to maximum.
    /// </summary>
    public virtual void SetToMax()
    {
        value = _max;
    }

    /// <summary>
    /// Sets value to a percentage of maximum.
    /// </summary>
    /// <param name="percent">Percent.</param>
    public virtual void SetToPercent(float percent)
    {
        value = min + (max - min) * percent;
    }

    /// <summary>
    /// Sets value to a linearly interpolated point between min and max.
    /// </summary>
    /// <param name="percent">Percent.</param>
    public virtual void Lerp(float percent)
    {
        value = min + (max - min) * percent;
    }

    public virtual void MatchValues(MinMaxFloat newMinMax)
    {
        min = newMinMax.min;
        max = newMinMax.max;
        value = newMinMax.value;
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
    public virtual void Add(MinMaxFloat newMinMax)
    {
        max += newMinMax.max;
        value += newMinMax.value;
    }
    /// <summary>
    /// Add another MinMax variable's values to this one
    /// </summary>
    /// <param name="newMinMax"></param>
    public virtual void Remove(MinMaxFloat newMinMax)
    {
        max += newMinMax.max;
        value += newMinMax.value;
    }

}
/// <summary>
/// MinMaxFloat that also includes EventHandlers.
/// </summary>
[System.Serializable]
public class MinMaxEventFloat : MinMaxFloat
{

    public delegate void EventHandler();
    public delegate void EventHandlerFloat(float value);

    public EventHandlerFloat OnValueChangeBy = null;
    public EventHandlerFloat OnValueChangeTo = null;
    public EventHandler OnValueChanged = null;
    public EventHandler OnValueIncrease = null;
    public EventHandler OnValueDecrease = null;
    public EventHandler OnValueMax = null;
    public EventHandler OnValueMin = null;
    public EventHandler OnValueAboveHalf = null;
    public EventHandler OnValueBelowHalf = null;
    public Threshold[] thresholds = new Threshold[10];
    //	public EventHandlerfloat OnValueAbove;
    //	public EventHandlerfloat OnValueBelow;
    //	public EventHandlerFloat OnValueAbovePercent; 
    //	public EventHandlerFloat OnValueBelowPercent;

    public override float value
    {
        get
        {
            return _value;
        }
        set
        {
            if (this.value == value) return;

            float oldValue = this.value;
            float oldPercentage = percentage;

            _value = Mathf.Clamp(value, _min, _max);

            float newPercentage = percentage;

            if (oldPercentage < 0.5f && newPercentage > 0.5f && OnValueAboveHalf != null) OnValueAboveHalf();
            if (oldPercentage > 0.5f && newPercentage < 0.5f && OnValueBelowHalf != null) OnValueBelowHalf();

            if (OnValueChanged != null) OnValueChanged();
            if (_value > oldValue)
            {
                if (OnValueIncrease != null) OnValueIncrease();
            }
            else
            {
                if (OnValueDecrease != null) OnValueDecrease();
            }
            if (_value == max && OnValueMax != null) OnValueMax();
            if (_value == min && OnValueMin != null) OnValueMin();

            float difference = value - oldValue;
            if (OnValueChangeBy != null) OnValueChangeBy(difference);

            if (OnValueChangeTo != null) OnValueChangeTo(_value);

            for (int n = 0; n < thresholds.Length - 1; n++)
            {
                if (thresholds[n] != null) thresholds[n].CheckActivation(oldValue, _value);
            }
        }
    }

    public MinMaxEventFloat()
    {
        min = 0f;
        max = 1f;
        value = 0f;
    }

    public MinMaxEventFloat(float min, float max, float value)
    {
        this.min = min;
        this.max = max;
        _value = Mathf.Clamp(value, min, max);
    }

    public Threshold AddThreshold(Threshold.Type type, float value, UnityAction result)
    {
        Threshold newThreshold = new Threshold(type, value, result);

        for (int n = 0; n < thresholds.Length; n++)
        {
            if (thresholds[n] == null) thresholds[n] = newThreshold;
        }

        return newThreshold;
    }

    public void ClearThresholds()
    {
        thresholds = new Threshold[10];
    }

    public class Threshold
    {
        public float value;
        public enum Type { Above, Below }
        public Type type;
        public UnityAction result;
        public EventHandler OnActivate = null;

        public Threshold(Type type, float value, UnityAction result)
        {
            this.type = type;
            this.value = value;
            this.result = result;

        }

        public void CheckActivation(float oldValue, float newValue)
        {
            if (newValue > oldValue && type == Type.Above)
            {
                Activate();
            }
            else if (newValue < oldValue && type == Type.Below)
            {
                Activate();
            }
        }

        void Activate()
        {
            if (OnActivate != null) OnActivate();
            if (result != null) result();
        }
    }

}