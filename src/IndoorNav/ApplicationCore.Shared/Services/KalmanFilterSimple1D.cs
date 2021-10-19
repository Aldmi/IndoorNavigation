namespace ApplicationCore.Shared.Services
{
    /// <summary>
    /// простой фильтр, не учитывает TimeStamp входных данных.
    /// </summary>
    public class KalmanFilterSimple1D
    {
        public double X0 {get; private set;} // predicted state
        public double P0 { get; private set; } // predicted covariance

        public double F { get; } // factor of real value to previous real value
        public double Q { get; } // measurement noise
        public double H { get; } // factor of measured value to real value
        public double R { get; } // environment noise

        public double State { get; private set; }
        public double Covariance { get; private set; }

        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="q">Сглаживающие свойства. определение шума процесса, насколько можем доверять предсказанным данным или данным полученным при корректировки.</param>
        /// <param name="r"> Сглаживающие свойства. ошибка измерения может быть определена испытанием измерительных приборов и определением погрешности их измерения.</param>
        /// <param name="f">Модель процесса. переменная описывающая динамику системы ( =1, мы указываем, что предсказываемое значение будет равно предыдущему состоянию)</param>
        /// <param name="h">Модель процесса. матрица определяющая отношение между измерениями и состоянием системы</param>
        public KalmanFilterSimple1D(double q, double r, double f = 1, double h = 1)
        {
            Q = q;
            R = r;
            F = f;
            H = h;
        }

        public void SetState(double state, double covariance)
        {
            State = state;
            Covariance = covariance;
        }

        public void Correct(double data)
        {
            //time update - prediction
            X0 = F*State;
            P0 = F*Covariance*F + Q;

            //measurement update - correction
            var K = H*P0/(H*P0*H + R);
            State = X0 + K*(data - H*X0);
            Covariance = (1 - K*H)*P0;            
        }
    }
}