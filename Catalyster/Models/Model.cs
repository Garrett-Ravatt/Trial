using Catalyster.Interfaces;

namespace Catalyster.Models
{
    public class Model<T>
    {
        public List<IStep<T>> Steps;
        private int? _seed;

        public Model()
        {
            Steps = new List<IStep<T>>();
        }

        public Model<T> Step(IStep<T> step)
        {
            Steps.Add(step);
            return this;
        }

        public Model<T> Seed(int seed)
        {
            _seed = seed;
            return this;
        }

        public T Process(T subject)
        {
            int seed;
            if (!_seed.HasValue)
                seed = (int) DateTime.UtcNow.Ticks;
            else
                seed = _seed.Value;

            foreach (var step in Steps)
            {
                subject = step.Step(subject, seed);
            }
            return subject;
        }
    }
}
