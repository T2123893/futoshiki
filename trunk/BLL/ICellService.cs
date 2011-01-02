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


using Models;

namespace BLL
{
    /// <summary>
    /// This interface is going to provide a grid for the puzzle.
    /// </summary>
    public interface ICellService
    {
        /// <summary>
        /// Generate a grid for a puzzle game.
        /// </summary>
        /// <returns>A Cell collection</returns>
        Cell[] DoGrid();
    }
}
