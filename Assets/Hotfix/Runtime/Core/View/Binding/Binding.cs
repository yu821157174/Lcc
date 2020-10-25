﻿namespace LccHotfix
{
    public class Binding<T>
    {
        public delegate void ValueChangeHandler(T oldValue, T newValue);
        public event ValueChangeHandler ValueChange;
        private T _value;
        public Binding()
        {
        }
        public Binding(T value)
        {
            Value = value;
        }
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!Equals(_value, value))
                {
                    T old = _value;
                    _value = value;
                    OnValueChange(old, _value);
                }
            }
        }
        public void OnValueChange(T oldValue, T newValue)
        {
            ValueChange?.Invoke(oldValue, newValue);
        }
    }
}