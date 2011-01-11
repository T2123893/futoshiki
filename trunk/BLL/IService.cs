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


using Models;

namespace BLL
{
    /// <summary>
    /// This interface provide some operation for any puzzel game that use grid
    /// or cell to produce.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Generate a new grid for a puzzle game.
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>a futoshiki game</returns>
        Futoshiki GetNewGrid(string userId);

        /// <summary>
        /// Get a grid for a puzzel game.
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>a puzzel</returns>
        Futoshiki GetGrid(string userId);

        /// <summary>
        /// Get a solution for a puzzel game.
        /// </summary>
        /// <param name="puzzleId">A puzzle game id</param>
        /// <returns>A puzzel</returns>
        Futoshiki GetSolution(string puzzleId);

        /// <summary>
        /// Save a game
        /// </summary>
        /// <param name="f">a puzzel game</param>
        /// <returns>bool</returns>
        bool Save(Futoshiki f);

        /// <summary>
        /// Check a solution
        /// </summary>
        /// <param name="f">A futoshiki</param>
        /// <returns>int - Incorrect count</returns>
        int CheckSolution(Futoshiki f);


    }
}