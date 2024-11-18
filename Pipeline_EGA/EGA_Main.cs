using System;
using System.IO;
using System.Linq;

namespace Pipeline_EGA
{
    internal class EGA_Main
    {
        static void ReadingTheInitialData(string[] s, int[][] matrix, int[] PenaltyCoefficients, int[] Deadlines, string[] FirstString)
        {
            for (int i = 0; i < int.Parse(FirstString[1]); ++i)
            {
                string[] CurrentString = s[i + 1].Trim().Split(' ');
                matrix[i] = new int[CurrentString.Length];
                for (int j = 0; j < int.Parse(FirstString[0]); ++j)
                {
                    matrix[i][j] = int.Parse(CurrentString[j].Trim());
                }
            }

            string[] PenaltyString = s[int.Parse(FirstString[1]) + 2].Trim().Split(' ');
            for (int i = 0; i < int.Parse(FirstString[0]); ++i)
            {
                PenaltyCoefficients[i] = int.Parse(PenaltyString[i].Trim());
            }

            string[] DeadlineString = s[int.Parse(FirstString[1]) + 4].Trim().Split(' ');
            for (int i = 0; i < int.Parse(FirstString[0]); ++i)
            {
                Deadlines[i] = int.Parse(DeadlineString[i].Trim());
            }
        }


        static void Main(string[] args)
        {
            string path = @"C:\Users\zahar\source\repos\Pipeline_EGA\Data for the pipeline.txt";
            string[] s = File.ReadAllLines(path);
            string[] FirstString = s[0].Trim().Split(' ');
            int[][] matrix = new int[int.Parse(FirstString[1])][];
            int[] PenaltyCoefficients = new int[int.Parse(FirstString[0])];
            int[] Deadlines = new int[int.Parse(FirstString[0])];
            ReadingTheInitialData(s, matrix, PenaltyCoefficients, Deadlines, FirstString);
            int CountOfRepresentatives = 5000;
            int MaximumNumberOfGenerations = 20;
            int NumberOfLaunches = 10;
            int minFine = int.MaxValue;
            int[] minOfMinIndividual = new int[int.Parse(FirstString[0])];
            /*Console.WriteLine("Введите количество особей в поколении: ");
            CountOfRepresentatives = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите максимальное количество поколений: ");
            MaximumNumberOfGenerations = int.Parse(Console.ReadLine());*/
            for (int i = 0; i < NumberOfLaunches; ++i)
            {
                IterationCrreation iterationCrreation = new IterationCrreation();
                Generation gen = new Generation(int.Parse(FirstString[0]), CountOfRepresentatives, PenaltyCoefficients, Deadlines, matrix);
                int[] minIndividual = gen.EGA(MaximumNumberOfGenerations);
                for (int j = 0; j < minIndividual.Count(); ++j)
                {
                    Console.Write(minIndividual[j] + " ");
                }
                Console.WriteLine();
                int Fine = iterationCrreation.FineCalculation(PenaltyCoefficients, Deadlines, matrix, minIndividual);
                Console.WriteLine($"Минимальный штраф: {Fine}");
                if (Fine < minFine)
                {
                    minFine = Fine;
                    minOfMinIndividual = minIndividual;
                }
            }
            Console.WriteLine("---------------------------------------");
            for (int j = 0; j < minOfMinIndividual.Count(); ++j)
            {
                Console.Write(minOfMinIndividual[j] + " ");
            }
            Console.WriteLine();
            Console.WriteLine($"Минимальный штраф: {minFine}");
            Console.ReadKey();
        }
    }
}
