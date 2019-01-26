public class EventVariable<T> 
{
    public delegate void OnVariableChangedEventHandler(T newValue);
    public event OnVariableChangedEventHandler OnVariableChanged;

    private T _Value;
    public T Value
    {
        get { return _Value; }
        set
        {
            if(_Value.Equals(value))
            {
                return;
            }

            _Value = value;

            if(OnVariableChanged != null)
            {
                OnVariableChanged(_Value);
            }
        }
    }

    public EventVariable(T value)
    {
        _Value = value;
    }
}
