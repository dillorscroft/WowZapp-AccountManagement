using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using LOLAccountManagement.Classes;
///using LOLCodeLibrary.ErrorsManagement;

namespace LOLAccountManagement
{
    [DataContract]
    public sealed class Contact
    {

        public struct ContactOAuth
        {
            public string OAuthID;
            public AccountOAuth.OAuthTypes OAuthType;
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

        [DataMember]
        public List<General.Error> Errors = new List<General.Error>();

        [DataMember]
        public Guid ContactID
        {
            get { return this._ContactID; }
            set { this._ContactID = value; }
        }

        [DataMember]
        public Guid OwnerAccountID
        {
            get { return this._OwnerAccountID; }
            set { this._OwnerAccountID = value; }
        }

        [DataMember]
        public Guid ContactAccountID
        {
            get { return this._ContactAccountID; }
            set { this._ContactAccountID = value; }
        }

        [DataMember]
        public bool Blocked
        {
            get { return this._Blocked; }
            set { this._Blocked = value; }
        }

        [DataMember]
        public User ContactUser
        {
            get { return this._ContactUser; }
            set { this._ContactUser = value; }
        }

        [DataMember]
        public List<ContactOAuth> ContactOAuths
        {
            get { return this._ContactOAuths; }
            set { this._ContactOAuths = value; }
        }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime DateLastUpdated { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Resident class
        /// </summary>
        public Contact()
        {
            _ContactID = Guid.Empty;
            _OwnerAccountID = Guid.Empty;
            _ContactAccountID = Guid.Empty;
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
            _OwnerAccountID = Guid.Empty;
            _ContactAccountID = Guid.Empty;
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
                _ContactID = Guid.Empty;
                _OwnerAccountID = Guid.Empty;
                _ContactAccountID = Guid.Empty;
                _ContactUser = new User();
                _ContactOAuths = new List<ContactOAuth>();
                _Blocked = false;

                if (!DBNull.Value.Equals(dr["ContactID"])) { this._ContactID = (Guid)dr["ContactID"]; }
                if (!DBNull.Value.Equals(dr["OwnerAccountID"])) { this._OwnerAccountID = (Guid)dr["OwnerAccountID"]; }
                if (!DBNull.Value.Equals(dr["ContactAccountID"])) { this._ContactAccountID = (Guid)dr["ContactAccountID"]; }
                if (!DBNull.Value.Equals(dr["Blocked"])) { this._Blocked = (bool)dr["Blocked"]; }

                if (dr.Table.Columns.Contains("AccountID"))
                    _ContactUser = new User(dr);
                else
                    _ContactUser = new User(_ContactAccountID);
                GetOAuths();

            }
            catch (Exception ex)
            {

                _ContactID = Guid.Empty;
                _OwnerAccountID = Guid.Empty;
                _ContactAccountID = Guid.Empty;
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
                if (this._ContactID == Guid.Empty)
                {
                    cmd.CommandText = "INSERT INTO Contacts (ContactID, OwnerAccountID, ContactAccountID, Blocked, DateLastUpdated) VALUES (@ContactID, @OwnerAccountID, @ContactAccountID, @Blocked, GETDATE());";
                    _ContactID = System.Guid.NewGuid();
                }
                else
                {
                    cmd.CommandText = "UPDATE Contacts SET OwnerAccountID = @OwnerAccountID, ContactAccountID = @ContactAccountID, Blocked = @Blocked, DateLastUpdated = GETDATE() WHERE ContactID = @ContactID;";
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

                LOLAccountRepository repo = new LOLAccountRepository(dt);
                repo.MethodName = "ContactSave";
                repo.MethodParameters = new List<MethodParameter>();
                repo.MethodParameters.Add( new MethodParameter("Blocked", this._Blocked.ToString()));
                repo.MethodParameters.Add(new MethodParameter("ContactAccountID", this._ContactAccountID.ToString()));
                repo.MethodParameters.Add(new MethodParameter("ContactID", this._ContactID.ToString()));

                if (this._ContactOAuths != null && this._ContactOAuths.Count > 0)
                {
                    for (int i = 0; i < this._ContactOAuths.Count; i++)
                    {
                        repo.MethodParameters.Add(new MethodParameter("ContactOAuths_" + i.ToString(), "OAuthID : " + this._ContactOAuths[i].OAuthID + "_" + ", OAuthType : " + this._ContactOAuths[i].OAuthType.ToString()));
                    }
                }

                repo.MethodParameters.Add(new MethodParameter("OwnerAccountID", this._OwnerAccountID.ToString()));
                repo.LastErrorMessage = ex.Message;
                repo.LogError();
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
                cmd.Parameters.Add("@ContactID", SqlDbType.UniqueIdentifier).Value = _ContactID;
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
            if(_ContactAccountID != Guid.Empty)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT DISTINCT OAuthID, OAuthTypeID FROM AccountOAuth WHERE AccountID = @AccountID";
                cmd.Parameters.Add("@AccountID",SqlDbType.UniqueIdentifier).Value = _ContactAccountID;

                DataSet ds = dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ContactOAuth tmpOAuth = new ContactOAuth();
                    tmpOAuth.OAuthID = (string)dr["OAuthID"];
                    tmpOAuth.OAuthType = (AccountOAuth.OAuthTypes)dr["OAuthTypeID"];
                    _ContactOAuths.Add(tmpOAuth);
                }
            }

        }



        #endregion Methods
    }
}
