using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using SnapObjects.Data;
using DWNet.Data;
using Newtonsoft.Json;
using System.Collections;

namespace delconsdb_api.Models
{
    [DataWindow("approval_test", DwStyle.Default)]
    [Table("APPROVAL_TEST")]
    #region DwSelectAttribute  
    [DwSelect("PBSELECT( VERSION(400) TABLE(NAME=\"APPROVAL_TEST\" ) @(_COLUMNS_PLACEHOLDER_) )")]
    #endregion
    [UpdateWhereStrategy(UpdateWhereStrategy.KeyColumns)]
    [DwKeyModificationStrategy(UpdateSqlStrategy.Update)]
    public class Approval_Test
    {
        [Key]
        [Identity]
        [DwColumn("APPROVAL_TEST", "ID")]
        public int Id { get; set; }

        [DwColumn("APPROVAL_TEST", "COMPANY_CODE")]
        public decimal Company_Code { get; set; }

        [StringLength(3)]
        [DwColumn("APPROVAL_TEST", "MODULE")]
        public string Module { get; set; }

        [StringLength(20)]
        [DwColumn("APPROVAL_TEST", "REPORTSYS")]
        public string Reportsys { get; set; }

        [StringLength(100)]
        [DwColumn("APPROVAL_TEST", "NAME")]
        public string Name { get; set; }

        [StringLength(32000)]
        [DwColumn("APPROVAL_TEST", "PARAMETERS")]
        public string Parameters { get; set; }

        [StringLength(100)]
        [DwColumn("APPROVAL_TEST", "DWOBJECT")]
        public string Dwobject { get; set; }

        [DwColumn("APPROVAL_TEST", "PRIORITY")]
        public int Priority { get; set; }

        [DwColumn("APPROVAL_TEST", "CREATED_DATE", TypeName = "datetime")]
        public DateTime Created_Date { get; set; }

        [StringLength(6)]
        [DwColumn("APPROVAL_TEST", "CREATED_BY")]
        public string Created_By { get; set; }

        [StringLength(50)]
        [DwColumn("APPROVAL_TEST", "ASSIGNED_SERVER")]
        public string Assigned_Server { get; set; }

        [DwColumn("APPROVAL_TEST", "ASSIGNED_WHEN", TypeName = "datetime")]
        public DateTime? Assigned_When { get; set; }

        [StringLength(32000)]
        [DwColumn("APPROVAL_TEST", "QUERY")]
        public string Query { get; set; }

        [DwColumn("APPROVAL_TEST", "PROCESSING", TypeName = "datetime")]
        public DateTime? Processing { get; set; }

        [DwColumn("APPROVAL_TEST", "PROCESSED", TypeName = "datetime")]
        public DateTime? Processed { get; set; }

        [StringLength(32000)]
        [DwColumn("APPROVAL_TEST", "ERROR_RESULT")]
        public string Error_Result { get; set; }

        [StringLength(6)]
        [DwColumn("APPROVAL_TEST", "USER_ID")]
        public string User_Id { get; set; }

        [DwColumn("APPROVAL_TEST", "SCHEDULE_WHEN", TypeName = "datetime")]
        public DateTime? Schedule_When { get; set; }

        [DwColumn("APPROVAL_TEST", "PERCENT_COMPLETED")]
        public double? Percent_Completed { get; set; }

        [DwColumn("APPROVAL_TEST", "STATUS")]
        public int? Status { get; set; }

        [StringLength(6)]
        [DwColumn("APPROVAL_TEST", "ASSIGNED_TO")]
        public string Assigned_To { get; set; }

        [StringLength(255)]
        [DwColumn("APPROVAL_TEST", "MESSAGE")]
        public string Message { get; set; }

        [StringLength(1)]
        [DwColumn("APPROVAL_TEST", "DOC_LOCKED_IND")]
        public string Doc_Locked_Ind { get; set; }

        [StringLength(20)]
        [DwColumn("APPROVAL_TEST", "APP_KEY")]
        public string App_Key { get; set; }

        [StringLength(150)]
        [DwColumn("APPROVAL_TEST", "SUBJECT")]
        public string Subject { get; set; }

        [StringLength(60)]
        [DwColumn("APPROVAL_TEST", "EMAIL")]
        public string Email { get; set; }

        [StringLength(8)]
        [DwColumn("APPROVAL_TEST", "ALERT_ID")]
        public string Alert_Id { get; set; }

        [StringLength(3)]
        [DwColumn("APPROVAL_TEST", "APPROVE_IND")]
        public string Approve_Ind { get; set; }

    }

}