using DiceGame.Application.Services.DiceConfiguration;
using DiceGame.Application.Services.NumberGeneration;
using DiceGame.Application.Services.TableGeneration;
using DiceGame.Domain;

namespace DiceGame.Application.Common
{
    public class ConsoleApplication
    {
        private readonly IDiceConfigurationService _configuration;
        private readonly INumberGenerationService _numberGeneration;
        private readonly ITableGenerationService _tableGeneration;

        private Dice _userDice;
        private Dice _computerDice;

        public ConsoleApplication(IDiceConfigurationService configuration, INumberGenerationService numberGeneration, ITableGenerationService tableGeneration)
        {
            _configuration = configuration;
            _numberGeneration = numberGeneration;
            _tableGeneration = tableGeneration;
        }

        public void Run(string[] args)
        {
            List<Dice> dice = _configuration.Parse(args);
            bool isUserFirst = DetermineFirstMove(dice);
            int userResult = -1;
            int computerResult = -1;

            Console.WriteLine("Welcome to the Non-Transitive Dice Game!");

            _tableGeneration.HelpMenu(dice);
            SelectDice(isUserFirst, dice);
            if (isUserFirst)
            {
                userResult = UserTurn();
                computerResult = ComputerTurn(dice);
            }
            else
            {
                computerResult = ComputerTurn(dice);
                userResult = UserTurn();
            }
            CompareRolls(userResult, computerResult);
            Console.WriteLine("Game over! Thank you for playing.");
        }
        private bool DetermineFirstMove(List<Dice> dice)
        {
            Console.WriteLine("Let's determine who goes first.");
            var (hmac, key, value) = _numberGeneration.GenerateRandom(2);
            Console.WriteLine($"HMAC: === {hmac} ===");

            _tableGeneration.FirstMoveMenu();
            string userInput;
            int userGuess = -1;
            do
            {
                userInput = Console.ReadLine()!.Trim().ToUpper();
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
                else if (userInput == "X")
                {
                    Console.WriteLine("Exit the game.");
                    Environment.Exit(0);
                }
                else if (userInput == "?")
                {
                    _tableGeneration.HelpMenu(dice);
                    continue;
                }
                if (!int.TryParse(userInput, out userGuess) || (userGuess != 0 && userGuess != 1))
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            } while (userGuess != 0 && userGuess != 1);

            Console.WriteLine($"Actual Value: {value}, Key: {Convert.ToHexString(key)}");
            bool userChoosesFirst = userGuess == value;

            Console.WriteLine(userChoosesFirst
                ? "You make the first move."
                : "The computer makes the first move.");
            return userChoosesFirst;
        }
        private void SelectDice(bool userChoosesFirst, List<Dice> dice)
        {
            string userInput = string.Empty;
            int userChoice = default;

            if (userChoosesFirst)
            {
                Console.WriteLine("Choose your dice from the following options:");
                _tableGeneration.DiceOption(dice);
                Console.WriteLine("Enter the number corresponding to your choice or \"?\" to get Winning chance for each die:");
                while (true)
                {
                    userInput = Console.ReadLine()?.Trim().ToUpper() ?? "";
                    if (userInput == "?")
                    {
                        _tableGeneration.HelpMenu(dice);
                        Console.WriteLine("Enter the number corresponding to your choice or \"?\" to get Winning chance for each die:");
                    }
                    else if (int.TryParse(userInput, out userChoice) && userChoice >= 0 && userChoice < dice.Count)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please try again.");
                    }
                }

                _userDice = dice[userChoice];
                dice.RemoveAt(userChoice);
                int computerChoice = _numberGeneration.Roll(dice.Count);
                _computerDice = dice[computerChoice];
            }
            else
            {
                int computerChoice = _numberGeneration.Roll(dice.Count);
                _computerDice = dice[computerChoice];
                Console.WriteLine("Computer's dice:" + _computerDice);
                dice.RemoveAt(computerChoice);

                Console.WriteLine("Choose your dice from the following options:");
                _tableGeneration.DiceOption(dice);
                Console.WriteLine("Enter the number corresponding to your choice or \"?\" to get Winning chance for each die:");

                while (true)
                {
                    userInput = Console.ReadLine()?.Trim().ToUpper() ?? "";

                    if (userInput == "?")
                    {
                        _tableGeneration.HelpMenu(dice);
                        Console.WriteLine("Enter the number corresponding to your choice or \"?\" to get Winning chance for each die:");
                    }
                    else if (int.TryParse(userInput, out userChoice) && userChoice >= 0 && userChoice < dice.Count)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please try again.");
                    }
                }
                _userDice = dice[userChoice];
            }
            Console.WriteLine($"Your dice: {_userDice}");
            Console.WriteLine($"Computer's dice: {_computerDice}");
        }
        private int UserTurn()
        {
            var (hmac, key, value) = _numberGeneration.GenerateRandom(6);
            Console.WriteLine("I choose a random number between 0-5");
            Console.WriteLine($"HMAC: === {hmac} ===");

            Console.WriteLine("It's your turn. Roll the dice!");
            int userThrow = GetPlayerThrow();
            int userResult = _userDice.Faces[(userThrow + value) % 6];
            Console.WriteLine($"Computer's random number: {value}, Key: {Convert.ToHexString(key)}");
            Console.WriteLine($"Your roll: {userResult} \n(({value} + {userThrow})%6)");

            return userResult;
        }
        private int ComputerTurn(List<Dice> dice)
        {
            Console.WriteLine("It's the computer's turn.");
            var (hmac, key, value) = _numberGeneration.GenerateRandom(6);
            Console.WriteLine($"HMAC: === {hmac} ===");
            Console.WriteLine("Add your number modulo 6 (0-5):");
            string userInput;
            int userValue;
            while (true)
            {
                userInput = Console.ReadLine()!.Trim().ToUpper();
                if (userInput == "X")
                {
                    Console.WriteLine("Exit the game.");
                    Environment.Exit(0);
                }
                else if (userInput == "?")
                {
                    _tableGeneration.HelpMenu(dice);
                    continue;
                }
                if (int.TryParse(userInput, out userValue) && userValue >= 0 && userValue < 6)
                {
                    break;
                }
                Console.WriteLine("Invalid choice. Please try again.");
            }

            int computerResult = _computerDice.Faces[(value + userValue) % 6];
            Console.WriteLine($"My random selection value between 0-5: {value}, Key: {Convert.ToHexString(key)}");
            Console.WriteLine($"Computer's roll: {computerResult} \n(({value} + {userValue})%6)");

            return computerResult;
        }
        private int GetPlayerThrow()
        {
            Console.WriteLine("Choose a number between 0 and 5 for your dice throw:");
            int choice;

            while (!int.TryParse(Console.ReadLine()?.Trim(), out choice) || choice < 0 || choice >= 6)
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
            return choice;
        }
        private void CompareRolls(int userResult, int computerResult)
        {
            Console.WriteLine("Your roll:" + userResult);
            Console.WriteLine("Computer's roll:" + computerResult);

            if (userResult > computerResult)
                Console.WriteLine("Congratulations! You win this round.");         
            else if (userResult < computerResult)
                Console.WriteLine("The computer wins this round.");      
            else
                Console.WriteLine("This round is a tie!"); 
        }
    }
}
