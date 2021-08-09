namespace ApplicationCore.Domain.DiscreteSteps
{
    public class CheckPointGraph
    {
        /// <summary>
        /// Текущий узел графа
        /// </summary>
        private CheckPoint _currentNode;
        
        /// <summary>
        /// Начала графа
        /// </summary>
        public CheckPoint Root { get; }
 
        
        
        public CheckPointGraph(CheckPoint root)
        {
            Root = root;
        }




        public void CalculateCurrentNode()//TODO: передавать объект 
        {
            
        }
        
        
    }
}