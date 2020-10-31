﻿using BLL.Users;
using Generics;
using Models.Users;
using System;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;

namespace FYP_Pharmacy.Forms
{
    public partial class Users : PageActionHandler
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region logging
            PageName = "Users";
            CONTEXT = "Users";
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

        protected void ddl_accesslevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_accesslevel.SelectedValue == "1001")
            {
                ddl_Company.DataSource = null;
                ddl_Company.DataBind();
                ddl_Company.Enabled = false;
            }
            else
            {
                UsersHandler usersHandler = new UsersHandler();
                usersHandler.FillCompanyDllAction(Convert.ToInt32(ddl_accesslevel.SelectedValue));
                DataTable ddlData = usersHandler.dt;
                MessageCollection.copyFrom(usersHandler.MessageCollection);

                if (MessageCollection.isErrorOccured)
                {
                    MessageCollection.PublishLog();
                    lbl_err.Text = MessageCollection.Messages[MessageCollection.Messages.Count - 1].ErrorMessage;
                    lbl_err.Visible = true;
                }
                else
                {
                    ddl_Company.DataSource = ddlData;
                    ddl_Company.DataTextField = "Name";
                    ddl_Company.DataValueField = "ID";
                    ddl_Company.DataBind();
                }
            }
        }

        #region DML Methods
        public void DoSaveAction()
        {
            var Model = MapToObject();
            UsersHandler usersHandler = new UsersHandler();
            usersHandler.Insert(Model);
            MessageCollection.copyFrom(usersHandler.MessageCollection);

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
            UsersHandler usersHandler = new UsersHandler();
            usersHandler.Update(Model);
            MessageCollection.copyFrom(usersHandler.MessageCollection);

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
            UsersHandler usersHandler = new UsersHandler();
            usersHandler.Delete(Convert.ToInt32(txt_id.Text));
            MessageCollection.copyFrom(usersHandler.MessageCollection);

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
            txt_usr.Text = string.Empty;
            txt_pw.Text = string.Empty;
            txt_id.Text = string.Empty;
            txt_pw.Attributes["value"] = string.Empty;
            ddl_accesslevel.SelectedIndex = 0;
        }
        public void DisableControls()
        {
            txt_pw.Enabled = false;
            txt_usr.Enabled = false;
            ddl_accesslevel.Enabled = false;
            ddl_Company.Enabled = false;
        }
        public void EnableControls()
        {
            txt_pw.Enabled = true;
            txt_usr.Enabled = true;
            ddl_accesslevel.Enabled = true;
        }
        public void FillGrid()
        {
            UsersHandler usersHandler = new UsersHandler();
            usersHandler.DoFillGridAction();
            DataTable gridData = usersHandler.dt;
            MessageCollection.copyFrom(usersHandler.MessageCollection);

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
            UsersHandler usersHandler = new UsersHandler();
            usersHandler.DoFillBackPanelAction(ID);
            DataTable UserData = usersHandler.dt;
            MessageCollection.copyFrom(usersHandler.MessageCollection);

            if (MessageCollection.isErrorOccured)
            {
                MessageCollection.PublishLog();
                lbl_err.Text = MessageCollection.Messages[MessageCollection.Messages.Count - 1].ErrorMessage;
                lbl_err.Visible = true;
            }
            else
            {
                txt_id.Text = UserData.Rows[0]["ID"].ToString();
                txt_usr.Text = UserData.Rows[0]["Name"].ToString();
                txt_pw.Attributes.Add("value", UserData.Rows[0]["Password"].ToString());
                ddl_accesslevel.SelectedValue = UserData.Rows[0]["AccessLevel"].ToString();
                ddl_accesslevel_SelectedIndexChanged(this, null);
                ddl_Company.SelectedValue = UserData.Rows[0]["Company"].ToString();
            }
        }
        public UserModel MapToObject()
        {
            return new UserModel()
            {
                AccessLevel = Convert.ToInt32(ddl_accesslevel.SelectedValue),
                CompanyKey = ddl_Company.SelectedValue,
                Name = txt_usr.Text.Trim(),
                Password = txt_pw.Text,
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