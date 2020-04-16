Bridge Dev Test Problem 1 Solution

Console application menu that calculates the following:

1. The distance along certain routes.
2. The number of different routes between two academies.
3. The shortest route between two academies.

Getting Started

Open solution
Build solution
Go to project folder bin/Debug/netcoreapp3.1
Run bridge-dev-test.exe

Prerequisites
.NET Core 3.1

Examples

Based on the requirements lets go through a couple of steps to obtain the desired results.

Once the app is running a recurring menu will appear until 5 is entered (Exit)

---------------------------------------------------------------------------
Choose an option:

1) Calculate the distance between routes
2) Calculate the number of routes between two academies
3) Calculate the shortest route between two academies
4) Run test input sample data (AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7)
5) Exit
---------------------------------------------------------------------------

We are presented with a couple of options

----------------------------------------------------------------------------------------------------------------------------------------------------------------------

1) Calculate the distance between routes gives you the distance between academies based on a path. For example, lets say you want to calculate the distance CEBCEBC

You would have to enter option 1 and consequently all the academies will be presented 
Now you want to enter the route CEBCEBC. Once you enter that route you will be presented with the total distance to go through it from start to finish

---------------------------------------------------------------------------
Select an option: 1

These are all the academies available:

A B C D E

Please enter route (ie; ABC, CDC, ADEC, BAC)

CEBCEBC

The distance between C and C is 18

---------------------------------------------------------------------------

In the case the route is not valid an error message appears 'NO SUCH ROUTE'

---------------------------------------------------------------------------

Select an option: 1

These are all the academies available:

A B C D E

Please enter route (ie; ABC, CDC, ADEC, BAC)

DAC

NO SUCH ROUTE

----------------------------------------------------------------------------------------------------------------------------------------------------------------------

2) Calculate the number of routes between two academies gives the total number of available routes between source and destination.

We are also presented with a couple of options like the following:

a) Do you need to know all the routes up to a maximum number of stops (Requirement 4)

b) Do you need to know all the routes with exactly a number of stops (Requirement 5)

c) Do you need to know all the routes that do not exceed a certain distance (Requirement 8)

Lets go through each scenario

---------------------------------------------------------------------
a) 

Select an option: 2

These are all the academies available:

A B C D E

Please enter source route (ie; A, B, C, D, E)

C

Please enter destination route (ie; A, B, C, D, E)

C

Do you have a maximum number of stops? (Y/N)

Y

Please enter the maximum number of stops

3

The number of trips between C and C is 2

---------------------------------------------------------------------	

b)

Select an option: 2

These are all the academies available:

A B C D E

Please enter source route (ie; A, B, C, D, E)

A

Please enter destination route (ie; A, B, C, D, E)

C

Do you have a maximum number of stops? (Y/N)

N

Do you have a maximum distance? (Y/N)

N

Please enter the number of stops

4

The number of trips between A and C is 3  

---------------------------------------------------------------------

c)

Select an option: 2

These are all the academies available:

A B C D E

Please enter source route (ie; A, B, C, D, E)

C

Please enter destination route (ie; A, B, C, D, E)

C

Do you have a maximum number of stops? (Y/N)

N

Do you have a maximum distance? (Y/N)

Y

Please enter the maximum distance:

30

The number of trips between C and C is 7

---------------------------------------------------------------------

3) Calculate the shortest route between two academies gives the most efficient way to get from source to destination

For example:

---------------------------------------------------------------------
Select an option: 3

Please enter source route (ie; A, B, C, D, E)

A

Please enter destination route (ie; A, B, C, D, E)

C

The shortest distance between A and C is 9

---------------------------------------------------------------------
Select an option: 3

Please enter source route (ie; A, B, C, D, E)

B

Please enter destination route (ie; A, B, C, D, E)

B

The shortest distance between B and B is 9

---------------------------------------------------------------------

4) Runs the test sample data provided but could be changed to accomodate for different routes/paths/distances

Select an option: 4

9 
22 
NO SUCH ROUTE 
2 
3 
9 
9 
7 

---------------------------------------------------------------------

5) Exits the program

---------------------------------------------------------------------


Built With
Visual Studio Community 2019 - .NET Core 3.1

Versioning

1.0.0

Author

Franco Zavagnini - Solution 1 - Bridge Academy

License
This project is licensed under the MIT License - see the LICENSE.md file for details

