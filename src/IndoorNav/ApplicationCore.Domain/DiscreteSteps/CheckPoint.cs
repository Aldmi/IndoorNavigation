using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DiscreteSteps
{
    /// <summary>
    /// Узел графа.
    /// </summary>
    public class CheckPoint
    {
        public CheckPointDescription Description { get;  }
        public BeaconId BeaconId { get;  }
        public CoverageArea Area { get;  }
        public CheckPoint[]? Childrens { get;  }
        
        public CheckPoint(
            BeaconId beaconId,
            CheckPointDescription description,
            CoverageArea area,
            CheckPoint[]? childrens)
        {
            BeaconId = beaconId;
            Description = description;
            Area = area;
            Childrens = childrens;
        }
    }
}