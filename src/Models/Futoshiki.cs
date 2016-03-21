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

using System;
using System.Text;
using System.Xml.Serialization;

namespace Models
{
    /// <summary>
    /// The domain model class of a Futoshiki. 
    /// </summary>
    [XmlRootAttribute]
    public class Futoshiki
    {      
        [XmlAttribute]
        public string Id { set; get; }

        /// <summary>
        /// The status of a Futoshiki
        /// </summary>
        [XmlAttribute]
        public int Status { set; get; }

        /// <summary>
        /// The value of a given size for generate a Futoshiki grid. The amount 
        /// of actual numerical cells of a row or column will obey this value.
        /// </summary>
        [XmlAttribute]
        public int Scale { get { return _scale; } set { _scale = value; } }
        private int _scale;

        /// <summary>
        /// All cells of futoshiki. 
        /// </summary>
        [XmlArrayAttribute]
        public Cell[] Cells { get { return _cells; } set { _cells = value; } }
        private Cell[] _cells;

        /// <summary>
        /// To indicate a user
        /// </summary> 
        [XmlIgnore]       
        public string Uid { set; get; }

        /// <summary>
        /// Three Modes of futoshiki game. It will be used to indicate which 
        /// status the futoshiki is.
        /// </summary>
        public enum Mode { New, Ongoing, Completed }

        /// <summary>
        /// A horizontal great-than sign "＞";
        /// </summary>
        public const string HG = "\xff1e";

        /// <summary>
        /// A horizontal less-than sign "＜";
        /// </summary>
        public const string HL = "\xff1c";

        /// <summary>
        /// A vertical great-than sign "∨";
        /// </summary>
        public const string VG = "\x2228";

        /// <summary>
        /// A vertical less-than sign "∧";
        /// </summary>
        public const string VL = "\x2227";

        /// <summary>
        /// The constant is a default size of the Futoshiki. It will be used when 
        /// the seed number not provided for the initialization
        /// </summary>
        public const int DftSize = 5;

        /// <summary>
        /// The real size of a row, whick includes numeric cells and sign cells.
        /// </summary>
        public int Size { get { return 2 * Scale - 1; } }

        /// <summary>
        /// Total cells.
        /// </summary>
        public int Length { get { return Size * Size; } }

        /// <summary>
        /// The amount of blank cells of a futoshi.
        /// </summary>
        public int Blanks { get { return (Scale - 1) * (Scale - 1); } }

        /// <summary>
        /// The amount of numeric cells of a futoshi.
        /// </summary>
        public int Nums { get { return Scale * Scale; } }

        /// <summary>
        /// The amount of sign cells of a futoshi.
        /// </summary>
        public int Signs { get { return Length - Nums - Blanks; } }

        /// <summary>
        ///The constant is a minimum size of the Futoshiki.
        /// </summary>
        private const int MinSize = 4;    

        /// <summary>
        /// The default constructor which only can have a standard (5*5) game 
        /// of Futoshiki.
        /// </summary>
        public Futoshiki(): this(DftSize) {}

        /// <summary>
        /// To construct a futoshiki of any size by given seed number.
        /// </summary>
        /// <param name="scale">seed number</param>
        public Futoshiki(int scale)
        {
            _scale = scale < MinSize ? DftSize : scale;
            _cells = new Cell[Length];

            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i] = new Cell {Row = i/Size, Col = i%Size};
                if (_cells[i].IsNum)
                {
                    _cells[i].Candidates = new int[_scale];
                    _cells[i].Repeat = new int[_scale];
                    for (int n = 1; n <= _scale; n++)
                    {
                        _cells[i].Candidates[n - 1] = n;
                    }
                }
                else
                {
                    _cells[i].Candidates = null;
                }
            }
            Status = (int) Mode.New;
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// A indexer can get a cell of Futoshiki. 
        /// </summary>
        public Cell this[int i]
        {
            get
            {
                if (i < 0 || i >= Length)
                {
                    throw new IndexOutOfRangeException("Cell out of range");
                }
                return _cells[i];
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder("{");
            str.AppendFormat("\"Id\":{0}", Id)
                .AppendFormat(",\"Scale\":{0}", Scale)
                .AppendFormat(",\"Status\":{0}", Status)
                .Append(",\"Cells\":[");
            PrintCells(str);
            str.Append("]}");
            return str.ToString();

        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Futoshiki)) return false;
            return Equals((Futoshiki) obj);
        }

        public bool Equals(Futoshiki other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._cells, _cells) && other._scale == _scale 
                && Equals(other.Id, Id);
        }

        private void PrintCells(StringBuilder s)
        {
            foreach(Cell c in Cells)
            {
                s.Append(c.ToString()).Append(",");
            }
            s.Remove(s.Length - 1, 1);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (_cells != null ? _cells.GetHashCode() : 0);
                result = (result*397) ^ _scale;
                result = (result*397) ^ (Id != null ? Id.GetHashCode() : 0);
                return result;
            }
        }
    }
}
