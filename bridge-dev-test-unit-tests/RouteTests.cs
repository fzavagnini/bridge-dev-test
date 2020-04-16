using System;
using System.Collections.Generic;
using System.Linq;
using bridge_dev_test.Services;
using NUnit.Framework;

namespace bridge_dev_test_unit_tests
{
    public class RouteTests
    {
        private RouteService _routeService;
        private List<Tuple<Tuple<string, string>, int>> _routesAndDistancesFromPaths;
        private static ILookup<int, string> _lookupRouteTable;
        private static Dictionary<string, List<Tuple<string, int>>> _academiesAvailable;
        private const int _fixedNumberOfStops = 20; //Potentially 20 stops should all it take reach from source to destination
        private const string _fixedRoutesPath = "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7"; // We could read this from file

        [SetUp]
        public void SetUp()
        {
            _routeService = new RouteService();
            _routesAndDistancesFromPaths = _routeService.ProcessAllRoutes(_fixedRoutesPath);
            _academiesAvailable = _routeService.ProcessRouteConnectionsAndDistances(_routesAndDistancesFromPaths);
            _lookupRouteTable = _routeService.GetRoutesBasedOnNumberOfStops(_academiesAvailable, _fixedNumberOfStops);
        }

        [TestCase("ABC")]
        [TestCase("AEBCD")]
        public void CalculateDistanceBetweenAcademies_IfThereIsAPath_ShouldReturnValue(string route)
        {
            var distance = _routeService.GetDistanceBetweenRoutes(route, _academiesAvailable); ;
            Assert.IsNotNull(distance, $"{route}");
        }

        
        [TestCase("AED")]
        public void CalculateDistanceBetweenAcademies_IfThereIsNotAPath_ShouldReturnNull(string route)
        {
            var distance = _routeService.GetDistanceBetweenRoutes(route, _academiesAvailable); ;
            Assert.IsNull(distance, null);
        }

        [TestCase("C", "C", 3, true, false, 0)]
        [TestCase("A", "C", 4, false, false, 0)]
        [TestCase("C", "C", 0, false, true, 30)]
        [TestCase("C", "C", 7, true, true, 30)]
        public void CalculateNumberOfTripsBetweenAcademies_ShouldReturnValue(
            string source, 
            string destination, 
            int numberOfStops, 
            bool hasMaximumNumberOfStops, 
            bool hasMaximumDistance,
            int maximumDistance)
        {
            var numberOfTripsBetweenAcademies =
                _routeService.GetNumberOfTripsBetweenTwoAcademies(
                    source, destination, _lookupRouteTable, _academiesAvailable,
                    hasMaximumNumberOfStops, hasMaximumDistance, numberOfStops, maximumDistance);

            Assert.IsNotNull(numberOfTripsBetweenAcademies);
        }

        [TestCase("A", "C")]
        [TestCase("B", "B")]
        public void CalculateShortestDistanceBetweenAcademies_ShouldReturnValue(string source, string destination)
        {
            var shortestDistance =
                _routeService.ProcessShortestDistanceBetweenTwoAcademies(source, destination, _lookupRouteTable,
                    _academiesAvailable);
            Assert.IsNotNull(shortestDistance);
        }

        [Test]
        public void AllPossiblePaths_ShouldReturnValidRoutes()
        {

            List<bool> validRoutes = new List<bool>();
            var routeCount = _lookupRouteTable.SelectMany(x => x).Count();
            
            foreach (var route in _lookupRouteTable)
            {
                foreach (var r in route)
                {
                    validRoutes.Add(_routeService.IsValidRoute(r, _academiesAvailable));
                }
            }

            Assert.AreEqual(routeCount, validRoutes.Count);
        }
    }
}