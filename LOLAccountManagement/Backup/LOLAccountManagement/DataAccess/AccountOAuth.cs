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
//using LOLCodeLibrary.ErrorsManagement;
using LOLAccountManagement.Classes.ErrorsMgmt;

namespace LOLAccountManagement
{
    [DataContract]
    public sealed class AccountOAuth
    {

        public enum OAuthTypes
        {
            LOL = 0,
            FaceBook = 1,
            Google = 2,
            YouTube = 3,
            LinkedIn = 4,
            Twitter = 5
        }


        #region PrivateVariables

        private DataTools dt = new DataTools();
        private Guid _AccountID;
        private OAuthTypes _OAuthType;
        private string _OAuthID;
        private string _OAuthToken;

        #endregion PrivateVariables

        #region Properties

        [DataMember]
        public List<General.Error> Errors { get; set; }

        [DataMember]
        public Guid AccountID
        {
            get { return this._AccountID; }
            set { this._AccountID = value; }
        }

        [DataMember]
        public OAuthTypes OAuthType
        {
            get { return this._OAuthType; }
            set { this._OAuthType = value; }
        }

        [DataMember]
        public string OAuthID
        {
            get { return this._OAuthID; }
            set { this._OAuthID = value; }
        }

        [DataMember]
        public string OAuthToken
        {
            get { return this._OAuthToken; }
            set { this._OAuthToken = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Resident class
        /// </summary>
        public AccountOAuth()
        {
            _AccountID = new Guid("00000000-0000-0000-0000-000000000000");
            _OAuthType = OAuthTypes.LOL;
            _OAuthID = string.Empty;
            _OAuthToken = string.Empty;
            this.Errors = new List<General.Error>();
        }

        /// <summary>
        /// Initializes a new instance of the AccountOAuth class
        /// </summary>
        /// <param name="residentID">Unique Database ID</param>
        public AccountOAuth(OAuthTypes oAuthType, string oAuthID)
        {
            this.Errors = new List<General.Error>();
            _AccountID = new Guid("00000000-0000-0000-0000-000000000000");
            _OAuthType = oAuthType;
            _OAuthID = oAuthID;
            _OAuthToken = string.Empty;

            this.Get();

        }


        public AccountOAuth(DataRow dr)
        {
            this.Errors = new List<General.Error>();

            try
            {

                _AccountID = new Guid("00000000-0000-0000-0000-000000000000");
                _OAuthType = OAuthTypes.LOL;
                _OAuthID = string.Empty;
                _OAuthToken = string.Empty;

                if (!DBNull.Value.Equals(dr["AccountID"])) { this._AccountID = (Guid)dr["AccountID"]; }
                if (!DBNull.Value.Equals(dr["OAuthType"])) { this._OAuthType = (OAuthTypes)dr["OAuthType"]; }
                if (!DBNull.Value.Equals(dr["OAuthID"])) { this._OAuthID = (string)dr["OAuthID"]; }
                if (!DBNull.Value.Equals(dr["OAuthToken"])) { this._OAuthToken = (string)dr["OAuthToken"]; }
            }
            catch (Exception ex)
            {

                _AccountID = new Guid("00000000-0000-0000-0000-000000000000");
                _OAuthType = OAuthTypes.LOL;
                _OAuthID = string.Empty;
                _OAuthToken = string.Empty;

                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "24";
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
                cmd.CommandText = "SELECT * FROM AccountOAuth WHERE OAuthTypeID = @OAuthType AND OAuthID = @OAuthID";
                cmd.Parameters.Add("@OAuthID", SqlDbType.NVarChar,100).Value = _OAuthID; ;
                cmd.Parameters.Add("@OAuthType", SqlDbType.Int).Value = (int)_OAuthType;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (!DBNull.Value.Equals(dr["AccountID"])) { this._AccountID = (Guid)dr["AccountID"]; }
                    if (!DBNull.Value.Equals(dr["OAuthType"])) { this._OAuthType = (OAuthTypes)dr["OAuthType"]; }
                    if (!DBNull.Value.Equals(dr["OAuthID"])) { this._OAuthID = (string)dr["OAuthID"]; }
                    if (!DBNull.Value.Equals(dr["OAuthToken"])) { this._OAuthToken = (string)dr["OAuthToken"]; }

                }
                else
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorNumber = "25";
                    tmpError.ErrorLocation = this.ToString() + ".Get" + this.ToString();
                    tmpError.ErrorTitle = "Unable To Load";
                    tmpError.ErrorDescription = "Unable to load " + this.ToString() + " record";
                    this.Errors.Add(tmpError);
                }

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "26";
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
                cmd.CommandText = "SELECT * FROM AccountOAuth WHERE OAuthTypeID = @OAuthType AND OAuthID = @OAuthID";
                cmd.Parameters.Add("@OAuthID", SqlDbType.NVarChar, 100).Value = _OAuthID; ;
                cmd.Parameters.Add("@OAuthType", SqlDbType.Int).Value = (int)_OAuthType;
                DataSet ds = this.dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);
                cmd = new SqlCommand();
                if (ds.Tables[0].Rows.Count == 0)
                {
                    cmd.CommandText = "INSERT INTO AccountOAuth (AccountID, OAuthTypeID, OAuthID, OAuthToken) VALUES (@AccountID, @OAuthType, @OAuthID, @OAuthToken);";
                }
                else
                {
                    cmd.CommandText = "UPDATE AccountOAuth SET AccountID = @AccountID, OAuthToken = @OAuthToken WHERE OAuthTypeID = @OAuthType AND OAuthID = @OAuthID;";
                }

                cmd.Parameters.Add("@OAuthID", SqlDbType.NVarChar, 100).Value = _OAuthID;
                cmd.Parameters.Add("@OAuthType", SqlDbType.Int).Value = (int)OAuthType;
                cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = _AccountID;
                cmd.Parameters.Add("@OAuthToken", SqlDbType.NVarChar, 100).Value = _OAuthToken;

                object tmpResults = this.dt.ExecuteScalar(cmd, DataTools.DataSources.LOLAccountManagement);

            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "22";
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
                // Does not support delete
            }
            catch (Exception ex)
            {
                General.Error tmpError = new General.Error();
                tmpError.ErrorNumber = "23";
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
