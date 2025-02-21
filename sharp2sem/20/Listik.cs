using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp2sem
{
    public class Listik
    {
        private ListikNode head;
        private ListikNode tail;

        public bool IsEmpty => head == null;

        public Listik()
        {
            head = null;
            tail = null;
        }

        public int TakeFirst()
        {
            if (IsEmpty)
            {
                throw new Exception("Список пуст!");
            }

            ListikNode firstElement = head;
            head = head.Next;
            if (IsEmpty)
            {
                tail = null;
            }
            return firstElement.Value;
        }

        public void AddToBegin(int value)
        {
            ListikNode newItem = new ListikNode(value);
            if (IsEmpty)
            {
                head = newItem;
                tail = newItem;
            }
            else
            {
                newItem.Next = head;
                head = newItem;
            }
        }

        public int TakeLast()
        {
            if (IsEmpty)
            {
                throw new Exception("Список пуст!");
            }

            ListikNode lastElement = head;
            if (head.Next == null)
            {
                head = null;
                tail = null;
            }
            else
            {
                while (lastElement.Next != tail)
                {
                    lastElement = lastElement.Next;
                }
                ListikNode temp = tail;
                tail = lastElement;
                lastElement = temp;
                tail.Next = null;
            }
            return lastElement.Value;
        }

        public void AddToEnd(int value)
        {
            ListikNode newItem = new ListikNode(value);
            if (IsEmpty)
            {
                head = newItem;
                tail = newItem;
            }
            else
            {
                tail.Next = newItem;
                tail = newItem;
            }
        }

        public ListikNode Find(ListikNode searchBegin, int key)
        {
            ListikNode pointer = searchBegin;
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

        public ListikNode Find(int key)
        {
            return Find(head, key);
        }

        public void Delete(int key)
        {
            if (IsEmpty)
            {
                throw new Exception("Список пуст!");
            }

            ListikNode currentElement = head;
            if (head.Next == null)
            {
                head = null;
                tail = null;
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
    }
}
