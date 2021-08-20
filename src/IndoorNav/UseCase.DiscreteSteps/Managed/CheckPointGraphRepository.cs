using System;
using ApplicationCore.Domain.DiscreteSteps;
using ApplicationCore.Shared.DataStruct;
using ApplicationCore.Shared.DataStruct.Tree;
using Libs.Beacons.Models;

namespace UseCase.DiscreteSteps.Managed
{
    public class CheckPointGraphRepository : ICheckPointGraphRepository  //TODO: вынести в Infrastructure
    {
        // /// <summary>
        // /// 
        // /// </summary>
        // public CheckPointGraph GetGraph()
        // {
        //     var root = new TreeNode<CheckPoint>(new CheckPoint(
        //         new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 65438, 43487),
        //         new CheckPointDescription("Вход", "Вход на вокзал"),
        //         new CoverageArea(2))
        //     );
        //     
        //     var k1= root.AddChild(new CheckPoint(
        //         new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 2),
        //         new CheckPointDescription("Коридор 1", "Главный коридор"),
        //         new CoverageArea(2)));
        //     
        //     k1.AddChildren(new CheckPoint(
        //         new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 3),
        //         new CheckPointDescription("К поездам", "Выход на преррон"),
        //         new CoverageArea(2)), new CheckPoint(
        //         new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 4),
        //         new CheckPointDescription("Выход в город", "Выход из вокзала в город"),
        //         new CoverageArea(2)));
        //     
        //     var k2=k1.AddChild(new CheckPoint(
        //         new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 5),
        //         new CheckPointDescription("Коридор 2", "Коридор 2"),
        //         new CoverageArea(2)));
        //     
        //     k2.AddChildren(new CheckPoint(
        //         new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 6),
        //         new CheckPointDescription("Кассы", "Кассы"),
        //         new CoverageArea(2)), new CheckPoint(
        //         new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 1, 7),
        //         new CheckPointDescription("Столовая", "Столовая"),
        //         new CoverageArea(2)));
        //     
        //     var graph = new CheckPointGraph(root);
        //     return graph;
        // }
        
        
        public CheckPointGraph GetGraph()
        {
            var root = new TreeNode<CheckPoint>(new CheckPoint(
                new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 65438, 43487),
                new CheckPointDescription("Лифт", "лифтовой холл"),
                new CoverageArea(1.3))
            );
            
            var k1= root.AddChild(new CheckPoint(
                new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 35144, 19824),
                new CheckPointDescription("Коридор", "Коридор"),
                new CoverageArea(1.3)));
            
            k1.AddChildren(new CheckPoint(
                new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 48943, 20570),
                new CheckPointDescription("Кв.'102'", "Конец коридора Кв '102'"),
                new CoverageArea(1.3)), new CheckPoint(
                new BeaconId(Guid.Parse("f7826da6-4fa2-4e98-8024-bc5b71e0893e"), 56954, 34501),
                new CheckPointDescription("Кв.'98'", "Конец коридора Кв '98'"),
                new CoverageArea(1.3)));
            
            var graph = new CheckPointGraph(root);
            return graph;
        }
    }
}