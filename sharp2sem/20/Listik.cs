using System;
using System.Text;

namespace sharp2sem._20
{
    public class Listik
    {
        private ListikNode _head;
        private ListikNode _tail;

        public bool IsEmpty => _head == null;

        public Listik()
        {
            _head = null;
            _tail = null;
        }

        public int TakeFirst()
        {
            if (IsEmpty)
            {
                throw new Exception("Список пуст!");
            }

            ListikNode firstElement = _head;
            _head = _head.Next;
            if (IsEmpty)
            {
                _tail = null;
            }

            return firstElement.Value;
        }

        public void AddToBegin(int value)
        {
            ListikNode newItem = new ListikNode(value);
            if (IsEmpty)
            {
                _head = newItem;
                _tail = newItem;
            }
            else
            {
                newItem.Next = _head;
                _head = newItem;
            }
        }

        public int TakeLast()
        {
            if (IsEmpty)
            {
                throw new Exception("Список пуст!");
            }

            ListikNode lastElement = _head;
            if (_head.Next == null)
            {
                _head = null;
                _tail = null;
            }
            else
            {
                while (lastElement.Next != _tail)
                {
                    lastElement = lastElement.Next;
                }

                (_tail, lastElement) = (lastElement, _tail);
                _tail.Next = null;
            }

            return lastElement.Value;
        }

        public void AddToEnd(int value)
        {
            ListikNode newItem = new ListikNode(value);
            if (IsEmpty)
            {
                _head = newItem;
                _tail = newItem;
            }
            else
            {
                _tail.Next = newItem;
                _tail = newItem;
            }
        }

        private ListikNode Find(ListikNode startSearchFrom, int key)
        {
            ListikNode pointer = startSearchFrom;
            while (pointer != null)
            {
                if (pointer.Value == key)
                {
                    break;
                }

                pointer = pointer.Next;
            }

            return pointer;
        }

        private ListikNode Find(int key)
        {
            return Find(_head, key);
        }

        public void Delete(int key)
        {
            if (IsEmpty)
            {
                throw new Exception("Список пуст!");
            }

            ListikNode currentElement = _head;
            if (_head.Next == null)
            {
                _head = null;
                _tail = null;
            }
            else
            {
                while (currentElement.Next.Value != key)
                {
                    currentElement = currentElement.Next;
                }

                ListikNode itemToDelete = currentElement.Next;
                currentElement.Next = itemToDelete.Next;
                itemToDelete.Next = null;
            }
        }

        public void Insert(ListikNode itemBeforeNew, int value)
        {
            ListikNode pointer = itemBeforeNew;
            if (pointer != null)
            {
                ListikNode newItem = new ListikNode(value);
                newItem.Next = pointer.Next;
                pointer.Next = newItem;
            }
        }

        public void DoubleOdds()
        {
            ListikNode currentItem = _head;
            while (currentItem != null)
            {
                if (Math.Abs(currentItem.Value) % 2 == 0)
                {
                    ListikNode duplicateNode = new ListikNode(currentItem);
                    duplicateNode.Next = currentItem.Next;
                    currentItem.Next = duplicateNode;
                    currentItem = duplicateNode.Next;
                }
                else
                {
                    currentItem = currentItem.Next;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            ListikNode current = _head;
            while (current != null)
            {
                if (current != _head)
                {
                    builder.Append(" ");
                }

                builder.Append(current.Value);
                current = current.Next;
            }

            return builder.ToString();
        }
    }
}