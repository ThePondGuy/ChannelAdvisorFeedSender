using ChannelAdvisorFeedSender.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;

namespace ChannelAdvisorFeedSender.Classes
{
    public class CV3JSONDownload
    {

        public bool ProductDownload()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //TLS 1.2
            Console.WriteLine("Downloading Product Data");
            string path = "C:\\TPGServiceLogs";
            var ws = new CV3.CV3DataxsdPortTypeClient();
            string strXML = "";

            string UserName = "", Password = "", ServiceID = "";
            CV3Creds c = new CV3Creds();
            List<CV3Creds> creds = c.GetCV3Creds().ToList();
            foreach (var cred in creds)
            {
                UserName = cred.user;
                Password = cred.pass;
                ServiceID = cred.serviceID;
            }


            //Console.WriteLine("UserName: " + UserName);
            //Console.WriteLine("Password: " + Password);
            //Console.WriteLine("ServiceID: " + ServiceID);

            try
            {

                strXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + System.Environment.NewLine +
                           "<CV3Data version=\"2.0\" response_format=\"json\">" + System.Environment.NewLine +
                           "   <request>" + System.Environment.NewLine +
                           "       <authenticate>" + System.Environment.NewLine +
                           "           <user>" + UserName + "</user>" + System.Environment.NewLine +
                           "           <pass>" + Password + "</pass>" + System.Environment.NewLine +
                           "           <serviceID>" + ServiceID + "</serviceID>" + System.Environment.NewLine +
                           "       </authenticate>" + System.Environment.NewLine +
                           "     <requests>" + System.Environment.NewLine +
                           "       <reqProducts>" + System.Environment.NewLine +
                           "         <reqProductRange start=\"1\" end=\"99999\"/>" + System.Environment.NewLine +
                           "       </reqProducts>" + System.Environment.NewLine +
                           "     </requests>" + System.Environment.NewLine +
                           "  </request>" + System.Environment.NewLine +
                           "</CV3Data>" + System.Environment.NewLine;



                byte[] ba = System.Text.Encoding.UTF8.GetBytes(strXML);
                string str64 = Convert.ToBase64String(ba);
                //byte[] bData = new byte[str64.Length + 1];
                //bData = System.Text.Encoding.ASCII.GetBytes(str64);

                Console.WriteLine("Sending Request to CV3");
                byte[] b = ws.CV3Data(str64);

                Console.WriteLine("Creating JSON File");
                if (File.Exists(path + "\\Products.json"))
                {
                    if (!Directory.Exists(path + "\\archiveJSON"))
                    {
                        Directory.CreateDirectory(path + "\\archiveJSON");
                    }
                    File.Move(path + "\\Products.json", path + "\\archiveJSON\\" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + "_Products.json");
                }
                System.IO.FileStream t = new System.IO.FileStream(path + "\\" + "Products.json", FileMode.OpenOrCreate);
                t.Write(b, 0, b.Length);
                t.Close();





                if (b.Length < 1000)
                {
                    Console.WriteLine("Failed to connect to CV3");
                    SendEmail.Send("CV3 Connection Failed", "Unable to download CV3 Products.  Reason: No Data.  It is likely that the API Password Expired. The password for the connector must match the one in CV3, it can be set in the Connector Store Configuration.", "itdevelopment@thepondguy.com", false);
                    return false;
                }

                Console.WriteLine("Uploading Product inforamtion to Database");
                if (!ParseFileToDB(path + "\\" + "Products.json", "Products"))
                {
                    SendEmail.Send("Error parsing Product Web data", "", "twilson@thepondguy.com");
                    return false;
                }


            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Console.WriteLine(error);
                return false;
            }

            return true;
        }
        public bool CategoryDownload()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //TLS 1.2
            Console.WriteLine("Downloading Category Data");
            string path = "C:\\TPGServiceLogs";
            var ws = new CV3.CV3DataxsdPortTypeClient();
            string strXML = "";


            try
            {
                string UserName = "", Password = "", ServiceID = "";
                CV3Creds c = new CV3Creds();
                List<CV3Creds> creds = c.GetCV3Creds().ToList();
                foreach (var cred in creds)
                {
                    UserName = cred.user;
                    Password = cred.pass;
                    ServiceID = cred.serviceID;
                }
                strXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + System.Environment.NewLine +
                           "<CV3Data version=\"2.0\" response_format=\"json\">" + System.Environment.NewLine +
                           "   <request>" + System.Environment.NewLine +
                           "       <authenticate>" + System.Environment.NewLine +
                           "           <user>" + UserName + "</user>" + System.Environment.NewLine +
                           "           <pass>" + Password + "</pass>" + System.Environment.NewLine +
                           "           <serviceID>" + ServiceID + "</serviceID>" + System.Environment.NewLine +
                           "       </authenticate>" + System.Environment.NewLine +
                           "     <requests>" + System.Environment.NewLine +
                           "       <reqCategories>" + System.Environment.NewLine +
                           "         <reqCategoryRange start=\"1\" end=\"9999\"/>" + System.Environment.NewLine +
                           "       </reqCategories>" + System.Environment.NewLine +
                           "     </requests>" + System.Environment.NewLine +
                           "  </request>" + System.Environment.NewLine +
                           "</CV3Data>" + System.Environment.NewLine;



                byte[] ba = System.Text.Encoding.UTF8.GetBytes(strXML);
                string str64 = Convert.ToBase64String(ba);
                //byte[] bData = new byte[str64.Length + 1];
                //bData = System.Text.Encoding.ASCII.GetBytes(str64);


                Console.WriteLine("Sending Request to CV3");
                byte[] b = ws.CV3Data(str64);

                Console.WriteLine("Creating JSON File");
                if (File.Exists(path + "\\Categories.json"))
                {
                    if (!Directory.Exists(path + "\\archiveJSON"))
                    {
                        Directory.CreateDirectory(path + "\\archiveJSON");
                    }
                    File.Move(path + "\\Categories.json", path + "\\archiveJSON\\" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + "_Categories.json");
                }
                System.IO.FileStream t = new System.IO.FileStream(path + "\\" + "Categories.json", FileMode.OpenOrCreate);
                t.Write(b, 0, b.Length);
                t.Close();

                if (b.Length < 1000)
                {
                    Console.WriteLine("Failed to connect to CV3");
                    SendEmail.Send("CV3 Connection Failed", "Unable to download CV3 Categories.  Reason: No Data.  It is likely that the API Password Expired. The password for the connector must match the one in CV3, it can be set in the Connector Store Configuration.", "itdevelopment@thepondguy.com", false);
                    return false;
                }

                Console.WriteLine("Uploading Categories inforamtion to Database");
                if (!ParseFileToDB(path + "\\" + "Categories.json", "Categories"))
                {
                    SendEmail.Send("Error parsing Category Web data", "", "twilson@thepondguy.com");
                    return false;
                }


            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Console.WriteLine(error);
                return false;
            }

