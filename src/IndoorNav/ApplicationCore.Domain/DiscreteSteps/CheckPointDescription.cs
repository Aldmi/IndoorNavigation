namespace ApplicationCore.Domain.DiscreteSteps
{
    public class CheckPointDescription
    {
        
        public CheckPointDescription(string name, string description)
        {
            Name = name;
            Description = description;
        }
        
        public string Name { get; }
        public string Description { get; }
        
    }
}