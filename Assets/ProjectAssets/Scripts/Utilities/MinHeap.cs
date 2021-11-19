using System;
using System.Collections.Generic;

namespace Project.Utilities
{
    public class MinHeap<T>
    {
        private const int INIT_CAPACITY = 4;

        private T[] _items;
        private int _lastItemIndex;
        private readonly IComparer<T> _comparer;

        public MinHeap() : this(INIT_CAPACITY, Comparer<T>.Default) { }

        public MinHeap(int capacity) : this(capacity, Comparer<T>.Default) { }

        public MinHeap(Comparison<T> comparison) : this(INIT_CAPACITY, Comparer<T>.Create(comparison)) { }

        public MinHeap(IComparer<T> comparer) : this(INIT_CAPACITY, comparer) { }

        public MinHeap(int capacity, IComparer<T> comparer)
        {
            _items = new T[capacity];
            _lastItemIndex = -1;
            _comparer = comparer;
        }

        public int Count => _lastItemIndex + 1;

        public void Add(T item)
        {
            if (_lastItemIndex == _items.Length - 1)
                Resize();

            _lastItemIndex++;
            _items[_lastItemIndex] = item;

            SortUp(_lastItemIndex);
        }

        public T Remove()
        {
            if (_lastItemIndex == -1)
                throw new InvalidOperationException("The heap is empty");

            var removedItem = _items[0];
            _items[0] = _items[_lastItemIndex];
            _lastItemIndex--;

            SortDown(0);

            return removedItem;
        }

        public T Peek()
        {
            if (_lastItemIndex == -1)
                throw new InvalidOperationException("The heap is empty");

            return _items[0];
        }

        public void Clear()
        {
            _lastItemIndex = -1;
        }

        private void SortUp(int index)
        {
            if (index == 0)
                return;

            var childIndex = index;
            var parentIndex = (index - 1) / 2;

            if (_comparer.Compare(_items[childIndex], _items[parentIndex]) < 0)
            {
                var temp = _items[childIndex];
                _items[childIndex] = _items[parentIndex];
                _items[parentIndex] = temp;

                SortUp(parentIndex);
            }
        }

        private void SortDown(int index)
        {
            var leftChildIndex = index * 2 + 1;
            var rightChildIndex = index * 2 + 2;
            var smallestItemIndex = index; 

            if (leftChildIndex <= _lastItemIndex &&
                _comparer.Compare(_items[leftChildIndex], _items[smallestItemIndex]) < 0)
            {
                smallestItemIndex = leftChildIndex;
            }

            if (rightChildIndex <= _lastItemIndex &&
                _comparer.Compare(_items[rightChildIndex], _items[smallestItemIndex]) < 0)
            {
                smallestItemIndex = rightChildIndex;
            }

            if (smallestItemIndex != index)
            {
                var temp = _items[index];
                _items[index] = _items[smallestItemIndex];
                _items[smallestItemIndex] = temp;

                SortDown(smallestItemIndex);
            }
        }

        private void Resize()
        {
            var newArr = new T[_items.Length * 2];
            
            for (int i = 0; i < _items.Length; i++)
                newArr[i] = _items[i];

            _items = newArr;
        }
    }
}