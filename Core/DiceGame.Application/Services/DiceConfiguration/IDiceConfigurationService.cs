using DiceGame.Domain;

namespace DiceGame.Application.Services.DiceConfiguration
{
    public interface IDiceConfigurationService
    {
        List<Dice> Parse(string[] args);
    }
}
