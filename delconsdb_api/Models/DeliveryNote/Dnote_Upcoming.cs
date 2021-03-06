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
    [DataWindow("d_dnote_upcoming", DwStyle.Default)]
    #region DwSelectAttribute  
    [DwSelect("SELECT \r\n "
                  +"@(_COLUMNS_PLACEHOLDER_) \r\n "
                  +"FROM APP_USER_CUSTOMER, \r\n "
                  +"DELIVERY_NOTE \r\n "
                  +"WHERE APP_USER_CUSTOMER.CUSTOMER_CODE = DELIVERY_NOTE.CUSTOMER_CODE \r\n "
                  +"AND APP_USER_CUSTOMER.SITE_NO =DELIVERY_NOTE.SITE_NO \r\n "
                  +"AND APP_USER_CUSTOMER.USER_ID=:as_userid \r\n "
                  +"AND ISNULL(DELIVERY_NOTE.DELIVERY_STATUS,'S') IN ('S','O') \r\n "
                  +"AND EXISTS(SELECT * FROM DELIVERY_NOTE_DETAIL WHERE COMPANY_CODE = DELIVERY_NOTE.COMPANY_CODE \r\n "
                  +"AND DNOTE_NO = DELIVERY_NOTE.DNOTE_NO)")]
    #endregion
    [DwParameter("as_userid", typeof(string))]
    public class Dnote_Upcoming
    {
        [StringLength(8)]
        [DwColumn("ORDER_NO")]
        public string Order_No { get; set; }

        [SqlCompute("ORDER_DATE =(SELECT TOP 1 ORDER_DATE FROM SALES_ORDER WHERE COMPANY_CODE = DELIVERY_NOTE.COMPANY_CODE " 
                  + "AND ORDER_NO = DELIVERY_NOTE.ORDER_NO)")]
        public DateTime? Order_Date { get; set; }

        [SqlCompute("LPO_NO = (SELECT TOP 1 LPO_NO FROM SALES_ORDER WHERE COMPANY_CODE =DELIVERY_NOTE.COMPANY_CODE AND ORDER_NO = DELIVERY_NOTE.ORDER_NO)")]
        public string Lpo_No { get; set; }

        [SqlCompute("LPO_DATE = (SELECT TOP 1 LPO_DATE FROM SALES_ORDER WHERE COMPANY_CODE =DELIVERY_NOTE.COMPANY_CODE AND ORDER_NO = DELIVERY_NOTE.ORDER_NO)")]
        public DateTime? Lpo_Date { get; set; }

        [StringLength(8)]
        [DwColumn("DELIVERY_NOTE", "DNOTE_NO", "DNOTE_NO")]
        public string Dnote_No { get; set; }

        [DwColumn("DELIVERY_NOTE", "DNOTE_DATE", "DNOTE_DATE")]
        public DateTime? Dnote_Date { get; set; }

        [StringLength(15)]
        [DwColumn("DELIVERY_NOTE", "SITE_NO", "SITE_NO")]
        public string Site_No { get; set; }

        [StringLength(10)]
        [DwColumn("DELIVERY_NOTE", "VEHICLE_NO", "VEHICLE_NO")]
        public string Vehicle_No { get; set; }

        [DwColumn("DELIVERY_NOTE", "GPS_ID", "GPS_ID")]
        public string Gps_Id { get; set; }

        [StringLength(35)]
        [DwColumn("DELIVERY_NOTE", "DRIVER_NAME", "DRIVER_NAME")]
        public string Driver_Name { get; set; }

        [SqlCompute("TRACK_API_USER_NAME = (SELECT TOP 1 GPS_INTERFACE_ID FROM GPS_INTERFACE)")]
        public string Track_Api_User_Name { get; set; }

        [SqlCompute("TRACK_API_PASSWORD = (SELECT TOP 1 GPS_INTERFACE_PWD FROM GPS_INTERFACE)")]
        public string Track_Api_Password { get; set; }

        [DwColumn("DELIVERY_NOTE", "ST_LONGITUDE", "ST_LONGITUDE")]
        public decimal? St_Longitude { get; set; }

        [DwColumn("DELIVERY_NOTE", "ST_LATITUDE", "ST_LATITUDE")]
        public decimal? St_Latitude { get; set; }

        [DwColumn("DELIVERY_NOTE", "EN_LONGITUDE", "EN_LONGITUDE")]
        public decimal? En_Longitude { get; set; }

        [DwColumn("DELIVERY_NOTE", "EN_LATITUDE", "EN_LATITUDE")]
        public decimal? En_Latitude { get; set; }

        [DwColumn("DELIVERY_NOTE", "PLATE_NO", "PLATE_NO")]
        public string Plate_No { get; set; }

        [DwColumn("DELIVERY_NOTE", "DELIVERY_STATUS", "STATUS")]
        public string Status { get; set; }
        
        public List<Dnote_Upcoming_Det> Product { get; set; }

    }

}