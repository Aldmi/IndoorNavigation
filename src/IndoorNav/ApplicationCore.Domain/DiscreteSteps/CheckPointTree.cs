﻿using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.Services;
using ApplicationCore.Shared;
using ApplicationCore.Shared.DataStruct;
using ApplicationCore.Shared.DataStruct.Tree;
using Libs.Beacons.Models;

namespace ApplicationCore.Domain.DiscreteSteps
{
    /// <summary>
    /// Дерево контрольных точек
    /// </summary>
    public class CheckPointTree
    {
        /// <summary>
        /// Начало дерева.
        /// </summary>
        private readonly TreeNode<CheckPoint> _root;
        public CheckPointTree(TreeNode<CheckPoint> root)
        {
            _root = root;
        }
        
        /// <summary>
        /// Узел дерева в котором мы находимся
        /// </summary>
        public TreeNode<CheckPoint>? CurrentNode { get; private set; }
        public bool CurrentNodeIsSet => CurrentNode != null;
        public BeaconId RootId => _root.Value.BeaconId;
 
        
        
        //TODO: перед обработкой упорядочевать по Distance inputData (сначала с малым расстоянием).
        
        /// <summary>
        /// Вычислить перемещение по узлам графа, анализируя входные данные.
        /// </summary>
        public Moving CalculateMove(IEnumerable<BeaconDistanceModel> inputDataList)
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
        public void Reset() => CurrentNode = null;
        
        
        private TreeNode<CheckPoint>? FindFirstCurrentNode(IEnumerable<BeaconDistanceModel> inputDataList)
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
        private TreeNode<CheckPoint>? FindAmongNeighborsOfCurrentNode(IEnumerable<BeaconDistanceModel> inputDataList)
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