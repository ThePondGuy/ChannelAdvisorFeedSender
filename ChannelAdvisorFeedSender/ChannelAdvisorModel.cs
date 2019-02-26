using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPGNotificationAndDataFeeds.Classes.Models
{
    class ChannelAdvisorModel
    {

       //price ship
        public string Item_Number { get; set; }
        public string Current_Cost { get; set; }
        public string lowestPrice { get; set; }
        public string availcode { get; set; }
        public string Item_Shipping_Weight { get; set; }
        public string shippingMethod { get; set; }
        public string xAddtionalHandling { get; set; }
        public bool DropShip { get; set; }

        //qty only
        public string Available { get; set; }
        public string WeeksOfInventory { get; set; }

        //full inventory
        public string SKU { get; set; }
        public string BRAND { get; set; }
        public string CLASSIFICATION { get; set; }
        public string DESCRIPTION { get; set; }
        public string MANUFACTURER { get; set; }
        public string MPN { get; set; }
        public string PARENTSKU { get; set; }
        public string RELATIONSHIPNAME { get; set; }
        public string INVENTORYSUBTITLE { get; set; }
        public string TITLE { get; set; }
        public string UPC { get; set; }
        public string PICTUREURLS { get; set; }
        public string HEIGHT { get; set; }
        public string LENGTH { get; set; }
        public string WIDTH { get; set; }
        public string SHIPPINGRESTRICTION { get; set; }
        public string SIZE { get; set; }
        public string SEARCHTERMS { get; set; }
        public string GOOGLEMERCHCATEGORY { get; set; }
        public string FULLPRODURL { get; set; }
        public string ISPARENT { get; set; }
        public string PARENTCHILDDESCRIPTION { get; set; }
        public string METATITLE { get; set; }
        public string METADESCRIPTION { get; set; }

        public string BULLET1 { get; set; }
        public string BULLET2 { get; set; }
        public string BULLET3 { get; set; }
        public string SDSSHEET { get; set; }
    }
}
