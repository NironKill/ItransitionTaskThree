using DiceGame.Application.Common;
using DiceGame.Application.Services.DiceConfiguration;
using DiceGame.Application.Services.NumberGeneration;
using DiceGame.Application.Services.TableGeneration;
using Microsoft.Extensions.DependencyInjection;

ServiceProvider serviceProvider = new ServiceCollection()
    .AddSingleton<IDiceConfigurationService, DiceConfigurationService>()
    .AddSingleton<INumberGenerationService, NumberGenerationService>()
    .AddSingleton<ITableGenerationService, TableGenerationService>()
    .AddSingleton<ConsoleApplication>()
    .BuildServiceProvider();

if (args.Length < 3)
{
    Console.WriteLine("Error: At least three dice configurations are required.");
    Console.WriteLine("Example: 1,2,3,4,5,6 2,3,4,5,6,7 3,4,5,6,7,8");
    return;
}

try
{
    using (IServiceScope scope = serviceProvider.CreateScope())
    {
        ConsoleApplication app = scope.ServiceProvider.GetRequiredService<ConsoleApplication>();
        while (true)
        {
            app.Run(args);
            Console.WriteLine("Do you want to play another round? (Y/N): ");
            string input = Console.ReadLine()?.Trim().ToUpper() ?? "N";
            if (input.ToLower() == "n" || input.ToLower() == "no")
            {
                Console.WriteLine("Thank you for playing! Goodbye!");
                break;
            }
            else if (input.ToLower() != "y" || input.ToLower() == "yes")
            {
                Console.WriteLine("Invalid input. Exiting the game.");
                break;
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
