using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{
    class KeyboardGridLocation
    {
        public int Column { get; set; } = 0;

        public int Row { get; set; } = 0;

        public KeyboardGridLocation(int column, int row)
        {
            Column = column;
            Row = row;
        }


        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                KeyboardGridLocation p = (KeyboardGridLocation)obj;
                return (Column == p.Column) && (Row == p.Row);
            }
        }

        public override int GetHashCode()
        {
            return (Column << 2) ^ Row;
        }

        public override string ToString()
        {
            return String.Format("Column ({0}, Row {1})", Column, Row);
        }
    }
}
