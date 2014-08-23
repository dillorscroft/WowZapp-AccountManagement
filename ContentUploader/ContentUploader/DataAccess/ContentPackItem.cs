namespace ContentUploader
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


    public sealed class ContentPackItem
    {

        #region PrivateVariables

        private DataTools dt = new DataTools();
        private int _ContentPackItemID;
        private int _ContentPackID;
        private string _ContentItemTitle;
        private Byte[] _ContentPackDataLarge;
        private Byte[] _ContentPackDataMedium;
        private Byte[] _ContentPackDataSmall;
        private Byte[] _ContentPackDataTiny;
        private Image _ContentPackItemIcon;

        #endregion PrivateVariables

        #region Properties

        public List<General.Error> Errors = new List<General.Error>();

        public int ContentPackItemID
        {
            get { return this._ContentPackItemID; }
            set { this._ContentPackItemID = value; }
        }

        public int ContentPackID
        {
            get { return this._ContentPackID; }
            set { this._ContentPackID = value; }
        }

        public string ContentItemTitle
        {
            get { return this._ContentItemTitle; }
            set { this._ContentItemTitle = value; }
        }

        public Byte[] ContentPackDataLarge
        {
            get { return this._ContentPackDataLarge; }
            set { this._ContentPackDataLarge = value; }
        }

        public Byte[] ContentPackDataMedium
        {
            get { return this._ContentPackDataMedium; }
            set { this._ContentPackDataMedium = value; }
        }

        public Byte[] ContentPackDataTiny
        {
            get { return this._ContentPackDataTiny; }
            set { this._ContentPackDataTiny = value; }
        }

        public Byte[] ContentPackDataSmall
        {
            get { return this._ContentPackDataSmall; }
            set { this._ContentPackDataSmall = value; }
        }


        public byte[] ContentPackItemIcon
        {
            get { return ImageHandler.imageToByteArray(this._ContentPackItemIcon); }
            set { this._ContentPackItemIcon = ImageHandler.byteArrayToImage(value); }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Resident class
        /// </summary>
        public ContentPackItem()
        {
            _ContentPackItemID = 0;
            _ContentPackID = 0;
            _ContentItemTitle = string.Empty;
            _ContentPackDataLarge = null;
            _ContentPackDataMedium = null;
            _ContentPackDataSmall = null;
            _ContentPackDataTiny = null;
            _ContentPackItemIcon = null;
        }

        /// <summary>
        /// Initializes a new instance of the ContentPackItem class
        /// </summary>
        /// <param name="residentID">Unique Database ID</param>
        public ContentPackItem(int blankID)
        {

            _ContentPackItemID = blankID;
            _ContentPackID = 0;
            _ContentItemTitle = string.Empty;
            _ContentPackDataLarge = null;
            _ContentPackDataMedium = null;
            _ContentPackDataSmall = null;
            _ContentPackDataTiny = null;

            _ContentPackItemIcon = null;

            this.Get();

        }


        public ContentPackItem(DataRow dr)
        {


            try
            {

                _ContentPackItemID = 0;
                _ContentPackID = 0;
                _ContentItemTitle = string.Empty;
                _ContentPackDataLarge = null;
                _ContentPackDataMedium = null;
                _ContentPackDataSmall = null;
                _ContentPackDataTiny = null;

                _ContentPackItemIcon = null;

                if (!DBNull.Value.Equals(dr["ContentPackItemID"])) { this._ContentPackItemID = (int)dr["ContentPackItemID"]; }
                if (!DBNull.Value.Equals(dr["ContentPackID"])) { this._ContentPackID = (int)dr["ContentPackID"]; }
                if (!DBNull.Value.Equals(dr["ContentItemTitle"])) { this._ContentItemTitle = (string)dr["ContentItemTitle"]; }
                if (!DBNull.Value.Equals(dr["ContentPackDataLarge"])) { this._ContentPackDataLarge = (Byte[])dr["ContentPackDataLarge"]; }
                if (!DBNull.Value.Equals(dr["ContentPackDataMedium"])) { this._ContentPackDataMedium = (Byte[])dr["ContentPackDataMedium"]; }
                if (!DBNull.Value.Equals(dr["ContentPackDataSmall"])) { this._ContentPackDataSmall = (Byte[])dr["ContentPackDataSmall"]; }
                if (!DBNull.Value.Equals(dr["ContentPackDataTiny"])) { this._ContentPackDataTiny = (Byte[])dr["ContentPackDataTiny"]; }
                if (!DBNull.Value.Equals(dr["ContentPackItemIcon"])) { _ContentPackItemIcon = ImageHandler.byteArrayToImage((Byte[])dr["ContentPackItemIcon"]); }

            }
            catch (Exception ex)
            {

                _ContentPackItemID = 0;
                _ContentPackID = 0;
                _ContentItemTitle = string.Empty;
                _ContentPackDataLarge = null;
                _ContentPackDataMedium = null;
                _ContentPackDataSmall = null;
                _ContentPackDataTiny = null;
                _ContentPackItemIcon = null;

                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "47";
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
                cmd.CommandText = "SELECT * FROM ContentPackItems WHERE ContentPackItemID = @ContentPackItemID";
                cmd.Parameters.Add("@ContentPackItemID", SqlDbType.Int).Value = _ContentPackItemID;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (!DBNull.Value.Equals(dr["ContentPackID"])) { this._ContentPackID = (int)dr["ContentPackID"]; }
                    if (!DBNull.Value.Equals(dr["ContentItemTitle"])) { this._ContentItemTitle = (string)dr["ContentItemTitle"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackDataLarge"])) { this._ContentPackDataLarge = (Byte[])dr["ContentPackDataLarge"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackDataMedium"])) { this._ContentPackDataMedium = (Byte[])dr["ContentPackDataMedium"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackDataSmall"])) { this._ContentPackDataSmall = (Byte[])dr["ContentPackDataSmall"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackDataTiny"])) { this._ContentPackDataTiny = (Byte[])dr["ContentPackDataTiny"]; }
                    if (!DBNull.Value.Equals(dr["ContentPackItemIcon"])) { _ContentPackItemIcon = ImageHandler.byteArrayToImage((Byte[])dr["ContentPackItemIcon"]); }

                }
                else
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "48";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                    this.Errors.Add(tmpError);
                }

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "49";
                tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                tmpError.ErrorTitle = "Unable To Load";
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }
        }

        public DataSet GetAll (int ContentPackID)
        {
            
                DataSet ds = null;

                this.dt = new DataTools();

                this.Errors = new List<General.Error>();

                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM ContentPackItems WHERE ContentPackID=@ContentPackID";
                    cmd.Parameters.Add("@ContentPackID", SqlDbType.Int).Value = ContentPackID;
                    ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        if (!DBNull.Value.Equals(dr["ContentPackID"])) { this._ContentPackID = (int)dr["ContentPackID"]; }
                        if (!DBNull.Value.Equals(dr["ContentItemTitle"])) { this._ContentItemTitle = (string)dr["ContentItemTitle"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackDataLarge"])) { this._ContentPackDataLarge = (Byte[])dr["ContentPackDataLarge"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackDataMedium"])) { this._ContentPackDataMedium = (Byte[])dr["ContentPackDataMedium"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackDataSmall"])) { this._ContentPackDataSmall = (Byte[])dr["ContentPackDataSmall"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackDataTiny"])) { this._ContentPackDataTiny = (Byte[])dr["ContentPackDataTiny"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackItemIcon"])) { _ContentPackItemIcon = ImageHandler.byteArrayToImage((Byte[])dr["ContentPackItemIcon"]); }
                        return ds;
                    }
                    else
                    {
                        General.Error tmpError = new General.Error();
                        tmpError.ErrorNumber = "48";
                        tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                        tmpError.ErrorTitle = "Unable To Load";
                        tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                        this.Errors.Add(tmpError);
                    }

                }
                catch (Exception ex)
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "49";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = ex.Message;
                    this.Errors.Add(tmpError);
                }

                return ds;  
        }


        public ContentPackItem GetItemByID(int ContentPackItemID)
        {
                DataSet ds = null;

                this.dt = new DataTools();
                
                var items = new ContentPackItem();

                this.Errors = new List<General.Error>();

                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM ContentPackItems WHERE ContentPackItemID = @ContentPackItemID";
                    cmd.Parameters.Add("@ContentPackItemID", SqlDbType.Int).Value = ContentPackItemID;
                    ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        if (!DBNull.Value.Equals(dr["ContentPackID"])) { items._ContentPackID = this._ContentPackID = (int)dr["ContentPackID"]; }
                        if (!DBNull.Value.Equals(dr["ContentItemTitle"])) { items._ContentItemTitle = this._ContentItemTitle = (string)dr["ContentItemTitle"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackDataLarge"])) { items._ContentPackDataLarge = this._ContentPackDataLarge = (Byte[])dr["ContentPackDataLarge"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackDataMedium"])) { items._ContentPackDataMedium = this._ContentPackDataMedium = (Byte[])dr["ContentPackDataMedium"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackDataSmall"])) { items._ContentPackDataSmall = this._ContentPackDataSmall = (Byte[])dr["ContentPackDataSmall"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackDataTiny"])) { items._ContentPackDataTiny = this._ContentPackDataTiny = (Byte[])dr["ContentPackDataTiny"]; }
                        if (!DBNull.Value.Equals(dr["ContentPackItemIcon"])) { items._ContentPackItemIcon = _ContentPackItemIcon = ImageHandler.byteArrayToImage((Byte[])dr["ContentPackItemIcon"]); }        
                    }
                        
                    else
                    {
                        General.Error tmpError = new General.Error();
                        tmpError.ErrorNumber = "48";
                        tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                        tmpError.ErrorTitle = "Unable To Load";
                        tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                        this.Errors.Add(tmpError);
                    }

                }
                catch (Exception ex)
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "49";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = ex.Message;
                    this.Errors.Add(tmpError);
                }

                return items;
 
        }




        public void Save()
        {
            this.DataValidation();

            if (this.Errors.Count != 0) { return; }

            this.dt = new DataTools();

            try
            {

                SqlCommand cmd = new SqlCommand();
                if (this._ContentPackItemID == 0)
                {
                    cmd.CommandText = "INSERT INTO ContentPackItems (ContentPackID, ContentItemTitle, ContentPackDataLarge, ContentPackDataMedium, ContentPackDataSmall, ContentPackDataTiny, ContentPackItemIcon) VALUES (@ContentPackID, @ContentItemTitle, @ContentPackDataLarge, @ContentPackDataMedium, @ContentPackDataSmall, @ContentPackDataTiny, @ContentPackItemIcon); SELECT @@IDENTITY;";
                }
                else
                {
                    cmd.CommandText = "UPDATE ContentPackItems SET ContentPackID = @ContentPackID, ContentItemTitle = @ContentItemTitle, ContentPackDataLarge = @ContentPackDataLarge, ContentPackDataMedium = @ContentPackDataMedium, ContentPackDataSmall = @ContentPackDataSmall, ContentPackDataTiny = @ContentPackDataTiny, ContentPackItemIcon = @ContentPackItemIcon WHERE ContentPackItemID = @ContentPackItemID;";
                    cmd.Parameters.Add("@ContentPackItemID", SqlDbType.Int).Value = this._ContentPackItemID;
                }

                cmd.Parameters.Add("@ContentPackID", SqlDbType.Int).Value = this.ContentPackID;
                cmd.Parameters.Add("@ContentItemTitle", SqlDbType.NVarChar, 25).Value = this.ContentItemTitle;
                cmd.Parameters.Add("@ContentPackDataLarge", SqlDbType.VarBinary).Value = this.ContentPackDataLarge;
                cmd.Parameters.Add("@ContentPackDataMedium", SqlDbType.VarBinary).Value = this.ContentPackDataMedium;
                cmd.Parameters.Add("@ContentPackDataSmall", SqlDbType.VarBinary).Value = this.ContentPackDataSmall;
                cmd.Parameters.Add("@ContentPackDataTiny", SqlDbType.VarBinary).Value = this.ContentPackDataTiny;
                cmd.Parameters.Add("@ContentPackItemIcon", SqlDbType.Image).Value = this.ContentPackItemIcon;

                object tmpResults = this.dt.ExecuteScalar(cmd, DataTools.DataSources.LOLAccountManagement);

                if (this._ContentPackItemID == 0)
                {
                    this._ContentPackItemID = int.Parse(tmpResults.ToString());
                }


            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "50";
                tmpError.ErrorLocation = this.ToString() + ".Save()";
                tmpError.ErrorTitle = "Unable To Save";
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }
        }

        public void Delete(int ContentPackItemID)
        {

            SqlCommand cmd = new SqlCommand();
            dt = new DataTools();
            Errors = new List<General.Error>();

            try
            {
                cmd.CommandText = "DELETE FROM ContentPackItems WHERE ContentPackItemID = @ContentPackItemID";
                cmd.Parameters.Add("@ContentPackItemID", SqlDbType.Int).Value = ContentPackItemID;
                dt.ExecuteNonQuery(cmd, DataTools.DataSources.LOLAccountManagement);
            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "51";
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
