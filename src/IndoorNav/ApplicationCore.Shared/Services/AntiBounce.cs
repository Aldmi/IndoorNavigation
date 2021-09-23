using System;

namespace ApplicationCore.Shared.Services
{
    /// <summary>
    /// Антидребезг входного значения.
    /// </summary>
    public class AntiBounce<T> where T : class, IEquatable<T>
    {
        private readonly int _acceptedCount;
        
        /// <summary>
        /// Кол-во одинаковых значений
        /// </summary>
        private int _currentAcceptedCount;
        
        /// <summary>
        /// Принятое значение
        /// прошедшее антидребезг.
        /// </summary>
        private T? _acceptedValue;
        
        /// <summary>
        /// Значение на предыдущем шаге
        /// </summary>
        private T? _lastValue;
        
        
        public AntiBounce(int acceptedCount)
        {
            _acceptedCount = acceptedCount;
            _acceptedValue = default;
        }
        
        
        /// <summary>
        /// Вызвать функцию антидребезга.
        /// </summary>
        /// <param name="value">входное значение</param>
        public T Invoke(T value)
        {
            if (_acceptedValue == null)
            {
                _acceptedValue = value;
                _lastValue = value;
                _currentAcceptedCount = 1;
                return _acceptedValue;
            }

            if (_lastValue.Equals(value))
            {
                _currentAcceptedCount++;
            }
            else
            {
                _lastValue = value;
                _currentAcceptedCount = 1;
            }
            
            if (_currentAcceptedCount >= _acceptedCount)
            {
                _acceptedValue = _lastValue;
            }
            return _acceptedValue;
        }
    }
}