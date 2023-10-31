namespace Catalyster.Helpers
{
    // Wiggles numbers in important ways.
    public class WiggleHelper
    {
        // Wiggle within +/- p, where p=1 is +/- 100%.
        public static int Wiggle(int x, double p)
        {
            var rand = new Random();
            return rand.Next(Convert.ToInt32(x * (1 - p)), Convert.ToInt32(x * (1 - p)));
        }
    }
}
