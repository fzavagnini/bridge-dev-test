using System;
using System.Collections.Generic;
using System.Linq;

namespace bridge_dev_test.Services
{
    public interface IRouteService
    {
        int? ProcessShortestDistanceBetweenTwoAcademies(string source, string destination,
            ILookup<int, string> lookupRoutesTable, Dictionary<string, List<Tuple<string, int>>> test);

        bool IsValidRoute(string route, Dictionary<string, List<Tuple<string, int>>> routeConnectionsAndDistances);

        int? GetDistanceBetweenRoutes(string routeString,
            Dictionary<string, List<Tuple<string, int>>> routeConnectionsAndDistances);

        Tuple<Tuple<string, string>, int> ProcessSingleRoute(string route);
        List<Tuple<Tuple<string, string>, int>> ProcessAllRoutes(string routes);

        ILookup<int, string> GetRoutesBasedOnNumberOfStops(Dictionary<string, List<Tuple<string, int>>> test,
            int numberOfStops);

        Dictionary<string, List<Tuple<string, int>>> ProcessRouteConnectionsAndDistances(
            List<Tuple<Tuple<string, string>, int>> routes);

        int? GetNumberOfTripsBetweenTwoAcademies(
            string source,
            string destination,
            ILookup<int, string> differentRoutes,
            Dictionary<string, List<Tuple<string, int>>> routeConnectionsAndDistances,
            bool hasMaxNumberOfStops = false, bool hasMaximumDistance = false, int numberOfStops = 0,
            int maximumDistance = 0);

    }
}