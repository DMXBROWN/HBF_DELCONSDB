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
    [DataWindow("d_sales_order_pend_del_detail", DwStyle.Default)]
    #region DwSelectAttribute  
    [DwSelect("SELECT \r\n "
                  +"@(_COLUMNS_PLACEHOLDER_) \r\n "
                  +"FROM \r\n "
                  +"SALES_ORDER_DNOTE_DETAIL, \r\n "
                  +"SALES_ORDER, \r\n "
                  +"APP_USER_CUSTOMER \r\n "
                  +"WHERE \r\n "
                  +"SALES_ORDER.COMPANY_CODE=SALES_ORDER_DNOTE_DETAIL.COMPANY_CODE AND \r\n "
                  +"SALES_ORDER.ORDER_NO=SALES_ORDER_DNOTE_DETAIL.ORDER_NO AND \r\n "
                  +"SALES_ORDER.CUSTOMER_CODE=APP_USER_CUSTOMER.CUSTOMER_CODE AND \r\n "
                  +"SALES_ORDER.SITE_NO=APP_USER_CUSTOMER.SITE_NO AND \r\n "
                  +"SALES_ORDER.ORDER_NO=:order_no AND \r\n "
                  +"ITEM_CODE=:item_code AND \r\n "
                  +"APP_USER_CUSTOMER.USER_ID=:as_userid AND \r\n "
                  +"APP_USER_CUSTOMER.CUSTOMER_CODE=:customer_code AND \r\n "
                  +"SALES_ORDER.SITE_NO=:site_no")]
    #endregion
    [DwParameter("as_userid", typeof(string))]
    [DwParameter("order_no", typeof(string))]
    [DwParameter("customer_code", typeof(string))]
    [DwParameter("site_no", typeof(string))]
    [DwParameter("item_code", typeof(string))]
    public class Pend_Del_Detail
    {
        [DwColumn("SALES_ORDER_DNOTE_DETAIL", "ORDER_NO", "ORDER_NO")]
        public string Order_No { get; set; }

        [DwColumn("SALES_ORDER_DNOTE_DETAIL", "DNOTE_NO", "DNOTE_NO")]
        public string Dnote_No { get; set; }

        [DwColumn("ITEM_CODE")]
        public string Item_Code { get; set; }

        [DwColumn("ITEM_DESCRIPTION")]
        public string Item_Description { get; set; }

        [DwColumn("SALES_ORDER_DNOTE_DETAIL", "DNOTE_DATE", "DNOTE_DATE")]
        public DateTime? Dnote_Date { get; set; }

        [DwColumn("QUANTITY", ColumnAlias = "QTY")]
        public decimal? Qty { get; set; }

    }

}