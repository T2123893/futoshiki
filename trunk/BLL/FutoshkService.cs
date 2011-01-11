/**
 * $Id$
 *
 * Coursework – Futoshiki.BLL
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 */

using System;
using System.Diagnostics;
using System.IO;
using DAL;
using Models;
using Models.Tools;

namespace BLL
{
    /// <summary>
    /// A class service for the Futoshiki game, which encapsulated an algorithm 
    /// to automatically generate Futoshiki cells according to given seed number.
    /// The game can be any size from 4*4 upwards.
    /// </summary>
    public class FutoshkService : IService
    {
        private IDao _dao;

        private readonly Futoshiki _f;

        /// <summary>
        /// A random number generator.
        /// </summary>
        private readonly Random _rdm;


        /// <summary>
        /// The default constructor which only can have a standard (5*5) game 
        /// of Futoshiki.
        /// </summary>
        public FutoshkService() : this(Futoshiki.DftSize) {}

        /// <summary>
        /// The constructor can create a grid of any size by given seed number.
        /// </summary>
        /// <param name="size"></param>
        public FutoshkService(int size)
        {
            _f = new Futoshiki(size);
            _rdm = new Random();
            _dao = new FutoshikiDao();
        }

        /// <summary>
        /// Get a futoshiki solution. now all solution is located file system.
        /// </summary>
        /// <param name="fid">futoshiki id</param>
        /// <returns>futoshiki object</returns>
        public Futoshiki GetSolution(string fid)
        {
            Futoshiki f = null;
            try
            {
                f = XmlHelper.Deserialize<Futoshiki>(fid, XmlHelper.S);
            }
            catch (FileNotFoundException)
            {
                try
                {
                    f = XmlHelper.Deserialize<Futoshiki>(fid, XmlHelper.S, 
                        XmlHelper.T);
                }
                catch(FileNotFoundException fnfex)
                {
                    Debug.WriteLine(GetType() + " - DID NOT FOUND SOLUTION ");
                    Debug.WriteLine(fnfex);
                }
            }

            return f;
        }

        /// <summary>
        /// <li>The new game is first time saving, change status and let it to 
        /// DB.</li><li>The ongoing game is serialize to a xml that named with 
        /// user id.It does not need to connect DB every time. But the ongoing 
        /// game may only be one for a free account.</li><li>The completed game 
        /// is saving to DB.</li>
        /// </summary>
        /// <param name="f">A futoshiki object</param>
        /// <returns>bool</returns>
        public bool Save(Futoshiki f)
        {
            bool isSaved = false;
            int affectedRows;
            switch (f.Status)
            {
                case (int)Futoshiki.Mode.New:
                    f.Status = (int)Futoshiki.Mode.Ongoing;
                    affectedRows = _dao.Create(f);
                    isSaved = affectedRows == f.Length ? true : false;
                    break;
                case (int)Futoshiki.Mode.Ongoing:
                    // serialize game for current user. 
                    isSaved = XmlHelper.Serialize(f, f.Uid, XmlHelper.U, 
                        f.Scale.ToString());
                    MoveSolution(f.Id);
                    break;
                case (int) Futoshiki.Mode.Completed:
                    affectedRows = _dao.Update(f);
                    isSaved = -1 < affectedRows ? true : false;                    
                    if (isSaved)
                    {   // delete cached futoshiki
                        XmlHelper.Del(f.Uid, XmlHelper.U, f.Scale.ToString());
                    }
                    break;
                default:
                    break;
            }
            return isSaved;
        }

        private void MoveSolution(string fid)
        {
            try
            {
                Futoshiki tempSolu = XmlHelper.Deserialize<Futoshiki>(fid, 
                    XmlHelper.S, XmlHelper.T);
                XmlHelper.Serialize(tempSolu, tempSolu.Id, XmlHelper.S);
                XmlHelper.Del(tempSolu.Id, XmlHelper.S, XmlHelper.T);
            } 
            catch (FileNotFoundException fnfe)
            {
                Debug.WriteLine(GetType() 
                    + " - cannot find solution in temp folder" + fnfe);
            }

        }

