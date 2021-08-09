namespace ApplicationCore.Domain.DiscreteSteps
{
    public class CheckPointDescription
    {
        public string Name { get; }
        public string Description { get; }
        
        public CheckPointDescription(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}