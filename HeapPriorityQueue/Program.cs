/*
A1. 
Insert 15: [15] no swaps
Insert 40: [40, 15] 1 swap
Insert 25: [40, 15, 25] no swaps
Insert 50: [50, 40, 25, 15] 2 swaps
Insert 10: [50, 40, 25, 15, 10] no swaps
Insert 45: [50, 40, 45, 15, 10, 25] 1 swap

A2. 
RemoveMax:
1: returns 50 | Array: [45, 40, 25, 15, 10]
2: returns 45 | Array: [40, 15, 25, 10]

A3. 
First heapify: 7/2 - 1 = 2 Last none-leaf node, all others are leaves and satisfy the heap property

Heapify index 2: [5, 12, 15, 20, 8, 3, 1]
Heapify index 1: [5, 20, 15, 12, 8, 3, 1]
Heapify index 0: [20, 12, 15, 5, 8, 3, 1]

A4. 
Round 1: [15, 12, 3, 5, 8, 1 ]
Sorted: [20]

Round 2: [12, 8, 3, 5, 1 ]
Sorted: [15, 20]

A5. 
Final Array: [10, 15, 25, 50, 40, 45]

A6. 
Way to build it     Insert      Peek        RemoveMax
Unsorted List       O(1)*       O(n)        O(n)
Sorted List         O(n)        O(1)        O(1)
Binary Heap         O(logn)     O(1)        O(logn)

A binary heap has constant access to the highest priority item, so it is only O(1) for peek. Insertion and RemoveMax are both logn, so it is efficient for big queues.

A7.
A good example would be for customer support where the items are support tickets and the priority is to handle urgent issues first

A second example would be air traffic control where the items are planes waiting to land and the priority is planes with emergencies, then the landing order of the schedule.

B5. 
TopK has time complexity O(nlogk). Each insertion or removal uses the heap with a max of k + 1 and extracting is O(klogk).

Sorting take O(nlogn). When we have a small k, TopK will be faster because it has a smal head and doesn't sor tall n scores. If we have a k of 1, TopK is O(n)
*/

using System;

public class Program
{
    static void Main()
    {
        Console.WriteLine("B1:");
        MinPriorityQueue queue = new MinPriorityQueue();

        int[] values = { 15, 40, 25, 50, 10, 45 };

        foreach (int value in values)
        {
            queue.Insert(value);
        }

        while (!queue.IsEmpty)
        {
            Console.Write(queue.RemoveMin());

            if (!queue.IsEmpty)
                Console.Write(", ");
        }

        Console.WriteLine();

        try
        {
            queue.RemoveMin();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\nB2:");
        Console.WriteLine(IsMaxHeap(new int[] { 90, 70, 80, 30, 60, 50 }));
        Console.WriteLine(IsMaxHeap(new int[] { 50, 40, 30, 45, 10 }));
        Console.WriteLine(IsMaxHeap(new int[] { 10, 20, 30 }));
        Console.WriteLine(IsMaxHeap(new int[] { 7 }));
        Console.WriteLine(IsMaxHeap(new int[] { }));

        Console.WriteLine("\nB3:");
        int[] numbers = { 5, 12, 3, 20, 8, 15, 1 };

        HeapSort(numbers);
        Console.WriteLine("Sorted: " + string.Join(", ", numbers));

        int[] heapTest = { 5, 12, 3, 20, 8, 15, 1 };

        BuildMaxHeap(heapTest);
        Console.WriteLine("Max heap: " + string.Join(", ", heapTest));
        Console.WriteLine("Is max heap: " + IsMaxHeap(heapTest));

        Console.WriteLine("\nB4:");

        int[] scores = { 42, 17, 99, 63, 8, 75, 31 };
        int[] topScores = TopK(scores, 3);

        Console.WriteLine(string.Join(", ", topScores));
    }
    static bool IsMaxHeap(int[] array)
    {
        for (int i = 0; i < array.Length / 2; i++)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (array[left] > array[i])
            {
                return false;
            }

            if (right < array.Length && array[right] > array[i])
            {
                return false;
            }
        }
        return true;
    }

    static void Heapify(int[] array, int size, int i)
    {
        int largest = i;
        int left = 2 * i + 1;
        int right = 2 * i + 2;

        if (left < size && array[left] > array[largest])
        {
            largest = left;
        }

        if (right < size && array[right] > array[largest])
        {
            largest = right;
        }

        if (largest != i)
        {
            int temp = array[i];
            array[i] = array[largest];
            array[largest] = temp;

            Heapify(array, size, largest);
        }
    }

    static void BuildMaxHeap(int[] array)
    {
        for (int i = array.Length / 2 - 1; i >= 0; i--)
        {
            Heapify(array, array.Length, i);
        }
    }

    static void HeapSort(int[] array)
    {
        BuildMaxHeap(array);

        for (int end = array.Length - 1; end > 0; end--)
        {
            int temp = array[0];
            array[0] = array[end];
            array[end] = temp;

            Heapify(array, end, 0);
        }
    }

    static int[] TopK(int[] scores, int k)
    {
        if (k <= 0)
        {
            return new int[0];
        }

        PriorityQueue<int, int> queue = new();

        foreach (int score in scores)
        {
            queue.Enqueue(score, score);

            if (queue.Count > k)
            {
                queue.Dequeue();
            }
        }

        int[] result = new int[queue.Count];

        for (int i = result.Length - 1; i >= 0; i--)
        {
            result[i] = queue.Dequeue();
        }

        return result;
    }
}

public class MinPriorityQueue
{
    private List<int> heap = new();
    public int Count => heap.Count;
    public bool IsEmpty => Count == 0;

    public int Parent(int i) => (i - 1) / 2;

    public void Insert(int priority)
    {
        heap.Add(priority);
        int current = heap.Count - 1;

        while (current > 0 && heap[current] < heap[Parent(current)])
        {
            Swap(current, Parent(current));
            current = Parent(current);
        }
    }

    public int Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Empty Queue");
        }
        else
        {
            return heap[0];
        }
    }

    public int RemoveMin()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Empty Queue");
        }

        int min = heap[0];

        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);

        if (heap.Count > 0)
        {
            SinkDown(0);
        }

        return min;

    }

    private void SinkDown(int index)
    {
        while (true)
        {
            int minIndex = index;
            int left = 2 * index + 1;
            int right = 2 * index + 2;

            if (left < heap.Count && heap[left] < heap[minIndex])
            {
                minIndex = left;
            }

            if (right < heap.Count && heap[right] < heap[minIndex])
            {
                minIndex = right;
            }

            if (minIndex == index)
            {
                return;
            }

            Swap(index, minIndex);
            index = minIndex;
        }
    }

    private void Swap(int a, int b)
    {
        (heap[a], heap[b]) = (heap[b], heap[a]);
    }
}