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
    [DataWindow("d_sales_order_enq_detail", DwStyle.Default)]
    #region DwSelectAttribute  
    [DwSelect("SELECT @(_COLUMNS_PLACEHOLDER_) \r\n "
                  +"FROM \r\n "
                  +"APP_USER_CUSTOMER, \r\n "
                  +"SALES_ORDER, \r\n "
                  +"SALES_ORDER_DETAIL \r\n "
                  +"WHERE APP_USER_CUSTOMER.USER_ID=:as_userid \r\n "
                  +"AND  APP_USER_CUSTOMER.CUSTOMER_CODE = SALES_ORDER.CUSTOMER_CODE \r\n "
                  +"AND  APP_USER_CUSTOMER.SITE_NO=SALES_ORDER.SITE_NO \r\n "
                  +"AND SALES_ORDER.COMPANY_CODE=SALES_ORDER_DETAIL.COMPANY_CODE \r\n "
                  +"AND SALES_ORDER.ORDER_NO=SALES_ORDER_DETAIL.ORDER_NO \r\n "
                  +"AND SALES_ORDER.ORDER_NO=:order_no \r\n "
                  +"AND SALES_ORDER.CUSTOMER_CODE=:customer_code \r\n "
                  +"AND SALES_ORDER.SITE_NO=:site_no")]
    #endregion
    [DwParameter("as_userid", typeof(string))]
    [DwParameter("order_no", typeof(string))]
    [DwParameter("customer_code", typeof(string))]
    [DwParameter("site_no", typeof(string))]
    public class Order_Enq_Detail
    {
        [DwColumn("SALES_ORDER_DETAIL", "ITEM_DESCRIPTION", "PRODUCT")]
        public string Product { get; set; }

        [StringLength(15)]
        [DwColumn("SALES_ORDER_DETAIL", "ITEM_CODE")]
        public string Item_Code { get; set; }

        [StringLength(6)]
        [DwColumn("SALES_ORDER_DETAIL", "UNIT")]
        public string Unit { get; set; }

        [DwColumn("SALES_ORDER_DETAIL", "ORDER_QUANTITY")]
        public decimal? Order_Quantity { get; set; }

        [DwColumn("SALES_ORDER_DETAIL", "DEL_QUANTITY")]
        public decimal? Del_Quantity { get; set; }

        [SqlCompute("BAL_QTY = ORDER_QUANTITY - ISNULL(DEL_QUANTITY,0)")]
        public decimal? Bal_Qty { get; set; }

        public List<Pend_Del_Detail> Delivery_Details { get; set; }

    }

}