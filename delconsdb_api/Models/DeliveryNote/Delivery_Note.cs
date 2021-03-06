using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SnapObjects.Data;
using DWNet.Data;
using Newtonsoft.Json;
using System.Collections;

namespace delconsdb_api.Models
{
    [DataWindow("d_delivery_note", DwStyle.Default)]
    #region DwSelectAttribute  
    [DwSelect("SELECT @(_COLUMNS_PLACEHOLDER_) \r\n "
                  +"FROM APP_USER_CUSTOMER, \r\n "
                  +"DELIVERY_NOTE, \r\n "
                  +"DELIVERY_NOTE_DETAIL \r\n "
                  +"WHERE APP_USER_CUSTOMER.CUSTOMER_CODE = DELIVERY_NOTE.CUSTOMER_CODE \r\n "
                  +"AND APP_USER_CUSTOMER.SITE_NO =DELIVERY_NOTE.SITE_NO \r\n "
                  +"AND DELIVERY_NOTE.COMPANY_CODE = DELIVERY_NOTE_DETAIL.COMPANY_CODE \r\n "
                  +"AND DELIVERY_NOTE.DNOTE_NO = DELIVERY_NOTE_DETAIL.DNOTE_NO \r\n "
                  +"AND APP_USER_CUSTOMER.USER_ID=:as_userid")]
    #endregion
    [DwParameter("as_userid", typeof(string))]
    public class Delivery_Note
    {
        [StringLength(8)]
        [DwColumn("DELIVERY_NOTE", "DNOTE_NO", "DNOTE_NO")]
        public string Dnote_No { get; set; }

        [DwColumn("DELIVERY_NOTE", "DNOTE_DATE", "DNOTE_DATE")]
        public DateTime? Dnote_Date { get; set; }

        [StringLength(15)]
        [DwColumn("DELIVERY_NOTE", "SITE_NO", "SITE_NO")]
        public string Site_No { get; set; }

        [StringLength(6)]
        [DwColumn("DELIVERY_NOTE", "CUSTOMER_CODE", "CUSTOMER_CODE")]
        public string Customer_Code { get; set; }

        [StringLength(35)]
        [DwColumn("DELIVERY_NOTE", "CUSTOMER_NAME", "CUSTOMER_NAME")]
        public string Customer_Name { get; set; }

        [StringLength(10)]
        [DwColumn("DELIVERY_NOTE", "VEHICLE_NO", "VEHICLE_NO")]
        public string Vehicle_No { get; set; }

        [StringLength(25)]
        [DwColumn("DELIVERY_NOTE", "GPS_ID", "GPS_ID")]
        public string Gps_Id { get; set; }

        [StringLength(35)]
        [DwColumn("DELIVERY_NOTE", "DRIVER_NAME", "DRIVER_NAME")]
        public string Driver_Name { get; set; }

        [StringLength(8)]
        [DwColumn("DELIVERY_NOTE", "ORDER_NO", "ORDER_NO")]
        public string Order_No { get; set; }

        [DwColumn("DELIVERY_NOTE_DETAIL", "SR_NO", "SR_NO")]
        public decimal? Sr_No { get; set; }

        [StringLength(15)]
        [DwColumn("DELIVERY_NOTE_DETAIL", "ITEM_CODE", "ITEM_CODE")]
        public string Item_Code { get; set; }

        [StringLength(6)]
        [DwColumn("DELIVERY_NOTE_DETAIL", "UNIT", "UNIT")]
        public string Unit { get; set; }

        [DwColumn("DELIVERY_NOTE_DETAIL", "QUANTITY", "QUANTITY")]
        public decimal? Quantity { get; set; }

    }

}