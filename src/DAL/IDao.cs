/*
 * $Id$
 * 
 * Coursework – Futoshiki.DAL
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 */

namespace DAL
{
    /// <summary>
    /// This interface provide interface of data persistence operation for 
    /// business logic layer.
    /// </summary>
    public interface IDao
    {
        /// <summary>
        /// Create an object to database.
        /// </summary>
        /// <param name="o">object</param>
        /// <returns>int - affected rows</returns>
        int Create(object o);

        /// <summary>
        /// Read an object from database.
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="scale">scale</param>
        /// <returns>object</returns>
        object Read(string userId, string scale);

        /// <summary>
        /// Update an object
        /// </summary>
        /// <param name="o">an object</param>
        /// <returns>int - affected rows</returns>
        int Update(object o);

        /// <summary>
        /// Delete an object
        /// </summary>
        /// <param name="o">an object</param>
        /// <returns>int - affected rows</returns>
        int Delete(object o);

    }
}
