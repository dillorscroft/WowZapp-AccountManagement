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
    


    
    public sealed class Contact
    {

        public struct ContactOAuth
        {
            public string OAuthID;
            public DeviceOAuth.OAuthTypes OAuthType;
        }

        #region PrivateVariables

        private DataTools dt = new DataTools();
        private Guid _ContactID;
        private Guid _OwnerAccountID;
        private Guid _ContactAccountID;
        private bool _Blocked;
        private User _ContactUser;
        private List<ContactOAuth> _ContactOAuths;

        #endregion PrivateVariables

        #region Properties

        
        public List<General.Error> Errors = new List<General.Error>();

        
        public Guid ContactID
        {
            get { return this._ContactID; }
            set { this._ContactID = value; }
        }

        
        public Guid OwnerAccountID
        {
            get { return this._OwnerAccountID; }
            set { this._OwnerAccountID = value; }
        }

        
        public Guid ContactAccountID
        {
            get { return this._ContactAccountID; }
            set { this._ContactAccountID = value; }
        }

        
        public bool Blocked
        {
            get { return this._Blocked; }
            set { this._Blocked = value; }
        }

        
        public User ContactUser
        {
            get { return this._ContactUser; }
            set { this._ContactUser = value; }
        }

        
        public List<ContactOAuth> ContactOAuths
        {
            get { return this._ContactOAuths; }
            set { this._ContactOAuths = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Resident class
        /// </summary>
        public Contact()
        {
            _ContactID = new Guid("00000000-0000-0000-0000-000000000000");
            _OwnerAccountID = new Guid("00000000-0000-0000-0000-000000000000");
            _ContactAccountID = new Guid("00000000-0000-0000-0000-000000000000");
            _ContactUser = new User();
            _ContactOAuths = new List<ContactOAuth>();
            _Blocked = false;
        }

        /// <summary>
        /// Initializes a new instance of the Contact class
        /// </summary>
        /// <param name="residentID">Unique Database ID</param>
        public Contact(Guid contactID)
        {

            _ContactID = contactID;
            _OwnerAccountID = new Guid("00000000-0000-0000-0000-000000000000");
            _ContactAccountID = new Guid("00000000-0000-0000-0000-000000000000");
            _ContactUser = new User();
            _ContactOAuths = new List<ContactOAuth>();
            _Blocked = false;

            this.Get();

            _ContactUser = new User(_ContactAccountID);
            GetOAuths();


        }


        public Contact(DataRow dr)
        {


            try
            {

                _ContactID = new Guid("00000000-0000-0000-0000-000000000000");
                _OwnerAccountID = new Guid("00000000-0000-0000-0000-000000000000");
                _ContactAccountID = new Guid("00000000-0000-0000-0000-000000000000");
                _ContactUser = new User();
                _ContactOAuths = new List<ContactOAuth>();
                _Blocked = false;

                if (!DBNull.Value.Equals(dr["ContactID"])) { this._ContactID = (Guid)dr["ContactID"]; }
                if (!DBNull.Value.Equals(dr["OwnerAccountID"])) { this._OwnerAccountID = (Guid)dr["OwnerAccountID"]; }
                if (!DBNull.Value.Equals(dr["ContactAccountID"])) { this._ContactAccountID = (Guid)dr["ContactAccountID"]; }
                if (!DBNull.Value.Equals(dr["Blocked"])) { this._Blocked = (bool)dr["Blocked"]; }

                _ContactUser = new User(_ContactAccountID);
                GetOAuths();

            }
            catch (Exception ex)
            {

                _ContactID = new Guid("00000000-0000-0000-0000-000000000000");
                _OwnerAccountID = new Guid("00000000-0000-0000-0000-000000000000");
                _ContactAccountID = new Guid("00000000-0000-0000-0000-000000000000");
                _ContactUser = new User();
                _ContactOAuths = new List<ContactOAuth>();
                _Blocked = false;

                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "37";
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
                cmd.CommandText = "SELECT * FROM Contacts WHERE ContactID = @ContactID";
                cmd.Parameters.Add("@ContactID", SqlDbType.UniqueIdentifier).Value = _ContactID; ;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (!DBNull.Value.Equals(dr["OwnerAccountID"])) { this._OwnerAccountID = (Guid)dr["OwnerAccountID"]; }
                    if (!DBNull.Value.Equals(dr["ContactAccountID"])) { this._ContactAccountID = (Guid)dr["ContactAccountID"]; }
                    if (!DBNull.Value.Equals(dr["Blocked"])) { this._Blocked = (bool)dr["Blocked"]; }

                }
                else
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "38";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                    this.Errors.Add(tmpError);
                }

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "39";
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
                if (this._ContactID == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    cmd.CommandText = "INSERT INTO Contacts (ContactID, OwnerAccountID, ContactAccountID, Blocked) VALUES (@ContactID, @OwnerAccountID, @ContactAccountID, @Blocked);";
                    _ContactID = System.Guid.NewGuid();
                }
                else
                {
                    cmd.CommandText = "UPDATE Contacts SET OwnerAccountID = @OwnerAccountID, ContactAccountID = @ContactAccountID, Blocked = @Blocked WHERE ContactID = @ContactID;";
                }

                cmd.Parameters.Add("@ContactID", SqlDbType.UniqueIdentifier).Value = this._ContactID;
                cmd.Parameters.Add("@OwnerAccountID", SqlDbType.UniqueIdentifier).Value = this._OwnerAccountID;
                cmd.Parameters.Add("@ContactAccountID", SqlDbType.UniqueIdentifier).Value = this._ContactAccountID;
                cmd.Parameters.Add("@Blocked", SqlDbType.Bit).Value = this._Blocked;

                object tmpResults = this.dt.ExecuteScalar(cmd, DataTools.DataSources.LOLAccountManagement);

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "40";
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
                cmd.CommandText = "DELETE FROM Contacts WHERE ContactID = @ContactID";
                cmd.Parameters.Add("@ContactID", SqlDbType.Int).Value = _ContactID;
                dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);
            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "41";
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

        private void GetOAuths()
        {
            if(_ContactAccountID != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT DISTINCT OAuthID, OAuthTypeID FROM DeviceOAuth WHERE DeviceID IN (SELECT DeviceID FROM Devices WHERE AccountID = @AccountID)";
                cmd.Parameters.Add("@AccountID",SqlDbType.UniqueIdentifier).Value = _ContactAccountID;

                DataSet ds = dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ContactOAuth tmpOAuth = new ContactOAuth();
                    tmpOAuth.OAuthID = (string)dr["OAuthID"];
                    tmpOAuth.OAuthType = (DeviceOAuth.OAuthTypes)dr["OAuthTypeID"];
                    _ContactOAuths.Add(tmpOAuth);
                }
            }

        }



        #endregion Methods
    }
}
