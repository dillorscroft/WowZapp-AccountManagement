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


    
    public sealed class User
    {

        #region PrivateVariables

        private DataTools dt = new DataTools();
        private Guid _AccountID;
        private string _FirstName;
        private string _LastName;
        private string _EmailAddress;
        private Image _Picture;
        private string _Password;
        private bool _AccountActive;
        private DateTime _DateOfBirth;
        private string _PictureURL;

        #endregion PrivateVariables

        #region Properties

        
        public List<General.Error> Errors = new List<General.Error>();

        
        public Guid AccountID
        {
            get { return this._AccountID; }
            set { this._AccountID = value; }
        }

        
        public string FirstName
        {
            get { return this._FirstName; }
            set { this._FirstName = value; }
        }

        
        public string LastName
        {
            get { return this._LastName; }
            set { this._LastName = value; }
        }
        
        public string EmailAddress
        {
            get { return this._EmailAddress; }
            set { this._EmailAddress = value; }
        }

        
        public byte[] Picture
        {
            get { return ImageHandler.imageToByteArray(this._Picture); }
            set { this._Picture = ImageHandler.byteArrayToImage(value); }
        }

        
        public string Password
        {
            get { return this._Password; }
            set { this._Password = value; }
        }

        
        public bool AccountActive
        {
            get { return this._AccountActive; }
            set { this._AccountActive = value; }
        }

        
        public DateTime DateOfBirth
        {
            get { return this._DateOfBirth; }
            set { this._DateOfBirth = value; }
        }

        
        public string PictureURL
        {
            get { return this._PictureURL; }
            set { this._PictureURL = value; }
        }


        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Resident class
        /// </summary>
        public User()
        {
            _AccountID = new Guid("00000000-0000-0000-0000-000000000000");
            _FirstName = string.Empty;
            _LastName = string.Empty;
            _EmailAddress = string.Empty;
            _Picture = null;
            _Password = string.Empty;
            _AccountActive = false;
            _DateOfBirth = new DateTime(1900,1,1);
            _PictureURL = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the User class
        /// </summary>
        /// <param name="residentID">Unique Database ID</param>
        public User(Guid accountID)
        {

            _AccountID = accountID;
            _FirstName = string.Empty;
            _LastName = string.Empty;
            _EmailAddress = string.Empty;
            _Picture = null;
            _Password = string.Empty;
            _AccountActive = false;
            _DateOfBirth = new DateTime(1900, 1, 1);
            _PictureURL = string.Empty;

            this.Get();

        }


        public User(DataRow dr)
        {


            try
            {

                _AccountID = new Guid("00000000-0000-0000-0000-000000000000");
                _FirstName = string.Empty;
                _LastName = string.Empty;
                _EmailAddress = string.Empty;
                _Picture = null;
                _Password = string.Empty;
                _AccountActive = false;
                _DateOfBirth = new DateTime(1900, 1, 1);
                _PictureURL = string.Empty;

                if (!DBNull.Value.Equals(dr["AccountID"])) { this._AccountID = (Guid)dr["AccountID"]; }
                if (!DBNull.Value.Equals(dr["FirstName"])) { this._FirstName = (string)dr["FirstName"]; }
                if (!DBNull.Value.Equals(dr["LastName"])) { this._LastName = (string)dr["LastName"]; }
                if (!DBNull.Value.Equals(dr["EmailAddress"])) { this._EmailAddress = Cryptography.Decrypt((string)dr["EmailAddress"]); }
                if (!DBNull.Value.Equals(dr["Picture"])) { _Picture = ImageHandler.byteArrayToImage((Byte[])dr["Picture"]); }
                if (!DBNull.Value.Equals(dr["Password"])) { this._Password = Cryptography.Decrypt((string)dr["Password"]); }
                if (!DBNull.Value.Equals(dr["AccountActive"])) { this._AccountActive = (bool)dr["AccountActive"]; }
                if (!DBNull.Value.Equals(dr["DateOfBirth"])) { this._DateOfBirth = (DateTime)dr["DateOfBirth"]; }
                if (!DBNull.Value.Equals(dr["PictureURL"])) { this._PictureURL = (string)dr["PictureURL"]; }

            }
            catch (Exception ex)
            {

                _AccountID = new Guid("00000000-0000-0000-0000-000000000000");
                _FirstName = string.Empty;
                _LastName = string.Empty;
                _EmailAddress = string.Empty;
                _Picture = null;
                _Password = string.Empty;
                _AccountActive = false;
                _DateOfBirth = new DateTime(1900, 1, 1);
                _PictureURL = string.Empty;

                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "27";
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
                cmd.CommandText = "SELECT * FROM Users WHERE AccountID = @AccountID";
                cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = _AccountID; ;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (!DBNull.Value.Equals(dr["AccountID"])) { this._AccountID = (Guid)dr["AccountID"]; }
                    if (!DBNull.Value.Equals(dr["FirstName"])) { this._FirstName = (string)dr["FirstName"]; }
                    if (!DBNull.Value.Equals(dr["LastName"])) { this._LastName = (string)dr["LastName"]; }
                    if (!DBNull.Value.Equals(dr["EmailAddress"])) { this._EmailAddress = Cryptography.Decrypt((string)dr["EmailAddress"]); }
                    if (!DBNull.Value.Equals(dr["Picture"])) { _Picture = ImageHandler.byteArrayToImage((Byte[])dr["Picture"]); }
                    if (!DBNull.Value.Equals(dr["Password"])) { this._Password = Cryptography.Decrypt((string)dr["Password"]); }
                    if (!DBNull.Value.Equals(dr["AccountActive"])) { this._AccountActive = (bool)dr["AccountActive"]; }
                    if (!DBNull.Value.Equals(dr["DateOfBirth"])) { this._DateOfBirth = (DateTime)dr["DateOfBirth"]; }
                    if (!DBNull.Value.Equals(dr["PictureURL"])) { this._PictureURL = (string)dr["PictureURL"]; }
                }
                else
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "28";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                    this.Errors.Add(tmpError);
                }

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "29";
                tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                tmpError.ErrorTitle = "Unable To Load";
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }
        }



        public void Save()
        {
            this.DataValidation();

            if (this.Errors.Count != 0) { return; }

            this.dt = new DataTools();

            try
            {

                SqlCommand cmd = new SqlCommand();
                if (this._AccountID == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    cmd.CommandText = "INSERT INTO Users (AccountID, FirstName, LastName, EmailAddress, Picture, Password, AccountActive, DateOfBirth, PictureURL) VALUES (@AccountID, @FirstName, @LastName, @EmailAddress, @Picture, @Password, @AccountActive, @DateOfBirth, @PictureURL);";
                    _AccountID = System.Guid.NewGuid();
                }
                else
                {
                    cmd.CommandText = "UPDATE Users SET FirstName = @FirstNAme, LastName = @LastName, EmailAddress = @EmailAddress, Picture = @Picture, Password = @Password, AccountActive = @AccountActive, DateOfBirth = @DateOfBirth, PictureURL = @PictureURL WHERE AccountID = @AccountID;";
                }

                cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = this._AccountID;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = this.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = this.LastName;
                cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 300).Value = Cryptography.Encrypt(this.EmailAddress);
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 200).Value = Cryptography.Encrypt(this.Password);
                cmd.Parameters.Add("@Picture", SqlDbType.Image).Value = this.Picture;
                cmd.Parameters.Add("@AccountActive", SqlDbType.Bit).Value = this.AccountActive;
                cmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = this.DateOfBirth;
                cmd.Parameters.Add("@PictureURL", SqlDbType.NVarChar, 200).Value = this._PictureURL;

                object tmpResults = this.dt.ExecuteScalar(cmd, DataTools.DataSources.LOLAccountManagement);

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "30";
                tmpError.ErrorLocation = this.ToString() + ".Save()";
                tmpError.ErrorTitle = "Unable To Save";
                tmpError.ErrorDescription = ex.Message;
                this.Errors.Add(tmpError);
            }
        }

        public void Delete()
        {

            SqlCommand cmd = new SqlCommand();
            dt = new DataTools();
            Errors = new List<General.Error>();

            try
            {
                AccountActive = false;
                Save();
            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "31";
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
