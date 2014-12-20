using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using API_Example.ClassService;
using API_Example.ClientService;

namespace API_Example
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Set credential values
            string SOURCENAME = "YourSourceName";
            string APIKEY = "YourAPIKey";
            //int[] siteIDs = { -99 };
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
            items[0].ID = 93;
            saleRequest.CartItems = items;

            // Create and add credit card info
            SaleService.CreditCardInfo[] payments = { new SaleService.CreditCardInfo() };
            payments[0].CreditCardNumber = "4111111111111111";
            payments[0].Amount = 2.00M;
            payments[0].BillingAddress = "123 Something";
            payments[0].BillingCity = "SLO";
            payments[0].BillingState = "CA";
            payments[0].BillingPostalCode = "93405";
            payments[0].BillingName = "MindBody";
            payments[0].ExpMonth = "7";
            payments[0].ExpYear = "2016";

            saleRequest.Payments = payments;

            // Run call with request and fill result 
            SaleService.CheckoutShoppingCartResult saleResult = saleService.CheckoutShoppingCart(saleRequest);

            // Display result in label
            Label.Text += "<br/>**************************************<div><p>" + saleResult.Message + "</p>";

        }
    }
}
