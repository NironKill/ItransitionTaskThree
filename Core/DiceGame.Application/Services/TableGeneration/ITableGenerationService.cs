using DiceGame.Domain;

namespace DiceGame.Application.Services.TableGeneration
{
    public interface ITableGenerationService
    {
        void Result(string message);
        void DiceOption(List<Dice> dice);
        void HelpMenu(List<Dice> dice);
        void FirstMoveMenu();
    }
}
