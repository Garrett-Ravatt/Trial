using Catalyster.Components;
using Catalyster.Hunks;

namespace Catalyster.Helpers
{
    public static class DetonationHelper
    {
        // Makes a base value explosive to test others
        public static Explosive Fuse()
        {
            // TODO: Make Fuse using the intended length
            var fuse = new Explosive
            {
                Resistance = new IntHunk(new int[] { 0, 0 }),
                Potential = new IntHunk(new int[] { 0, 0 }),
            };

            return fuse;
        }

        // TODO: refactor as an extension to the Explosive struct
        // Tests if an active explosive can detonate another
        public static bool Detonates(Explosive active, Explosive resistant)
        {
            return active.Potential.AnyGreaterOrEqual(resistant.Resistance);
        }

        public static bool[] Detonate(Explosive[] explosives)
        {
            var detonations = new bool[explosives.Count()];

            // TODO: fix to intended hunk length
            var basePotential = new IntHunk(new int[] { 0, 0 });
            bool didSomething;
            do
            {
                didSomething = false;
                for (int i = 0; i < explosives.Count(); i++)
                {
                    if (detonations[i] == false && basePotential.AnyGreaterOrEqual(explosives[i].Resistance))
                    {
                        basePotential = IntHunk.Max(basePotential, explosives[i].Potential);
                        // TODO: Add up totals
                        detonations[i] = true;
                        didSomething = true;
                    }
                }
            }
            while (didSomething);

            return detonations;
        }
    }
}
