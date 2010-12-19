/**
 * $Id$
 *
 * Coursework – The business logic layer of the Futoshiki puzzle.
 *
 * This file is the result of my own work. Any contributions to the work by third parties,
 * other than tutors, are stated clearly below this declaration. Should this statement prove to
 * be untrue I recognise the right and duty of the Board of Examiners to take appropriate
 * action in line with the university's regulations on assessment. 
 */

using System;
using Models;

namespace BLL
{
    /// <summary>
    /// A class service for the cell.
    /// </summary>
    public class CellService
    {
        private const int DftSize = 5;
        private const int MinSize = 3;
        private Cell[] _cells = null;
        
        /// <summary>
        /// Create empty Cells according to given size, which will use default 
        /// size if the size is not gave or less than Mininum size. The game 
        /// can be any size from 4 x 4 upwards.
        /// </summary>
        /// <param name="size">The numerical grid size of Futoshiki game.</param>
        /// <returns>A Cell collection</returns>
        public Cell[] DoBlankCells(int size)
        {
            int scalar = size<MinSize ? DftSize : size;
            int rowSize = 2*scalar - 1;
            int cellsAmount = rowSize*rowSize;
            _cells = new Cell[cellsAmount];
            for (int index = 0; index < _cells.Length; index++)
            {
                _cells[index] = new Cell();
                _cells[index].Row = index / rowSize;
                _cells[index].Column = index % rowSize;

                InitializeCandidates(index, scalar);
            }

            return _cells;
        }

        private void InitializeCandidates(int index, int scalar)
        {
            if (_cells[index].IsNumeric)
            {
                _cells[index].Candidates = new int[scalar];
                for (int n = 1; n <= scalar; n++)
                {
                    _cells[index].Candidates[n - 1] = n;
                }
            }
            else
            {
                _cells[index].Candidates = null;
            }
        }

        // TODO: pick a number from the candidate numbers of the Cell
        public Cell[] DoRandomVal()
        {
            if (null == _cells)
            {
                DoBlankCells(DftSize);
            }

            Random rdm = new Random();
            for (int i = 0; _cells[i].IsNumeric && i < _cells.Length; i++)
            {
                Cell c = _cells[i];
                int val = rdm.Next(DftSize);
                c.Value = c.Candidates[val].ToString();

                // 把用过的数值从这个Cell的候选数列表中删掉
                RemoveCandidate(c, val);

                // 冻结相关单元格的该候选数
                // 记录多次删除的候选数
                // 移到下一格，if（赋值失败）恢复相关单元格候选数，重新调用赋值
            }

            return _cells;
        }
        

        // Remove the number is used from the candidates of the Cell
        private bool RemoveCandidate(Cell cell, int unneededIdx)
        {
            bool isRemoved = true;

            try
            {
                int originalLength = cell.Candidates.Length;
                int newLength = originalLength - 1;
                int[] newCandidates = new int[newLength];
                if (unneededIdx != newLength)
                {
                    cell.Candidates[unneededIdx] = cell.Candidates[newLength];
                }
                Array.Copy(cell.Candidates, newCandidates, newLength);
                cell.Candidates = newCandidates;
            } catch(Exception e)
            {
                isRemoved = false;
            }

            return isRemoved;

        }

        // TODO: Freeze the number is used in the Candidates of the other Cells on relative row & column
        private bool FreezeCandidate(int candidate)
        {
            bool isFreezed = true;
            try
            {
                
            }catch(Exception e)
            {
            }

            return isFreezed;
        }


        // TODO: Record duplicate Candidates freezing

        // TODO: 

    }
}
