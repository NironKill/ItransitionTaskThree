namespace DiceGame.Application.Services.NumberGeneration
{
    public interface INumberGenerationService
    {
        (string HMAC, byte[] Key, int Number) GenerateRandom(int range);
        int Roll(int range);
    }
}
