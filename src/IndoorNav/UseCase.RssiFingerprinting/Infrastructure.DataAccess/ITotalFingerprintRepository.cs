using System.Collections.Generic;
using ApplicationCore.Domain.RssiFingerprinting.Model;

namespace UseCase.RssiFingerprinting.Infrastructure.DataAccess
{
    public interface ITotalFingerprintRepository
    {
        List<TotalFingerprint> GetTotalFingerprintList();
    }
}