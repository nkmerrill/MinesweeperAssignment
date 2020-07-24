using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    /// <summary>
    /// Enumeration for a Cell's current state.
    /// </summary>
    public enum CellState
    {
        Hidden=0,
        One=1,
        Two=2,
        Three=3,
        Four=4,
        Five=5,
        Six=6,
        Seven=7,
        Eight=8,
        Bomb=9,
        Blank = 10
    }

}
