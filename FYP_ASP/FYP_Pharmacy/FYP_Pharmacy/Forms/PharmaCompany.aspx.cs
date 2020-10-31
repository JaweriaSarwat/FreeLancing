﻿using BLL.PharmaCompany;
using Generics;
using Models.PharmaCompany;
using System;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;

namespace FYP_Pharmacy.Forms
{
    public partial class PharmaCompany : PageActionHandler
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region logging
            PageName = "PharmaCompany";
            CONTEXT = "PharmaCompany";
            method = MethodBase.GetCurrentMethod();
            MessageCollection.addMessage(new Message()
            {
                Context = CONTEXT,
                ErrorCode = 0,
                isError = false,
                WebPage = PageName,
                LogType = Generics.Enums.LogType.Functional,
                Function = method.Name
            });
            #endregion

            if (!IsPostBack)
            {
                if (Session == null || Session[Generics.Enums.SessionName.UserDetails.ToString()] == null)
                {
                    DataTable dt = (DataTable)Session[Generics.Enums.SessionName.UserDetails.ToString()];
                    if (string.IsNullOrWhiteSpace(dt.Rows[0]["accesslevel"].ToString()) || dt.Rows[0]["accesslevel"].ToString() != "1001")
                    {
                        Response.Redirect("login.aspx");
                    }
                }
            }

