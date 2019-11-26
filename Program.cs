using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LiftManagement
{
    class Program
    {
        private static readonly int floorCount = 10;
        private static int currentfloor = 0;
        private static char direction = 'U';//U for upwards and D for downwards
        private static List<int> uplift;
        private static List<int> downlift;

        public static int FloorCount => floorCount;

        public static int Currentfloor { get => currentfloor; set => currentfloor = value; }
        public static char Direction { get => direction; set => direction = value; }
        public static List<int> Uplift { get => uplift; set => uplift = value; }
        public static List<int> Downlift { get => downlift; set => downlift = value; }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Lift!Lift is at Ground floor");
            Uplift = new List<int>();
            Downlift = new List<int>();
            LiftFunction();

        }
        public static void StartandStop(int inputFloor)
        {
            bool stopped = false;
            Thread t = new Thread(new ParameterizedThreadStart((x) =>
            {
                int counter = Currentfloor;
                inputFloor = Convert.ToInt32(x);
                while (!stopped)
                {
                    if (counter == inputFloor)
                    {
                        Console.WriteLine($"Lift reached at {counter}");
                        ReachedDestination(counter);
                        stopped = true;
                        break;
                    }
                    else if (counter < inputFloor)
                    {

                        counter++;
                        Console.WriteLine($"Lift is at floor {counter}.Enter q to stop the lift");
                        Currentfloor = counter;
                        Thread.Sleep(3000);
                    }
                    else if (counter > inputFloor)
                    {
                        Thread.Sleep(3000);
                        counter--;
                        Console.WriteLine($"Lift is at floor {counter}.Enter q to stop the lift");
                        Currentfloor = counter;
                    }

                }
                while (stopped)
                {
                    LiftFunction();
                }
            }));

            t.Start(inputFloor);
            if (inputFloor > Currentfloor)
            {
                Direction = 'U';
                Console.WriteLine("Lift is going UP.Press q to stop the lift.");
            }
            else
            {
                Direction = 'D';
                Console.WriteLine("Lift is going DOWN.Press q to stop the lift.");
            }
            char c = Console.ReadKey().KeyChar;
            if (c == 'q' || c == 'Q')
            {
                stopped = true;
                Console.WriteLine("");
                Console.WriteLine($"You are at floor {Currentfloor}");
            }

            t.Join();
        }

        public static void LiftFunction()
        {
            Console.WriteLine("Enter destination floor or press s to start the list");
            string inputFloor = Console.ReadLine();
            int destFloor = 0;
            bool Invalidfloor = false;
            if (Int32.TryParse(inputFloor, out destFloor))
            {
                if (destFloor > FloorCount || Currentfloor == destFloor)
                {
                    Invalidfloor = true;
                }
                else
                {
                    if (destFloor > Currentfloor) {
                        Direction = 'U';
                        Uplift.Add(destFloor);
                    }
                    else 
                    {
                        if (Direction == 'U' && Uplift.Count == 0) { 
                            Direction = 'D';
                        }
                        
                        Downlift.Add(destFloor);
                    }

                    NextDestination();
                }
            }
            else if (inputFloor == "s" || inputFloor == "S")
            {
                NextDestination();
            }
            else { Invalidfloor = true; }
            if (Invalidfloor)
            {
                Console.WriteLine("Invalid Input");
            }
        }

        public static void NextDestination()
        {
            if (Direction == 'U' && Uplift.Count>0)
            {
                Uplift.Sort();
                foreach (int dest in Uplift)
                {
                    if (dest > Currentfloor)
                    {
                        StartandStop(dest);
                    }
                }
            }
            else
            {
                Direction = 'D';
                Downlift.OrderByDescending(x=>x);
                foreach (int dest in  Downlift)
                {
                    if (dest < Currentfloor)
                    {
                        StartandStop(dest);
                    }
                }
            }

        }

        public static void ReachedDestination(int destination)
        {
            if (Direction == 'U')
            {
                if (Uplift.Count > 0)
                {
                    Uplift.Remove(destination);
                }
            }
            else {
                if (Downlift.Count > 0)
                {
                    Downlift.Remove(destination);
                }
            }

        }
    }


}
