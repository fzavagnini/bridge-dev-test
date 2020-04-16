using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace bridge_dev_test.Services
{
    public class RouteService : IRouteService
    {
        public int? ProcessShortestDistanceBetweenTwoAcademies(string source, string destination, ILookup<int, string> lookupRoutesTable,
            Dictionary<string, List<Tuple<string, int>>> test)
        {
            List<int?> shortestDistanceBetweenPoints = new List<int?>();
            List<Tuple<int?, string>> shortestDistanceBetweenPointsWithRoute = new List<Tuple<int?, string>>();
            foreach (var routes in lookupRoutesTable)
            {
                var routeOptions = routes.Where(x => x.StartsWith(source) && x.EndsWith(destination)).ToList();

                foreach (var routeOption in routeOptions)
                {
                    var distance = GetDistanceBetweenRoutes(routeOption, test);
                    shortestDistanceBetweenPoints.Add(distance);
                    shortestDistanceBetweenPointsWithRoute.Add(new Tuple<int?, string>(distance, routeOption));
                }
            }

            return shortestDistanceBetweenPoints.Min();
        }

        public bool IsValidRoute(string route, Dictionary<string, List<Tuple<string, int>>> routeConnectionsAndDistances)
        {
            return GetDistanceBetweenRoutes(route, routeConnectionsAndDistances) != null;
        }

        public int? GetDistanceBetweenRoutes(string routeString, Dictionary<string, List<Tuple<string, int>>> routeConnectionsAndDistances)
        {
            var routeListChar = routeString.ToCharArray().ToList();

            int? finalDistance = 0;

            for (int i = 0; i < routeListChar.Count - 1; i++)
            {
                var nextConnection = routeListChar[i + 1];
                var connections = routeConnectionsAndDistances[routeListChar[i].ToString()];
                var distance = connections.FirstOrDefault(x => x.Item1 == nextConnection.ToString())?.Item2;
                finalDistance += distance;
            }

            return finalDistance;
        }

        public Tuple<Tuple<string, string>, int> ProcessSingleRoute(string route)
        {
            return new Tuple<Tuple<string, string>, int>(new Tuple<string, string>(Convert.ToString(route[0]), Convert.ToString(route[1])), Convert.ToInt32(Regex.Match(route, @"\d+").Value));
        }

        public List<Tuple<Tuple<string, string>, int>> ProcessAllRoutes(string routes)
        {
            List<Tuple<Tuple<string, string>, int>> dataStructure = new List<Tuple<Tuple<string, string>, int>>();

            List<string> routeList = routes.Split(',').Select(x => x.Trim()).ToList();

            foreach (var route in routeList)
            {
                dataStructure.Add(ProcessSingleRoute(route));
            }

            return dataStructure;
        }

        public ILookup<int, string> GetRoutesBasedOnNumberOfStops(Dictionary<string, List<Tuple<string, int>>> test, int numberOfStops)
        {
            List<string> routesBasedOnNumberOfStops = new List<string>();

            foreach (var key in test.Keys)
            {
                for (int i = 0; i < test[key].Count; i++)
                {
                    routesBasedOnNumberOfStops.Add(key + test[key][i].Item1);
                }
            }

            for (int i = 0; i < routesBasedOnNumberOfStops.Count; i++)
            {

                if (routesBasedOnNumberOfStops[i].Length - 1 == numberOfStops)
                {
                    break;
                }

                {
                    foreach (var connectedChars in test[routesBasedOnNumberOfStops[i].Last().ToString()])
                    {
                        routesBasedOnNumberOfStops.Add(routesBasedOnNumberOfStops[i] + connectedChars.Item1);
                    }
                }
            }
            return routesBasedOnNumberOfStops.ToLookup(route => route.Length - 1);
        }

        public Dictionary<string, List<Tuple<string, int>>> ProcessRouteConnectionsAndDistances(List<Tuple<Tuple<string, string>, int>> routes)
        {
            Dictionary<string, List<Tuple<string, int>>> routeConnectionsDict = new Dictionary<string, List<Tuple<string, int>>>();

            foreach (var route in routes)
            {
                if (!routeConnectionsDict.TryGetValue(route.Item1.Item1, out var connectionRouteList))
                {
                    connectionRouteList = new List<Tuple<string, int>>();
                    connectionRouteList.Add(new Tuple<string, int>(route.Item1.Item2, route.Item2));
                    routeConnectionsDict.Add(route.Item1.Item1, connectionRouteList);
                }
                else
                {
                    connectionRouteList = routeConnectionsDict[route.Item1.Item1];
                    connectionRouteList.Add(new Tuple<string, int>(route.Item1.Item2, route.Item2));
                    routeConnectionsDict[route.Item1.Item1] = connectionRouteList;
                }
            }

            return routeConnectionsDict;
        }

        public int? GetNumberOfTripsBetweenTwoAcademies(string source, string destination, ILookup<int, string> differentRoutes,
            Dictionary<string, List<Tuple<string, int>>> routeConnectionsAndDistances, bool hasMaxNumberOfStops = false, bool hasMaximumDistance = false,
            int numberOfStops = 0, int maximumDistance = 0)
        {
            var listOfMaxNumberOfStops = new List<string>();
            var listOfMaxDistances = new List<string>();
            var stops = new List<string>();
            var finalStops = new List<string>();

            if (hasMaximumDistance ^ hasMaxNumberOfStops)
            {
                if (hasMaxNumberOfStops)
                {
                    for (int i = 1; i <= numberOfStops; i++)
                    {
                        stops = differentRoutes[i].ToList();
                        finalStops = stops.Where(x => x.StartsWith(source) && x.EndsWith(destination)).ToList();
                        listOfMaxNumberOfStops.AddRange(finalStops);
                    }

                    return listOfMaxNumberOfStops.Count;
                }

                if (hasMaximumDistance)
                {
                    foreach (var routes in differentRoutes)
                    {
                        var routeOptions = routes.Where(x => x.StartsWith(source) && x.EndsWith(destination)).ToList();

                        foreach (var routeOption in routeOptions)
                        {
                            var distance = GetDistanceBetweenRoutes(routeOption, routeConnectionsAndDistances);

                            if (distance < maximumDistance)
                            {
                                listOfMaxDistances.Add(routeOption);
                            }
                        }
                    }

                    return listOfMaxDistances.Count();
                }
            }

            
            else if (hasMaximumDistance && hasMaxNumberOfStops)
            {
                for (int i = 1; i <= numberOfStops; i++)
                {
                    stops = differentRoutes[i].ToList();
                    finalStops = stops.Where(x => x.StartsWith(source) && x.EndsWith(destination)).ToList();
                    listOfMaxNumberOfStops.AddRange(finalStops);
                }

                foreach (var routeOption in listOfMaxNumberOfStops)
                {
                    var distance = GetDistanceBetweenRoutes(routeOption, routeConnectionsAndDistances);

                    if (distance < maximumDistance)
                    {
                        listOfMaxDistances.Add(routeOption);
                    }
                }

                return listOfMaxDistances.Count();
            }

            else
            {
                stops = differentRoutes[numberOfStops].ToList();
                finalStops = stops.Where(x => x.StartsWith(source) && x.EndsWith(destination)).ToList();
                listOfMaxNumberOfStops.AddRange(finalStops);
            }

            return listOfMaxNumberOfStops.Count;
        }
    }
}