        /// <summary>
        /// The folder named solution and temp both will be checked. 
        /// After thant, if user logged and solution file in the temp folder, 
        /// then move it out.
        /// </summary>
        /// <param name="f">a futoshiki object</param>
        /// <returns>incorrect count</returns>
        public int CheckSolution(Futoshiki f)
        {
            int count = 0;
            Futoshiki solution = GetSolution(f.Id);
            bool isOk = null != solution;

            if (!string.IsNullOrEmpty(f.Uid) && isOk)
            {
                MoveSolution(f.Id);
            }

            for (int i = 0; isOk && i < f.Length; i++)
            {
                if (f[i].IsNum && !solution[i].Val.Equals(f[i].Val))
                {
                    count++;
                }
            }

            return isOk ? count : -1;
        }

        /// <summary>
        /// Get a futoshiki grid. Every time user logged in, firstly checks the 
        /// local xml file that named by user id. 
        /// If there is no saved XML for current user then select DB. 
        /// If got result from DB, then serialize to XML.
        /// </summary>
        /// <param name="userId">The id of current user who logged in</param>
        /// <returns>A futoshiki instance</returns>
        public Futoshiki GetGrid(string userId)
        {
            Futoshiki ready = null;
            if (null == userId) { return ready; }
            
            try 
            {
                ready = XmlHelper.Deserialize<Futoshiki>(userId, XmlHelper.U, 
                    _f.Scale.ToString());
            }
            catch (Exception)
            {
                ready = _dao.Read(userId, _f.Scale.ToString()) as Futoshiki;
                if (null != ready)
                {
                    ready.Status = (int) Futoshiki.Mode.Ongoing;
                    XmlHelper.Serialize(
                        ready, ready.Uid, XmlHelper.U, _f.Scale.ToString());
                }
            }

            return ready;
        }

        /// <summary>
        /// Generate a Futoshiki Cells according to given size of initilization
        /// stage. whick will fill numeric cells up.
        /// </summary>
        /// <returns>A Cell collection</returns>
        public Futoshiki GetNewGrid(string userId)
        {
            bool isOk = DoNumericCells(0) && DoSignCells();

            if (isOk && !string.IsNullOrEmpty(userId))
            {
                _f.Uid = userId;
                Save(_f);
                return _f;
            }
            if (isOk && string.IsNullOrEmpty(userId))
            {
                return _f;
            }
            return null;
        }

        // Generating inequality symbol randomly.
        private bool DoSignCells()
        {
            var idxesNumCells = new int[_f.Nums];
            var idxesOfSigns = new int[_f.Signs];
            for (int i = 0, s = 0, n = 0; i < _f.Cells.Length; i++)
            {
                if (_f[i].IsHorizontalSign || _f[i].IsVerticalSign)
                {
                    // get ready an array that contains all indexes of sign cells      
                    idxesOfSigns[s] = i;
                    ++s;
                }
                else if (_f[i].IsNum)
                {
                    // get ready an array that contains all indexes of numeric cells.
                    idxesNumCells[n] = i;
                    ++n;
                }
            }

            bool isSuccess = true;

            // get ready a quantity of inequality symbols
            int sMin = _f.Scale - 1;
            int sMax = _f.Scale*_f.Scale/2;
            int signsQuantity = _rdm.Next(sMin, sMax);
            isSuccess &= DoSignCells(idxesOfSigns, signsQuantity);

            // To cache the futoshiki grid as a solution to xml string before 
            // veiling some numeric cells. This is a raw solution, after veil
            // numeric cells, the property IsWritable need to update.
            string rawSolution = XmlHelper.Serialize(_f);

            // get ready a quantity of numeric number
            int numsQuantity = _rdm.Next(sMax - signsQuantity);
            int veiled = _f.Nums - numsQuantity;
            isSuccess &= VeilNumCells(idxesNumCells, veiled);

            UpdateRawSolution(isSuccess, rawSolution);           

            return isSuccess;
        }

