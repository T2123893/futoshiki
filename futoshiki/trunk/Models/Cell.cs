/**
 * $Id$
 * 
 * Coursework – Futoshiki.Models
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 */

using System.Text;
using System.Xml.Serialization;

namespace Models
{
    /// <summary>
    /// The domain model class of a Cell. 
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// The Row number of a Cell
        /// </summary>
        [XmlAttribute]
        public int Row { get; set; }

        /// <summary>
        /// The column number of a Cell.
        /// </summary>
        [XmlAttribute]
        public int Col { get; set; }
        
        /// <summary>
        /// To indicate the numeric cell cannot be set by client.
        /// </summary>
        [XmlAttribute]
        public bool IsWritable { get; set; }

        /// <summary>
        /// The Value of a Cell
        /// </summary>
        [XmlText]
        public string Val { get; set; }

        /// <summary>
        /// The candidates list of a Cell
        /// </summary>
        [XmlIgnore]
        public int[] Candidates { get; set; }

        /// <summary>
        /// To record which candidates were removed repeatedly.
        /// </summary>
        [XmlIgnore]
        public int[] Repeat { get; set; }

        /// <summary>
        /// To indicate this is a numeric cell
        /// </summary>
        public bool IsNum
        {
            get { return 0 == Row%2 && 0 == Col%2; }
        }

        /// <summary>
        /// To indicate this is a sign cell on vertical.
        /// </summary>
        public bool IsVerticalSign
        {
            get { return 0 != Row%2 && 0 == Col%2; }
        }

        /// <summary>
        /// To indicate this is a sign cell on horizontal.
        /// </summary>
        public bool IsHorizontalSign
        {
            get { return 0 == Row%2 && 0 != Col%2; }
        }

        public override string ToString()
        {
            return new StringBuilder("{")
                .AppendFormat("\"Row\":{0}", Row)
                .AppendFormat(",\"Col\":{0}", Col)
                .AppendFormat(",\"Val\":\"{0}\"", Val)
                .AppendFormat(",\"IsWritable\":{0}", IsWritable)
                .Append("}").ToString();
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
            return other.Col == Col && other.Row == Row;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Col*397) ^ Row;
            }
        }

    }
}
