using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Shared;
using ApplicationCore.Shared.DataStruct;
using Libs.Beacons.Models;

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
        public BeaconId RootId => _root.Value.BeaconId;
 
        
        
        //TODO: перед обработкой упорядочевать по Range inputData (сначала с малым расстоянием).
        
        /// <summary>
        /// Вычислить перемещение по узлам графа, анализируя входные данные.
        /// </summary>
        public Moving CalculateMove(IEnumerable<InputData> inputDataList)
        {
            Moving moving;
            if (!CurrentNodeIsSet)
            {
                var findNode= FindFirstCurrentNode(inputDataList);
                if (findNode != null)
                {
                    CurrentNode = findNode;
                    moving= Moving.InitSegment(CurrentNode.Value);                             //Выставить первый раз Стартовый сегмент.  
                }
                else
                {
                    moving= Moving.UnknownSegment();                                            //Стартовый сегмент не найден.
                }
            }
            else
            {
                var findNode= FindAmongNeighborsOfCurrentNode(inputDataList);
                if (findNode != null)
                {
                    if (findNode == CurrentNode)
                    {
                        moving = Moving.StartSegment(CurrentNode!.Value);                      //Стоим около стартового сегмента.
                    }
                    else
                    {
                        moving = Moving.CompleteSegment(CurrentNode!.Value, findNode.Value);  //Однократно выставили завершающий сегмент.
                        CurrentNode = findNode;
                    }
                }
                else
                {
                    moving = Moving.GoToEnd(CurrentNode!.Value);                              //Начали движение от стартового сегмента.
                }
            }
            return moving;
        }

        
        /// <summary>
        /// Сбросить текщее положение в графе.
        /// </summary>
        public void Reset() =>  CurrentNode = null;
        
        
        private TreeNode<CheckPoint>? FindFirstCurrentNode(IEnumerable<InputData> inputDataList)
        {
            foreach (var inputData in inputDataList)
            {
                var findNode = _root.FindInDepth(node => node.Value.GetZone(inputData) == Zone.In);
                if (findNode != null)
                    return findNode;
            }
            return null;
        }
        
        
        /// <summary>
        /// Найти среди соседей текущего узла
        /// </summary>
        /// <param name="inputDataList"></param>
        /// <returns></returns>
        private TreeNode<CheckPoint>? FindAmongNeighborsOfCurrentNode(IEnumerable<InputData> inputDataList)
        {
            foreach (var inputData in inputDataList)
            {
                var findNode = CurrentNode!.FindForNeighbors(node => node.Value.GetZone(inputData) == Zone.In);
                if (findNode != null)
                    return findNode;
            }
            return null;
        }

        

   
    }
}