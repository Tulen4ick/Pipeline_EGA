using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pipeline_EGA
{
    internal class Generation
    {
        public List<int[]> generation;
        public int CountOfGenes;
        public int CountOfRepresentatives;
        public int[] Penalty;
        public int[] Deadlines;
        public int[][] matrix;
        int NumberOfParticipants = 10;
        public Generation(int CountOfGenes, int CountOfRepresentatives, int[] penalty, int[] deadlines, int[][] matrix)
        {
            Random rnd = new Random();
            generation = new List<int[]>();
            this.CountOfGenes = CountOfGenes;
            this.CountOfRepresentatives = CountOfRepresentatives;
            for (int i = 0; i < CountOfRepresentatives; i++)
            {
                int[] ProbablePermutation = PermutationGeneration(rnd);
                if (this.generation.Contains(ProbablePermutation))
                {
                    while (generation.Contains(ProbablePermutation))
                    {
                        ProbablePermutation = PermutationGeneration(rnd);
                    }
                }
                generation.Add(ProbablePermutation);
            }

            Penalty = penalty;
            Deadlines = deadlines;
            this.matrix = matrix;
        }

        public int[] PermutationGeneration(Random rnd)
        {
            
            int[] permutation = new int[CountOfGenes];
            List<int> PoolOfRemainingNumbers = new List<int>();
            for (int i = 0; i < CountOfGenes; i++)
            {
                permutation[i] = 0;
                PoolOfRemainingNumbers.Add(i + 1);
            }
            for (int i = 0; i < CountOfGenes; i++)
            {
                int ProbableNumber = rnd.Next(0, PoolOfRemainingNumbers.Count);
                permutation[i] = PoolOfRemainingNumbers[ProbableNumber];
                PoolOfRemainingNumbers.RemoveAt(ProbableNumber);
            }
            return permutation;
        }
        public List<int[]> Reproduction()
        {
            Random rnd = new Random();
            List<int[]> newGeneration = new List<int[]>();
            List<int[]> CurrentGenerationCopy = generation.ToList();
            for (int i = 0; i < CountOfRepresentatives / 2; ++i)
            {
                int FirstParentIndice = rnd.Next(0, CurrentGenerationCopy.Count);
                int[] FirstParent = CurrentGenerationCopy[FirstParentIndice];
                CurrentGenerationCopy.RemoveAt(FirstParentIndice);
                int SecondParentIndice = rnd.Next(0, CurrentGenerationCopy.Count);
                int[] SecondParent = CurrentGenerationCopy[SecondParentIndice];
                CurrentGenerationCopy.RemoveAt(SecondParentIndice);
                int[] FirstDescendant = new int[CountOfGenes];
                int[] SecondDescendant = new int[CountOfGenes];
                for (int j = 0; j < CountOfGenes; ++j)
                {
                    FirstDescendant[j] = 0;
                    SecondDescendant[j] = 0;
                }
                int FirstSplitIndice = CountOfGenes / 3;
                int SecondSplitIndice = CountOfGenes - CountOfGenes / 3;
                Array.Copy(FirstParent, FirstSplitIndice, FirstDescendant, FirstSplitIndice, SecondSplitIndice - FirstSplitIndice);
                Array.Copy(SecondParent, FirstSplitIndice, SecondDescendant, FirstSplitIndice, SecondSplitIndice - FirstSplitIndice);
                int[] FirstParentCopy = FirstParent.ToArray();
                int[] SecondParentCopy = SecondParent.ToArray();
                for (int j = 0; j < CountOfGenes; ++j)
                {
                    if (FirstDescendant.Contains(SecondParentCopy[j]))
                    {
                        SecondParentCopy[j] = 0;
                    }
                    if (SecondDescendant.Contains(FirstParentCopy[j]))
                    {
                        FirstParentCopy[j] = 0;
                    }
                }
                int FirstDescendantStep = 1;
                int SecondDescendantStep = 1;
                for (int j = 0; j < CountOfGenes; ++j)
                {
                    if (FirstDescendant[j] == 0)
                    {
                        int ProbableNumber = SecondParentCopy[((SecondSplitIndice + FirstDescendantStep) % (CountOfGenes))];
                        while (ProbableNumber == 0)
                        {
                            FirstDescendantStep++;
                            ProbableNumber = SecondParentCopy[((SecondSplitIndice + FirstDescendantStep) % (CountOfGenes))];
                        }
                        FirstDescendant[j] = ProbableNumber;
                        FirstDescendantStep++;
                    }
                    if (SecondDescendant[j] == 0)
                    {
                        int ProbableNumber = FirstParentCopy[((SecondSplitIndice + SecondDescendantStep) % (CountOfGenes))];
                        while (ProbableNumber == 0)
                        {
                            SecondDescendantStep++;
                            ProbableNumber = FirstParentCopy[((SecondSplitIndice + SecondDescendantStep) % (CountOfGenes))];
                        }
                        SecondDescendant[j] = ProbableNumber;
                        SecondDescendantStep++;
                    }
                }
                newGeneration.Add(FirstDescendant);
                newGeneration.Add(SecondDescendant);
            }
            return newGeneration;
        }

        public List<int[]> Mutation(List<int[]> newGeneration)
        {
            Random rnd = new Random();
            List<int[]> MutatedGeneraion = new List<int[]>();
            List<int[]> newGenerationCopy = newGeneration.ToList();
            for (int i = 0; i < newGenerationCopy.Count; ++i)
            {
                int MetatedOrNot = rnd.Next(1, 101); // с вероятностью 1 процент произойдет мутация
                if (MetatedOrNot == 1)
                {
                    int MutatedGene1 = rnd.Next(0, newGenerationCopy[i].Length);
                    int MutatedGene2 = rnd.Next(0, newGenerationCopy[i].Length);
                    while (MutatedGene1 == MutatedGene2)
                    {
                        MutatedGene1 = rnd.Next(0, newGenerationCopy[i].Length);
                        MutatedGene2 = rnd.Next(0, newGenerationCopy[i].Length);
                    }
                    int temp = newGenerationCopy[i][MutatedGene1];
                    newGenerationCopy[i][MutatedGene1] = newGenerationCopy[i][MutatedGene2];
                    newGenerationCopy[i][MutatedGene2] = temp;
                    MutatedGeneraion.Add(newGenerationCopy[i]);
                }
            }
            return MutatedGeneraion;
        }

        public List<int[]> FormationOfNewGenerationTournament(List<int[]> newGeneration, List<int[]> mutatedGeneration)
        {
            Random rnd = new Random();
            List<int[]> AllDescendants = newGeneration.Concat(mutatedGeneration).ToList();
            List<int[]> BestDescendants = new List<int[]>();
            IterationCrreation iterationCrreation = new IterationCrreation();
            for (int i = 0; i < CountOfRepresentatives / 2; ++i)
            {
                List<int[]> TournamentParticipants = new List<int[]>();
                for (int j = 0; j < AllDescendants.Count; ++j)
                {
                    int SelectedOrNot = rnd.Next(1, CountOfRepresentatives / 2 + 1);
                    if (SelectedOrNot == 1)
                    {
                        TournamentParticipants.Add(AllDescendants[j]);
                    }
                    if (TournamentParticipants.Count >= NumberOfParticipants)
                    {
                        break;
                    }
                }
                if(TournamentParticipants.Count > 0)
                {
                    int minFine = iterationCrreation.FineCalculation(Penalty, Deadlines, matrix, TournamentParticipants[0]);
                    int[] minPermutation = TournamentParticipants[0];
                    for (int j = 0; j < TournamentParticipants.Count; ++j)
                    {
                        int ExpectedMinimum = iterationCrreation.FineCalculation(Penalty, Deadlines, matrix, TournamentParticipants[j]);
                        if (ExpectedMinimum < minFine)
                        {
                            minFine = ExpectedMinimum;
                            minPermutation = TournamentParticipants[j];
                        }
                    }
                    BestDescendants.Add(minPermutation);
                }
            } //выборка из потомков
            while(BestDescendants.Count < CountOfRepresentatives)
            {
                for (int i = 0; i < CountOfRepresentatives; ++i)
                {
                    int SelectedOrNot = rnd.Next(0, 2);
                    if ((SelectedOrNot == 0) && (BestDescendants.Count < CountOfRepresentatives) && (!BestDescendants.Contains(generation[i])))
                    {
                        BestDescendants.Add(generation[i]);
                    }
                    /*if (BestDescendants.Count < CountOfRepresentatives)
                    {
                        BestDescendants.Add(generation[i]);
                    }*/
                }
            }//выборка из родителей
            return BestDescendants;
        }

        public List<int[]> FormationOfNewGenerationSimple(List<int[]> newGeneration, List<int[]> mutatedGeneration)
        {
            Random rnd = new Random();
            List<int[]> AllDescendants = newGeneration.Concat(mutatedGeneration).ToList();
            List<int[]> BestDescendants = new List<int[]>();
            while (BestDescendants.Count < CountOfRepresentatives/2)
            {
                for (int i = 0; i < AllDescendants.Count; ++i)
                {
                    int SelectedOrNot = rnd.Next(0, 2);
                    if ((SelectedOrNot == 0) && (BestDescendants.Count < CountOfRepresentatives/2) && (!BestDescendants.Contains(AllDescendants[i])))
                    {
                        BestDescendants.Add(AllDescendants[i]);
                    }
                }
            }
            while (BestDescendants.Count < CountOfRepresentatives)
            {
                for (int i = 0; i < CountOfRepresentatives; ++i)
                {
                    int SelectedOrNot = rnd.Next(0, 2);
                    if ((SelectedOrNot == 0) && (BestDescendants.Count < CountOfRepresentatives) && (!BestDescendants.Contains(generation[i])))
                    {
                        BestDescendants.Add(generation[i]);
                    }
                }
            }
            return BestDescendants;
        }

        public int[] EGA(int MaximumNumberOfGenerations)
        {
            int currentMinFin = int.MaxValue;
            int minFine = int.MaxValue;
            int CountOfEqualFines = 0;
            int[] minIndividual = new int[CountOfGenes];
            for (int i = 0; i < MaximumNumberOfGenerations; ++i)
            {
                minFine = int.MaxValue;
                List<int[]> newGeneration = Reproduction();
                List<int[]> mutetedGeneration = Mutation(newGeneration);
                /*this.generation = FormationOfNewGenerationTournament(newGeneration, mutetedGeneration);*/
                this.generation = FormationOfNewGenerationSimple(newGeneration, mutetedGeneration);
                for (int j = 0; j < generation.Count; ++j)
                {
                    IterationCrreation iterationCrreation = new IterationCrreation();
                    int ExpectedMinimum = iterationCrreation.FineCalculation(Penalty, Deadlines, matrix, generation[j]);
                    if (ExpectedMinimum < minFine)
                    {
                        minFine = ExpectedMinimum;
                        minIndividual = generation[j];
                    }
                }
                if(minFine == currentMinFin)
                {
                    CountOfEqualFines++;
                    if(CountOfEqualFines == 2)
                    {
                        break;
                    }
                }
                else
                {
                    CountOfEqualFines = 0;
                }
                currentMinFin = minFine;
            }
            return minIndividual;
        }
    }
}
