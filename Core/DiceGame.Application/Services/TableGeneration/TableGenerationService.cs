using ConsoleTables;
using DiceGame.Application.Math;
using DiceGame.Domain;
using System.Collections.Generic;
using System.Threading.Channels;

namespace DiceGame.Application.Services.TableGeneration
{
    public class TableGenerationService : ITableGenerationService
    {
        public void Result(string message)
        {
            Console.WriteLine($"{message}");
        }
        public void DiceOption(List<Dice> dice)
        {
            Console.WriteLine("=== Choose Your Dice ===");

            ConsoleTable table = CreateTabl1e("Option", "Dice Configuration");

            for (int i = 0; i < dice.Count; i++)
            {
                table.AddRow($"{i}", $"{dice[i]}");
            }
            table.Write(Format.Alternative);
        }
        public void HelpMenu(List<Dice> dice)
        {
            Console.WriteLine("=== Winning chance of each dice ===");

            ConsoleTable table = CreateTabl1e("User Dice");
            table.AddColumn(dice.Select(x => x.ToString()));

            for (int i = 0; i < dice.Count; i++)
            {
                List<string> row = new List<string> { dice[i].ToString() };

                for (int j = 0; j < dice.Count; j++)
                {   
                    double probability = ProbabilityCalculation.CalculateWinProbability(dice[i], dice[j]);
                    row.Add(probability.ToString("P2"));
                }
                table.AddRow(row.ToArray());
            }
            table.Write(Format.Alternative);
        }
        public void FirstMoveMenu()
        {
            Console.WriteLine("=== First Move Menu ===");

            var table = CreateTabl1e("Command", "Description");
            table.AddRow("0", "0");
            table.AddRow("1", "1");
            table.AddRow("X", "Exit");
            table.AddRow("?", "Help");

            table.Write(Format.Alternative);
        }

        private ConsoleTable CreateTabl1e(params string[] columnHeaders)
        {
            ConsoleTable table = new ConsoleTable(columnHeaders).Configure(o => o.NumberAlignment = Alignment.Left);

            return table;
        }
    }
}
