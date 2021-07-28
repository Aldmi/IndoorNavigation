namespace Api.Forms.Infrastructure
{
    public class Group<T> : ObservableList<T>
    {
        public Group(string name, string shortName)
        {
            Name = name;
            ShortName = shortName;
        }


        public string Name { get; }
        public string ShortName { get; }
    }
}
