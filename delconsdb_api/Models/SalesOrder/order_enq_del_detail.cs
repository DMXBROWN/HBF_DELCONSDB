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
    [DataWindow("d_sales_order_enq_del_detail", DwStyle.Default)]
    #region DwSelectAttribute  
    [DwSelect("SELECT @(_COLUMNS_PLACEHOLDER_) \r\n "
                  +"FROM \r\n "
                  +"APP_USER_CUSTOMER, \r\n "
                  +"DELIVERY_NOTE, \r\n "
                  +"DELIVERY_NOTE_DETAIL \r\n "
                  +"WHERE APP_USER_CUSTOMER.USER_ID=:as_userid \r\n "
                  +"AND  APP_USER_CUSTOMER.CUSTOMER_CODE = DELIVERY_NOTE.CUSTOMER_CODE \r\n "
                  +"AND  APP_USER_CUSTOMER.SITE_NO=DELIVERY_NOTE.SITE_NO \r\n "
                  +"AND DELIVERY_NOTE.COMPANY_CODE=DELIVERY_NOTE_DETAIL.COMPANY_CODE \r\n "
                  +"AND DELIVERY_NOTE.DNOTE_NO=DELIVERY_NOTE_DETAIL.DNOTE_NO \r\n "
                  +"AND DELIVERY_NOTE.ORDER_NO=:order_no \r\n "
                  +"AND DELIVERY_NOTE_DETAIL.ITEM_CODE=:item_code \r\n "
                  +"AND DELIVERY_NOTE.CUSTOMER_CODE =:customer_code \r\n "
                  +"AND DELIVERY_NOTE.SITE_NO =:site_no")]
    #endregion
    [DwParameter("as_userid", typeof(string))]
    [DwParameter("order_no", typeof(string))]
    [DwParameter("customer_code", typeof(string))]
    [DwParameter("site_no", typeof(string))]
    [DwParameter("item_code", typeof(string))]
    public class order_enq_del_detail
    {
        [StringLength(8)]
        [DwColumn("DELIVERY_NOTE", "DNOTE_NO", "DELIVERY_NO")]
        public string Delivery_No { get; set; }

        [DwColumn("DELIVERY_NOTE_DETAIL", "QUANTITY", "QTY")]
        public decimal? Qty { get; set; }

        [DwColumn("DELIVERY_NOTE", "DNOTE_DATE", "DATE")]
        public DateTime? Date { get; set; }

        [StringLength(10)]
        [DwColumn("DELIVERY_NOTE", "VEHICLE_NO", "VEHICLE_NO")]
        public string Vehicle_No { get; set; }

        [DwColumn("DELIVERY_NOTE", "PLATE_NO", "PLATE_NO")]
        public string Plate_No { get; set; }

        [StringLength(35)]
        [DwColumn("DELIVERY_NOTE", "DRIVER_NAME", "DRIVER")]
        public string Driver { get; set; }

    }

}