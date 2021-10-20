namespace ApplicationCore.Domain.RssiFingerprinting.Model
{
    /// <summary>
    /// Похожесть отпечатков
    /// </summary>
    public class SimilarCompassFingerprint
    {


        public static bool operator >(SimilarCompassFingerprint s1, SimilarCompassFingerprint s2)
        {
            return true;
        }

        public static bool operator <(SimilarCompassFingerprint s1, SimilarCompassFingerprint s2)
        {
            return false;
        }
        
        
        public static SimilarCompassFingerprint Create(CompassFingerprint fp1, CompassFingerprint fp2)
        {
            return new SimilarCompassFingerprint();
        }
    }
}