using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ApplicationCore.Shared.DataStruct
{
    public class TreeNode<T>
    {
        private readonly T _value;
        private readonly List<TreeNode<T>> _children = new List<TreeNode<T>>();

        
        public TreeNode(T value)
        {
            _value = value;
        }

        
        public TreeNode<T> this[int i] => _children[i];
        public TreeNode<T> Parent { get; private set; }
        public T Value => _value;
        public ReadOnlyCollection<TreeNode<T>> Children => _children.AsReadOnly();
        public bool IsRoot => Parent == null;
        public bool IsLeaf => Children.Count == 0;
        public int Level
        {
            get
            {
                if (IsRoot)
                    return 0;
                return Parent.Level + 1;
            }
        }

        
        public TreeNode<T> AddChild(T value)
        {
            var node = new TreeNode<T>(value) {Parent = this};
            _children.Add(node);
            return node;
        }

        
        public TreeNode<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        
        public bool RemoveChild(TreeNode<T> node)
        {
            return _children.Remove(node);
        }
        

        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in _children)
                child.Traverse(action);
        }
        
        
        /// <summary>
        /// Поиск в глубину.
        /// обход дерева от Parent к Child.
        /// </summary>
        public TreeNode<T> FindInDepth(Func<TreeNode<T>, bool> predicate)
        {
            if (predicate(this))
                return this;

            foreach (var child in _children)
            {
               var res= child.FindInDepth(predicate);
               if (res != null)
                   return res;
            }
            return null;
        }


        /// <summary>
        /// Поиск вблизи узла.
        /// в одном родителе и во всех наследниках.
        /// </summary>
        public TreeNode<T> FindForNeighbors(Func<TreeNode<T>, bool> predicate)
        {
            // if (predicate(this))
            //     return this;

            if (predicate(Parent))
                return Parent;
            
            var res= _children.FirstOrDefault(predicate);
            return res;
        }

        public IEnumerable<T> Flatten()
        {
            return new[] {Value}.Concat(_children.SelectMany(x => x.Flatten()));
        }
        
        public override string ToString() => Value != null ? Value.ToString() : "[data null]";
    }
}