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

namespace Models
{
    /// <summary>
    /// The domain model class of a Futoshiki. 
    /// </summary>
    public class Futoshiki
    {

        /// <summary>
        /// A horizontal great-than sign "＞";
        /// </summary>
        public const string HGreater = "\xff1e";

        /// <summary>
        /// A horizontal less-than sign "＜";
        /// </summary>
        public const string HLesser = "\xff1c";

        /// <summary>
        /// A vertical great-than sign "∨";
        /// </summary>
        public const string VGreater = "\x2228";

        /// <summary>
        /// A vertical less-than sign "∧";
        /// </summary>
        public const string VLesser = "\x2227";

        /// <summary>
        /// The constant is a default size of the Futoshiki. It will be used when 
        /// the seed number not provided for the initialization
        /// </summary>
        public const int DftSize = 5;
        
        /// <summary>
        ///The constant is a minimum size of the Futoshiki.
        /// </summary>
        private const int MinSize = 4;

        /// <summary>
        /// The value of a given size for generate a Futoshiki grid. The amount 
        /// of actual numerical cells of a row or column will obey this value.
        /// </summary>
        private readonly int _scalarSize;
        public int Scalar { get { return _scalarSize; } }

        /// <summary>
        /// The real size of a row, whick includes numeric cells and sign cells.
        /// </summary>
        public int Size { get { return 2*Scalar - 1; } }

        /// <summary>
        /// Total cells.
        /// </summary>
        public int Length { get { return Size*Size; } }

        /// <summary>
        /// The amount of blank cells of a futoshi.
        /// </summary>
        public int Blanks { get { return (Scalar - 1)*(Scalar - 1); } }

        /// <summary>
        /// The amount of numeric cells of a futoshi.
        /// </summary>
        public int Nums { get { return Scalar*Scalar; } }

        /// <summary>
        /// The amount of sign cells of a futoshi.
        /// </summary>
        public int Signs { get { return Length - Nums - Blanks; }}

        /// <summary>
        /// All cells of futoshiki. 
        /// </summary>
        private readonly Cell[] _cells;
        public Cell[] Cells { get { return _cells; } }

        /// <summary>
        /// A indexer can get a cell of Futoshiki. 
        /// </summary>
        public Cell this[int i] { get
        {
            if (i < 0 || i>= Length)
            {
                throw new IndexOutOfRangeException("no cell");
            }
            return _cells[i];
        } }

        /// <summary>
        /// The default constructor which only can have a standard (5*5) game 
        /// of Futoshiki.
        /// </summary>
        public Futoshiki(): this(DftSize) {}

        /// <summary>
        /// The constructor can create a futoshiki of any size by given seed number.
        /// </summary>
        /// <param name="size">The size of futoshiki</param>
        public Futoshiki(int size)
        {
            _scalarSize = size < MinSize ? DftSize : size;
            _cells = new Cell[Length];

            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i] = new Cell {Row = i/Size, Column = i%Size};
                if (_cells[i].IsNumeric)
                {
                    _cells[i].Candidates = new int[_scalarSize];
                    _cells[i].FrzRecord = new int[_scalarSize];
                    for (int n = 1; n <= _scalarSize; n++)
                    {
                        _cells[i].Candidates[n - 1] = n;
                    }
                }
                else
                {
                    _cells[i].Candidates = null;
                }
            }
        }
    }
}
