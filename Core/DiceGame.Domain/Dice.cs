namespace DiceGame.Domain
{
    public class Dice
    {
        public int[] Faces { get; set; }

        public Dice(int[] faces) => Faces = faces;

        public override string ToString()
        {
            return string.Join(",", Faces);
        }
    }
}
