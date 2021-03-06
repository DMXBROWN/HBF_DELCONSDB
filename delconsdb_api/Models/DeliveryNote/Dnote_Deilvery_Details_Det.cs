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
    [DataWindow("d_dnote_deilvery_details_det", DwStyle.Default)]
    #region DwSelectAttribute  
    [DwSelect("SELECT @(_COLUMNS_PLACEHOLDER_) \r\n "
                  +"FROM APP_USER_CUSTOMER, \r\n "
                  +"DELIVERY_NOTE, \r\n "
                  +"DELIVERY_NOTE_DETAIL \r\n "
                  +"WHERE APP_USER_CUSTOMER.CUSTOMER_CODE = DELIVERY_NOTE.CUSTOMER_CODE \r\n "
                  +"AND APP_USER_CUSTOMER.SITE_NO =DELIVERY_NOTE.SITE_NO \r\n "
                  +"AND APP_USER_CUSTOMER.USER_ID=:as_userid \r\n "
                  +"AND DELIVERY_NOTE.COMPANY_CODE = DELIVERY_NOTE_DETAIL.COMPANY_CODE \r\n "
                  +"AND DELIVERY_NOTE.DNOTE_NO = DELIVERY_NOTE_DETAIL.DNOTE_NO \r\n "
                  +"AND DELIVERY_NOTE.DNOTE_NO =:dnote_no")]
    #endregion
    [DwParameter("as_userid", typeof(string))]
    [DwParameter("dnote_no", typeof(string))]
    public class Dnote_Deilvery_Details_Det
    {
        [DwColumn("DELIVERY_NOTE_DETAIL", "ITEM_DESCRIPTION", "PRODUCT")]
        public string Product { get; set; }

        [DwColumn("DELIVERY_NOTE_DETAIL", "SR_NO", "SR_NO")]
        public decimal? Sr_No { get; set; }

        [DwColumn("DELIVERY_NOTE_DETAIL", "ITEM_CODE", "ITEM_CODE")]
        public string Item_Code { get; set; }

        [DwColumn("DELIVERY_NOTE_DETAIL", "UNIT", "UNIT")]
        public string Unit { get; set; }

        [SqlCompute("ORDER_QTY =(SELECT SUM(ORDER_QUANTITY) FROM SALES_ORDER_DETAIL WHERE COMPANY_CODE = DELIVERY_NOTE.COMPANY_CODE AND ORDER_NO = DELIVERY_NOTE.ORDER_NO AND ITEM_CODE =DELIVERY_NOTE_DETAIL.ITEM_CODE )")]
        public decimal? Order_Qty { get; set; }

        [DwColumn("DELIVERY_NOTE_DETAIL", "QUANTITY", "DEL_QTY")]
        public decimal? Del_Qty { get; set; }

        [SqlCompute("BAL_QTY = (SELECT SUM(ORDER_QUANTITY) FROM SALES_ORDER_DETAIL WHERE COMPANY_CODE = DELIVERY_NOTE.COMPANY_CODE AND ORDER_NO = DELIVERY_NOTE.ORDER_NO AND ITEM_CODE =DELIVERY_NOTE_DETAIL.ITEM_CODE )  -  DELIVERY_NOTE_DETAIL.QUANTITY")]
        public decimal? Bal_Qty { get; set; }

    }

}