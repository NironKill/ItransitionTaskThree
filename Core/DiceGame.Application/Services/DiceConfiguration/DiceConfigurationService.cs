using DiceGame.Domain;

namespace DiceGame.Application.Services.DiceConfiguration
{
    public class DiceConfigurationService : IDiceConfigurationService
    {     
        public List<Dice> Parse(string[] args)
        {
            List<Dice> diceList = new List<Dice>();

            foreach (string arg in args)
            {
                try
                {
                    int[] faces = arg.Split(',').Select(int.Parse).ToArray();

                    if (faces.Length != 6)
                    {
                        Console.WriteLine("Each dice configuration should contain 6 comma-separated integers (e.g., 1,2,3,4,5,6)");
                        Environment.Exit(1);
                    }

                    diceList.Add(new Dice(faces));
                }
                catch (Exception)
                {
                    Console.WriteLine("Each dice configuration should contain 6 comma-separated integers (e.g., 1,2,3,4,5,6)");
                    Environment.Exit(1);
                }
            }
            return diceList;
        }
    }
}
