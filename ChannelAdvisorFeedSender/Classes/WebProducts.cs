using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelAdvisorFeedSender.Classes
{
    class WebProducts
    {
        //product level attributes
        public bool _allow_fractional_qty { get; set; }
        public bool _comparable { get; set; }
        public bool _content_only { get; set; }
        public bool _featured { get; set; }
        public bool _google_checkout_exempt { get; set; }
        public bool _hidden { get; set; }
        public bool _inactive { get; set; }
        public bool _is_donation { get; set; }
        public bool _kit { get; set; }
        public bool _new { get; set; }
        public bool _out_of_season { get; set; }
        public bool _tax_exempt { get; set; }
        public bool _text_field { get; set; }

        public string Brand { get; set; }
        public string DescriptionHeader { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public string ProdID { get; set; }
        public string Rating { get; set; }
        public string SKU { get; set; }
        public string URLName { get; set; }
        public string Keywords { get; set; }
        public string DefaultCategory { get; set; }
        public string DateCreated { get; set; }


    }


    class Attributes
    {
        public string SKU { get; set; }
        public bool _active { get; set; }

    }
    class AttributesTitles
    {
        public string SKU { get; set; }
        public string _column { get; set; }
        public string content { get; set; }
    }
    class AttributesProduct
    {
        public string ParentSKU { get; set; }
        public string _status { get; set; }
        public bool _is_donation { get; set; }
        public string SKU { get; set; }
        public string AltID { get; set; }
        public string ShipWeight { get; set; }
    }

    class Combination
    {
        public string SKU { get; set; }
        public string _column { get; set; }
        public string content { get; set; }
    }
    class Codes
    {
        public string SKU { get; set; }
        public string _column { get; set; }
        public string content { get; set; }
    }
    class Categories
    {
        public string SKU { get; set; }
        public bool _replacing_existing { get; set; }
        public string Category { get; set; }
    }
    class Custom
    {
        public string SKU { get; set; }
        public string _id { get; set; }
        public string _content { get; set; }
    }
    class Images
    {
        public string SKU { get; set; }
        public bool _replacing_existing { get; set; }
        public string _inactive { get; set; }
        public string CurrentRank { get; set; }
        public string Large { get; set; }
        public string PopUp { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
    }
    class CategoryFilters
    {
        public string SKU { get; set; }
        public string FilterIndex { get; set; }
        public string Filter { get; set; }
        public string Value { get; set; }
        public string SortValue { get; set; }
    }
    class InventoryControl
    {
        public string SKU { get; set; }
        public bool _ignore_backorder { get; set; }
        public bool _inventory_control_exempt { get; set; }
        public string Inventory { get; set; }
        public string OnOrder { get; set; }
        public string OutOfStockPoint { get; set; }
        public string Status { get; set; }
    }
    class Meta
    {
        public string SKU { get; set; }
        public string Keywords { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
    class QuantityRestrictions
    {
        public string SKU { get; set; }
        public string MaximumQuantity { get; set; }
        public string MinimumQuantity { get; set; }
        public string NumIterationsDisplayed { get; set; }
        public string QuantityInSet { get; set; }
    }
    class Retail
    {
        public string SKU { get; set; }
        public bool _active { get; set; }
        public string _price_category { get; set; }
        public string SpecialPrice { get; set; }
        public string StandardPrice { get; set; }
    }
    class AdditionalProducts
    {
        public string SKU { get; set; }
        public string AdditionalSKUs { get; set; }
    }
    class SubProducts
    {
        public string ParentSKU { get; set; }
        public bool _active { get; set; }
        public bool _allow_fractional_qty { get; set; }
        public bool _google_checkout_exempt { get; set; }
        public bool _inactive { get; set; }
        public bool _is_donation { get; set; }
        public bool _out_of_season { get; set; }
        public bool _tax_exempt { get; set; }
        public string Name { get; set; }
        public string ProdID { get; set; }
        public string SKU { get; set; }
        public string DateCreated { get; set; }
    }
    class Weight
    {
        public string SKU { get; set; }
        public string ShipWeight { get; set; }
        public string DisplayWeight { get; set; }
        public string DisplayUnit { get; set; }
    }
    class Shipping
    {
        public string SKU { get; set; }
        public string ShippingPreference { get; set; }
        public string FixedRate { get; set; }
        public string PromoFixedRate { get; set; }
    }
    class CategoriesMap
    {
        public bool _invisible { get; set; }
        public bool _featured { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string URLName { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string Template { get; set; }
        public string NumProductsPerPage { get; set; }
        public string CategoryPath { get; set; }

    }
    class CategoriesMapP
    {
       
        public string ID { get; set; }
   
        public string Products { get; set; }
     

    }
    class CategoriesMapF
    {

        public string ID { get; set; }

        public string FeaturedProducts { get; set; }


    }
    class CategoriesMapC
    {

        public string ID { get; set; }

        public string _id { get; set; }

        public string content { get; set; }

    }
}
