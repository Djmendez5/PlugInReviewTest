using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
namespace Plugin
{
    namespace PlugInReviewTest
    {

        public class class1 : IPlugin
        {
            public void Execute(IServiceProvider serviceProvider)
            {
                // Obtain the tracing service
                ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                // Obtain the execution context from the service provider.  
                IPluginExecutionContext context = (IPluginExecutionContext)
                    serviceProvider.GetService(typeof(IPluginExecutionContext));

                // The InputParameters collection contains all the data passed in the message request.  
                if (context.InputParameters.Contains("Target") &&
                    context.InputParameters["Target"] is Entity)
                {
                    // Obtain the target entity from the input parameters.  
                    Entity account = (Entity)context.InputParameters["Target"];

                    // Obtain the organization service reference which you will need for  
                    // web service calls.  
                    IOrganizationServiceFactory serviceFactory =
                        (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                    try
                    {
                        tracingService.Trace("test");
                        OptionSetValue option = new OptionSetValue(6);
                        tracingService.Trace("account name " + account.Attributes["name"]);
                        tracingService.Trace("option field: " + ((OptionSetValue)account.Attributes["customertypecode"]).Value);
                        tracingService.Trace("parent look up field" + account.Attributes["parentaccountid"]);
                        tracingService.Trace("option field: " + (account.FormattedValues["customertypecode"]).ToString());
                        account.Attributes["customertypecode"] = option;

                        service.Update(account);
                        EntityReference parent = (EntityReference)account.Attributes["parentaccountid"];
                        var cols = new ColumnSet(
          new String[] { "name"});
                        Entity parentAccount = service.Retrieve(parent.LogicalName, parent.Id,cols);

                        tracingService.Trace("parent account name =" + parentAccount.Attributes["name"]);

                        /*if (bankTransaction.Attributes.Contains("new_bankaccount"))
                        {
                            tracingService.Trace("Look up field" + bankTransaction.Attributes["new_bankaccount"]);
                            tracingService.Trace("Look up field" + bankTransaction.Attributes["new_bankaccount"].ToString());

                            decimal amount = ((Money)bankTransaction.Attributes["new_transactionamount"]).Value;

                            //get info
                            EntityReference bank = (EntityReference)bankTransaction.Attributes["new_bankaccount"];
                            Entity entity = service.Retrieve(bank.LogicalName, bank.Id,
                         new ColumnSet("new_customername", "new_totalamount"));

                            //assign it 
                            bankTransaction.Attributes.Add("new_customername", entity.Attributes["new_customername"]);

                            tracingService.Trace("Transaction amount" + amount.ToString());

                            tracingService.Trace("name: " + entity.Attributes["new_customername"].ToString());

                            decimal total = ((Money)entity.Attributes["new_totalamount"]).Value;

                            decimal left = total - amount;

                            bankTransaction.Attributes.Add("new_totalleft", new Money(left));

                            tracingService.Trace("total left: " + left);


                        }
                        else
                        {
                            tracingService.Trace("No look up field");
                        }*/                




                    }
                    catch (FaultException<OrganizationServiceFault> ex)
                    {
                        throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin.", ex);
                    }

                    catch (Exception ex)
                    {
                        tracingService.Trace("FollowUpPlugin: {0}", ex.ToString());
                        throw;
                    }
                }
            }









        }
    }
}