            return true;
        }
        public bool ParseFileToDB(string filepath, string parseType)
        {
            List<WebProducts> wpList = new List<WebProducts>();
            List<Attributes> aList = new List<Attributes>();
            List<Categories> catList = new List<Categories>();
            List<Custom> cList = new List<Custom>();
            List<Images> iList = new List<Images>();
            List<InventoryControl> icList = new List<InventoryControl>();
            List<Meta> mList = new List<Meta>();
            List<QuantityRestrictions> qrList = new List<QuantityRestrictions>();
            List<Retail> rList = new List<Retail>();
            List<AdditionalProducts> apList = new List<AdditionalProducts>();
            List<SubProducts> spList = new List<SubProducts>();
            List<Weight> wList = new List<Weight>();
            List<Shipping> sList = new List<Shipping>();

            List<Custom> cListSub = new List<Custom>();
            List<Images> iListSub = new List<Images>();
            List<InventoryControl> icListSub = new List<InventoryControl>();
            List<QuantityRestrictions> qrListSub = new List<QuantityRestrictions>();
            List<Retail> rListSub = new List<Retail>();
            List<Weight> wListSub = new List<Weight>();
            List<Shipping> sListSub = new List<Shipping>();

            List<AttributesTitles> atListA = new List<AttributesTitles>();
            List<AttributesProduct> apListA = new List<AttributesProduct>();
            List<Combination> cListA = new List<Combination>();
            List<Codes> cdListA = new List<Codes>();

            List<CategoriesMap> cmapList = new List<CategoriesMap>();
            List<CategoriesMapP> pListSub = new List<CategoriesMapP>();
            List<CategoriesMapF> fListSub = new List<CategoriesMapF>();
            List<CategoriesMapC> cmapListSub = new List<CategoriesMapC>();

            try
            {
                if (parseType == "Products")
                {
                    //List<WebProducts> wpList = new List<WebProducts>();
                    using (StreamReader rdr = new StreamReader(filepath))
                    {
                        string json = rdr.ReadToEnd();
                        dynamic array = JsonConvert.DeserializeObject(json);
                        int itemIndex = 0;



                        foreach (dynamic item in array.CV3Data.products)
                        {
                            if (Convert.ToBoolean(item.Value._inactive))
                            {
                                continue;
                            }

                            itemIndex++;


                            WebProducts wp = new WebProducts();
                            Attributes a = new Attributes();
                            Categories cat = new Categories();


                            InventoryControl ic = new InventoryControl();
                            Meta m = new Meta();
                            QuantityRestrictions qr = new QuantityRestrictions();

                            AdditionalProducts ap = new AdditionalProducts();

                            Weight w = new Weight();
                            Shipping s = new Shipping();

                            string ParentSKU = (item.Value.SKU != null) ? item.Value.SKU.ToString() : "";

                            //if(item.Value.SKU == "article-pl-001")
                            //{
                            //    string error = "";
                            //}


                            //web products
                            wp._allow_fractional_qty = Convert.ToBoolean(item.Value._allow_fractional_qty);
                            wp._comparable = Convert.ToBoolean(item.Value._comparable);
                            wp._content_only = Convert.ToBoolean(item.Value._content_only);
                            wp._featured = Convert.ToBoolean(item.Value._featured);
                            wp._google_checkout_exempt = Convert.ToBoolean(item.Value._google_checkout_exempt);
                            wp._hidden = Convert.ToBoolean(item.Value._hidden);
                            wp._inactive = Convert.ToBoolean(item.Value._inactive);
                            wp._is_donation = Convert.ToBoolean(item.Value._is_donation);
                            wp._kit = Convert.ToBoolean(item.Value._kit);
                            wp._new = Convert.ToBoolean(item.Value._new);
                            wp._out_of_season = Convert.ToBoolean(item.Value._out_of_season);
                            wp._tax_exempt = Convert.ToBoolean(item.Value._tax_exempt);
                            wp._text_field = Convert.ToBoolean(item.Value._text_field);

                            wp.Brand = (item.Value.Brand != null) ? item.Value.Brand.ToString() : "";
                            if (Convert.ToBoolean(item.Value._content_only))
                            {
                                wp.DescriptionHeader = (item.Value.Description != null) ? item.Value.Description.ToString() : "";
                            }
                            else
                            {
                                wp.DescriptionHeader = (item.Value.DescriptionHeader != null) ? item.Value.DescriptionHeader.ToString() : "";
                            }
                            wp.Manufacturer = (item.Value.Manufacturer != null) ? item.Value.Manufacturer.ToString() : "";
                            wp.Name = (item.Value.Name != null) ? item.Value.Name.ToString() : "";
                            wp.ProdID = (item.Value.ProdID != null) ? item.Value.ProdID.ToString() : "";
                            wp.Rating = (item.Value.Rating != null) ? item.Value.Rating.ToString() : "";
                            wp.SKU = ParentSKU;
                            wp.URLName = (item.Value.URLName != null) ? item.Value.URLName.ToString() : "";
                            wp.Keywords = (item.Value.Keywords != null) ? item.Value.Keywords.ToString() : "";
                            wp.DefaultCategory = (item.Value.DefaultCategory != null) ? item.Value.DefaultCategory.ToString() : "";
                            wp.DateCreated = (item.Value.DateCreated != null) ? item.Value.DateCreated.ToString() : "";
                            wpList.Add(wp);
                            //Attributes
                            if (item.Value.Attributes != null)
                            {
                                a.SKU = ParentSKU;
                                a._active = Convert.ToBoolean(item.Value.Attributes._active);
                                aList.Add(a);
                            }

                            if (Convert.ToBoolean(item.Value.Attributes._active))
                            {



                                if (item.Value.Attributes.Titles != null)
                                {
                                    foreach (dynamic _AttributesTitles in item.Value.Attributes.Titles)
                                    {




                                        AttributesTitles at = new AttributesTitles();
                                        at.SKU = ParentSKU;
                                        at._column = (_AttributesTitles.Value._column != null) ? _AttributesTitles.Value._column.ToString() : "";
                                        at.content = (_AttributesTitles.Value.content != null) ? _AttributesTitles.Value.content.ToString() : "";
                                        atListA.Add(at);


                                    }
                                }
                                foreach (dynamic _Attributes in item.Value.Attributes)
                                {

                                    if (_Attributes.Name != "_active" && _Attributes.Name != "Titles")
                                    {
                                        foreach (dynamic _AttributesProduct in _Attributes)
                                        {
                                            AttributesProduct apsbu = new AttributesProduct();
                                            apsbu.ParentSKU = ParentSKU;
                                            apsbu._status = (_AttributesProduct._status != null) ? _AttributesProduct._status.ToString() : "";
                                            apsbu._is_donation = Convert.ToBoolean(_AttributesProduct._is_donation);
                                            apsbu.SKU = (_AttributesProduct.SKU != null) ? _AttributesProduct.SKU.ToString() : "";
                                            apsbu.AltID = (_AttributesProduct.AltID != null) ? _AttributesProduct.AltID.ToString() : "";
                                            apsbu.ShipWeight = (_AttributesProduct.ShipWeight != null) ? _AttributesProduct.ShipWeight.ToString() : "";
                                            apListA.Add(apsbu);

                                            //////Combination
                                            //if (_AttributesProduct.Value.Custom != null)
                                            //{
                                            //    foreach (dynamic _Custom in _SubProducts.Value.Custom)
                                            //    {
                                            //        Custom cSub = new Custom();
                                            //        cSub.SKU = SubSKU;
                                            //        cSub._id = (_Custom.Value._id != null) ? _Custom.Value._id.ToString() : "";
                                            //        cSub._content = (_Custom.Value.content != null) ? _Custom.Value.content.ToString() : "";
                                            //        cListSub.Add(cSub);
                                            //    }
                                            //}
                                            //////Codes
                                            //if (_SubProducts.Value.Custom != null)
                                            //{
                                            //    foreach (dynamic _Custom in _SubProducts.Value.Custom)
                                            //    {
                                            //        Custom cSub = new Custom();
                                            //        cSub.SKU = SubSKU;
                                            //        cSub._id = (_Custom.Value._id != null) ? _Custom.Value._id.ToString() : "";
                                            //        cSub._content = (_Custom.Value.content != null) ? _Custom.Value.content.ToString() : "";
                                            //        cListSub.Add(cSub);
                                            //    }
                                            //}

                                            ////InventoryControl
                                            //if (_SubProducts.Value.InventoryControl != null)
                                            //{
                                            //    icSub.SKU = SubSKU;
                                            //    icSub._ignore_backorder = Convert.ToBoolean(_SubProducts.Value.InventoryControl._ignore_backorder);
                                            //    icSub._inventory_control_exempt = Convert.ToBoolean(_SubProducts.Value.InventoryControl._inventory_control_exempt);
                                            //    icSub.Inventory = (_SubProducts.Value.InventoryControl.Inventory != null) ? _SubProducts.Value.InventoryControl.Inventory.ToString() : "";
                                            //    icSub.OnOrder = (_SubProducts.Value.InventoryControl.OnOrder != null) ? _SubProducts.Value.InventoryControl.OnOrder.ToString() : "";
                                            //    icSub.OutOfStockPoint = (_SubProducts.Value.InventoryControl.OutOfStockPoint != null) ? _SubProducts.Value.InventoryControl.OutOfStockPoint.ToString() : "";
                                            //    icSub.Status = (_SubProducts.Value.InventoryControl.Status != null) ? _SubProducts.Value.InventoryControl.Status.ToString() : "";
                                            //    icListSub.Add(icSub);
                                            //}
                                            //////Retail
                                            //if (_SubProducts.Value.Retail != null)
                                            //{
                                            //    int indexCountRetail = 1;
                                            //    foreach (dynamic _Retail in _SubProducts.Value.Retail)
                                            //    {
                                            //        Retail rSub = new Retail();
                                            //        if (indexCountRetail == 1)
                                            //        {

                                            //        }
                                            //        else
                                            //        {
                                            //            rSub.SKU = SubSKU;
                                            //            rSub._active = Convert.ToBoolean(item.Value.Retail._active);
                                            //            rSub._price_category = (_Retail.Value._price_category != null) ? _Retail.Value._price_category.ToString() : "";
                                            //            rSub.SpecialPrice = (_Retail.Value.SpecialPrice != null) ? _Retail.Value.SpecialPrice.ToString() : "";
                                            //            rSub.StandardPrice = (_Retail.Value.StandardPrice != null) ? _Retail.Value.StandardPrice.ToString() : "";
                                            //            rList.Add(rSub);
                                            //        }
                                            //        indexCountRetail++;
                                            //    }


                                            //}
                                        }
                                    }
                                }
                            }
                            //Category
                            if (item.Value.Categories != null)
                            {
                                int indexCountCat = 1;
                                bool _replacing_existingCat = false;
                                foreach (dynamic _Cat in item.Value.Categories)
                                {
                                    Categories i = new Categories();
                                    if (indexCountCat == 1)
                                    {
                                        _replacing_existingCat = Convert.ToBoolean(item.Value._replacing_existing);
                                    }
                                    else
                                    {
                                        i.SKU = ParentSKU;
                                        i._replacing_existing = _replacing_existingCat;
                                        i.Category = (_Cat.Value != null) ? _Cat.Value.ToString() : "";

                                        catList.Add(i);
                                    }
                                    indexCountCat++;
                                }

                            }
                            ////Custom
                            if (item.Value.Custom != null)
                            {
                                foreach (dynamic _Custom in item.Value.Custom)
                                {
                                    Custom c = new Custom();
                                    c.SKU = ParentSKU;
                                    c._id = (_Custom.Value._id != null) ? _Custom.Value._id.ToString() : "";
                                    c._content = (_Custom.Value.content != null) ? _Custom.Value.content.ToString() : "";
                                    cList.Add(c);
                                }
                            }
                            //Images
                            if (item.Value.Images != null)
                            {
                                int indexCountImages = 1;
                                bool _replacing_existingImages = false;
                                foreach (dynamic _Image in item.Value.Images)
                                {
                                    Images i = new Images();
                                    if (indexCountImages == 1)
                                    {
                                        _replacing_existingImages = Convert.ToBoolean(item.Value._replacing_existing);
                                    }
                                    else
                                    {
                                        i.SKU = ParentSKU;
                                        i._replacing_existing = _replacing_existingImages;
                                        i._inactive = (_Image.Value._inactive != null) ? _Image.Value._inactive.ToString() : "";
                                        i.CurrentRank = (_Image.Value.CurrentRank != null) ? _Image.Value.CurrentRank.ToString() : "";
                                        i.Large = (_Image.Value.Large != null) ? _Image.Value.Large.ToString() : "";
                                        i.PopUp = (_Image.Value.PopUp != null) ? _Image.Value.PopUp.ToString() : "";
                                        i.Thumbnail = (_Image.Value.Thumbnail != null) ? _Image.Value.Thumbnail.ToString() : "";
                                        i.Title = (_Image.Value.Title != null) ? _Image.Value.Title.ToString() : "";
                                        i.Type = (_Image.Value.Type != null) ? _Image.Value.Type.ToString() : "";
                                        i.Link = "";
                                        iList.Add(i);
                                    }
                                    indexCountImages++;
                                }
                            }

                            //InventoryControl
                            if (item.Value.InventoryControl != null)
                            {
                                ic.SKU = ParentSKU;
                                ic._ignore_backorder = Convert.ToBoolean(item.Value.InventoryControl._ignore_backorder);
                                ic._inventory_control_exempt = Convert.ToBoolean(item.Value.InventoryControl._inventory_control_exempt);
                                ic.Inventory = (item.Value.InventoryControl.Inventory != null) ? item.Value.InventoryControl.Inventory.ToString() : "";
                                ic.OnOrder = (item.Value.InventoryControl.OnOrder != null) ? item.Value.InventoryControl.OnOrder.ToString() : "";
                                ic.OutOfStockPoint = (item.Value.InventoryControl.OutOfStockPoint != null) ? item.Value.InventoryControl.OutOfStockPoint.ToString() : "";
                                ic.Status = (item.Value.InventoryControl.Status != null) ? item.Value.InventoryControl.Status.ToString() : "";
                                icList.Add(ic);
                            }
                            ////Meta
                            if (item.Value.Meta != null)
                            {
                                m.SKU = ParentSKU;
                                m.Keywords = (item.Value.Meta.Keywords != null) ? item.Value.Meta.Keywords.ToString() : "";
                                m.Title = (item.Value.Meta.Title != null) ? item.Value.Meta.Title.ToString() : "";
                                m.Description = (item.Value.Meta.Description != null) ? item.Value.Meta.Description.ToString() : "";
                                mList.Add(m);
                            }
                            ////QuantityRestrictions
                            if (item.Value.QuantityRestrictions != null)
                            {
                                qr.SKU = ParentSKU;
                                qr.MaximumQuantity = (item.Value.QuantityRestrictions.MaximumQuantity != null) ? item.Value.QuantityRestrictions.MaximumQuantity.ToString() : "";
                                qr.MinimumQuantity = (item.Value.QuantityRestrictions.MinimumQuantity != null) ? item.Value.QuantityRestrictions.MinimumQuantity.ToString() : "";
                                qr.NumIterationsDisplayed = (item.Value.QuantityRestrictions.NumIterationsDisplayed != null) ? item.Value.QuantityRestrictions.NumIterationsDisplayed.ToString() : "";
                                qr.QuantityInSet = (item.Value.QuantityRestrictions.QuantityInSet != null) ? item.Value.QuantityRestrictions.QuantityInSet.ToString() : "";
                                qrList.Add(qr);
                            }


                            ////Retail
                            if (item.Value.Retail != null)
                            {
                                int indexCountRetail = 1;
                                foreach (dynamic _Retail in item.Value.Retail)
                                {
                                    Retail r = new Retail();
                                    if (indexCountRetail == 1)
                                    {

                                    }
                                    else
                                    {
                                        r.SKU = ParentSKU;
                                        r._active = Convert.ToBoolean(item.Value.Retail._active);
                                        r._price_category = (_Retail.Value._price_category != null) ? _Retail.Value._price_category.ToString() : "";
                                        r.SpecialPrice = (_Retail.Value.SpecialPrice != null) ? _Retail.Value.SpecialPrice.ToString() : "";
                                        r.StandardPrice = (_Retail.Value.StandardPrice != null) ? _Retail.Value.StandardPrice.ToString() : "";
                                        rList.Add(r);
                                    }
                                    indexCountRetail++;
                                }
                            }

                            ////AdditionalProducts
                            if (item.Value.AdditionalProducts != null)
                            {
                                ap.SKU = ParentSKU;
                                ap.AdditionalSKUs = (item.Value.AdditionalProducts.SKU != null) ? item.Value.AdditionalProducts.SKU.ToString() : "";
                                apList.Add(ap);
                            }

                            ////Weight
                            if (item.Value.Weight != null)
                            {
                                w.SKU = ParentSKU;
                                w.ShipWeight = (item.Value.Weight.ShipWeight != null) ? item.Value.Weight.ShipWeight.ToString() : "";
                                w.DisplayWeight = (item.Value.Weight.DisplayWeight != null) ? item.Value.Weight.DisplayWeight.ToString() : "";
                                w.DisplayUnit = (item.Value.Weight.DisplayUnit != null) ? item.Value.Weight.DisplayUnit.ToString() : "";
                                wList.Add(w);
                            }
                            ////Shipping
                            if (item.Value.Shipping != null)
                            {
                                s.SKU = ParentSKU;
                                s.ShippingPreference = (item.Value.Shipping.ShippingPreference != null) ? item.Value.Shipping.ShippingPreference.ToString() : "";
                                s.FixedRate = (item.Value.Shipping.FixedRate != null) ? item.Value.Shipping.FixedRate.ToString() : "";
                                s.PromoFixedRate = (item.Value.Shipping.PromoFixedRate != null) ? item.Value.Shipping.PromoFixedRate.ToString() : "";
                                sList.Add(s);
                            }


                            //////SubProducts                            
                            if (((Newtonsoft.Json.Linq.JContainer)item.Value.SubProducts).Count > 1)
                            {
                                int indexCountSubProducts = 1;


                                foreach (dynamic _SubProducts in item.Value.SubProducts)
                                {
                                    

                                    SubProducts sp = new SubProducts();
                                    if (indexCountSubProducts == 1)
                                    {
                                        sp._active = Convert.ToBoolean(item.Value.SubProducts._active);
                                    }
                                    else
                                    {

                                        if (Convert.ToBoolean(_SubProducts.Value._inactive))
                                        {
                                            continue;
                                        }
                                        InventoryControl icSub = new InventoryControl();
                                        QuantityRestrictions qrSub = new QuantityRestrictions();
                                        Weight wSub = new Weight();
                                        Shipping sSub = new Shipping();

                                        string SubSKU = (_SubProducts.Value.SKU != null) ? _SubProducts.Value.SKU.ToString() : "";

                                        sp._allow_fractional_qty = Convert.ToBoolean(_SubProducts.Value._allow_fractional_qty);
                                        sp._google_checkout_exempt = Convert.ToBoolean(_SubProducts.Value._google_checkout_exempt);
                                        sp._inactive = Convert.ToBoolean(_SubProducts.Value._inactive);
                                        sp._is_donation = Convert.ToBoolean(_SubProducts.Value._is_donation);
                                        sp._out_of_season = Convert.ToBoolean(_SubProducts.Value._out_of_season);
                                        sp._tax_exempt = Convert.ToBoolean(_SubProducts.Value._tax_exempt);
                                        sp.Name = (_SubProducts.Value.Name != null) ? _SubProducts.Value.Name.ToString() : "";
                                        sp.ProdID = (_SubProducts.Value.ProdID != null) ? _SubProducts.Value.ProdID.ToString() : "";
                                        sp.DateCreated = (_SubProducts.Value.DateCreated != null) ? _SubProducts.Value.DateCreated.ToString() : "";
                                        sp.SKU = SubSKU;
                                        sp.ParentSKU = ParentSKU;
                                        spList.Add(sp);

                                        ////Custom
                                        if (_SubProducts.Value.Custom != null)
                                        {
                                            foreach (dynamic _Custom in _SubProducts.Value.Custom)
                                            {
                                                Custom cSub = new Custom();
                                                cSub.SKU = SubSKU;
                                                cSub._id = (_Custom.Value._id != null) ? _Custom.Value._id.ToString() : "";
                                                cSub._content = (_Custom.Value.content != null) ? _Custom.Value.content.ToString() : "";
                                                cListSub.Add(cSub);
                                            }
                                        }
                                        //Images
                                        if (_SubProducts.Value.Image != null)
                                        {
                                            foreach (dynamic _ImageSub in _SubProducts.Value.Image)
                                            {
                                                Images iSub = new Images();
                                                iSub.SKU = SubSKU;
                                                iSub._replacing_existing = false;
                                                iSub._inactive = "";
                                                iSub.CurrentRank = "";
                                                iSub.Large = "";
                                                iSub.PopUp = "";
                                                iSub.Thumbnail = "";
                                                iSub.Title = "";
                                                iSub.Type = "";
                                                iSub.Link = (_SubProducts.Value.Image.Link != null) ? _SubProducts.Value.Image.Link.ToString() : "";
                                                iListSub.Add(iSub);
                                            }
                                        }
                                        //InventoryControl
                                        if (_SubProducts.Value.InventoryControl != null)
                                        {
                                            icSub.SKU = SubSKU;
                                            icSub._ignore_backorder = Convert.ToBoolean(_SubProducts.Value.InventoryControl._ignore_backorder);
                                            icSub._inventory_control_exempt = Convert.ToBoolean(_SubProducts.Value.InventoryControl._inventory_control_exempt);
                                            icSub.Inventory = (_SubProducts.Value.InventoryControl.Inventory != null) ? _SubProducts.Value.InventoryControl.Inventory.ToString() : "";
                                            icSub.OnOrder = (_SubProducts.Value.InventoryControl.OnOrder != null) ? _SubProducts.Value.InventoryControl.OnOrder.ToString() : "";
                                            icSub.OutOfStockPoint = (_SubProducts.Value.InventoryControl.OutOfStockPoint != null) ? _SubProducts.Value.InventoryControl.OutOfStockPoint.ToString() : "";
                                            icSub.Status = (_SubProducts.Value.InventoryControl.Status != null) ? _SubProducts.Value.InventoryControl.Status.ToString() : "";
                                            icListSub.Add(icSub);
                                        }
                                        ////QuantityRestrictions
                                        if (_SubProducts.Value.QuantityRestrictions != null)
                                        {
                                            qrSub.SKU = SubSKU;
                                            qrSub.MaximumQuantity = (_SubProducts.Value.QuantityRestrictions.MaximumQuantity != null) ? _SubProducts.Value.QuantityRestrictions.MaximumQuantity.ToString() : "";
                                            qrSub.MinimumQuantity = (_SubProducts.Value.QuantityRestrictions.MinimumQuantity != null) ? _SubProducts.Value.QuantityRestrictions.MinimumQuantity.ToString() : "";
                                            qrSub.NumIterationsDisplayed = (_SubProducts.Value.QuantityRestrictions.NumIterationsDisplayed != null) ? _SubProducts.Value.QuantityRestrictions.NumIterationsDisplayed.ToString() : "";
                                            qrSub.QuantityInSet = (_SubProducts.Value.QuantityRestrictions.QuantityInSet != null) ? _SubProducts.Value.QuantityRestrictions.QuantityInSet.ToString() : "";
                                            qrListSub.Add(qrSub);
                                        }
                                        ////Retail
                                        if (_SubProducts.Value.Retail != null)
                                        {
                                            int indexCountRetail = 1;
                                            foreach (dynamic _Retail in _SubProducts.Value.Retail)
                                            {
                                                Retail rSub = new Retail();
                                                if (indexCountRetail == 1)
                                                {

                                                }
                                                else
                                                {
                                                    rSub.SKU = SubSKU;
                                                    rSub._active = Convert.ToBoolean(item.Value.Retail._active);
                                                    rSub._price_category = (_Retail.Value._price_category != null) ? _Retail.Value._price_category.ToString() : "";
                                                    rSub.SpecialPrice = (_Retail.Value.SpecialPrice != null) ? _Retail.Value.SpecialPrice.ToString() : "";
                                                    rSub.StandardPrice = (_Retail.Value.StandardPrice != null) ? _Retail.Value.StandardPrice.ToString() : "";
                                                    rList.Add(rSub);
                                                }
                                                indexCountRetail++;
                                            }


                                        }
                                        ////Weight
                                        if (_SubProducts.Value.Weight != null)
                                        {
                                            wSub.SKU = SubSKU;
                                            wSub.ShipWeight = (_SubProducts.Value.Weight.ShipWeight != null) ? _SubProducts.Value.Weight.ShipWeight.ToString() : "";
                                            wSub.DisplayWeight = (_SubProducts.Value.Weight.DisplayWeight != null) ? _SubProducts.Value.Weight.DisplayWeight.ToString() : "";
                                            wSub.DisplayUnit = (_SubProducts.Value.Weight.DisplayUnit != null) ? _SubProducts.Value.Weight.DisplayUnit.ToString() : "";
                                            wListSub.Add(wSub);
                                        }
                                        ////Shipping
                                        if (_SubProducts.Value.Shipping != null)
                                        {
                                            sSub.SKU = SubSKU;
                                            sSub.ShippingPreference = (_SubProducts.Value.Shipping.ShippingPreference != null) ? _SubProducts.Value.Shipping.ShippingPreference.ToString() : "";
                                            sSub.FixedRate = (_SubProducts.Value.Shipping.FixedRate != null) ? _SubProducts.Value.Shipping.FixedRate.ToString() : "";
                                            sSub.PromoFixedRate = (_SubProducts.Value.Shipping.PromoFixedRate != null) ? _SubProducts.Value.Shipping.PromoFixedRate.ToString() : "";
                                            sListSub.Add(sSub);
                                        }

                                    }
                                    indexCountSubProducts++;
                                }
                            }




                        }
                    }


                }

                if (parseType == "Categories")
                {
                    using (StreamReader rdr = new StreamReader(filepath))
                    {
                        string json = rdr.ReadToEnd();
                        dynamic array = JsonConvert.DeserializeObject(json);
                        int itemIndex = 0;



                        foreach (dynamic item in array.CV3Data.categories)
                        {
                            itemIndex++;

                            CategoriesMap c = new CategoriesMap();

                            string ID = (item.Value.ID != null) ? item.Value.ID.ToString() : "";

                            //web products
                            c._invisible = Convert.ToBoolean(item.Value._invisible);
                            c._featured = Convert.ToBoolean(item.Value._featured);
                            c.Name = (item.Value.Name != null) ? item.Value.Name.ToString() : "";
                            c.ID = (item.Value.ID != null) ? item.Value.ID.ToString() : "";
                            c.URLName = (item.Value.URLName != null) ? item.Value.URLName.ToString() : "";
                            c.Description = (item.Value.Description != null) ? item.Value.Description.ToString() : "";
                            c.MetaTitle = (item.Value.MetaTitle != null) ? item.Value.MetaTitle.ToString() : "";
                            c.MetaDescription = (item.Value.MetaDescription != null) ? item.Value.MetaDescription.ToString() : "";
                            c.MetaKeywords = (item.Value.MetaKeywords != null) ? item.Value.MetaKeywords.ToString() : "";
                            c.Template = (item.Value.Template != null) ? item.Value.Template.ToString() : "";
                            c.NumProductsPerPage = (item.Value.NumProductsPerPage != null) ? item.Value.NumProductsPerPage.ToString() : "";
                            c.CategoryPath = (item.Value.CategoryPath != null) ? item.Value.CategoryPath.ToString() : "";
                            ////Retail
                            if (item.Value.Products != null)
                            {
                                int indexCountProducts = 0;
                                foreach (dynamic _Products in item.Value.Products)
                                {
                                    CategoriesMapP r = new CategoriesMapP();
                                    r.ID = ID;
                                    r.Products = (_Products.Value != null) ? _Products.Value.ToString() : "";
                                    pListSub.Add(r);

                                    indexCountProducts++;
                                }


                            }
                            if (item.Value.FeaturedProducts != null)
                            {
                                int indexCountFeaturedProducts = 0;
                                foreach (dynamic _FeaturedProducts in item.Value.FeaturedProducts)
                                {
                                    CategoriesMapF r = new CategoriesMapF();
                                    r.ID = ID;
                                    r.FeaturedProducts = (_FeaturedProducts.Value != null) ? _FeaturedProducts.Value.ToString() : "";
                                    fListSub.Add(r);

                                    indexCountFeaturedProducts++;
                                }


                            }
                            if (item.Value.Custom != null)
                            {
                                int indexCountCustom = 0;
                                foreach (dynamic _Custom in item.Value.Custom)
                                {
                                    CategoriesMapC r = new CategoriesMapC();
                                    r.ID = ID;
                                    r._id = (_Custom.Value._id != null) ? _Custom.Value._id.ToString() : "";
                                    r.content = (_Custom.Value.content != null) ? _Custom.Value.content.ToString() : "";
                                    cmapListSub.Add(r);

                                    indexCountCustom++;
                                }


                            }

                            cmapList.Add(c);


                        }

                    }
                }


                if (parseType == "Products")
                {
                    //Product
                    //Attributes
                    BulkSaveListToDatabase(aList, "spcpNDFSaveAttributes");
                    BulkSaveListToDatabase(atListA, "spcpNDFSaveAttributesTitles");
                    BulkSaveListToDatabase(apListA, "spcpNDFSaveAttributesProducts");
                    //Parent
                    BulkSaveListToDatabase(wpList, "spcpNDFSaveWebProducts");
                    BulkSaveListToDatabase(catList, "spcpNDFSaveCategories");
                    BulkSaveListToDatabase(cList, "spcpNDFSaveCustom");
                    BulkSaveListToDatabase(iList, "spcpNDFSaveImages");
                    BulkSaveListToDatabase(icList, "spcpNDFSaveInventoryControl");
                    BulkSaveListToDatabase(mList, "spcpNDFSaveMeta");
                    BulkSaveListToDatabase(qrList, "spcpNDFSaveQuantityRestrictions");
                    BulkSaveListToDatabase(rList, "spcpNDFSaveRetail");
                    BulkSaveListToDatabase(apList, "spcpNDFSaveAdditionalProducts");
                    BulkSaveListToDatabase(wList, "spcpNDFSaveWeight");
                    BulkSaveListToDatabase(sList, "spcpNDFSaveShipping");
                    BulkSaveListToDatabase(spList, "spcpNDFSaveSubProducts");
                    //Sub
                    BulkSaveListToDatabase(cListSub, "spcpNDFSaveCustom");
                    BulkSaveListToDatabase(iListSub, "spcpNDFSaveImages");
                    BulkSaveListToDatabase(icListSub, "spcpNDFSaveInventoryControl");
                    BulkSaveListToDatabase(qrListSub, "spcpNDFSaveQuantityRestrictions");
                    BulkSaveListToDatabase(rListSub, "spcpNDFSaveRetail");
                    BulkSaveListToDatabase(wListSub, "spcpNDFSaveWeight");
                    BulkSaveListToDatabase(sListSub, "spcpNDFSaveShipping");
                }
                if (parseType == "Categories")
                {
                    //Category
                    //Parent
                    BulkSaveListToDatabase(cmapList, "spcpNDFSaveCategoriesMap");
                    //Sub
                    BulkSaveListToDatabase(pListSub, "spcpNDFSaveCategoriesMapProduct");
                    BulkSaveListToDatabase(fListSub, "spcpNDFSaveCategoriesMapFeaturedProduct");
                    //BulkSaveListToDatabase(cmapListSub, "spcpNDFSaveCategoriesMapCustom");
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Console.WriteLine(error);
            }
            return true;
        }
        private void BulkSaveListToDatabase<T>(List<T> list, string storedProc)
        {
            Console.WriteLine("Writing Data to: " + storedProc);
            string sClassType = typeof(T).ToString();
            try
            {
                Console.WriteLine("Rows to write back:" + list.Count);
                Console.WriteLine("sClassType: " + sClassType);
                foreach (var item in list)
                {
                    using (SqlConnection con = new SqlConnection(ConnectionStrings.SalesPad))
                    {
                        SqlCommand cmd = new SqlCommand(storedProc, con);
                        switch (sClassType.Trim())
                        {
                            case "ChannelAdvisorFeedSender.Classes.WebProducts":
                                WebProducts itemConverted = (WebProducts)Convert.ChangeType(item, typeof(WebProducts));
                                cmd.Parameters.AddWithValue("@_allow_fractional_qty", itemConverted._allow_fractional_qty);
                                cmd.Parameters.AddWithValue("@_comparable", itemConverted._comparable);
                                cmd.Parameters.AddWithValue("@_content_only", itemConverted._content_only);
                                cmd.Parameters.AddWithValue("@_featured", itemConverted._featured);
                                cmd.Parameters.AddWithValue("@_google_checkout_exempt", itemConverted._google_checkout_exempt);
                                cmd.Parameters.AddWithValue("@_hidden", itemConverted._hidden);
                                cmd.Parameters.AddWithValue("@_inactive", itemConverted._inactive);
                                cmd.Parameters.AddWithValue("@_is_donation", itemConverted._is_donation);
                                cmd.Parameters.AddWithValue("@_kit", itemConverted._kit);
                                cmd.Parameters.AddWithValue("@_new", itemConverted._new);
                                cmd.Parameters.AddWithValue("@_out_of_season", itemConverted._out_of_season);
                                cmd.Parameters.AddWithValue("@_tax_exempt", itemConverted._tax_exempt);
                                cmd.Parameters.AddWithValue("@_text_field", itemConverted._text_field);
                                cmd.Parameters.AddWithValue("@Brand", itemConverted.Brand);
                                cmd.Parameters.AddWithValue("@DescriptionHeader", itemConverted.DescriptionHeader);
                                cmd.Parameters.AddWithValue("@Manufacturer", itemConverted.Manufacturer);
                                cmd.Parameters.AddWithValue("@Name", itemConverted.Name.Replace("<![CDATA[", "").Replace("]]>", ""));
                                cmd.Parameters.AddWithValue("@ProdID", itemConverted.ProdID);
                                cmd.Parameters.AddWithValue("@Rating", itemConverted.Rating);
                                cmd.Parameters.AddWithValue("@SKU", itemConverted.SKU);
                                cmd.Parameters.AddWithValue("@URLName", itemConverted.URLName);
                                cmd.Parameters.AddWithValue("@Keywords", itemConverted.Keywords);
                                cmd.Parameters.AddWithValue("@DefaultCategory", itemConverted.DefaultCategory);
                                cmd.Parameters.AddWithValue("@DateCreated", itemConverted.DateCreated);
                                break;
                            case "ChannelAdvisorFeedSender.Classes.Attributes":
                                Attributes itemConvertedAttributes = (Attributes)Convert.ChangeType(item, typeof(Attributes));
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedAttributes.SKU);
                                cmd.Parameters.AddWithValue("@_active", itemConvertedAttributes._active);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.AttributesTitles":
                                AttributesTitles itemConvertedAttributesTitles = (AttributesTitles)Convert.ChangeType(item, typeof(AttributesTitles));
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedAttributesTitles.SKU);
                                cmd.Parameters.AddWithValue("@_column", itemConvertedAttributesTitles._column);
                                cmd.Parameters.AddWithValue("@content", itemConvertedAttributesTitles.content);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.AttributesProduct":
                                AttributesProduct itemConvertedAttributesProduct = (AttributesProduct)Convert.ChangeType(item, typeof(AttributesProduct));
                                cmd.Parameters.AddWithValue("@ParentSKU", itemConvertedAttributesProduct.ParentSKU);
                                cmd.Parameters.AddWithValue("@_status", itemConvertedAttributesProduct._status);
                                cmd.Parameters.AddWithValue("@_is_donation", itemConvertedAttributesProduct._is_donation);
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedAttributesProduct.SKU);
                                cmd.Parameters.AddWithValue("@AltID", itemConvertedAttributesProduct.AltID);
                                cmd.Parameters.AddWithValue("@ShipWeight", itemConvertedAttributesProduct.ShipWeight);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.Categories":
                                Categories itemConvertedCategories = (Categories)Convert.ChangeType(item, typeof(Categories));
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedCategories.SKU);
                                cmd.Parameters.AddWithValue("@_replacing_existing", itemConvertedCategories._replacing_existing);
                                cmd.Parameters.AddWithValue("@Category", itemConvertedCategories.Category);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.Custom":
                                Custom itemConvertedCustom = (Custom)Convert.ChangeType(item, typeof(Custom));
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedCustom.SKU);
                                cmd.Parameters.AddWithValue("@_id", itemConvertedCustom._id);
                                cmd.Parameters.AddWithValue("@_content", itemConvertedCustom._content);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.Images":
                                Images itemConvertedImages = (Images)Convert.ChangeType(item, typeof(Images));
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedImages.SKU);
                                cmd.Parameters.AddWithValue("@_replacing_existing", itemConvertedImages._replacing_existing);
                                cmd.Parameters.AddWithValue("@_inactive", itemConvertedImages._inactive);
                                cmd.Parameters.AddWithValue("@Large", itemConvertedImages.Large);
                                cmd.Parameters.AddWithValue("@CurrentRank", itemConvertedImages.CurrentRank);
                                cmd.Parameters.AddWithValue("@PopUp", itemConvertedImages.PopUp);
                                cmd.Parameters.AddWithValue("@Thumbnail", itemConvertedImages.Thumbnail);
                                cmd.Parameters.AddWithValue("@Title", itemConvertedImages.Title);
                                cmd.Parameters.AddWithValue("@Type", itemConvertedImages.Type);
                                cmd.Parameters.AddWithValue("@Link", itemConvertedImages.Link);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.InventoryControl":
                                InventoryControl itemConvertedInventoryControl = (InventoryControl)Convert.ChangeType(item, typeof(InventoryControl));
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedInventoryControl.SKU);
                                cmd.Parameters.AddWithValue("@_ignore_backorder", itemConvertedInventoryControl._ignore_backorder);
                                cmd.Parameters.AddWithValue("@_inventory_control_exempt", itemConvertedInventoryControl._inventory_control_exempt);
                                cmd.Parameters.AddWithValue("@Inventory", itemConvertedInventoryControl.Inventory);
                                cmd.Parameters.AddWithValue("@OnOrder", itemConvertedInventoryControl.OnOrder);
                                cmd.Parameters.AddWithValue("@OutOfStockPoint", itemConvertedInventoryControl.OutOfStockPoint);
                                cmd.Parameters.AddWithValue("@Status", itemConvertedInventoryControl.Status);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.Meta":
                                Meta itemConvertedMeta = (Meta)Convert.ChangeType(item, typeof(Meta));
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedMeta.SKU);
                                cmd.Parameters.AddWithValue("@Keywords", itemConvertedMeta.Keywords);
                                cmd.Parameters.AddWithValue("@Title", itemConvertedMeta.Title);
                                cmd.Parameters.AddWithValue("@Description", itemConvertedMeta.Description);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.QuantityRestrictions":
                                QuantityRestrictions itemConvertedQuantityRestrictions = (QuantityRestrictions)Convert.ChangeType(item, typeof(QuantityRestrictions));
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedQuantityRestrictions.SKU);
                                cmd.Parameters.AddWithValue("@MaximumQuantity", itemConvertedQuantityRestrictions.MaximumQuantity);
                                cmd.Parameters.AddWithValue("@MinimumQuantity", itemConvertedQuantityRestrictions.MinimumQuantity);
                                cmd.Parameters.AddWithValue("@NumIterationsDisplayed", itemConvertedQuantityRestrictions.NumIterationsDisplayed);
                                cmd.Parameters.AddWithValue("@QuantityInSet", itemConvertedQuantityRestrictions.QuantityInSet);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.Retail":
                                Retail itemConvertedRetail = (Retail)Convert.ChangeType(item, typeof(Retail));
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedRetail.SKU);
                                cmd.Parameters.AddWithValue("@_active", itemConvertedRetail._active);
                                cmd.Parameters.AddWithValue("@_price_category", itemConvertedRetail._price_category);
                                cmd.Parameters.AddWithValue("@SpecialPrice", itemConvertedRetail.SpecialPrice);
                                cmd.Parameters.AddWithValue("@StandardPrice", itemConvertedRetail.StandardPrice);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.AdditionalProducts":
                                AdditionalProducts itemConvertedAdditionalProducts = (AdditionalProducts)Convert.ChangeType(item, typeof(AdditionalProducts));
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedAdditionalProducts.SKU);
                                cmd.Parameters.AddWithValue("@AdditionalSKUs", itemConvertedAdditionalProducts.AdditionalSKUs);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.SubProducts":
                                SubProducts itemConvertedSubProducts = (SubProducts)Convert.ChangeType(item, typeof(SubProducts));
                                cmd.Parameters.AddWithValue("@ParentSKU", itemConvertedSubProducts.ParentSKU);
                                cmd.Parameters.AddWithValue("@_active", itemConvertedSubProducts._active);
                                cmd.Parameters.AddWithValue("@_allow_fractional_qty", itemConvertedSubProducts._allow_fractional_qty);
                                cmd.Parameters.AddWithValue("@_google_checkout_exempt", itemConvertedSubProducts._google_checkout_exempt);
                                cmd.Parameters.AddWithValue("@_inactive", itemConvertedSubProducts._inactive);
                                cmd.Parameters.AddWithValue("@_is_donation", itemConvertedSubProducts._is_donation);
                                cmd.Parameters.AddWithValue("@_out_of_season", itemConvertedSubProducts._out_of_season);
                                cmd.Parameters.AddWithValue("@_tax_exempt", itemConvertedSubProducts._tax_exempt);
                                cmd.Parameters.AddWithValue("@Name", itemConvertedSubProducts.Name.Replace("<![CDATA[", "").Replace("]]>", ""));
                                cmd.Parameters.AddWithValue("@ProdID", itemConvertedSubProducts.ProdID);
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedSubProducts.SKU);
                                cmd.Parameters.AddWithValue("@DateCreated", itemConvertedSubProducts.DateCreated);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.Weight":
                                Weight itemConvertedWeight = (Weight)Convert.ChangeType(item, typeof(Weight));
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedWeight.SKU);
                                cmd.Parameters.AddWithValue("@ShipWeight", itemConvertedWeight.ShipWeight);
                                cmd.Parameters.AddWithValue("@DisplayWeight", itemConvertedWeight.DisplayWeight);
                                cmd.Parameters.AddWithValue("@DisplayUnit", itemConvertedWeight.DisplayUnit);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.Shipping":
                                Shipping itemConvertedShipping = (Shipping)Convert.ChangeType(item, typeof(Shipping));
                                cmd.Parameters.AddWithValue("@SKU", itemConvertedShipping.SKU);
                                cmd.Parameters.AddWithValue("@ShippingPreference", itemConvertedShipping.ShippingPreference);
                                cmd.Parameters.AddWithValue("@FixedRate", itemConvertedShipping.FixedRate);
                                cmd.Parameters.AddWithValue("@PromoFixedRate", itemConvertedShipping.PromoFixedRate);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.CategoriesMap":
                                CategoriesMap itemConvertedCategoriesMap = (CategoriesMap)Convert.ChangeType(item, typeof(CategoriesMap));
                                cmd.Parameters.AddWithValue("@_invisible", itemConvertedCategoriesMap._invisible);
                                cmd.Parameters.AddWithValue("@_featured", itemConvertedCategoriesMap._featured);
                                cmd.Parameters.AddWithValue("@Name", itemConvertedCategoriesMap.Name);
                                cmd.Parameters.AddWithValue("@ID", itemConvertedCategoriesMap.ID);
                                cmd.Parameters.AddWithValue("@URLName", itemConvertedCategoriesMap.URLName);
                                cmd.Parameters.AddWithValue("@Description", itemConvertedCategoriesMap.Description);
                                cmd.Parameters.AddWithValue("@MetaTitle", itemConvertedCategoriesMap.MetaTitle);
                                cmd.Parameters.AddWithValue("@MetaDescription", itemConvertedCategoriesMap.MetaDescription);
                                cmd.Parameters.AddWithValue("@MetaKeywords", itemConvertedCategoriesMap.MetaKeywords);
                                cmd.Parameters.AddWithValue("@Template", itemConvertedCategoriesMap.Template);
                                cmd.Parameters.AddWithValue("@NumProductsPerPage", itemConvertedCategoriesMap.NumProductsPerPage);
                                cmd.Parameters.AddWithValue("@CategoryPath", itemConvertedCategoriesMap.CategoryPath);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.CategoriesMapP":
                                CategoriesMapP itemConvertedCategoriesMapP = (CategoriesMapP)Convert.ChangeType(item, typeof(CategoriesMapP));
                                cmd.Parameters.AddWithValue("@ID", itemConvertedCategoriesMapP.ID);
                                cmd.Parameters.AddWithValue("@Products", itemConvertedCategoriesMapP.Products);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.CategoriesMapF":
                                CategoriesMapF itemConvertedCategoriesMapF = (CategoriesMapF)Convert.ChangeType(item, typeof(CategoriesMapF));
                                cmd.Parameters.AddWithValue("@ID", itemConvertedCategoriesMapF.ID);
                                cmd.Parameters.AddWithValue("@FeaturedProducts", itemConvertedCategoriesMapF.FeaturedProducts);

                                break;
                            case "ChannelAdvisorFeedSender.Classes.CategoriesMapC":
                                CategoriesMapC itemConvertedCategoriesMapC = (CategoriesMapC)Convert.ChangeType(item, typeof(CategoriesMapC));
                                cmd.Parameters.AddWithValue("@ID", itemConvertedCategoriesMapC.ID);
                                cmd.Parameters.AddWithValue("@_id", itemConvertedCategoriesMapC._id);
                                cmd.Parameters.AddWithValue("@content", itemConvertedCategoriesMapC.content);

                                break;


                            default:
                                break;
                        }
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 180;
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }

                }
                Console.WriteLine("" + sClassType + " Successful");
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Console.WriteLine(error);
            }
        }
    }
}