        private void UpdateRawSolution(bool isSuccess, string rawSolution)
        {
            if (!isSuccess)
            {
                return;
            }

            // update solution cache and serialize to xml file.   
            Futoshiki raw = XmlHelper.Deserialize<Futoshiki>(rawSolution);
            for (int i = 0; i < _f.Length; i++)
            {
                if (_f[i].IsNum && raw[i].IsNum)
                {
                    raw[i].IsWritable = _f[i].IsWritable;
                }
            }
            XmlHelper.Serialize(raw, raw.Id, XmlHelper.S, XmlHelper.T);
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
            _f[idx].Val = null;
            _f[idx].IsWritable = true;
            RemoveElement(
                ref idxesNumCells, idxesNumCells[idxesNumCells.Length - 1]);

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
            if (_f[i].IsHorizontalSign)
            {
                bool g = int.Parse(_f[i - 1].Val) > int.Parse(_f[i + 1].Val);
                _f[i].Val = g ? Futoshiki.HG : Futoshiki.HL;
                RemoveElement(ref idxesOfsigns, i);
                return DoSignCells(idxesOfsigns, signsQuantity);
            }
            if (_f[i].IsVerticalSign)
            {
                bool g = int.Parse(_f[i - _f.Size].Val) > int.Parse(_f[i + _f.Size].Val);
                _f[i].Val = g ? Futoshiki.VG : Futoshiki.VL;
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
            if (i >= _f.Length)
            {
                return true;
            }
            Cell cell = GetNumericCell(i);
            int idx = cell.Row*_f.Size + cell.Col;
            if (!string.IsNullOrEmpty(cell.Val))
            {
                return DoNumericCells(idx + 1);
            }
            var backup = new int[cell.Candidates.Length];
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
            cell.Val = null;
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
            if (cell.Repeat[candidate - 1] > 0)
            {
                cell.Repeat[candidate - 1]--;
            }
            else
            {
                var temp = new int[cell.Candidates.Length + 1];
                Array.Copy(cell.Candidates, temp, cell.Candidates.Length);
                temp[temp.Length - 1] = candidate;
                cell.Candidates = temp;
            }
        }

        private bool Freeze(Cell cell, int candidate)
        {
            int availableCount = cell.Candidates.Length - 1;
            bool isFrz = RemoveCandidate(cell, candidate);

            if (0 == availableCount && string.IsNullOrEmpty(cell.Val))
            {
                isFrz = false;
            }

            if (1 == availableCount && string.IsNullOrEmpty(cell.Val))
            {
                int onlyValue = cell.Candidates[0];
                cell.Val = onlyValue.ToString();
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
            int candidate = int.Parse(cell.Val);
            int startIndex = isRow ? cell.Row*_f.Size : cell.Col;
            int endIndex = isRow
                               ? (cell.Row + 1)*_f.Size - 1
                               : _f.Length - _f.Size + cell.Col;
            int increase = isRow ? 1 : _f.Size;

            for (; startIndex <= endIndex; startIndex += increase)
            {
                bool doNothing = isRow
                     ? startIndex%_f.Size == cell.Col || !_f[startIndex].IsNum
                     : startIndex/_f.Size == cell.Row || !_f[startIndex].IsNum;
                if (doNothing)
                {
                    /* do nothing if this is the current assigned cell OR is 
                       not a numeric cell. */
                }
                else if (isActivate)
                {   // To activate related candidates.
                    Activate(_f[startIndex], candidate);
                }
                else if (-1 != Array.IndexOf(_f[startIndex].Candidates, candidate))
                {   // To freeze related Candidates if the candidate number exist;
                    isSuccess &= Freeze(_f[startIndex], candidate);
                }
                else
                {   // let refreeze records increase at the candidate position
                    _f[startIndex].Repeat[candidate - 1]++;
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
                var temp = new int[array.Length - 1];
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
                var temp = new int[len - 1];
                Array.Copy(cell.Candidates, temp, len - 1);
                cell.Candidates = temp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                isSuccess = false;
            }
            return isSuccess;
        }

        private bool RemoveCandidate(Cell cell)
        {
            int c = int.Parse(cell.Val);
            return RemoveCandidate(cell, c);
        }

        private bool SetCellValue(Cell cell)
        {
            if (0 == cell.Candidates.Length)
            {
                return false;
            }

            int rdmIndex = _rdm.Next(cell.Candidates.Length);
            cell.Val = cell.Candidates[rdmIndex].ToString();
            return true;
        }

        private Cell GetNumericCell(int index)
        {
            Cell c = _f[index];
            return c.IsNum ? c : GetNumericCell(++index);
        }
    }
}