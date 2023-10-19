using Catalyster.Interfaces;

namespace Catalyster.Models
{
    public class Model<T>
    {
        public List<IStep<T>> Steps;
        // TODO: Random with seed

        public Model()
        {
            Steps = new List<IStep<T>>();
        }

        public Model<T> Step(IStep<T> step)
        {
            Steps.Add(step);
            return this;
        }

        public T Process(T subject)
        {
            foreach (var step in Steps)
            {
                subject = step.Step(subject);
            }
            return subject;
        }
    }
}
