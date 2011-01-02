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
    /// A class service for the cell, which encapsulated an algorithm to 
    /// automatically generate Futoshiki cells according to given seed number.
    /// The game can be any size from 4*4 upwards.
    /// </summary>
    public class CellService : ICellService
    {
        /// <summary>
        /// The constant is a default size of the Futoshiki. It will be used when 
        /// the seed number not provided for the initialization
        /// </summary>
        private const int DftSize = 5;

        /// <summary>
        ///The constant is a minimum size of the Futoshiki.
        /// </summary>
        private const int MinSize = 4;

        /// <summary>
        /// The array to put all cells of Futoshiki. 
        /// </summary>
        private readonly Cell[] _cells;

        /// <summary>
        /// The value of a given size for generate a Futoshiki grid. The amount 
        /// of actual numerical cells of a row or column will obey this value.
        /// </summary>
        private readonly int _scalarSize = DftSize;

        /// <summary>
        /// The real size of a row, whick includes numeric cells and sign cells.
        /// </summary>
        private readonly int _rowSize;

        /// <summary>
        /// Total cells.
        /// </summary>
        private readonly int _cellsAmount;

        /// <summary>
        /// A random number generator.
        /// </summary>
        private readonly Random _rdm;

        /// <summary>
        /// A horizontal great-than sign "＞";
        /// </summary>
        private const string HorizontalGreater = "\xff1e";

        /// <summary>
        /// A horizontal less-than sign "＜";
        /// </summary>
        private const string HorizontalLesser = "\xff1c";

        /// <summary>
        /// A vertical great-than sign "∨";
        /// </summary>
        private const string VerticalGreater = "\x2228";

        /// <summary>
        /// A vertical less-than sign "∧";
        /// </summary>
        private const string VerticalLesser = "\x2227";

        /// <summary>
        /// The default constructor which only can have a standard (5*5) game 
        /// of Futoshiki.
        /// </summary>
        public CellService() : this(DftSize)
        {
        }

        /// <summary>
        /// The constructor can create a grid of any size by given seed number.
        /// </summary>
        /// <param name="size"></param>
        public CellService(int size)
        {
            _scalarSize = size < MinSize ? DftSize : size;
            _rowSize = 2*_scalarSize - 1;
            _cellsAmount = _rowSize*_rowSize;
            _cells = new Cell[_cellsAmount];
            _rdm = new Random();
            InitializeCell();
        }

        private void InitializeCell()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i] = new Cell {Row = i/_rowSize, Column = i%_rowSize};
                InitializeCandidates(i);
            }
        }

        private void InitializeCandidates(int index)
        {
            if (_cells[index].IsNumeric)
            {
                _cells[index].Candidates = new int[_scalarSize];
                _cells[index].FrzRecord = new int[_scalarSize];
                for (int n = 1; n <= _scalarSize; n++)
                {
                    _cells[index].Candidates[n - 1] = n;
                }
            }
            else
            {
                _cells[index].Candidates = null;
            }
        }

        /// <summary>
        /// Generate a Futoshiki Cells according to given size of initilization
        /// stage. whick will fill numeric cells up.
        /// </summary>
        /// <returns>A Cell collection</returns>
        public Cell[] DoGrid()
        {
            // TODO: If save the numeric cells as a solution to a XML file here?

            return DoNumericCells(0) && DoSignCells() ? _cells : null;
        }

        // Generating inequality symbol randomly.
        private bool DoSignCells()
        {
            int blanksCount = (_scalarSize - 1)*(_scalarSize - 1);
            int numCount = _scalarSize*_scalarSize;
            int[] idxesNumCells = new int[numCount];
            int signsCount = _cellsAmount - numCount - blanksCount;
            int[] idxesOfSigns = new int[signsCount];
            for (int i = 0, s = 0, n = 0; i < _cells.Length; i++)
            {
                if (_cells[i].IsHorizontalSign || _cells[i].IsVerticalSign)
                {
                    // get ready an array that contains all indexes of sign cells      
                    idxesOfSigns[s] = i;
                    ++s;
                }
                else if (_cells[i].IsNumeric)
                {
                    // get ready an array that contains all indexes of numeric cells.
                    idxesNumCells[n] = i;
                    ++n;
                }
            }

            bool isSuccess = true;

            // get ready a quantity of inequality symbols
            int sMin = _scalarSize - 1;
            int sMax = _scalarSize*_scalarSize/2;
            int signsQuantity = _rdm.Next(sMin, sMax);
            isSuccess &= DoSignCells(idxesOfSigns, signsQuantity);

            // get ready a quantity of numeric number
            int numsQuantity = _rdm.Next(sMax - signsQuantity);
            int veiled = numCount - numsQuantity;
            isSuccess &= VeilNumCells(idxesNumCells, veiled);

            return isSuccess;
        }

        // To veil some numeric cells
        private bool VeilNumCells(int[] idxesNumCells, int veiled)
        {
            while (idxesNumCells.Length > veiled)
            {
                int i = _rdm.Next(idxesNumCells.Length);
                RemoveElement(ref idxesNumCells, idxesNumCells[i]);
            }

            if (idxesNumCells.Length <= 0)
            {
                return true;
            }

            int idx = idxesNumCells[idxesNumCells.Length - 1];
            _cells[idx].Value = null;
            RemoveElement(ref idxesNumCells, idxesNumCells[idxesNumCells.Length - 1]);

            return VeilNumCells(idxesNumCells, veiled);
        }

        // To put some less-than and great-than signs into cells.
        private bool DoSignCells(int[] idxesOfsigns, int signsQuantity)
        {
            while (idxesOfsigns.Length > signsQuantity)
            {
                int v = idxesOfsigns[_rdm.Next(idxesOfsigns.Length)];
                RemoveElement(ref idxesOfsigns, v);
            }

            if (idxesOfsigns.Length <= 0)
            {
                return true;
            }

            int i = idxesOfsigns[_rdm.Next(idxesOfsigns.Length)];
            if (_cells[i].IsHorizontalSign)
            {
                bool g = int.Parse(_cells[i - 1].Value) > int.Parse(_cells[i + 1].Value);
                _cells[i].Value = g ? HorizontalGreater : HorizontalLesser;
                RemoveElement(ref idxesOfsigns, i);
                return DoSignCells(idxesOfsigns, signsQuantity);
            }
            if (_cells[i].IsVerticalSign)
            {
                bool g = int.Parse(_cells[i - _rowSize].Value) > int.Parse(_cells[i + _rowSize].Value);
                _cells[i].Value = g ? VerticalGreater : VerticalLesser;
                RemoveElement(ref idxesOfsigns, i);
                return DoSignCells(idxesOfsigns, signsQuantity);
            }

            return false;
        }

        // To generate numeric cells.
        private bool DoNumericCells(int i)
        {
            bool isSuccess = false;
            bool isProceed = false;
            if (i >= _cellsAmount)
            {
                return true;
            }
            Cell cell = GetNumericCell(i);
            int idx = cell.Row*_rowSize + cell.Column;
            if (!string.IsNullOrEmpty(cell.Value))
            {
                return DoNumericCells(idx + 1);
            }
            int[] backup = new int[cell.Candidates.Length];
            Array.Copy(cell.Candidates, backup, backup.Length);

            if (TryCandidates(cell))
            {
                isSuccess = DoNumericCells(idx + 1);
                if (isSuccess)
                {
                    return true;
                }
                ActivateRelatedCandidatesOnRow(cell);
                ActivateRelatedCandidatesOnColumn(cell);
                isProceed = true;
            }

            cell.Candidates = backup;
            cell.Value = null;
            if (isProceed)
            {
                isSuccess = DoNumericCells(idx);
            }

            return isSuccess;
        }

        // To choose a number from candidates array.
        private bool TryCandidates(Cell cell)
        {
            if (!SetCellValue(cell))
            {
                return false;
            }

            RemoveCandidate(cell);

            if (!FreezeRelatedCandidatesOnRow(cell))
            {
                ActivateRelatedCandidatesOnRow(cell);
                return TryCandidates(cell);
            }

            if (!FreezeRelatedCandidatesOnColumn(cell))
            {
                ActivateRelatedCandidatesOnColumn(cell);
                return TryCandidates(cell);
            }

            return true;
        }

        private bool FreezeRelatedCandidatesOnRow(Cell cell)
        {
            return HandleRelatedCells(cell, false, true);
        }

        private bool FreezeRelatedCandidatesOnColumn(Cell cell)
        {
            return HandleRelatedCells(cell, false, false);
        }

        private bool ActivateRelatedCandidatesOnRow(Cell cell)
        {
            return HandleRelatedCells(cell, true, true);
        }

        private bool ActivateRelatedCandidatesOnColumn(Cell cell)
        {
            return HandleRelatedCells(cell, true, false);
        }

        // To activate freezed candidate on related cell.
        private void Activate(Cell cell, int candidate)
        {
            if (cell.FrzRecord[candidate - 1] > 0)
            {
                cell.FrzRecord[candidate - 1]--;
            }
            else
            {
                int[] temp = new int[cell.Candidates.Length + 1];
                Array.Copy(cell.Candidates, temp, cell.Candidates.Length);
                temp[temp.Length - 1] = candidate;
                cell.Candidates = temp;
            }
        }

        private bool Freeze(Cell cell, int candidate)
        {
            int availableCount = cell.Candidates.Length - 1;
            bool isFrz = RemoveCandidate(cell, candidate);

            if (0 == availableCount && string.IsNullOrEmpty(cell.Value))
            {
                isFrz = false;
            }

            if (1 == availableCount && string.IsNullOrEmpty(cell.Value))
            {
                int onlyValue = cell.Candidates[0];
                cell.Value = onlyValue.ToString();
                RemoveCandidate(cell, onlyValue);

                if (!FreezeRelatedCandidatesOnRow(cell))
                {
                    ActivateRelatedCandidatesOnRow(cell);
                    cell.Candidates = new[] {onlyValue};
                    isFrz = false;
                }

                if (!FreezeRelatedCandidatesOnColumn(cell))
                {
                    ActivateRelatedCandidatesOnColumn(cell);
                    cell.Candidates = new[] {onlyValue};
                    isFrz = false;
                }
            }

            return isFrz;
        }

        private bool HandleRelatedCells(Cell cell, bool isActivate, bool isRow)
        {
            bool isSuccess = true;
            int candidate = int.Parse(cell.Value);
            int startIndex = isRow ? cell.Row*_rowSize : cell.Column;
            int endIndex = isRow 
                ? (cell.Row + 1)*_rowSize - 1 
                : _cellsAmount - _rowSize + cell.Column;
            int increase = isRow ? 1 : _rowSize;

            for (; startIndex <= endIndex; startIndex += increase)
            {
                bool doNothing = isRow
                     ? startIndex%_rowSize == cell.Column || !_cells[startIndex].IsNumeric
                     : startIndex/_rowSize == cell.Row || !_cells[startIndex].IsNumeric;
                if (doNothing)
                {
                    //do nothing if this is the current assigned cell OR is not a numeric cell.
                }
                else if (isActivate)
                {
                    // To activate related candidates.
                    Activate(_cells[startIndex], candidate);
                }
                else if (-1 != Array.IndexOf(_cells[startIndex].Candidates, candidate))
                {
                    // To freeze related Candidates if the candidate number exist;
                    isSuccess &= Freeze(_cells[startIndex], candidate);
                }
                else
                {
                    // let refreeze records increase at the candidate position
                    _cells[startIndex].FrzRecord[candidate - 1]++;
                }
            }

            return isSuccess;
        }

        private bool RemoveElement(ref int[] array, int v)
        {
            try
            {
                int i = Array.IndexOf(array, v);
                if (v != array[array.Length - 1])
                {
                    array[i] = array[array.Length - 1];
                }
                int[] temp = new int[array.Length - 1];
                Array.Copy(array, temp, array.Length - 1);
                array = temp;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool RemoveCandidate(Cell cell, int cdt)
        {
            bool isSuccess = true;
            try
            {
                int i = Array.IndexOf(cell.Candidates, cdt);
                int len = cell.Candidates.Length;
                if (cdt != cell.Candidates[len - 1])
                {
                    cell.Candidates[i] = cell.Candidates[len - 1];
                }
                int[] temp = new int[len - 1];
                Array.Copy(cell.Candidates, temp, len - 1);
                cell.Candidates = temp;
            }
            catch (Exception e)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        private bool RemoveCandidate(Cell cell)
        {
            int c = int.Parse(cell.Value);
            return RemoveCandidate(cell, c);
        }

        private bool SetCellValue(Cell cell)
        {
            if (0 == cell.Candidates.Length)
            {
                return false;
            }

            int rdmIndex = _rdm.Next(cell.Candidates.Length);
            cell.Value = cell.Candidates[rdmIndex].ToString();
            return true;
        }

        private Cell GetNumericCell(int index)
        {
            Cell c = GetCell(index);
            return c.IsNumeric ? c : GetNumericCell(++index);
        }

        private Cell GetCell(int index)
        {
            if (index < 0 || index >= _cellsAmount)
            {
                throw new IndexOutOfRangeException("no cell");
            }
            return _cells[index];
        }
    }
}
