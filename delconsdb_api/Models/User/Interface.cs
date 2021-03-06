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
    [DataWindow("d_gps_interface", DwStyle.Default)]
    [Table("GPS_INTERFACE")]
    #region DwSelectAttribute  
    [DwSelect("PBSELECT( VERSION(400) TABLE(NAME=\"GPS_INTERFACE\" ) @(_COLUMNS_PLACEHOLDER_) )")]
    #endregion
    [DwKeyModificationStrategy(UpdateSqlStrategy.DeleteThenInsert)]
    [UpdateWhereStrategy(UpdateWhereStrategy.KeyAndConcurrencyCheckColumns)]
    public class Interface
    {
        [Key]
        [StringLength(15)]
        [DwColumn("GPS_INTERFACE", "GPS_INTERFACE_ID")]
        public string Gps_Interface_Id { get; set; }

        [ConcurrencyCheck]
        [StringLength(20)]
        [DwColumn("GPS_INTERFACE", "GPS_INTERFACE_PWD")]
        public string Gps_Interface_Pwd { get; set; }

    }

}