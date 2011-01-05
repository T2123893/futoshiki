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
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using BLL;

namespace WebUI
{
    /// <summary>
    /// The home page class of the Futoshiki puzzle site.
    /// </summary>
    public partial class Default : Page
    {
        private int size = 5;
        private ICellService _cellService;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            string s = Request["size"];
            size = !string.IsNullOrEmpty(s) ? int.Parse(s): size;
            _cellService = new CellService(size);
            ShowFutoshiki();
            
        }

        private void ShowFutoshiki()
        {
            Cell[] cells = _cellService.DoGrid();  
            int rowSize = (int) Math.Sqrt(cells.Length);
            double n = 1.00/rowSize;
            
            for (int r = 0; r < rowSize; r++)
            {
                TableRow tr = new TableRow();

                for (int c = 0; c < rowSize; c++)
                {
                    TableCell td = new TableCell {ID = r + "_" + c};
                    Unit u = new Unit(n.ToString("P"));
                    td.Height = td.Width = u;                              
                    td.Text = cells[r*rowSize+c].Value;
                    td.Attributes.Add("row", r.ToString());
                    td.Attributes.Add("col", c.ToString());
                    


                    // 如果不是数字格，设置符号格样式);););
                    if (cells[r*rowSize+c].IsNumeric)
                    {
                        td.Attributes.Add("onClick", "onCellClick(this)");                        
                        td.Attributes.Add("isNum", "true");
                        td.CssClass = "number";                        
                    }
                    else if (cells[r*rowSize+c].IsHorizontalSign)
                    {
                        td.Attributes.Add("isHS", "true");
                    }
                    else if (cells[r*rowSize+c].IsVerticalSign)
                    {
                        td.Attributes.Add("isVS", "true");
                    }
                    tr.Cells.Add(td);
                }
                FutoshikiTable.Rows.Add(tr);
            }
            
        }

    }
}
