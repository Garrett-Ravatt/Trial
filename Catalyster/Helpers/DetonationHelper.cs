using Catalyster.Components;
using Catalyster.Hunks;
using RogueSharp.DiceNotation;

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
            var basePotential = new IntHunk(new int[2]);
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

        public static bool[] Detonate(List<Explosive> explosives)
        {
            return Detonate(explosives.ToArray());
        }

        public static List<Explosive> Detonated(List<Explosive> explosives)
        {
            var bools = Detonate(explosives);
            var detonated = new List<Explosive>();

            for(int i = 0; i < explosives.Count(); i++)
                if (bools[i])
                    detonated.Add(explosives[i]);
            
            return detonated;
        }

        // returns the damage for an explosion
        public static DiceExpression DamageDice(List<Explosive> explosives)
        {
            explosives = Detonated(explosives);
            if (explosives.Count() == 0) // skip out if it's a dud.
                return Dice.Parse("0");

            // TODO: fix to intended hunk length
            var sumHunk = new IntHunk(new int[2]);
            foreach(var explosive in explosives)
            {
                sumHunk = sumHunk.Add(explosive.Potential);
            }

            // dominant index
            var i = 0;

            // dominant sum
            var dSum = sumHunk.Array[i];

            // max entry of dominant
            var dMax = 0;
            foreach (Explosive explosive in explosives)
                if (explosive.Potential.Array[i] > dMax)
                    dMax = explosive.Potential.Array[i];

            // sum total potential
            var tSum = 0;
            foreach(int x in sumHunk.Array)
                tSum += x;

            // TODO: make the die sides correspond to a more grounded dice table (prevent a d14, etc).
            return new DiceExpression().Dice(dSum, dMax * 2).Constant(tSum);
        }
    }
}
