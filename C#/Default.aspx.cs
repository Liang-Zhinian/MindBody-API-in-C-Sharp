using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using API_Example.ClassService;
using API_Example.ClientService;
using API_Example.SaleService;
using XMLDetailLevel = API_Example.SaleService.XMLDetailLevel;

namespace API_Example
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Set credential values
            string SOURCENAME = "YourSourceName";
            string APIKEY = "YourAPIKey";
            var siteIDs = new List<int> { -99 };


            ///////////////////////
            // Standard API call //
            ///////////////////////

            // Create Service
            ClassService.ClassServiceSoapClient classService = new ClassServiceSoapClient();

            // Create request
            ClassService.GetClassesRequest classRequest = new ClassService.GetClassesRequest();

            // Create and fill credentials
            classRequest.SourceCredentials = new ClassService.SourceCredentials();
            classRequest.SourceCredentials.SourceName = SOURCENAME;
            classRequest.SourceCredentials.Password = APIKEY;
            classRequest.SourceCredentials.SiteIDs = new ClassService.ArrayOfInt { -99 };

            // Run call with request and fill result  
            ClassService.GetClassesResult classResult = classService.GetClasses(classRequest);



            // Display result in label
            foreach (ClassService.Class thisClass in classResult.Classes)
            {
                Label.Text += "<br/>______________________________________<div><p>" +
                    thisClass.ClassDescription.Name + "</p>" +
                    "<p>" + thisClass.ID + "</p>" +
                    "<p>" + thisClass.EndDateTime + " - " + thisClass.EndDateTime + "</p>" +
                    "<p>" + thisClass.ClassDescription.Description + "</p></div>";
            }


            ////////////////////////////
            // SSL protected API call //
            ////////////////////////////

            //Call service 
            ClientService.ClientServiceSoapClient clientService = new ClientServiceSoapClient();

            // Create request
            AddOrUpdateClientsRequest addClientsRequest = new AddOrUpdateClientsRequest();

            addClientsRequest.UpdateAction = "AddNew";

            // Create and fill credentials
            addClientsRequest.SourceCredentials = new ClientService.SourceCredentials();
            addClientsRequest.SourceCredentials.SourceName = SOURCENAME;
            addClientsRequest.SourceCredentials.Password = APIKEY;
            addClientsRequest.SourceCredentials.SiteIDs = new ClientService.ArrayOfInt { -99 };

            // Create new client
            addClientsRequest.Clients = new ClientService.Client[
                Convert.ToInt32(ID = "123")
                ];

            clientService.AddOrUpdateClients(addClientsRequest);


            // Create Service
            SaleService.SaleServiceSoapClient saleService = new SaleService.SaleServiceSoapClient();

            // Create request
            SaleService.CheckoutShoppingCartRequest saleRequest = new SaleService.CheckoutShoppingCartRequest();



            // Create and fill credentials
            saleRequest.SourceCredentials = new SaleService.SourceCredentials();
            saleRequest.SourceCredentials.SourceName = SOURCENAME;
            saleRequest.SourceCredentials.Password = APIKEY;
            saleRequest.SourceCredentials.SiteIDs = new SaleService.ArrayOfInt { -99 };

            // Add ClientID
            saleRequest.ClientID = "123";

            // Create and add cart items
            SaleService.CartItem[] items = { new SaleService.CartItem() };
            items[0].Quantity = 1;
            items[0].Item = new Product { ID = "1042" };
            saleRequest.CartItems = items;

            // Create and add credit card info

            SaleService.PaymentInfo paymentInfo = new PaymentInfo();

            PaymentInfo[] paymentsInfo = new PaymentInfo[] {new SaleService.CreditCardInfo
            {
                CreditCardNumber = "4111111111111111",
                Amount = 194.40M,
                BillingAddress = "123 Something",
                BillingCity = "SLO",
                BillingState = "CA",
                BillingPostalCode = "93405",
                BillingName = "MindBody",
                ExpMonth = "7",
                ExpYear = "2016"  
            }};

            saleRequest.Payments = paymentsInfo;
            saleRequest.XMLDetail = XMLDetailLevel.Full;
            saleRequest.PageSize = 10;
            saleRequest.CurrentPageIndex = 0;
            saleRequest.Test = true;
            saleRequest.InStore = true;


            // Run call with request and fill result 
            SaleService.CheckoutShoppingCartResult saleResult = saleService.CheckoutShoppingCart(saleRequest);

            // Display result in label
            Label.Text += "<br/>**************************************<div>";

            Label.Text += "<p> You purchased the following items: </p>";

            for (int i = 0; i < saleResult.ShoppingCart.CartItems.Count(); i++)
            {
                SaleService.CartItem[] tempCartItems = { new SaleService.CartItem() };
                tempCartItems = saleResult.ShoppingCart.CartItems;
                if (saleResult.ShoppingCart.CartItems[i].Item.GetType().FullName == "API_Example.SaleService.Product")
                {
                    SaleService.Product product = (Product)saleResult.ShoppingCart.CartItems[i].Item;
                    Label.Text += "<p>Product Name: " + product.Name + "</p>";
                    Label.Text += "<p>Product Price: " + decimal.Parse(product.Price.ToString()).ToString("C", CultureInfo.CurrentCulture) + "</p>";
                }

            }

            Label.Text += "<p> Total Tax: " + double.Parse(saleResult.ShoppingCart.TaxTotal.ToString()).ToString("C", CultureInfo.CurrentCulture) + " </p>";
            Label.Text += "<p> Grand Total: " + double.Parse(saleResult.ShoppingCart.GrandTotal.ToString()).ToString("C", CultureInfo.CurrentCulture) + " </p>";

        }
    }
}
