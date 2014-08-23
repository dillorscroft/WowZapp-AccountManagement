﻿namespace ContentUploader
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Runtime.Serialization;
    
using System.Drawing;


    
    public sealed class ContentPack
    {

        public enum ContentPackType
        {
            Comicon = 1,
            Comix = 2,
            RubberStamp = 3,
            Callout = 4,
            SoundFX = 5,
            Emoticon = 6
        }

        #region PrivateVariables

        private DataTools dt = new DataTools();
        private int _ContentPackID;
        private ContentPackType _ContentPackTypeID;
        private string _ContentPackTitle;
        private string _ContentPackDescription;
        private Image _ContentPackAd;
        private decimal _ContentPackPrice;
        private decimal _ContentPackSalePrice;
        private DateTime _ContentPackSaleEndDate;
        private DateTime _ContentPackAvailableDate;
        private DateTime _ContentPackEndDate;
        private bool _ContentPackIsFree;
        private Image _ContentPackIcon;

        #endregion PrivateVariables

        #region Properties

        
        public List<General.Error> Errors = new List<General.Error>();

        
        public int ContentPackID
        {
            get { return this._ContentPackID; }
            set { this._ContentPackID = value; }
        }

        
        public ContentPackType ContentPackTypeID
        {
            get { return this._ContentPackTypeID; }
            set { this._ContentPackTypeID = value; }
        }

        
        public string ContentPackTitle
        {
            get { return this._ContentPackTitle; }
            set { this._ContentPackTitle = value; }
        }

        
        public string ContentPackDescription
        {
            get { return this._ContentPackDescription; }
            set { this._ContentPackDescription = value; }
        }

        
        public byte[] ContentPackAd
        {
            get { return ImageHandler.imageToByteArray(this._ContentPackAd); }
            set { this._ContentPackAd = ImageHandler.byteArrayToImage(value); }
        }

        public byte[] ContentPackIcon
        {
            get { return ImageHandler.imageToByteArray(this._ContentPackIcon); }
            set { this._ContentPackIcon = ImageHandler.byteArrayToImage(value); }
        }

        
        public decimal ContentPackPrice
        {
            get { return this._ContentPackPrice; }
            set { this._ContentPackPrice = value; }
        }

        
        public decimal ContentPackSalePrice
        {
            get { return this._ContentPackSalePrice; }
            set { this._ContentPackSalePrice = value; }
        }

        
        public DateTime ContentPackSaleEndDate
        {
            get { return this._ContentPackSaleEndDate; }
            set { this._ContentPackSaleEndDate = value; }
        }

        
        public DateTime ContentPackAvailableDate
        {
            get { return this._ContentPackAvailableDate; }
            set { this._ContentPackAvailableDate = value; }
        }

        
        public DateTime ContentPackEndDate
        {
            get { return this._ContentPackEndDate; }
            set { this._ContentPackEndDate = value; }
        }

        
        public bool ContentPackIsFree
        {
            get { return this._ContentPackIsFree; }
            set { this._ContentPackIsFree = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Resident class
        /// </summary>
        public ContentPack()
        {
            _ContentPackID = 0;
            _ContentPackTypeID = ContentPackType.Comicon;
            _ContentPackTitle = string.Empty;
            _ContentPackDescription = string.Empty;
            _ContentPackAd = null;
            _ContentPackPrice = (decimal)0.00;
            _ContentPackSalePrice = (decimal)0.00;
            _ContentPackSaleEndDate = new DateTime(1900, 1, 1);
            _ContentPackAvailableDate = new DateTime(1900, 1, 1);
            _ContentPackEndDate = new DateTime(1900, 1, 1);
            _ContentPackIsFree = false;
            _ContentPackIcon = null;

        }

        /// <summary>
        /// Initializes a new instance of the ContentPack class
        /// </summary>
        /// <param name="residentID">Unique Database ID</param>
        public ContentPack(int blankID)
        {

            _ContentPackID = blankID;
            _ContentPackTypeID = ContentPackType.Comicon;
            _ContentPackTitle = string.Empty;
            _ContentPackDescription = string.Empty;
            _ContentPackAd = null;
            _ContentPackPrice = (decimal)0.00;
            _ContentPackSalePrice = (decimal)0.00;
            _ContentPackSaleEndDate = new DateTime(1900,1,1);
            _ContentPackAvailableDate = new DateTime(1900, 1, 1);
            _ContentPackEndDate = new DateTime(1900, 1, 1);
            _ContentPackIsFree = false;
            _ContentPackIcon = null;

            this.Get();

        }


        public ContentPack(DataRow dr)
        {


            try
            {

                _ContentPackID = 0;
                _ContentPackTypeID = ContentPackType.Comicon;
                _ContentPackTitle = string.Empty;
                _ContentPackDescription = string.Empty;
                _ContentPackAd = null;
                _ContentPackPrice = (decimal)0.00;
                _ContentPackSalePrice = (decimal)0.00;
                _ContentPackSaleEndDate = new DateTime(1900, 1, 1);
                _ContentPackAvailableDate = new DateTime(1900, 1, 1);
                _ContentPackEndDate = new DateTime(1900, 1, 1);
                _ContentPackIsFree = false;
                _ContentPackIcon = null;

                if (!DBNull.Value.Equals(dr["ContentPackID"])) { this._ContentPackID = (int)dr["ContentPackID"]; }
                if (!DBNull.Value.Equals(dr["ContentPackTypeID"])) { this._ContentPackTypeID = (ContentPackType)dr["ContentPackTypeID"]; }
                if (!DBNull.Value.Equals(dr["ContentPackTitle"])) { this._ContentPackTitle = (string)dr["ContentPackTitle"]; }
                if (!DBNull.Value.Equals(dr["ContentPackDescription"])) { this._ContentPackDescription = (string)dr["ContentPackDescription"]; }
                if (!DBNull.Value.Equals(dr["ContentPackAd"])) { this._ContentPackAd = ImageHandler.byteArrayToImage((Byte[])dr["ContentPackAd"]); }
                if (!DBNull.Value.Equals(dr["ContentPackPrice"])) { this._ContentPackPrice = (decimal)dr["ContentPackPrice"]; }
                if (!DBNull.Value.Equals(dr["ContentPackSalePrice"])) { this._ContentPackSalePrice = (decimal)dr["ContentPackSalePrice"]; }
                if (!DBNull.Value.Equals(dr["ContentPackSaleEndDate"])) { this._ContentPackSaleEndDate = (DateTime)dr["ContentPackSaleEndDate"]; }
                if (!DBNull.Value.Equals(dr["ContentPackAvailableDate"])) { this._ContentPackAvailableDate = (DateTime)dr["ContentPackAvailableDate"]; }
                if (!DBNull.Value.Equals(dr["ContentPackEndDate"])) { this._ContentPackEndDate = (DateTime)dr["ContentPackEndDate"]; }
                if (!DBNull.Value.Equals(dr["ContentPackIsFree"])) { this._ContentPackIsFree = (bool)dr["ContentPackIsFree"]; }
                if (!DBNull.Value.Equals(dr["ContentPackIcon"])) { this._ContentPackIcon = ImageHandler.byteArrayToImage((Byte[])dr["ContentPackIcon"]); }
            }
            catch (Exception ex)
            {

                _ContentPackID = 0;
                _ContentPackTypeID = ContentPackType.Comicon;
                _ContentPackTitle = string.Empty;
                _ContentPackDescription = string.Empty;
                _ContentPackAd = null;
                _ContentPackPrice = (decimal)0.00;
                _ContentPackSalePrice = (decimal)0.00;
                _ContentPackSaleEndDate = new DateTime(1900, 1, 1);
                _ContentPackAvailableDate = new DateTime(1900, 1, 1);
                _ContentPackEndDate = new DateTime(1900, 1, 1);
                _ContentPackIsFree = false;
                _ContentPackIcon = null;

                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "42";
                tmpError.ErrorLocation = this.ToString() + "(DataRow dataRow)" + this.ToString();
                tmpError.ErrorTitle = "Unable To Load From DataRow";
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }

        }

        #endregion Constructors

        #region Methods

        public void Get()
        {
            this.dt = new DataTools();

            this.Errors = new List<General.Error>();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM ContentPacks WHERE ContentPackID = @ContentPackID";
                cmd.Parameters.Add("@ContentPackID", SqlDbType.Int).Value = _ContentPackID; ;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (!DBNull.Value.Equals(dr["ContentPackID"])) { this._ContentPackID = (int)dr["ContentPackID"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackTypeID"])) { this._ContentPackTypeID = (ContentPackType)dr["ContentPackTypeID"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackTitle"])) { this._ContentPackTitle = (string)dr["ContentPackTitle"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackDescription"])) { this._ContentPackDescription = (string)dr["ContentPackDescription"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackAd"])) { this._ContentPackAd = ImageHandler.byteArrayToImage((Byte[])dr["ContentPackAd"]); }
                    if (!DBNull.Value.Equals(dr["ContentPackPrice"])) { this._ContentPackPrice = (decimal)dr["ContentPackPrice"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackSalePrice"])) { this._ContentPackSalePrice = (decimal)dr["ContentPackSalePrice"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackSaleEndDate"])) { this._ContentPackSaleEndDate = (DateTime)dr["ContentPackSaleEndDate"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackAvailableDate"])) { this._ContentPackAvailableDate = (DateTime)dr["ContentPackAvailableDate"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackEndDate"])) { this._ContentPackEndDate = (DateTime)dr["ContentPackEndDate"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackIsFree"])) { this._ContentPackIsFree = (bool)dr["ContentPackIsFree"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackIcon"])) { this._ContentPackIcon = ImageHandler.byteArrayToImage((Byte[])dr["ContentPackIcon"]); }
                }
                else
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "43";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                    this.Errors.Add(tmpError);
                }

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "44";
                tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                tmpError.ErrorTitle = "Unable To Load";
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }
        }
       

        public ContentPack GetPackByID(int ItemPackID)
        {
            this.dt = new DataTools();
            var items = new ContentPack();
            this.Errors = new List<General.Error>();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM ContentPacks WHERE ContentPackID = @ContentPackID";
                cmd.Parameters.Add("@ContentPackID", SqlDbType.Int).Value = ItemPackID;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (!DBNull.Value.Equals(dr["ContentPackID"])) { items._ContentPackID = this._ContentPackID = (int)dr["ContentPackID"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackTypeID"])) { items._ContentPackTypeID = this._ContentPackTypeID = (ContentPackType)dr["ContentPackTypeID"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackTitle"])) { items._ContentPackTitle = this._ContentPackTitle = (string)dr["ContentPackTitle"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackDescription"])) { items._ContentPackDescription = this._ContentPackDescription = (string)dr["ContentPackDescription"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackAd"])) { items._ContentPackAd = this._ContentPackAd = ImageHandler.byteArrayToImage((Byte[])dr["ContentPackAd"]); }
                    if (!DBNull.Value.Equals(dr["ContentPackPrice"])) { items._ContentPackPrice = this._ContentPackPrice = (decimal)dr["ContentPackPrice"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackSalePrice"])) { items._ContentPackSalePrice = this._ContentPackSalePrice = (decimal)dr["ContentPackSalePrice"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackSaleEndDate"])) { items._ContentPackSaleEndDate = this._ContentPackSaleEndDate = (DateTime)dr["ContentPackSaleEndDate"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackAvailableDate"])) { items._ContentPackAvailableDate = this._ContentPackAvailableDate = (DateTime)dr["ContentPackAvailableDate"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackEndDate"])) { items._ContentPackEndDate = this._ContentPackEndDate = (DateTime)dr["ContentPackEndDate"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackIsFree"])) { items._ContentPackIsFree = this._ContentPackIsFree = (bool)dr["ContentPackIsFree"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackIcon"])) { items._ContentPackIcon = this._ContentPackIcon = ImageHandler.byteArrayToImage((Byte[])dr["ContentPackIcon"]); }
                }
                else
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "43";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                    this.Errors.Add(tmpError);
                }

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "44";
                tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                tmpError.ErrorTitle = "Unable To Load";
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }

            return items;
        }


        public DataSet GetPacksByType(int ContentPackTypeID)
        {
                this.dt = new DataTools();
                var items = new ContentPack();
                this.Errors = new List<General.Error>();
                DataSet ds = null;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM ContentPacks WHERE ContentPackTypeID = @ContentPackTypeID";
                    cmd.Parameters.Add("@ContentPackTypeID", SqlDbType.Int).Value = ContentPackTypeID;
                    ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        if (!DBNull.Value.Equals(dr["ContentPackID"])) { items._ContentPackID = this._ContentPackID = (int)dr["ContentPackID"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackTypeID"])) { items._ContentPackTypeID = this._ContentPackTypeID = (ContentPackType)dr["ContentPackTypeID"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackTitle"])) { items._ContentPackTitle = this._ContentPackTitle = (string)dr["ContentPackTitle"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackDescription"])) { items._ContentPackDescription = this._ContentPackDescription = (string)dr["ContentPackDescription"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackAd"])) { items._ContentPackAd = this._ContentPackAd = ImageHandler.byteArrayToImage((Byte[])dr["ContentPackAd"]); }
                        if (!DBNull.Value.Equals(dr["ContentPackPrice"])) { items._ContentPackPrice = this._ContentPackPrice = (decimal)dr["ContentPackPrice"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackSalePrice"])) { items._ContentPackSalePrice = this._ContentPackSalePrice = (decimal)dr["ContentPackSalePrice"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackSaleEndDate"])) { items._ContentPackSaleEndDate = this._ContentPackSaleEndDate = (DateTime)dr["ContentPackSaleEndDate"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackAvailableDate"])) { items._ContentPackAvailableDate = this._ContentPackAvailableDate = (DateTime)dr["ContentPackAvailableDate"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackEndDate"])) { items._ContentPackEndDate = this._ContentPackEndDate = (DateTime)dr["ContentPackEndDate"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackIsFree"])) { items._ContentPackIsFree = this._ContentPackIsFree = (bool)dr["ContentPackIsFree"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackIcon"])) { items._ContentPackIcon = this._ContentPackIcon = ImageHandler.byteArrayToImage((Byte[])dr["ContentPackIcon"]); }
 
                    }
                    else
                    {
                        General.Error tmpError = new General.Error();
                        tmpError.ErrorNumber = "43";
                        tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                        tmpError.ErrorTitle = "Unable To Load";
                        tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                        this.Errors.Add(tmpError);
                    }

                }
                catch (Exception ex)
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "44";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = ex.Message;
                    this.Errors.Add(tmpError);
                }

                return ds;
            
        }


        public void Save()
        {
            this.DataValidation();

            if (this.Errors.Count != 0) { return; }

            this.dt = new DataTools();

            try
            {

                SqlCommand cmd = new SqlCommand();
                if (this._ContentPackID == 0)
                {
                    cmd.CommandText = "INSERT INTO ContentPacks (ContentPackTypeID, ContentPackTitle, ContentPackDescription, ContentPackAd, ContentPackPrice, ContentPackSalePrice, ContentPackSaleEndDate, ContentPackAvailableDate, ContentPackEndDate, ContentPackIsFree, ContentPackIcon) VALUES (@ContentPackTypeID, @ContentPackTitle, @ContentPackDescription, @ContentPackAd, @ContentPackPrice, @ContentPackSalePrice, @ContentPackSaleEndDate, @ContentPackAvailableDate, @ContentPackEndDate, @ContentPackIsFree, @ContentPackIcon); SELECT @@IDENTITY;";
                }
                else
                {
                    cmd.CommandText = "UPDATE ContentPacks SET ContentPackTypeID = @ContentPackTypeID, ContentPackTitle = @ContentPackTitle, ContentPackDescription = @ContentPackDescription, ContentPackAd = @ContentPackAd, ContentPackPrice = @ContentPackPrice, ContentPackSalePrice = @ContentPackSalePrice, ContentPackSaleEndDate = @ContentPackSaleEndDate, ContentPackAvailableDate = @ContentPackAvailableDate, ContentPackEndDate = @ContentPackEndDate, ContentPackIsFree = @ContentPackIsFree, ContentPackIcon = @ContentPackIcon WHERE ContentPackID = @ContentPackID;";
                    cmd.Parameters.Add("@ContentPackID", SqlDbType.Int).Value = this._ContentPackID;
                }

                cmd.Parameters.Add("@ContentPackTypeID", SqlDbType.Int).Value = (int)this.ContentPackTypeID;
                cmd.Parameters.Add("@ContentPackTitle", SqlDbType.NVarChar, 25).Value = this.ContentPackTitle;
                cmd.Parameters.Add("@ContentPackDescription", SqlDbType.NVarChar).Value = this.ContentPackDescription;
                cmd.Parameters.Add("@ContentPackAd", SqlDbType.Image).Value = this.ContentPackAd;
                cmd.Parameters.Add("@ContentPackPrice", SqlDbType.Money).Value = this.ContentPackPrice;
                cmd.Parameters.Add("@ContentPackSalePrice", SqlDbType.Money).Value = this.ContentPackSalePrice;
                cmd.Parameters.Add("@ContentPackSaleEndDate", SqlDbType.DateTime).Value = this.ContentPackSaleEndDate;
                cmd.Parameters.Add("@ContentPackAvailableDate", SqlDbType.DateTime).Value = this.ContentPackAvailableDate;
                cmd.Parameters.Add("@ContentPackEndDate", SqlDbType.DateTime).Value = this.ContentPackEndDate;
                cmd.Parameters.Add("@ContentPackIsFree", SqlDbType.Bit).Value = this.ContentPackIsFree;
                cmd.Parameters.Add("@ContentPackIcon", SqlDbType.Image).Value = this.ContentPackIcon;

                object tmpResults = this.dt.ExecuteScalar(cmd, DataTools.DataSources.LOLAccountManagement);

                if (this._ContentPackID == 0)
                {
                    this._ContentPackID = int.Parse(tmpResults.ToString());
                }


            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "45";
                tmpError.ErrorLocation = this.ToString() + ".Save()";
                tmpError.ErrorTitle = "Unable To Save";
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }
        }

        public void Delete(int ContentPackID)
        {

            SqlCommand cmd = new SqlCommand();
            dt = new DataTools();
            Errors = new List<General.Error>();

            try
            {
                cmd.CommandText = "DELETE FROM ContentPacks WHERE ContentPackID = @ContentPackID";
                cmd.Parameters.Add("@ContentPackID", SqlDbType.Int).Value = ContentPackID;
                dt.ExecuteNonQuery(cmd, DataTools.DataSources.LOLAccountManagement);
            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "46";
                tmpError.ErrorLocation = this.ToString() + ".Delete()";
                tmpError.ErrorTitle = "Unable To Delete";
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }

        }
        /// <summary>
        /// Performs data validation prior to saving record. Resets and increments error collection count
        /// </summary>
        private void DataValidation()
        {
            this.Errors = new List<General.Error>();

            this.dt = new DataTools();

        }

        #endregion Methods
    }
}
