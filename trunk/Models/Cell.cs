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

using System;
using System.Text;

namespace Models
{
    /// <summary>
    /// The domain model class of a Cell. 
    /// </summary>
    public class Cell
    {
//        private Cell[] _cells;

        public Cell() {}

//        public Cell(int size)
//        {
//            Length = size*size - 1;
//            _cells = new Cell[Length];
//            Candidates = new int[size];
//            for(int n = 1; n <= size; n++)
//            {
//                Candidates[n - 1] = n;
//            }
//            for (int index = 0; index < Length; index++)
//            {
//                this[index].Row = index / size;
//                this[index].Column = index % size;
//            }
//        }

//        public Cell this[int index]
//        {
//            get
//            {
//                if (index < 0 || index > _cells.Length)
//                {
//                    throw new IndexOutOfRangeException();
//                }
//                return _cells[index];
//            }
//            set
//            {
//                if (index < 0 || index > _cells.Length)
//                {
//                    throw new IndexOutOfRangeException();
//                }
//                _cells[index] = value;
//            }
//        }

//        public int Length { get; private set; }

        //public int Index { get; set; }

        public int Column { get; set; }

        public int Row { get; set; }

        public string Value { get; set; }

        public int[] Candidates { get; set; }

        public bool IsNumeric
        {
            get { return 0 == Row%2 && 0 == Column%2; }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder("{");
            str.AppendFormat("\"Column\":{0}", Column)
                .AppendFormat(",\"Row\":{0}", Row)
                .AppendFormat(",\"Value\":\"{0}\"", Value)
                .Append(",\"Candidates\":[");
            PrintArray(Candidates, str);
            str.Append("]}").ToString();

            return str.ToString();
//            
//                    "{\"Column\":") + Column + 
//                   ",\"Row\":" + Row + 
//                   ",\"Value\":\"" + Value + 
//                   "\",\"Candidates\":" + PrintArray(Candidates) +
//                   "}";
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
            if (null == array)
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
