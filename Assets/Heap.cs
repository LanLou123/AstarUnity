using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Heap<T> where T:IHeapItem<T>
    {
        T[] items;
        int curItemcount;
        public Heap(int maxHeapSize)
        {
            items = new T[maxHeapSize];
        }
        public void add(T item)
        {
            item.HeapIdx = curItemcount;
            items[curItemcount] = item;
            SortUp(item);
            curItemcount++;
        }

        public int Count
        {
            get
            {
                return curItemcount;
            }
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        public bool Contains(T item)
        {
            return Equals(items[item.HeapIdx], item);
        }
        public T RemoveFirst()
        {
            T firstItem = items[0];
            curItemcount--;
            items[0] = items[curItemcount];
            items[0].HeapIdx = 0;
            SortDown(items[0]);
            return firstItem;
        }

        void SortDown(T item)
        {
            while(true)
            {
                int childidxL = item.HeapIdx * 2 + 1;
                int childidxR = item.HeapIdx * 2 + 2;
                int swapIdx = 0;
                if (childidxL < curItemcount)
                {
                    swapIdx = childidxL;
                    if (childidxR < curItemcount)
                    {
                        if (items[childidxL].CompareTo(items[childidxR]) < 0)
                        {
                            swapIdx = childidxR;
                        }
                    }
                    if (item.CompareTo(items[swapIdx]) < 0)
                    {
                        Swap(item, items[swapIdx]);
                    }
                    else
                        return;
                }
                else
                    return;

            }
        }

        void SortUp(T item)
        {
            int parentIdx = (item.HeapIdx - 1) / 2;
            while (true)
            {
                T parentItem = items[parentIdx];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                    break;
                parentIdx = (item.HeapIdx - 1) / 2;
            }
        }
        void Swap(T _A,T _B)
        {
            items[_A.HeapIdx] = _B;
            items[_B.HeapIdx] = _A;
            int itemAidx = _A.HeapIdx;
            _A.HeapIdx = _B.HeapIdx;
            _B.HeapIdx = itemAidx;
        }
    }
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIdx
        {
            get;
            set;
        }
    }
}
