using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    public class Node
    {
        public int ID { get; set; }
        public long Value { get; set; }
        public Node? Next { get; set; }
        public Node? Prev { get; set; }

        public Node(int iD, long value)
        {
            ID = iD;
            Value = value;
            Next = null;
            Prev = null;
        }
    }

    public class DblLinkedList
    {
        public Node? Head { get; set; }
        public Node? Tail { get; set; }
        public int Count { get; set; }

        public DblLinkedList()
        {
            Head = null;
            Tail = null;
            
            Count = 0;
        }

        public DblLinkedList(Node node) 
        {
            Head = node;
            Tail = node;

            // make circular
            Head.Prev = node;
            Head.Next = node;
            Tail.Prev = node;
            Tail.Next = node;

            Count = 1;
        }

        public void AddFirst(Node node)
        {
            //Console.WriteLine("AddFirst()");
            Head = node;
            Tail = node;

            // make circular
            Head.Prev = node;
            Head.Next = node;
            Tail.Prev = Head;
            Tail.Next = Head;

            Count++;
        }

        public int AddAfter(Node firstNode, Node newNode)
        {
            //Console.WriteLine($"AddAfter({newNode.ID},{newNode.Value})");
            // insert new node
            newNode.Prev = firstNode;
            newNode.Next = firstNode.Next;

            // connect right node to new node
            firstNode.Next.Prev = newNode;

            // connect left node to new node
            firstNode.Next = newNode;

            // update last added
            Tail = newNode;

            Count++;
            return firstNode.ID;
        }

        public int AddBefore(Node firstNode, Node newNode)
        {
            //Console.WriteLine($"AddBefore({newNode.ID},{newNode.Value})");
            // insert new node
            newNode.Next = firstNode;
            newNode.Prev = firstNode.Prev;

            // connect left node to new node
            firstNode.Prev.Next = newNode;

            // connect right node to new node
            firstNode.Prev = newNode;

            // update last added
            Tail = newNode;

            Count++;
            return firstNode.ID;
        }

        public Node Remove(Node remNode)
        {
            //Console.WriteLine($"Remove({remNode.ID},{remNode.Value})");
            Tail = remNode.Prev;

            // connect prev and next to remove node
            remNode.Prev.Next = remNode.Next;
            remNode.Next.Prev = remNode.Prev;

            remNode.Prev = null;
            remNode.Next = null;

            Count--;
            return remNode;
        }

        public Node FindId(int id)
        {
            Node rover = Head;
            while (rover.ID != id)
                rover = rover.Next;

            return rover;
        }

        public Node FindZero()
        {
            Node rover = Head;

            while (rover.Value != 0)
                rover = rover.Next;

            return rover;
        }
    }

}
