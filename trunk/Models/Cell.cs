/**
 * $Id$
 *
 * Coursework – The domian model layer of the Futoshiki puzzle.
 *
 * This file is the result of my own work. Any contributions to the work by third parties,
 * other than tutors, are stated clearly below this declaration. Should this statement prove to
 * be untrue I recognise the right and duty of the Board of Examiners to take appropriate
 * action in line with the university's regulations on assessment. 
 */

using System.Text;

namespace Models
{
    /// <summary>
    /// The domain model class of a Cell. 
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// The column number of a Cell.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// The Row number of a Cell
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// The Value of a Cell
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The candidates list of a Cell
        /// </summary>
        public int[] Candidates { get; set; }

        /// <summary>
        /// The candidates freeezing records of a cell
        /// </summary>
        public int[] FrzRecord { get; set; }

        /// <summary>
        /// To indicate this is a numeric cell or sign cell
        /// </summary>
        public bool IsNumeric
        {
            get { return 0 == Row%2 && 0 == Column%2; }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder("{");
            str.AppendFormat("\"IsNumeric\":{0}",IsNumeric)
                .AppendFormat(",\"Row\":{0}", Row)
                .AppendFormat(",\"Column\":{0}", Column)
                .AppendFormat(",\"Value\":\"{0}\"", Value)
                .Append(",\"Candidates\":[");
            PrintArray(Candidates, str);
            str.AppendFormat("],\"FrzRecord\":[");
            PrintArray(FrzRecord,str);
            str.Append("]}");

            return str.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Cell)) return false;
            return Equals((Cell) obj);
        }

        public bool Equals(Cell other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Column == Column && other.Row == Row;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Column*397) ^ Row;
            }
        }

        private void PrintArray(int[] array, StringBuilder str)
        {
            if (null == array || 0 == array.Length)
            {
                str.AppendFormat("{0}", "null");
                return;
            }

            foreach (int a in array)
            {
                str.AppendFormat("{0},", a);
            }

            str.Remove(str.Length-1, 1);
        }
    }
}
