using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using Telerik.Web.UI;
using System.IO;
using System.Text;

namespace ContentUploader
{
    public partial class ContentPackItems : System.Web.UI.Page
    {
        DataTools dt = new DataTools();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                Response.Redirect("~/Account/Login.aspx");
            }

            if (Request.QueryString["Item"] != null)
            {
                var ContentPackItemID = Request.QueryString["Item"];
                checkForDelete(Convert.ToInt32(ContentPackItemID));
            }

            if (!IsPostBack)
            {
                LoadContentPack();

                if (Request.QueryString["Item"] != null)
                {
                    var ContentPackItemID = Request.QueryString["Item"];

                    ContentPackItem temp = new ContentPackItem().GetItemByID(Convert.ToInt32(ContentPackItemID));
                    Session.Remove("ContentPackItem");
                    Session["ContentPackItem"] = temp;
                    if (temp != null)
                    {
                        editPackTitle.Text = temp.ContentItemTitle;
                        titleLegend.Text = temp.ContentItemTitle;
                        ItemPackID.Value = ContentPackItemID.ToString();
                        if (temp.ContentPackItemIcon != null)
                        {
                            bool img1 = IsValidImage(temp.ContentPackItemIcon);

                            if (img1 == true)
                            {
                                Image1.DataValue = temp.ContentPackItemIcon;
                            }
                            else
                            {
                                Image1.Visible = false;
                            }
                        }
                        else
                        {
                            Image1.Visible = false;
                        }
                        
                    }

                    foreach (ListItem items in selectedPackType.Items)
                    {
                        if (items.Value == temp.ContentPackID.ToString())
                        {
                            items.Selected = true;
                        }
                    }

                    pnlPackList.Visible = false;
                    pnlPackItem.Visible = true;
                }

            }

        }

        protected void rptPackItems_NeedDataSource(object source, RadListViewNeedDataSourceEventArgs e)
        {
            if (Request.QueryString["Items"] != null)
            {
                var parentID = Request.QueryString["Items"];
                GetContentPackItems(Convert.ToInt32(parentID));
            }
        }

        protected void LoadContentPack()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT ContentPackID, ContentPackTitle FROM ContentPacks ORDER BY ContentPackTitle";

            DataSet ds = dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            if (Request.QueryString["Item"] != null)
            {
                selectedPackType.DataSource = ds.Tables[0];
                selectedPackType.DataTextField = "ContentPackTitle";
                selectedPackType.DataValueField = "ContentPackID";
                selectedPackType.DataBind();

            }
        }

        protected void GetContentPackItems(int parentID)
        {
            ContentPackItem temp = new ContentPackItem();
            var list = temp.GetAll(parentID);
            rptPackItems.DataSource = list;
        }

        protected void insertPackItem_Click(object sender, EventArgs e)
        {

            byte[] icon = fileIcon.FileBytes;
            byte[] dataLarge = fileDataLarge.FileBytes;
            byte[] dataMedium = fileDataMedium.FileBytes;
            byte[] dataSmall = fileDataSmall.FileBytes;
            byte[] dataTiny = fileDataTiny.FileBytes;

            ContentPackItem tmpItem = new ContentPackItem();
            tmpItem.ContentItemTitle = txtContentPackTitle.Text;
            tmpItem.ContentPackDataLarge = dataLarge;
            tmpItem.ContentPackDataMedium = dataMedium;
            tmpItem.ContentPackDataSmall = dataSmall;
            tmpItem.ContentPackDataTiny = dataTiny;
            tmpItem.ContentPackItemIcon = icon;
            if (Request.QueryString["Items"] != null)
            {
                var temp = Request.QueryString["Items"];
                tmpItem.ContentPackID = Convert.ToInt32(temp);
            }

            tmpItem.Save();

            txtContentPackTitle.Text = "";

            pnlInsertNotification.Visible = true;

        }

        protected void editPackItem_Click(object sender, EventArgs e)
        {
            if (Session["ContentPackItem"] != null)
            {
                var temp = (ContentPackItem) Session["ContentPackItem"];

                byte[] icon = editFileIcon.FileBytes;
                byte[] dataLarge = editFileDataLarge.FileBytes;
                byte[] dataMedium = editFileDataMedium.FileBytes;
                byte[] dataSmall = editFileDataSmall.FileBytes;
                byte[] dataTiny = editFileDataTiny.FileBytes;

                ContentPackItem tmpItem = new ContentPackItem();
                tmpItem.ContentPackItemID = Convert.ToInt32(ItemPackID.Value);
                tmpItem.ContentItemTitle = editPackTitle.Text;
                if (editFileDataLarge.HasFile == true)
                {
                    tmpItem.ContentPackDataLarge = dataLarge;
                }
                else
                {
                    tmpItem.ContentPackDataLarge = temp.ContentPackDataLarge;
                }
                if (editFileDataMedium.HasFile == true)
                {
                    tmpItem.ContentPackDataMedium = dataMedium;
                }
                else
                {
                    tmpItem.ContentPackDataMedium = temp.ContentPackDataMedium;
                }
                if (editFileDataSmall.HasFile == true)
                {
                    tmpItem.ContentPackDataSmall = dataSmall;
                }
                else
                {
                    tmpItem.ContentPackDataSmall = temp.ContentPackDataSmall;
                }
                if (editFileDataTiny.HasFile == true)
                {
                    tmpItem.ContentPackDataTiny = dataTiny;
                }
                else
                {
                    tmpItem.ContentPackDataTiny = temp.ContentPackDataTiny;
                }
                if (editFileIcon.HasFile == true)
                {
                    tmpItem.ContentPackItemIcon = icon;
                }
                else
                {
                    tmpItem.ContentPackItemIcon = temp.ContentPackItemIcon;
                }

                tmpItem.ContentPackID = int.Parse(selectedPackType.SelectedValue);

                tmpItem.Save();

                updateNotification.Visible = true;
            }
        }

        protected void btnNewItem_Click(object sender, EventArgs e)
        {
            pnlNewPackItems.Visible = true;
            pnlPackList.Visible = false;
        }

        protected void btnBackFromEdit_Click(object sender, EventArgs e)
        {
            if (Session["tempUrl"] != null)
            {
                string tempUrl = Session["tempUrl"].ToString();
                Response.Redirect(tempUrl);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Session["backUrl"] != null)
            {
                string BackUrl = Session["backUrl"].ToString();
                Response.Redirect(BackUrl);
            }
        }

        protected void btnBackInsert_Click(object sender, EventArgs e)
        {
            
            rptPackItems.Rebind();
            pnlNewPackItems.Visible = false;
            pnlPackList.Visible = true;
        }

        void checkForDelete(int ContentPackItemID)
        {
            DataSet ds = null;
            SqlCommand cmd = new SqlCommand();

            ContentPackItem temp = new ContentPackItem();
            
            cmd.CommandText = "SELECT TOP 1 ContentPackItemID FROM MessageSteps WHERE ContentPackItemID=@ContentPackItemID";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@ContentPackItemID", SqlDbType.Int).Value = ContentPackItemID;
            ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLMessages);

            if (ds.Tables[0].Rows.Count > 0)
            {
                btnDeletePack.Enabled = false;
            }
    
        }

        protected void btnDeletePack_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Item"] != null)
            {
                var ContentPackItemID = Request.QueryString["Item"];

                ContentPackItem tempItem = new ContentPackItem();

                tempItem.Delete(Convert.ToInt32(ContentPackItemID));
                rptPackItems.Rebind();

                if (Session["tempUrl"] != null)
                {
                    string tempUrl = Session["tempUrl"].ToString();
                    Response.Redirect(tempUrl);
                }
            }
        }

        protected void rptPackItems_ItemDataBound(object sender, RadListViewItemEventArgs e)
        {
            RadBinaryImage imageContentPackItemIcon = (RadBinaryImage)e.Item.FindControl("imageContentPackItemIcon");

            if (imageContentPackItemIcon != null)
            {
                //string tempurl = "http://" + temp + ":64366" + imageContentPackItemIcon.ImageUrl.Replace("~", "");

                bool checkImgI = IsValidImage(imageContentPackItemIcon.DataValue);

                if (checkImgI == false)
                {
                    imageContentPackItemIcon.Visible = false;
                }
                
            }
  
        }

        public static bool IsValidImage(byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                    System.Drawing.Image.FromStream(ms);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

    }
}
