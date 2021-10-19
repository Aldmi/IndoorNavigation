namespace Test.Beacons.UseCaseTests
{

    // public class TrillaterationFlowTest
    // {
    //     [Fact]
    //     public async Task ManagedScanFlowTest()
    //     {
    //         //Arrange
    //         var options = FourBeaconInRoom.CreateOption();
    //         var sphereFactory= new SphereFactory(RssiHelpers.CalculateDistance, options);
    //         var sourse = FourBeaconInRoom.CreateFlowImmediatly();
    //         var whiteList = options.Select(o => o.BeaconId).ToList();
    //         
    //          //act
    //          var spheres= sourse.ManagedScanFlow(whiteList, TimeSpan.FromSeconds(0.1), sphereFactory);
    //          var filteredBeacons =(await spheres.ToTask()).ToList();
    //         
    //          //Assert
    //          filteredBeacons.Count.Should().Be(4);
    //     }
    // }
}