/**
 * $Id$
 *
 * Coursework – The presentation layer of the Futoshiki puzzle.
 * 
 * This file is the result of my own work. Any contributions to the work by third parties,
 * other than tutors, are stated clearly below this declaration. Should this statement prove to
 * be untrue I recognise the right and duty of the Board of Examiners to take appropriate
 * action in line with the university's regulations on assessment. 
 */

using System;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebUI
{
    /// <summary>
    /// The home page class of the Futoshiki puzzle site.
    /// </summary>
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DisplayFutoshikiTable(5);
        }

        public void DisplayFutoshikiTable(int radix)
        {
            for (int row = 2 * radix - 1; row > 0; row--)
            {
                TableRow tr = new TableRow();
                
                for (int column = 2 * radix - 1; column > 0; column--)
                {
                    TableCell td = new TableCell();
                    td.ID = "td_" + row + "_" + column;
                    if (row %2 != 0 && column % 2 != 0)
                    {
                        td.Text = column.ToString();
                    }
                    else
                    {
                        td.CssClass = "sign";
                    }
                    
                    tr.Cells.Add(td);

                }

                FutoshikiTable.Rows.Add(tr);
            }

        }

    }
}
