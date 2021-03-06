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
    [DataWindow("d_sales_order", DwStyle.Default)]
    #region DwSelectAttribute  
    [DwSelect("SELECT DISTINCT @(_COLUMNS_PLACEHOLDER_) \r\n "
                  +"FROM \r\n "
                  +"APP_USER_CUSTOMER, \r\n "
                  +"SALES_ORDER, \r\n "
                  +"SALES_ORDER_DETAIL \r\n "
                  +"WHERE APP_USER_CUSTOMER.USER_ID=:as_userid \r\n "
                  +"AND  APP_USER_CUSTOMER.CUSTOMER_CODE = SALES_ORDER.CUSTOMER_CODE \r\n "
                  +"AND  APP_USER_CUSTOMER.SITE_NO=SALES_ORDER.SITE_NO \r\n "
                  +"AND SALES_ORDER.COMPANY_CODE=SALES_ORDER_DETAIL.COMPANY_CODE \r\n "
                  +"AND SALES_ORDER.ORDER_NO=SALES_ORDER_DETAIL.ORDER_NO")]
    #endregion
    [DwParameter("as_userid", typeof(string))]
    public class Order
    {
        [StringLength(8)]
        [DwColumn("SALES_ORDER", "ORDER_NO")]
        public string Order_No { get; set; }

        [DwColumn("SALES_ORDER", "ORDER_DATE")]
        public DateTime Order_Date { get; set; }

        [DwColumn("LPO_NO")]
        public string Lpo_No { get; set; }

        [DwColumn("LPO_DATE")]
        public DateTime? Lpo_Date { get; set; }

        [StringLength(15)]
        [DwColumn("SALES_ORDER", "SITE_NO")]
        public string Site_No { get; set; }

        [DwColumn("SALES_ORDER", "CUSTOMER_CODE")]
        public string Customer_Code { get; set; }

        public List<Order_Detail> Product{ get; set; }

    }

}