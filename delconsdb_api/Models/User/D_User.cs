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
    [DataWindow("d_user", DwStyle.Default)]
    [Table("APP_USER")]
    #region DwSelectAttribute  
    [DwSelect("SELECT @(_COLUMNS_PLACEHOLDER_) \r\n "
                  +"FROM APP_USER")]
    #endregion
    [DwKeyModificationStrategy(UpdateSqlStrategy.DeleteThenInsert)]
    [UpdateWhereStrategy(UpdateWhereStrategy.KeyAndConcurrencyCheckColumns)]
    public class D_User
    {
        [Key]
        [StringLength(6)]
        [DwColumn("APP_USER", "USER_ID")]
        public string User_Id { get; set; }

        [ConcurrencyCheck]
        [StringLength(60)]
        [DwColumn("APP_USER", "USER_NAME")]
        public string User_Name { get; set; }

        [ConcurrencyCheck]
        [StringLength(60)]
        [DwColumn("APP_USER", "PASSWD")]
        public string Passwd { get; set; }

        [ConcurrencyCheck]
        [StringLength(6)]
        [DwColumn("APP_USER", "CUSTOMER_CODE")]
        public string Customer_Code { get; set; }

        [ConcurrencyCheck]
        [StringLength(40)]
        [DwColumn("APP_USER", "CUSTOMER_NAME")]
        public string Customer_Name { get; set; }

        [ConcurrencyCheck]
        [StringLength(255)]
        [DwColumn("APP_USER", "TOKEN")]
        public string Token { get; set; }

        [PropertySave(SaveStrategy.Ignore)]
        [DwColumn("APP_USER", "ROLE")]
        public string Role { get; set; }

        [PropertySave(SaveStrategy.Ignore)]
        [DwColumn("PHONE_NO")]
        public string Phone_No { get; set; }

        [PropertySave(SaveStrategy.Ignore)]
        [DwColumn("USER_IMAGE")]
        public string User_Image { get; set; }

    }

}