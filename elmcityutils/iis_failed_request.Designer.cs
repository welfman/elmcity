﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Data.Objects.DataClasses.EdmSchemaAttribute()]

// Original file name:
// Generation date: 8/12/2010 4:55:28 PM
namespace ElmcityUtils
{
    
    /// <summary>
    /// There are no comments for iis_failed_request_entities in the schema.
    /// </summary>
    public partial class iis_failed_request_entities : global::System.Data.Objects.ObjectContext
    {
        /// <summary>
        /// Initializes a new iis_failed_request_entities object using the connection string found in the 'iis_failed_request_entities' section of the application configuration file.
        /// </summary>
        public iis_failed_request_entities() : 
                base("name=iis_failed_request_entities", "iis_failed_request_entities")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Initialize a new iis_failed_request_entities object.
        /// </summary>
        public iis_failed_request_entities(string connectionString) : 
                base(connectionString, "iis_failed_request_entities")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Initialize a new iis_failed_request_entities object.
        /// </summary>
        public iis_failed_request_entities(global::System.Data.EntityClient.EntityConnection connection) : 
                base(connection, "iis_failed_request_entities")
        {
            this.OnContextCreated();
        }
        partial void OnContextCreated();
        /// <summary>
        /// There are no comments for iis_failed_request in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Data.Objects.ObjectQuery<iis_failed_request> iis_failed_request
        {
            get
            {
                if ((this._iis_failed_request == null))
                {
                    this._iis_failed_request = base.CreateQuery<iis_failed_request>("[iis_failed_request]");
                }
                return this._iis_failed_request;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Data.Objects.ObjectQuery<iis_failed_request> _iis_failed_request;
        /// <summary>
        /// There are no comments for iis_failed_request in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToiis_failed_request(iis_failed_request iis_failed_request)
        {
            base.AddObject("iis_failed_request", iis_failed_request);
        }
    }
    /// <summary>
    /// There are no comments for iis_failed_request.iis_failed_request in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="iis_failed_request", Name="iis_failed_request")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    public partial class iis_failed_request : global::System.Data.Objects.DataClasses.EntityObject
    {
        /// <summary>
        /// Create a new iis_failed_request object.
        /// </summary>
        /// <param name="id">Initial value of ID.</param>
        /// <param name="url">Initial value of url.</param>
        /// <param name="status">Initial value of status.</param>
        /// <param name="duration">Initial value of duration.</param>
        /// <param name="created">Initial value of created.</param>
        /// <param name="computer">Initial value of computer.</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static iis_failed_request Createiis_failed_request(int id, string url, int status, int duration, global::System.DateTime created, string computer)
        {
            iis_failed_request iis_failed_request = new iis_failed_request();
            iis_failed_request.ID = id;
            iis_failed_request.url = url;
            iis_failed_request.status = status;
            iis_failed_request.duration = duration;
            iis_failed_request.created = created;
            iis_failed_request.computer = computer;
            return iis_failed_request;
        }
        /// <summary>
        /// There are no comments for property ID in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public int ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this.OnIDChanging(value);
                this.ReportPropertyChanging("ID");
                this._ID = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("ID");
                this.OnIDChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private int _ID;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIDChanging(int value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIDChanged();
        /// <summary>
        /// There are no comments for property url in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string url
        {
            get
            {
                return this._url;
            }
            set
            {
                this.OnurlChanging(value);
                this.ReportPropertyChanging("url");
                this._url = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("url");
                this.OnurlChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _url;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnurlChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnurlChanged();
        /// <summary>
        /// There are no comments for property reason in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string reason
        {
            get
            {
                return this._reason;
            }
            set
            {
                this.OnreasonChanging(value);
                this.ReportPropertyChanging("reason");
                this._reason = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("reason");
                this.OnreasonChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _reason;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnreasonChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnreasonChanged();
        /// <summary>
        /// There are no comments for property status in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public int status
        {
            get
            {
                return this._status;
            }
            set
            {
                this.OnstatusChanging(value);
                this.ReportPropertyChanging("status");
                this._status = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("status");
                this.OnstatusChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private int _status;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnstatusChanging(int value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnstatusChanged();
        /// <summary>
        /// There are no comments for property duration in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public int duration
        {
            get
            {
                return this._duration;
            }
            set
            {
                this.OndurationChanging(value);
                this.ReportPropertyChanging("duration");
                this._duration = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("duration");
                this.OndurationChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private int _duration;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OndurationChanging(int value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OndurationChanged();
        /// <summary>
        /// There are no comments for property created in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.DateTime created
        {
            get
            {
                return this._created;
            }
            set
            {
                this.OncreatedChanging(value);
                this.ReportPropertyChanging("created");
                this._created = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("created");
                this.OncreatedChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.DateTime _created;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OncreatedChanging(global::System.DateTime value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OncreatedChanged();
        /// <summary>
        /// There are no comments for property computer in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string computer
        {
            get
            {
                return this._computer;
            }
            set
            {
                this.OncomputerChanging(value);
                this.ReportPropertyChanging("computer");
                this._computer = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("computer");
                this.OncomputerChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _computer;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OncomputerChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OncomputerChanged();
    }
}