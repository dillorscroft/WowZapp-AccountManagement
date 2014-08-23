using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.IO;
using System.Data.Linq;
using System.Drawing;

namespace ContentUploader
{
    public partial class _Default : System.Web.UI.Page
    {
        DataTools dt = new DataTools();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                Response.Redirect("~/Account/Login.aspx");
            }

            if (Request.QueryString["Pack"] != null)
            {
                var ContentPackID = Request.QueryString["Pack"];

                checkForDelete(Convert.ToInt32(ContentPackID));
            }

            if (!IsPostBack)
            {
                LoadPackType();

                SaleEndDate.SelectedDate = Convert.ToDateTime("1/1/1900");
                AvailableDate.SelectedDate = Convert.ToDateTime("1/1/1900");
                EndDate.SelectedDate = Convert.ToDateTime("1/1/1900");

                if (Request.QueryString["Pack"] != null)
                {
                    var ContentPackID = Request.QueryString["Pack"];

                    ContentPack temp = new ContentPack().GetPackByID(Convert.ToInt32(ContentPackID));
                    Session.Remove("ContentPack");
                    Session["ContentPack"] = temp;
                    if (temp != null)
                    {
                        editPackTitle.Text = temp.ContentPackTitle;
                        titleLegend.Text = temp.ContentPackTitle;
                        ItemPackID.Value = ContentPackID.ToString();
                        editPackDescription.Text = temp.ContentPackDescription;

                        if (temp.ContentPackAd != null)
                        {
                            EditimageContentPackAD.DataValue = temp.ContentPackAd;
                        }
                        else
                        {
                            EditimageContentPackAD.Visible = false;
                        }

                        editContentPackPrice.Text = temp.ContentPackPrice.ToString();
                        editContentPackSalePrice.Text = temp.ContentPackSalePrice.ToString();

                        if (temp.ContentPackIsFree == true)
                        {
                            editIsFree.Checked = true;
                        }
                        else
                        {
                            editIsFree.Checked = false;
                        }

                        if (temp.ContentPackIcon != null)
                        {
                            EditimageContentPackIcon.DataValue = temp.ContentPackIcon;
                        }
                        else
                        {
                            EditimageContentPackIcon.Visible = false;
                        }
                    }

                    foreach (ListItem item in selectedPackType.Items)
                    {
                        if (item.Text == temp.ContentPackTypeID.ToString())
                        {
                            item.Selected = true;
                        }
                    }

                    editSaleEndDate.SelectedDate = temp.ContentPackSaleEndDate;
                    editEndDate.SelectedDate = temp.ContentPackEndDate;
                    editAvailableDate.SelectedDate = temp.ContentPackAvailableDate;

                    pnlPackList.Visible = false;
                    pnlPackItem.Visible = true;
                }

            }

        }

        protected void rptPacks_NeedDataSource(object source, RadListViewNeedDataSourceEventArgs e)
        {
            GetContentPacks(Convert.ToInt32(contentType.SelectedValue));
        }

        protected void contentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            rptPacks.Rebind();
        }

        protected void LoadPackType()
        {
            ContentPackType.DataSource = Enumeration.GetAll<ContentPack.ContentPackType>();
            ContentPackType.DataTextField = "Value";
            ContentPackType.DataValueField = "Key";
            ContentPackType.DataBind();

            contentType.DataSource = Enumeration.GetAll<ContentPack.ContentPackType>();
            contentType.DataTextField = "Value";
            contentType.DataValueField = "Key";
            contentType.DataBind();

            if (Request.QueryString["Pack"] != null)
            {
                selectedPackType.DataSource = Enumeration.GetAll<ContentPack.ContentPackType>();
                selectedPackType.DataTextField = "Value";
                selectedPackType.DataValueField = "Key";
                selectedPackType.DataBind();
            }
        }

        protected void GetContentPacks(int ContentPackTypeID)
        {
            ContentPack temp = new ContentPack();
            var list = temp.GetPacksByType(ContentPackTypeID);
            rptPacks.DataSource = list;
        }

        protected void insertPackItem_Click(object sender, EventArgs e)
        {

            byte[] dataAd = filePackAd.FileBytes;
            byte[] dataIcon = filePackIcon.FileBytes;

            ContentPack tmpItem = new ContentPack();
            tmpItem.ContentPackTitle = txtContentPackTitle.Text;
            tmpItem.ContentPackDescription = txtPackDescription.Text;

            if (ContentPackType.SelectedItem.Text == "Comicon")
            {
                tmpItem.ContentPackTypeID = ContentPack.ContentPackType.Comicon;
            }
            if (ContentPackType.SelectedItem.Text == "Callout")
            {
                tmpItem.ContentPackTypeID = ContentPack.ContentPackType.Callout;
            }
            if (ContentPackType.SelectedItem.Text == "Comix")
            {
                tmpItem.ContentPackTypeID = ContentPack.ContentPackType.Comix;
            }
            if (ContentPackType.SelectedItem.Text == "RubberStamp")
            {
                tmpItem.ContentPackTypeID = ContentPack.ContentPackType.RubberStamp;
            }
            if (ContentPackType.SelectedItem.Text == "SoundFX")
            {
                tmpItem.ContentPackTypeID = ContentPack.ContentPackType.SoundFX;
            }
            if (ContentPackType.SelectedItem.Text == "Emoticon")
            {
                tmpItem.ContentPackTypeID = ContentPack.ContentPackType.Emoticon;
            }
            
            tmpItem.ContentPackAd = dataAd;
            tmpItem.ContentPackPrice = int.Parse(txtContentPackPrice.Text);
            tmpItem.ContentPackSalePrice = int.Parse(txtContentPackSalePrice.Text);
            tmpItem.ContentPackEndDate = Convert.ToDateTime(EndDate.SelectedDate);
            tmpItem.ContentPackAvailableDate = Convert.ToDateTime(AvailableDate.SelectedDate);
            tmpItem.ContentPackSaleEndDate = Convert.ToDateTime(SaleEndDate.SelectedDate);
            tmpItem.ContentPackIcon = dataIcon;

            if (IsFree.Checked == true)
            {
                tmpItem.ContentPackIsFree = true;
            }
            else
            {
                tmpItem.ContentPackIsFree = false;
            }

            tmpItem.Save();
            pnlNewRecord.Visible = true;

            txtContentPackTitle.Text = "";
            txtPackDescription.Text = "";


        }

        protected void editPackItem_Click(object sender, EventArgs e)
        {
            if (Session["ContentPack"] != null)
            {
                var temp = (ContentPack)Session["ContentPack"];

                byte[] dataAd = editPackAd.FileBytes;
                byte[] dataIcon = editFileIcon.FileBytes;

                ContentPack tmpItem = new ContentPack();
                tmpItem.ContentPackID = temp.ContentPackID;
                tmpItem.ContentPackTitle = editPackTitle.Text;
                tmpItem.ContentPackDescription = editPackDescription.Text;

                if (selectedPackType.SelectedItem.Text == "Comicon")
                {
                    tmpItem.ContentPackTypeID = ContentPack.ContentPackType.Comicon;
                }
                if (selectedPackType.SelectedItem.Text == "Callout")
                {
                    tmpItem.ContentPackTypeID = ContentPack.ContentPackType.Callout;
                }
                if (selectedPackType.SelectedItem.Text == "Comix")
                {
                    tmpItem.ContentPackTypeID = ContentPack.ContentPackType.Comix;
                }
                if (selectedPackType.SelectedItem.Text == "RubberStamp")
                {
                    tmpItem.ContentPackTypeID = ContentPack.ContentPackType.RubberStamp;
                }
                if (selectedPackType.SelectedItem.Text == "SoundFX")
                {
                    tmpItem.ContentPackTypeID = ContentPack.ContentPackType.SoundFX;
                }
                if (selectedPackType.SelectedItem.Text == "Emoticon")
                {
                    tmpItem.ContentPackTypeID = ContentPack.ContentPackType.Emoticon;
                }

                if (editPackAd.HasFile == true)
                {
                    tmpItem.ContentPackAd = dataAd;
                }
                else
                {
                    tmpItem.ContentPackAd = temp.ContentPackAd;
                }

                if (editFileIcon.HasFile == true)
                {
                    tmpItem.ContentPackIcon = dataIcon;
                }
                else
                {
                    tmpItem.ContentPackIcon = temp.ContentPackIcon;
                }

                if (editIsFree.Checked == true)
                {
                    tmpItem.ContentPackIsFree = true;
                }
                else
                {
                    tmpItem.ContentPackIsFree = false;
                }

                tmpItem.ContentPackPrice = decimal.Parse(editContentPackPrice.Text);
                tmpItem.ContentPackSalePrice = decimal.Parse(editContentPackSalePrice.Text);
                tmpItem.ContentPackEndDate = Convert.ToDateTime(EndDate.SelectedDate);
                tmpItem.ContentPackAvailableDate = Convert.ToDateTime(editAvailableDate.SelectedDate);
                tmpItem.ContentPackSaleEndDate = Convert.ToDateTime(editEndDate.SelectedDate);

                tmpItem.Save();
                updateNotification.Visible = true;
                Response.Redirect(Request.Url.AbsoluteUri.ToString());
               // txtContentPackTitle.Text = "";
               // txtPackDescription.Text = "";
            }
        }

        protected void btnNewCategory_Click(object sender, EventArgs e)
        {
            pnlNewItem.Visible = true;
            pnlPackList.Visible = false;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlNewItem.Visible = false;
            pnlPackList.Visible = true;
            rptPacks.Rebind();
        }

        protected void btnBackFromEdit_Click(object sender, EventArgs e)
        {
            pnlNewItem.Visible = false;
            pnlPackList.Visible = true;
            pnlPackItem.Visible = false;
        }

        protected void btnViewItems_Click(object sender, EventArgs e)
        {
            string parentUrl = Request.Url.AbsoluteUri.ToString();
            Session.Remove("backUrl");
            Session["backUrl"] = parentUrl;

            string tempUrl = "~/ContentPackItems.aspx?Items=" + ItemPackID.Value;
            Session.Remove("tempUrl");
            Session["tempUrl"] = tempUrl;

            Response.Redirect(tempUrl);
        }

        void checkForDelete(int ContentPackID)
        {
            DataSet ds = null;
            SqlCommand cmd = new SqlCommand();
            
            ContentPackItem temp = new ContentPackItem();
            var list = temp.GetAll(Convert.ToInt32(ContentPackID));

            if (list.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in list.Tables[0].Rows)
                {
                    int ContentPackItemID = (int)row["ContentPackItemID"];
                    cmd.CommandText = "SELECT * FROM MessageSteps WHERE ContentPackItemID=@ContentPackItemID";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@ContentPackItemID", SqlDbType.Int).Value = ContentPackItemID;
                    ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLMessages);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        btnDeletePack.Enabled = false;
                        break;
                    }
                }
            }
        }

        protected void btnDeletePack_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Pack"] != null)
            {
                var ContentPackID = Request.QueryString["Pack"];

                ContentPack tempParent = new ContentPack();
                ContentPackItem tempItem = new ContentPackItem();

                var list = tempItem.GetAll(Convert.ToInt32(ContentPackID));

                if (list.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in list.Tables[0].Rows)
                    {
                        int ContentPackItemID = (int)row["ContentPackItemID"];
                        tempItem.Delete(ContentPackItemID);
                    }
                }

                tempParent.Delete(Convert.ToInt32(ContentPackID));
                rptPacks.Rebind();
                Response.Redirect("~/Default.aspx");
            }
        }

        public static class Enumeration
        {
            public static IDictionary<int, string> GetAll<TEnum>() where TEnum : struct
            {
                var enumerationType = typeof(TEnum);

                if (!enumerationType.IsEnum)
                    throw new ArgumentException("Enumeration type is expected.");

                var dictionary = new Dictionary<int, string>();

                foreach (int value in Enum.GetValues(enumerationType))
                {
                    var name = Enum.GetName(enumerationType, value);
                    dictionary.Add(value, name);
                }

                return dictionary;
            }
        }

    }
    
}
