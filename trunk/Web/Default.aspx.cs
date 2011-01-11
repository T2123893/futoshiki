/*
 * $Id$
 * 
 * Coursework – Futoshiki.Web
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 */


using System;
using System.Diagnostics;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Models;
using Models.Tools;

namespace Web
{
    public partial class Default : Page 
    {
        private int size = 5;
        private IService _gridServ;
        private string _uId;

        protected void Page_Load(object sender, EventArgs e)
        {
            // initialize a Futoshiki service
            string scale = Request["ScaleList"];
            size = string.IsNullOrEmpty(scale) ? size : int.Parse(scale);
            _gridServ = new FutoshkService(size);

            // get current user
            string u = User.Identity.Name;
            MembershipUser user = Membership.GetUser(u);

            MsgDiv.Visible = false;

            if (string.IsNullOrEmpty(u) || null == user)
            {
                SpanLogout.Visible = false;
                BtnSave.Attributes.Add("disabled","true");
                BtnSave.Style["cursor"] = "default";
                ScaleList.Attributes.Add("disabled","true");
            }
            else 
            {
                if (user.ProviderUserKey != null)
                {
                    _uId = user.ProviderUserKey.ToString();
                }
                SpanLogin.Visible = false;
                LblUsername.Text = string.Format(
                    "{0}, Last login date is {1}", u, user.LastLoginDate);
            }

            if (!IsPostBack)
            {
                ScaleList.SelectedValue = size.ToString();
                Futoshiki f = _gridServ.GetGrid(_uId) ?? _gridServ.GetNewGrid(_uId);
                ShowFutoshiki(f);
            }
        }

        private void ShowFutoshiki(Futoshiki f)
        {
            double n = 1.00 / f.Size;
            FutoshikiTable.Attributes.Add("status", f.Status.ToString());
            FutoshikiTable.Attributes.Add("fid", f.Id);
            for (int r = 0; r < f.Size; r++)
            {
                TableRow tr = new TableRow();

                for (int c = 0; c < f.Size; c++)
                {
                    TableCell td = new TableCell { ID = r + "_" + c };
                    Unit u = new Unit(n.ToString("P"));
                    td.Height = td.Width = u;
                    int ind = r * f.Size + c;
                    td.Text = f[ind].Val;
                    td.Attributes.Add("row", r.ToString());
                    td.Attributes.Add("col", c.ToString());

                    if (f[ind].IsNum)
                    {
                        td.Attributes.Add("onclick", "onCellClick(this)");
                        td.Attributes.Add("isnum", "true");
                        td.CssClass = "number";
                        if (f[ind].IsWritable)
                        {
                            td.Attributes.Add("iswritable", "true");
                        }
                    }
                    else if (f[ind].IsHorizontalSign)
                    {
                        td.Attributes.Add("ishs", "true");
                    }
                    else if (f[ind].IsVerticalSign)
                    {
                        td.Attributes.Add("isvs", "true");
                    }
                    tr.Cells.Add(td);
                }
                FutoshikiTable.Rows.Add(tr);
            }

        }

        protected void LinkLogoutClick(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();        
        }

        protected void BtnSaveClick(object sender, EventArgs e)
        {
            bool canSave = true;
            Futoshiki f = XmlHelper.Deserialize<Futoshiki>(HidData.Value);
            f.Uid = _uId;
            Debug.WriteLine(GetType() + " - " + f);   
  
            /* 
             * if f.status is set by -1, this actually is not a valid futoshiki 
             * status code, just use this number to indicate that all cells are 
             * finished but need check with solution yet.
             */
            if (f.Status == -1)
            {
                // begin check with solution, let's set f.Status back to ongoing
                f.Status = (int) Futoshiki.Mode.Ongoing;
                int count = _gridServ.CheckSolution(f);
                if (count > 0)
                {
                    canSave = false;
                }
            }

            if (canSave && _gridServ.Save(f))
            {
                Msg("The game was successfully saved.");
            }
            else if (!canSave)
            {
                Msg("Your solution is not correct, please try harder!");
            }
            else {
                Msg("Sorry, unsuccessful save, please try later.");
            }
            ClientShow(f);
        }

        protected void BtnChkSlnClick(object sender, EventArgs e)
        {
            Futoshiki f = XmlHelper.Deserialize<Futoshiki>(HidData.Value);
            f.Uid = _uId;
            int count = _gridServ.CheckSolution(f);
            if (1 == count)
            {
                Msg("There is 1 incorrect cell.");
            }
            else if (count > 1)
            {
                Msg("There are " + count + " incorrect cells");
            } else if (count < 0)
            {
                Msg("Cannot get data, please contact webmaster.");
            }
            else
            {
                Msg("Congratulations! You completely right!");
            }
            ClientShow(f);
        }

        protected void BtnShowSolution(object sender, EventArgs e)
        {
            string fid = HidData.Value;
            Futoshiki f = _gridServ.GetSolution(fid);
            ShowFutoshiki(f);
        }

        private void Msg(string msg)
        {
            MsgDiv.InnerText = msg;
        }

        private void ClientShow(Futoshiki f)
        {
            MsgDiv.Visible = true;
            //MsgDiv.Style["display"] = "block";
            MsgDiv.Attributes.Add("onclick", "hidMsg()");
            ShowFutoshiki(f);
        }

        protected void ScaleListSelectedIndexChanged(object sender, EventArgs e)
        {
            Futoshiki f = _gridServ.GetGrid(_uId) ?? _gridServ.GetNewGrid(_uId);
            ShowFutoshiki(f);
        }
    }
}
