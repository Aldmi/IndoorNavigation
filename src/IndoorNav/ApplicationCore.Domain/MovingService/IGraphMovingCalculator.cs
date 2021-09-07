using System;
using System.Collections.Generic;
using ApplicationCore.Domain.CheckPointModel;
using ApplicationCore.Domain.DistanceService;
using ApplicationCore.Domain.DistanceService.Model;
using ApplicationCore.Domain.MovingService.Model;
using ApplicationCore.Shared.DataStruct.GraphNotOriented;

namespace ApplicationCore.Domain.MovingService
{
    /// <summary>
    /// Вычислитель перемещения.
    /// </summary>
    public interface IGraphMovingCalculator
    {
        Vertex<CheckPointBase>? CurrentVertex { get; }
        bool CurrentVertexIsSet { get; }
        Guid SharedUuid { get; }

        Moving CalculateMove(IEnumerable<BeaconDistance> inputDataList);
        void Reset();
    }
}