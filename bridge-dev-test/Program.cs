using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bridge_dev_test.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace bridge_dev_test
{
    class Program
    {
        private static IRouteService _routeService;
        static void Main(string[] args)
        {

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IRouteService, RouteService>()
                .BuildServiceProvider();

            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            
            _routeService = serviceProvider.GetService<IRouteService>();

            InitializeMenu();

            Console.WriteLine();
            Console.WriteLine("Thank you for using the route tool!!");
            Console.WriteLine();

        }

        private static void InitializeMenu()
        {
            List<Tuple<Tuple<string, string>, int>> dataStructure = _routeService.ProcessAllRoutes("AB3, AF4, AH18, BC14, BJ50, CD10, CF12, DE8, DF9, ED8, EF6, EG5, FD9, GA2, GE5, HA18, HI20, IJ25, IB35, JB50");

            var routeConnectionsAndDistances = _routeService.ProcessRouteConnectionsAndDistances(dataStructure);

            //Assuming a maximum of 20 stops for route creation lookup table
            var maxNumberOfStops = 20;

            var differentRoutes = _routeService.GetRoutesBasedOnNumberOfStops(routeConnectionsAndDistances, maxNumberOfStops);

            bool showMenu = true;

            while (showMenu)
            {
                showMenu = MainMenu(routeConnectionsAndDistances, differentRoutes);
            }

        }

        private static bool MainMenu(Dictionary<string, List<Tuple<string, int>>> academiesAvailable, ILookup<int, string> lookupRoutes)
        {

            Console.WriteLine("Choose an option:");
            Console.WriteLine();
            Console.WriteLine("1) Calculate the distance between routes");
            Console.WriteLine("2) Calculate the number of routes between two academies");
            Console.WriteLine("3) Calculate the shortest route between two academies");
            Console.WriteLine("4) Run test input sample data (AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7)");
            Console.WriteLine("5) Exit");
            Console.Write("\r\nSelect an option: ");

            int? distanceTest = 0;
            string distanceTestDisplay = string.Empty;

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine();
                    Console.WriteLine("These are all the academies available: ");
                    Console.WriteLine();

                    foreach (var academy in academiesAvailable)
                    {
                        Console.Write(academy.Key + " ");
                    }

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Please enter route (ie; ABC, CDC, ADEC, BAC) ");
                    Console.WriteLine();

                    var route = Console.ReadLine();
                    var distance = _routeService.GetDistanceBetweenRoutes(route, academiesAvailable);

                    if (distance == null)
                    {
                        Console.WriteLine();
                        Console.WriteLine("NO SUCH ROUTE");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("The distance between " + route?[0] + " and " + route?[^1] + " is " + distance);
                    }

                    Console.WriteLine();
                    return true;

                case "2":

                    Console.WriteLine();
                    Console.WriteLine("These are all the academies available: ");
                    Console.WriteLine();

                    foreach (var academy in academiesAvailable)
                    {
                        Console.Write(academy.Key + " ");
                    }

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Please enter source route (ie; A, B, C, D, E) ");
                    Console.WriteLine();

                    var source = Console.ReadLine();

                    Console.WriteLine();
                    Console.WriteLine("Please enter destination route (ie; A, B, C, D, E) ");
                    Console.WriteLine();

                    var destination = Console.ReadLine();

                    Console.WriteLine();
                    Console.WriteLine("Do you have a maximum number of stops? (Y/N)");
                    Console.WriteLine();

                    var maxNumberOfStopsOption = Console.ReadLine();

                    if (maxNumberOfStopsOption == "Y")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please enter the maximum number of stops");
                        Console.WriteLine();

                        var maxNumberOfStops = Console.ReadLine();

                        Console.WriteLine();

                        var numberOfTripsBetweenTwoPoints = _routeService.GetNumberOfTripsBetweenTwoAcademies(
                            source,
                            destination,
                            lookupRoutes,
                            academiesAvailable,
                            true, false, Convert.ToInt32(maxNumberOfStops), 0);

                        Console.WriteLine("The number of trips between " + source + " and " + destination + " is " + numberOfTripsBetweenTwoPoints);

                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Do you have a maximum distance? (Y/N)");
                        Console.WriteLine();

                        var maximumDistanceOption = Console.ReadLine();

                        if (maximumDistanceOption == "Y")
                        {

                            Console.WriteLine();
                            Console.WriteLine("Please enter the maximum distance: ");
                            Console.WriteLine();

                            var maximumDistance = Console.ReadLine();

                            var maxDistance = _routeService.GetNumberOfTripsBetweenTwoAcademies(
                                source,
                                destination,
                                lookupRoutes,
                                academiesAvailable,
                                false, true, 0, Convert.ToInt32(maximumDistance));

                            Console.WriteLine();
                            Console.WriteLine("The number of trips between " + source + " and " + destination + " is " + maxDistance);

                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Please enter the number of stops");
                            Console.WriteLine();

                            var numberOfStops = Console.ReadLine();

                            Console.WriteLine();

                            var numberOfTripsBetweenTwoPoints = _routeService.GetNumberOfTripsBetweenTwoAcademies(
                                source,
                                destination,
                                lookupRoutes,
                                academiesAvailable,
                                false, false, Convert.ToInt32(numberOfStops), 0);

                            Console.WriteLine("The number of trips between " + source + " and " + destination + " is " + numberOfTripsBetweenTwoPoints);

                        }

                    }

                    Console.WriteLine();

                    return true;

                case "3":

                    Console.WriteLine();
                    Console.WriteLine("Please enter source route (ie; A, B, C, D, E) ");
                    Console.WriteLine();

                    var shortestSource = Console.ReadLine();

                    Console.WriteLine();
                    Console.WriteLine("Please enter destination route (ie; A, B, C, D, E) ");
                    Console.WriteLine();

                    var shortestDestination = Console.ReadLine();

                    Console.WriteLine();

                    var shortestDistanceBetweenAcademies = _routeService.ProcessShortestDistanceBetweenTwoAcademies(shortestSource, shortestDestination, lookupRoutes, academiesAvailable);

                    Console.WriteLine("The shortest distance between " + shortestSource + " and " + shortestDestination + " is " + shortestDistanceBetweenAcademies);
                    Console.WriteLine();

                    return true;

                case "4":

                    Console.WriteLine();

                    List<Tuple<Tuple<string, string>, int>> dataStructure = _routeService.ProcessAllRoutes("AB3, AF4, AH18, BC14, BJ50, CD10, CF12, DE8, DF9, ED8, EF6, EG5, FD9, GA2, GE5, HA18, HI20, IJ25, IB35, JB50");
                    var fixedNumberOfStops = 10;
                    var differentRoutes = _routeService.GetRoutesBasedOnNumberOfStops(academiesAvailable, fixedNumberOfStops);

                    var routeConnectionsAndDistances = _routeService.ProcessRouteConnectionsAndDistances(dataStructure);
                    //1
                    distanceTest = _routeService.GetDistanceBetweenRoutes("ABC", routeConnectionsAndDistances);
                    distanceTestDisplay = distanceTest == null ? "NO SUCH ROUTE" : Convert.ToString(distanceTest);
                    Console.WriteLine(distanceTestDisplay);

                    //2
                    distanceTest = _routeService.GetDistanceBetweenRoutes("AEBCD", routeConnectionsAndDistances);
                    distanceTestDisplay = distanceTest == null ? "NO SUCH ROUTE" : Convert.ToString(distanceTest);
                    Console.WriteLine(distanceTestDisplay);

                    //3
                    distanceTest = _routeService.GetDistanceBetweenRoutes("AED", routeConnectionsAndDistances);
                    distanceTestDisplay = distanceTest == null ? "NO SUCH ROUTE" : Convert.ToString(distanceTest);
                    Console.WriteLine(distanceTestDisplay);

                    //4
                    var numberOfTripsBetweenTwoPointss = _routeService.GetNumberOfTripsBetweenTwoAcademies("C", "C", differentRoutes, routeConnectionsAndDistances, true, false, 3, 0);
                    Console.WriteLine(numberOfTripsBetweenTwoPointss);

                    //5
                    var numberOfTripsBetweenTwoPointsTwo = _routeService.GetNumberOfTripsBetweenTwoAcademies("A", "C", differentRoutes, routeConnectionsAndDistances, false, false, 4, 0);
                    Console.WriteLine(numberOfTripsBetweenTwoPointsTwo);

                    //6
                    var shortestDistance =
                        _routeService.ProcessShortestDistanceBetweenTwoAcademies("A", "C", differentRoutes, routeConnectionsAndDistances);
                    Console.WriteLine(shortestDistance);

                    //7
                    var shortestDistanceTwo =
                        _routeService.ProcessShortestDistanceBetweenTwoAcademies("B", "B", differentRoutes, routeConnectionsAndDistances);
                    Console.WriteLine(shortestDistanceTwo);

                    //8
                    var maxNumberWithDistance = _routeService.GetNumberOfTripsBetweenTwoAcademies("C", "C", differentRoutes, routeConnectionsAndDistances, false,
                        true, 0, 30);

                    Console.WriteLine(maxNumberWithDistance);
                    Console.WriteLine();

                    return true;

                case "5":
                    return false;

                default:
                    return true;
            }
        }
    }
}
