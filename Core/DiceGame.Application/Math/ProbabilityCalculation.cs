using DiceGame.Domain;

namespace DiceGame.Application.Math
{
    public static class ProbabilityCalculation
    {
        public static double CalculateWinProbability(Dice diceOne, Dice diceTwo)
        {
            int totalRounds = 0;
            int diceWins = 0;

            foreach (var facesOne in diceOne.Faces)
            {
                foreach (var facesTwo in diceTwo.Faces)
                {
                    if (facesOne > facesTwo) diceWins++;
                    totalRounds++;
                }
            }

            return (double)diceWins / totalRounds;
        }
    }
}
