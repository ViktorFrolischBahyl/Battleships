using Battleships.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Battleships.Tests.Services;

[TestClass]
public sealed class LoadShipsDefinitionsFromFileServiceTests
{
    public LoadShipsDefinitionsFromFileServiceTests()
    {
        var builder = Program.CreateHostBuilder(args: []);

        var host = builder.Build();

        this.LoadShipsDefinitionsService = ActivatorUtilities.GetServiceOrCreateInstance<ILoadShipsDefinitionsService>(host.Services);
    }

    private ILoadShipsDefinitionsService LoadShipsDefinitionsService { get; }

    [TestMethod]
    public void InstanceTest()
    {
        var service = this.LoadShipsDefinitionsService;

        Assert.IsNotNull(service);
    }

    [TestMethod]
    public void LoadShipsDefinitionsTest()
    {
        var ships = this.LoadShipsDefinitionsService.LoadShipsDefinitions();

        Assert.IsNotNull(ships);
        Assert.IsTrue(ships.Count == 7);

        for (int x = 0; x < ships.Count; x++)
        {
            var ship = ships[x];

            Assert.IsNotNull(ship);

            switch (x)
            {
                case 0:
                case 1:
                    Assert.IsNotNull(ship.Position);
                    Assert.AreEqual("Submarine", ship.Type);

                    Assert.IsNotNull(ship.Shape);
                    Assert.AreEqual(1, ship.Shape.GetLength(0));
                    Assert.AreEqual(1, ship.Shape.GetLength(1));

                    Assert.AreEqual(1, ship.GetNumberOfShipCells());
                    break;

                case 2:
                case 3:
                    Assert.IsNotNull(ship.Position);
                    Assert.AreEqual("Boat", ship.Type);

                    Assert.IsNotNull(ship.Shape);
                    Assert.AreEqual(2, ship.Shape.GetLength(0));
                    Assert.AreEqual(1, ship.Shape.GetLength(1));

                    Assert.AreEqual(2, ship.GetNumberOfShipCells());
                    break;

                case 4:
                    Assert.IsNotNull(ship.Position);
                    Assert.AreEqual("Carrier", ship.Type);

                    Assert.IsNotNull(ship.Shape);
                    Assert.AreEqual(3, ship.Shape.GetLength(0));
                    Assert.AreEqual(1, ship.Shape.GetLength(1));

                    Assert.AreEqual(3, ship.GetNumberOfShipCells());
                    break;

                case 5:
                    Assert.IsNotNull(ship.Position);
                    Assert.AreEqual("Plus", ship.Type);

                    Assert.IsNotNull(ship.Shape);
                    Assert.AreEqual(3, ship.Shape.GetLength(0));
                    Assert.AreEqual(3, ship.Shape.GetLength(1));

                    Assert.AreEqual(5, ship.GetNumberOfShipCells());
                    break;

                case 6:
                    Assert.IsNotNull(ship.Position);
                    Assert.AreEqual("L-Boat", ship.Type);

                    Assert.IsNotNull(ship.Shape);
                    Assert.AreEqual(4, ship.Shape.GetLength(0));
                    Assert.AreEqual(2, ship.Shape.GetLength(1));

                    Assert.AreEqual(5, ship.GetNumberOfShipCells());
                    break;
            }
        }
    }
}