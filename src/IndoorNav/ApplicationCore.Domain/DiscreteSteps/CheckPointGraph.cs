using System;
using ApplicationCore.Shared;
using ApplicationCore.Shared.DataStruct;

namespace ApplicationCore.Domain.DiscreteSteps
{
    /// <summary>
    /// Граф контрольных точек
    /// </summary>
    public class CheckPointGraph
    {
        /// <summary>
        /// Начала графа.
        /// </summary>
        private readonly TreeNode<CheckPoint> _root;
        public CheckPointGraph(TreeNode<CheckPoint> root)
        {
            _root = root;
        }
        
        /// <summary>
        /// Узел графа в котором мы находимся
        /// </summary>
        public TreeNode<CheckPoint>? CurrentNode { get; private set; }
        public bool CurrentNodeIsSet => CurrentNode != null;
 
        
        /// <summary>
        /// Попытка передвинуться по узлам графа, анализируя входные данные
        /// </summary>
        public Moving TryMove(InputData inputData)
        {
            var predicate = new Func<TreeNode<CheckPoint>, bool>(node => node.Value.GetZone(inputData) == Zone.In);
            var findNode = CurrentNodeIsSet ?
                _root.FindForNeighbors(predicate) :
                _root.FindInDepth(predicate);


            //TODO: Расписатьт все варианты движениея
            Moving moving = null;
            if (findNode != null)
            {
                if (findNode != CurrentNode)
                {
                    CurrentNode = findNode;
                    moving= Moving.CompleteSegment(CurrentNode.Value, findNode.Value);
                }
            }
            else
            {
                moving= CurrentNodeIsSet ?
                    Moving.StartSegment(CurrentNode.Value) :
                    Moving.UnknownSegment();
            }
            
            return moving;
        }
    }
}