            FillGrid();
        }
        #region Click Actions
        protected void AddNew_Click(object sender, EventArgs e)
        {
            pnl_front.Visible = false;
            pnl_back.Visible = true;
            btn_SaveUpdDel.Text = Enums.ButtonControl.Save.ToString();
            EmptyFields();
        }
        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            pnl_back.Visible = false;
            pnl_front.Visible = true;
            EmptyFields();
        }
        protected void btn_SaveUpdDel_Click(object sender, EventArgs e)
        {
            if (btn_SaveUpdDel.Text.Equals(Enums.ButtonControl.Save.ToString()))
            {
                DoSaveAction();
            }
            else if (btn_SaveUpdDel.Text.Equals(Enums.ButtonControl.Update.ToString()))
            {
                DoUpdateAction();
            }
            else if (btn_SaveUpdDel.Text.Equals(Enums.ButtonControl.Delete.ToString()))
            {
                DoDeleteAction();
            }
            pnl_back.Visible = false;
            pnl_front.Visible = true;
            EmptyFields();
            EnableControls();
            FillGrid();
        }
        #endregion

        #region DML Methods
        public void DoSaveAction()
        {
            var Model = MapToObject();
            PharmaCompanyHandler PharmaCompanyHandler = new PharmaCompanyHandler();
            PharmaCompanyHandler.Insert(Model);
            MessageCollection.copyFrom(PharmaCompanyHandler.MessageCollection);

            if (MessageCollection.isErrorOccured)
            {
                MessageCollection.PublishLog();
                lbl_err.Text = MessageCollection.Messages[MessageCollection.Messages.Count - 1].ErrorMessage;
                lbl_err.Visible = true;
            }
            else
            {
                lbl_err.Text = "Record Inserted Successfully";
                lbl_err.ForeColor = System.Drawing.Color.Green;
            }

        }
        public void DoUpdateAction()
        {
            var Model = MapToObject();
            PharmaCompanyHandler PharmaCompanyHandler = new PharmaCompanyHandler();
            PharmaCompanyHandler.Update(Model);
            MessageCollection.copyFrom(PharmaCompanyHandler.MessageCollection);

            if (MessageCollection.isErrorOccured)
            {
                MessageCollection.PublishLog();
                lbl_err.Text = MessageCollection.Messages[MessageCollection.Messages.Count - 1].ErrorMessage;
                lbl_err.Visible = true;
            }
            else
            {
                lbl_err.Text = "Record Updated Successfully";
                lbl_err.ForeColor = System.Drawing.Color.Green;
            }
        }
        public void DoDeleteAction()
        {
            PharmaCompanyHandler PharmaCompanyHandler = new PharmaCompanyHandler();
            PharmaCompanyHandler.Delete(Convert.ToInt32(txt_id.Text));
            MessageCollection.copyFrom(PharmaCompanyHandler.MessageCollection);

            if (MessageCollection.isErrorOccured)
            {
                MessageCollection.PublishLog();
                lbl_err.Text = MessageCollection.Messages[MessageCollection.Messages.Count - 1].ErrorMessage;
                lbl_err.Visible = true;
            }
            else
            {
                lbl_err.Text = "Record Deleted Successfully";
                lbl_err.ForeColor = System.Drawing.Color.Green;
            }
        }
        #endregion

        #region HelperMethods
        public void EmptyFields()
        {
            txt_addr.Text = string.Empty;
            txt_cnumber.Text = string.Empty;
            txt_desc.Text = string.Empty;
            txt_email.Text = string.Empty;
            txt_name.Text = string.Empty;
            txt_id.Text = string.Empty;
        }
        public void DisableControls()
        {
            txt_addr.Enabled = false;
            txt_cnumber.Enabled = false;
            txt_desc.Enabled = false;
            txt_email.Enabled = false;
            txt_name.Enabled = false;
        }
        public void EnableControls()
        {
            txt_addr.Enabled = true;
            txt_cnumber.Enabled = true;
            txt_desc.Enabled = true;
            txt_email.Enabled = true;
            txt_name.Enabled = true;
        }
        public void FillGrid()
        {
            PharmaCompanyHandler PharmaCompanyHandler = new PharmaCompanyHandler();
            PharmaCompanyHandler.DoFillGridAction();
            DataTable gridData = PharmaCompanyHandler.dt;
            MessageCollection.copyFrom(PharmaCompanyHandler.MessageCollection);

            if (MessageCollection.isErrorOccured)
            {
                MessageCollection.PublishLog();
                lbl_err.Text = MessageCollection.Messages[MessageCollection.Messages.Count - 1].ErrorMessage;
                lbl_err.Visible = true;
            }

            gridPharmacy.DataSource = gridData;
            gridPharmacy.DataBind();


        }
        public void FillFields(int ID)
        {
            PharmaCompanyHandler PharmaCompanyHandler = new PharmaCompanyHandler();
            PharmaCompanyHandler.DoFillBackPanelAction(ID);
            DataTable Data = PharmaCompanyHandler.dt;
            MessageCollection.copyFrom(PharmaCompanyHandler.MessageCollection);

            if (MessageCollection.isErrorOccured)
            {
                MessageCollection.PublishLog();
                lbl_err.Text = MessageCollection.Messages[MessageCollection.Messages.Count - 1].ErrorMessage;
                lbl_err.Visible = true;
            }
            else
            {
                txt_addr.Text = Data.Rows[0]["Address"].ToString();
                txt_cnumber.Text = Data.Rows[0]["ContactNumber"].ToString();
                txt_desc.Text = Data.Rows[0]["Description"].ToString();
                txt_email.Text = Data.Rows[0]["Email"].ToString();
                txt_name.Text = Data.Rows[0]["Name"].ToString();
                txt_id.Text = Data.Rows[0]["ID"].ToString();
            }
        }
        public PharmaCompanyModel MapToObject()
        {
            return new PharmaCompanyModel()
            {
                Address = txt_addr.Text,
                ContactNumber = txt_cnumber.Text,
                Description = txt_desc.Text,
                Email = txt_email.Text,
                Name = txt_name.Text,
                ID = string.IsNullOrWhiteSpace(txt_id.Text) ? 0 : Convert.ToInt32(txt_id.Text)
            };
        }
        #endregion
        protected void gridPharmacy_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gridPharmacy.Rows[rowIndex];
            int ID = int.Parse(row.Cells[0].Text);
            FillFields(ID);

            if (e.CommandName.Equals(Enums.GridCommand.EditRow.ToString()))
            {
                btn_SaveUpdDel.Text = Enums.ButtonControl.Update.ToString();
                EnableControls();
            }
            else if (e.CommandName.Equals(Enums.GridCommand.DeleteRow.ToString()))
            {
                btn_SaveUpdDel.Text = Enums.ButtonControl.Delete.ToString();
                DisableControls();
            }
            pnl_front.Visible = false;
            pnl_back.Visible = true;

        }
    }
}