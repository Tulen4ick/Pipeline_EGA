using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pipeline_EGA
{
    internal class IterationCrreation
    {
        public List<Tuple<int, int, int, int>> distributtion; //номер прибора, номер заявки, время начала, время конца

        public IterationCrreation()
        {
            distributtion = new List<Tuple<int, int, int, int>>();
        }

        public int StartTime(List<Tuple<int, int, int, int>> distribution, int[][] matrix, int DeviceNumber, int NumberOfIteration, int[] permuntations)
        {
            int time = 0;
            if (NumberOfIteration == 0)
            {
                if (DeviceNumber != 0)
                {
                    var CurrentCell2 = from c in distribution where c.Item1 == DeviceNumber - 1 select c;
                    foreach (Tuple<int, int, int, int> cell in CurrentCell2)
                    {
                        if (cell.Item2 == permuntations[0] - 1)
                        {
                            time = cell.Item4;
                        }
                    }
                }
            }
            else
            {
                if (DeviceNumber == 0)
                {
                    var CurrentCell = from c in distribution where c.Item1 == DeviceNumber select c;
                    foreach (Tuple<int, int, int, int> cell in CurrentCell)
                    {
                        if (cell.Item2 == permuntations[NumberOfIteration - 1] - 1)
                        {
                            time = cell.Item4;
                        }
                    }
                }
                else
                {
                    var CurrentCell = from c in distribution where c.Item1 == DeviceNumber select c;
                    var CurrentCell2 = from c in distribution where c.Item1 == DeviceNumber - 1 select c;
                    int time1 = 0;
                    int time2 = 0;
                    foreach (Tuple<int, int, int, int> cell in CurrentCell)
                    {
                        if (cell.Item2 == permuntations[NumberOfIteration - 1] - 1)
                        {
                            time1 = cell.Item4;
                        }
                    }
                    foreach (Tuple<int, int, int, int> cell in CurrentCell2)
                    {
                        if (cell.Item2 == permuntations[NumberOfIteration] - 1)
                        {
                            time2 = cell.Item4;
                        }
                    }
                    time = Math.Max(time1, time2);
                }

            }
            return time;

        }

        public int FineCalculation(int[] Penalty, int[] Deadlines, int[][] matrix, int[] permutation)
        {

            for (int i = 0; i < matrix.Length; ++i) //5
            {
                for (int j = 0; j < matrix[i].Length; j++) //9
                {
                    int Start = StartTime(distributtion, matrix, i, j, permutation);
                    int End = StartTime(distributtion, matrix, i, j, permutation) + matrix[i][permutation[j] - 1];
                    distributtion.Add(new Tuple<int, int, int, int>(i, permutation[j] - 1, Start, End));
                }
            }
            int Fine = 0;
            var CurrentCell = from c in distributtion where c.Item1 == (matrix.Length - 1) select c;
            foreach (Tuple<int, int, int, int> cell in CurrentCell)
            {
                Fine += Penalty[cell.Item2] * (Math.Max(cell.Item4 - Deadlines[cell.Item2], 0));
            }
            return Fine;
        }
    }
}
