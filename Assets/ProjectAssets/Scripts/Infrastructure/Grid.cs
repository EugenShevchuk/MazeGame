using System;
using System.Collections.Generic;
using Project.Components;

namespace Project.Infrastructure
{
    public class Grid
    {
        public Cell[][] Cells;
        public Cell[] SpawnerCells;
        public Cell[] SurroundingCells;
        public Processor Processor;

        public bool IsGenerated = false;

        public int Size;
        
        public Cell RandomCell(Random random)
        {
            var row = random.Next(Cells.Length);
            var column = random.Next(Cells[row].Length);
            return GetCell(row, column);
        }

        public void DelAt(int row, int column)
        {
            var arr = Cells[row];
            var newArray = new Cell[arr.Length - 1];
            
            for (int i = 0; i < column; i++)
                newArray[i] = arr[i];

            for (int i = column + 1; i < arr.Length; i++)
                newArray[i - 1] = arr[i];

            Cells[row] = newArray;
        }

        public IEnumerable<Cell[]> EachRow()
        {
            foreach (var cellRow in Cells)
                yield return cellRow;
        }

        public IEnumerable<Cell> EachCell()
        {
            foreach (var cellRow in Cells)
                foreach (var cell in cellRow)
                    yield return cell;
        }

        public Cell GetCell(int row, int column)
        {
            return Cells[row][column];
        }
    }
}