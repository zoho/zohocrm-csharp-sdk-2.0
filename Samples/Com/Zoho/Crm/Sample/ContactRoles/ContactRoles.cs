using System;

using System.Collections;

using System.Collections.Generic;

using System.Reflection;

using Com.Zoho.Crm.API;

using Com.Zoho.Crm.API.ContactRoles;

using Com.Zoho.Crm.API.Users;

using Com.Zoho.Crm.API.Util;

using Newtonsoft.Json;

using static Com.Zoho.Crm.API.ContactRoles.ContactRolesOperations;

using ActionHandler = Com.Zoho.Crm.API.ContactRoles.ActionHandler;

using ActionResponse = Com.Zoho.Crm.API.ContactRoles.ActionResponse;

using ActionWrapper = Com.Zoho.Crm.API.ContactRoles.ActionWrapper;

using APIException = Com.Zoho.Crm.API.ContactRoles.APIException;

using BodyWrapper = Com.Zoho.Crm.API.ContactRoles.BodyWrapper;

using ResponseHandler = Com.Zoho.Crm.API.ContactRoles.ResponseHandler;

using ResponseWrapper = Com.Zoho.Crm.API.ContactRoles.ResponseWrapper;

using SuccessResponse = Com.Zoho.Crm.API.ContactRoles.SuccessResponse;

namespace Com.Zoho.Crm.Sample.ContactRoles
{
    public class ContactRoles
    {
        /// <summary>
        /// This method is used to get all the Contact Roles and print the response.
        /// </summary>
        public static void GetContactRoles()
        {
            //Get instance of ContactRolesOperations Class
            ContactRolesOperations contactRolesOperations = new ContactRolesOperations();
            
            //Call GetContactRoles method
            APIResponse<ResponseHandler> response = contactRolesOperations.GetContactRoles();
            
            if(response != null)
            {
                //Get the status code from response
                Console.WriteLine("Status Code: " + response.StatusCode);
                
                if(new List<int>() { 204, 304 }.Contains(response.StatusCode))
                {
                    Console.WriteLine(response.StatusCode == 204? "No Content" : "Not Modified");

                    return;
                }
                
                //Check if expected response is received
                if(response.IsExpected)
                {
                    //Get object from response
                    ResponseHandler responseHandler = response.Object;
                    
                    if(responseHandler is ResponseWrapper)
                    {
                        //Get the received ResponseWrapper instance
                        ResponseWrapper responseWrapper = (ResponseWrapper) responseHandler;
                        
                        //Get the list of obtained ContactRole instances
                        List<ContactRole> contactRoles = responseWrapper.ContactRoles;
                        
                        foreach(ContactRole contactRole in contactRoles)
                        {
                            //Get the ID of each ContactRole
                            Console.WriteLine("ContactRole ID: " + contactRole.Id);
                            
                            //Get the name of each ContactRole
                            Console.WriteLine("ContactRole Name: " + contactRole.Name);
                            
                            //Get the sequence number each ContactRole
                            Console.WriteLine("ContactRole SequenceNumber: " + contactRole.SequenceNumber);
                        }
                    }
                    //Check if the request returned an exception
                    else if(responseHandler is APIException)
                    {
                        //Get the received APIException instance
                        APIException exception = (APIException) responseHandler;
                        
                        //Get the Status
                        Console.WriteLine("Status: " + exception.Status.Value);
                        
                        //Get the Code
                        Console.WriteLine("Code: " + exception.Code.Value);
                        
                        Console.WriteLine("Details: " );
                        
                        //Get the details map
                        foreach(KeyValuePair<string, object> entry in exception.Details)
                        {
                            //Get each value in the map
                            Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                        }
                        
                        //Get the Message
                        Console.WriteLine("Message: " + exception.Message.Value);
                    }
                }
                else
                { //If response is not as expected

                    //Get model object from response
                    Model responseObject = response.Model;

                    //Get the response object's class
                    Type type = responseObject.GetType();

                    //Get all declared fields of the response class
                    Console.WriteLine("Type is: {0}", type.Name);

                    PropertyInfo[] props = type.GetProperties();

                    Console.WriteLine("Properties (N = {0}):", props.Length);

                    foreach (var prop in props)
                    {
                        if (prop.GetIndexParameters().Length == 0)
                        {
                            Console.WriteLine("{0} ({1}) in {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(responseObject));
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1}) in <Indexed>", prop.Name, prop.PropertyType.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to create Contact Roles and print the response.
        /// </summary>
        public static void CreateContactRoles()
        {
            //Get instance of ContactRolesOperations Class
            ContactRolesOperations contactRolesOperations = new ContactRolesOperations();
            
            //Get instance of BodyWrapper Class that will contain the request body
            BodyWrapper bodyWrapper = new BodyWrapper();
            
            //List of ContactRole instances
            List<ContactRole> contactRoles = new List<ContactRole>();
            
            for(int i = 1; i <= 5; i++)
            {
                //Get instance of ContactRole Class
                ContactRole contactRole = new ContactRole();

                //Set name of the Contact Role
                contactRole.Name = "contactRole2" + i;

                //Set sequence number of the Contact Role
                contactRole.SequenceNumber = i;
                
                //Add ContactRole instance to the list
                contactRoles.Add(contactRole);
            }
            
            //Set the list to contactRoles in BodyWrapper instance
            bodyWrapper.ContactRoles = contactRoles;
            
            //Call CreateContactRoles method that takes BodyWrapper instance as parameter 
            APIResponse<ActionHandler> response = contactRolesOperations.CreateContactRoles(bodyWrapper);
            
            if(response != null)
            {
                //Get the status code from response
                Console.WriteLine("Status Code: " + response.StatusCode);
                
                //Check if expected response is received
                if(response.IsExpected)
                {
                    //Get object from response
                    ActionHandler actionHandler = response.Object;
                    
                    if(actionHandler is ActionWrapper)
                    {
                        //Get the received ActionWrapper instance
                        ActionWrapper actionWrapper = (ActionWrapper) actionHandler;
                        
                        //Get the list of obtained ActionResponse instances
                        List<ActionResponse> actionResponses = actionWrapper.ContactRoles;
                        
                        foreach(ActionResponse actionResponse in actionResponses)
                        {
                            //Check if the request is successful
                            if(actionResponse is SuccessResponse)
                            {
                                //Get the received SuccessResponse instance
                                SuccessResponse successResponse = (SuccessResponse)actionResponse;
                                
                                //Get the Status
                                Console.WriteLine("Status: " + successResponse.Status.Value);
                                
                                //Get the Code
                                Console.WriteLine("Code: " + successResponse.Code.Value);
                                
                                Console.WriteLine("Details: " );
                                
                                //Get the details map
                                foreach(KeyValuePair<string, object> entry in successResponse.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + " : " + JsonConvert.SerializeObject(entry.Value));
                                }
                                
                                //Get the Message
                                Console.WriteLine("Message: " + successResponse.Message.Value);
                            }
                            //Check if the request returned an exception
                            else if(actionResponse is APIException)
                            {
                                //Get the received APIException instance
                                APIException exception = (APIException) actionResponse;
                                
                                //Get the Status
                                Console.WriteLine("Status: " + exception.Status.Value);
                                
                                //Get the Code
                                Console.WriteLine("Code: " + exception.Code.Value);
                                
                                Console.WriteLine("Details: " );
                                
                                //Get the details map
                                foreach(KeyValuePair<string, object> entry in exception.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + " : " + JsonConvert.SerializeObject(entry.Value));
                                }
                                
                                //Get the Message
                                Console.WriteLine("Message: " + exception.Message.Value);
                            }
                        }
                    }
                    //Check if the request returned an exception
                    else if(actionHandler is APIException)
                    {
                        //Get the received APIException instance
                        APIException exception = (APIException) actionHandler;
                        
                        //Get the Status
                        Console.WriteLine("Status: " + exception.Status.Value);
                        
                        //Get the Code
                        Console.WriteLine("Code: " + exception.Code.Value);
                        
                        Console.WriteLine("Details: " );
                        
                        //Get the details map
                        foreach(KeyValuePair<string, object> entry in exception.Details)
                        {
                            //Get each value in the map
                            Console.WriteLine(entry.Key + " : " + JsonConvert.SerializeObject(entry.Value));
                        }
                        
                        //Get the Message
                        Console.WriteLine("Message: " + exception.Message.Value);
                    }
                }
                else
                { //If response is not as expected

                    //Get model object from response
                    Model responseObject = response.Model;

                    //Get the response object's class
                    Type type = responseObject.GetType();

                    //Get all declared fields of the response class
                    Console.WriteLine("Type is: {0}", type.Name);

                    PropertyInfo[] props = type.GetProperties();

                    Console.WriteLine("Properties (N = {0}):", props.Length);

                    foreach (var prop in props)
                    {
                        if (prop.GetIndexParameters().Length == 0)
                        {
                            Console.WriteLine("{0} ({1}) in {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(responseObject));
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1}) in <Indexed>", prop.Name, prop.PropertyType.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to update Contact Roles and print the response.
        /// </summary>
        public static void UpdateContactRoles()
        {
            //Get instance of ContactRolesOperations Class
            ContactRolesOperations contactRolesOperations = new ContactRolesOperations();
            
            //Get instance of BodyWrapper Class that will contain the request body
            BodyWrapper bodyWrapper = new BodyWrapper();
            
            //List of ContactRole instances
            List<ContactRole> contactRolesList = new List<ContactRole>();

            //Get instance of ContactRole Class
            ContactRole cr1 = new ContactRole();

            //Set ID to the ContactRole instance
            cr1.Id = 34770617540001;

            //Set name to the ContactRole instance
            cr1.Name = "Edited122";
            
            //Add ContactRole instance to the list
            contactRolesList.Add(cr1);
            
            //Get instance of ContactRole Class
            ContactRole cr2 = new ContactRole();
            
            //Set ID to the ContactRole instance
            cr2.Id = 34770617540002;
            
            //Set name to the ContactRole instance
            //cr2.Name = "Edit";
            
            //Add ContactRole instance to the list
            contactRolesList.Add(cr2);
            
            //Set the list to contactRoles in BodyWrapper instance
            bodyWrapper.ContactRoles = contactRolesList;
            
            //Call UpdateContactRoles method that takes BodyWrapper instance as parameter
            APIResponse<ActionHandler> response = contactRolesOperations.UpdateContactRoles(bodyWrapper);
            
            if(response != null)
            {
                //Get the status code from response
                Console.WriteLine("Status Code: " + response.StatusCode);
                
                //Check if expected response is received
                if(response.IsExpected)
                {
                    //Get object from response
                    ActionHandler actionHandler = response.Object;
                    
                    if(actionHandler is ActionWrapper)
                    {
                        //Get the received ActionWrapper instance
                        ActionWrapper actionWrapper = (ActionWrapper) actionHandler;
                        
                        //Get the list of obtained ActionResponse instances
                        List<ActionResponse> actionResponses = actionWrapper.ContactRoles;
                        
                        foreach(ActionResponse actionResponse in actionResponses)
                        {
                            //Check if the request is successful
                            if(actionResponse is SuccessResponse)
                            {
                                //Get the received SuccessResponse instance
                                SuccessResponse successResponse = (SuccessResponse)actionResponse;
                                
                                //Get the Status
                                Console.WriteLine("Status: " + successResponse.Status.Value);
                                
                                //Get the Code
                                Console.WriteLine("Code: " + successResponse.Code.Value);
                                
                                Console.WriteLine("Details: " );
                                
                                //Get the details map
                                foreach(KeyValuePair<string, object> entry in successResponse.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                                }
                                
                                //Get the Message
                                Console.WriteLine("Message: " + successResponse.Message.Value);
                            }
                            //Check if the request returned an exception
                            else if(actionResponse is APIException)
                            {
                                //Get the received APIException instance
                                APIException exception = (APIException) actionResponse;
                                
                                //Get the Status
                                Console.WriteLine("Status: " + exception.Status.Value);
                                
                                //Get the Code
                                Console.WriteLine("Code: " + exception.Code.Value);
                                
                                Console.WriteLine("Details: " );
                                
                                //Get the details map
                                foreach(KeyValuePair<string, object> entry in exception.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                                }
                                
                                //Get the Message
                                Console.WriteLine("Message: " + exception.Message.Value);
                            }
                        }
                    }
                    //Check if the request returned an exception
                    else if(actionHandler is APIException)
                    {
                        //Get the received APIException instance
                        APIException exception = (APIException) actionHandler;
                        
                        //Get the Status
                        Console.WriteLine("Status: " + exception.Status.Value);
                        
                        //Get the Code
                        Console.WriteLine("Code: " + exception.Code.Value);
                        
                        Console.WriteLine("Details: " );
                        
                        //Get the details map
                        foreach( KeyValuePair<string, object> entry in exception.Details)
                        {
                            //Get each value in the map
                            Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                        }
                        
                        //Get the Message
                        Console.WriteLine("Message: " + exception.Message.Value);
                    }
                }
                else
                { //If response is not as expected

                    //Get model object from response
                    Model responseObject = response.Model;

                    //Get the response object's class
                    Type type = responseObject.GetType();

                    //Get all declared fields of the response class
                    Console.WriteLine("Type is: {0}", type.Name);

                    PropertyInfo[] props = type.GetProperties();

                    Console.WriteLine("Properties (N = {0}):", props.Length);

                    foreach (var prop in props)
                    {
                        if (prop.GetIndexParameters().Length == 0)
                        {
                            Console.WriteLine("{0} ({1}) in {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(responseObject));
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1}) in <Indexed>", prop.Name, prop.PropertyType.Name);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// This method is used to delete Contact Roles and print the response.
        /// </summary>
        /// <param name="contactRoleIds">The List of ContactRole IDs to be deleted.</param>
        public static void DeleteContactRoles(List<string> contactRoleIds)
        {
            //example
            //List<string> contactRoleIds = new List<long>() { "34770615208001", "34770615208002", "34770615177003", "34770616104001" };

            //Get instance of ContactRolesOperations Class
            ContactRolesOperations contactRolesOperations = new ContactRolesOperations();
            
            //Get instance of ParameterMap Class
            ParameterMap paramInstance = new ParameterMap();
            
            foreach(string id in contactRoleIds)
            {
                paramInstance.Add(DeleteContactRolesParam.IDS, id);
            }
            
            //Call DeleteContactRoles method that takes paramInstance as parameter 
            APIResponse<ActionHandler> response = contactRolesOperations.DeleteContactRoles(paramInstance);
            
            if(response != null)
            {
                //Get the status code from response
                Console.WriteLine("Status Code: " + response.StatusCode);
                
                //Check if expected response is received
                if(response.IsExpected)
                {
                    //Get object from response
                    ActionHandler actionHandler = response.Object;
                    
                    if(actionHandler is ActionWrapper)
                    {
                        //Get the received ActionWrapper instance
                        ActionWrapper actionWrapper = (ActionWrapper) actionHandler;
                        
                        //Get the list of obtained ContactRole instances
                        List<ActionResponse> actionResponses = actionWrapper.ContactRoles;
                        
                        foreach(ActionResponse actionResponse in actionResponses)
                        {
                            //Check if the request is successful
                            if(actionResponse is SuccessResponse)
                            {
                                //Get the received SuccessResponse instance
                                SuccessResponse successResponse = (SuccessResponse)actionResponse;
                                
                                //Get the Status
                                Console.WriteLine("Status: " + successResponse.Status.Value);
                                
                                //Get the Code
                                Console.WriteLine("Code: " + successResponse.Code.Value);
                                
                                Console.WriteLine("Details: " );
                                
                                //Get the details map
                                foreach( KeyValuePair<string, object> entry in successResponse.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                                }
                                
                                //Get the Message
                                Console.WriteLine("Message: " + successResponse.Message.Value);
                            }
                            //Check if the request returned an exception
                            else if(actionResponse is APIException)
                            {
                                //Get the received APIException instance
                                APIException exception = (APIException) actionResponse;
                                
                                //Get the Status
                                Console.WriteLine("Status: " + exception.Status.Value);
                                
                                //Get the Code
                                Console.WriteLine("Code: " + exception.Code.Value);
                                
                                Console.WriteLine("Details: " );
                                
                                //Get the details map
                                foreach( KeyValuePair<string, object> entry in exception.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                                }
                                
                                //Get the Message
                                Console.WriteLine("Message: " + exception.Message.Value);
                            }
                        }
                    }
                    //Check if the request returned an exception
                    else if(actionHandler is APIException)
                    {
                        //Get the received APIException instance
                        APIException exception = (APIException) actionHandler;
                        
                        //Get the Status
                        Console.WriteLine("Status: " + exception.Status.Value);
                        
                        //Get the Code
                        Console.WriteLine("Code: " + exception.Code.Value);
                        
                        Console.WriteLine("Details: " );
                        
                        //Get the details map
                        foreach( KeyValuePair<string, object> entry in exception.Details)
                        {
                            //Get each value in the map
                            Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                        }
                        
                        //Get the Message
                        Console.WriteLine("Message: " + exception.Message.Value);
                    }
                }
                else
                { //If response is not as expected

                    //Get model object from response
                    Model responseObject = response.Model;

                    //Get the response object's class
                    Type type = responseObject.GetType();

                    //Get all declared fields of the response class
                    Console.WriteLine("Type is: {0}", type.Name);

                    PropertyInfo[] props = type.GetProperties();

                    Console.WriteLine("Properties (N = {0}):", props.Length);

                    foreach (var prop in props)
                    {
                        if (prop.GetIndexParameters().Length == 0)
                        {
                            Console.WriteLine("{0} ({1}) in {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(responseObject));
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1}) in <Indexed>", prop.Name, prop.PropertyType.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to get single Contact Role with ID and print the response.
        /// </summary>
        /// <param name="contactRoleId">The ID of the ContactRole to be obtained</param>
        public static void GetContactRole(long contactRoleId)
        {
            //example
            //long contactRoleId = 34770615177004;
            
            //Get instance of ContactRolesOperations Class
            ContactRolesOperations contactRolesOperations = new ContactRolesOperations();
            
            //Call GetContactRole method that takes contactRoleId as parameter
            APIResponse<ResponseHandler> response = contactRolesOperations.GetContactRole(contactRoleId);
            
            if(response != null)
            {
                //Get the status code from response
                Console.WriteLine("Status Code: " + response.StatusCode);
                
                if(new List<int>() { 204, 304 }.Contains(response.StatusCode))
                {
                    Console.WriteLine(response.StatusCode == 204? "No Content" : "Not Modified");
                    
                    return;
                }
                
                //Check if expected response is received
                if(response.IsExpected)
                {
                    //Get object from response
                    ResponseHandler responseHandler = response.Object;
                    
                    if(responseHandler is ResponseWrapper)
                    {
                        //Get the received ResponseWrapper instance
                        ResponseWrapper responseWrapper = (ResponseWrapper) responseHandler;
                        
                        //Get the list of obtained ContactRole instances
                        List<ContactRole> contactRoles = responseWrapper.ContactRoles;
                        
                        foreach(ContactRole contactRole in contactRoles)
                        {
                            //Get the ID of each ContactRole
                            Console.WriteLine("ContactRole ID: " + contactRole.Id);
                            
                            //Get the name of each ContactRole
                            Console.WriteLine("ContactRole Name: " + contactRole.Name);
                            
                            //Get the sequence number each ContactRole
                            Console.WriteLine("ContactRole SequenceNumber: " + contactRole.SequenceNumber);
                        }
                    }
                    //Check if the request returned an exception
                    else if(responseHandler is APIException)
                    {
                        //Get the received APIException instance
                        APIException exception = (APIException) responseHandler;
                        
                        //Get the Status
                        Console.WriteLine("Status: " + exception.Status.Value);
                        
                        //Get the Code
                        Console.WriteLine("Code: " + exception.Code.Value);
                        
                        Console.WriteLine("Details: " );
                        
                        //Get the details map
                        foreach( KeyValuePair<string, object> entry in exception.Details)
                        {
                            //Get each value in the map
                            Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                        }
                        
                        //Get the Message
                        Console.WriteLine("Message: " + exception.Message.Value);
                    }
                }
                else
                { //If response is not as expected

                    //Get model object from response
                    Model responseObject = response.Model;

                    //Get the response object's class
                    Type type = responseObject.GetType();

                    //Get all declared fields of the response class
                    Console.WriteLine("Type is: {0}", type.Name);

                    PropertyInfo[] props = type.GetProperties();

                    Console.WriteLine("Properties (N = {0}):", props.Length);

                    foreach (var prop in props)
                    {
                        if (prop.GetIndexParameters().Length == 0)
                        {
                            Console.WriteLine("{0} ({1}) in {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(responseObject));
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1}) in <Indexed>", prop.Name, prop.PropertyType.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to update single Contact Role with ID and print the response.
        /// </summary>
        /// <param name="contactRoleId">The ID of the ContactRole to be updated</param>
        public static void UpdateContactRole(long contactRoleId)
        {
            //ID of the ContactRole to be updated
            //long contactRoleId = 5255085067923;
            
            //Get instance of ContactRolesOperations Class
            ContactRolesOperations contactRolesOperations = new ContactRolesOperations();
            
            //Get instance of BodyWrapper Class that will contain the request body
            BodyWrapper bodyWrapper = new BodyWrapper();
            
            //List of ContactRole instances
            List<ContactRole> contactRolesList = new List<ContactRole>();

            //Get instance of ContactRole Class
            ContactRole cr1 = new ContactRole();

            //Set name to the ContactRole instance
            cr1.Name = "contactRoleUpdate1";

            //Set sequence number to the ContactRole instance
            cr1.SequenceNumber = 2;

            //Add ContactRole instance to the list
            contactRolesList.Add(cr1);
            
            //Set the list to contactRoles in BodyWrapper instance
            bodyWrapper.ContactRoles = contactRolesList;

            //Call UpdateContactRole method that takes contactRoleId  and BodyWrapper instance as parameters
            APIResponse<ActionHandler> response = contactRolesOperations.UpdateContactRole(contactRoleId, bodyWrapper);
            
            if(response != null)
            {
                //Get the status code from response
                Console.WriteLine("Status Code: " + response.StatusCode);
                
                //Check if expected response is received
                if(response.IsExpected)
                {
                    //Get object from response
                    ActionHandler actionHandler = response.Object;
                    
                    if(actionHandler is ActionWrapper)
                    {
                        //Get the received ActionWrapper instance
                        ActionWrapper actionWrapper = (ActionWrapper) actionHandler;
                        
                        //Get the list of obtained ActionResponse instances
                        List<ActionResponse> actionResponses = actionWrapper.ContactRoles;
                        
                        foreach(ActionResponse actionResponse in actionResponses)
                        {
                            //Check if the request is successful
                            if(actionResponse is SuccessResponse)
                            {
                                //Get the received SuccessResponse instance
                                SuccessResponse successResponse = (SuccessResponse)actionResponse;
                                
                                //Get the Status
                                Console.WriteLine("Status: " + successResponse.Status.Value);
                                
                                //Get the Code
                                Console.WriteLine("Code: " + successResponse.Code.Value);
                                
                                Console.WriteLine("Details: " );
                                
                                //Get the details map
                                foreach( KeyValuePair<string, object> entry in successResponse.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                                }
                                
                                //Get the Message
                                Console.WriteLine("Message: " + successResponse.Message.Value);
                            }
                            //Check if the request returned an exception
                            else if(actionResponse is APIException)
                            {
                                //Get the received APIException instance
                                APIException exception = (APIException) actionResponse;
                                
                                //Get the Status
                                Console.WriteLine("Status: " + exception.Status.Value);
                                
                                //Get the Code
                                Console.WriteLine("Code: " + exception.Code.Value);
                                
                                Console.WriteLine("Details: " );
                                
                                //Get the details map
                                foreach( KeyValuePair<string, object> entry in exception.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                                }
                                
                                //Get the Message
                                Console.WriteLine("Message: " + exception.Message.Value);
                            }
                        }
                    }
                    //Check if the request returned an exception
                    else if(actionHandler is APIException)
                    {
                        //Get the received APIException instance
                        APIException exception = (APIException) actionHandler;
                        
                        //Get the Status
                        Console.WriteLine("Status: " + exception.Status.Value);
                        
                        //Get the Code
                        Console.WriteLine("Code: " + exception.Code.Value);
                        
                        Console.WriteLine("Details: " );
                        
                        //Get the details map
                        foreach( KeyValuePair<string, object> entry in exception.Details)
                        {
                            //Get each value in the map
                            Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                        }
                        
                        //Get the Message
                        Console.WriteLine("Message: " + exception.Message.Value);
                    }
                }
                else
                { //If response is not as expected

                    //Get model object from response
                    Model responseObject = response.Model;

                    //Get the response object's class
                    Type type = responseObject.GetType();

                    //Get all declared fields of the response class
                    Console.WriteLine("Type is: {0}", type.Name);

                    PropertyInfo[] props = type.GetProperties();

                    Console.WriteLine("Properties (N = {0}):", props.Length);

                    foreach (var prop in props)
                    {
                        if (prop.GetIndexParameters().Length == 0)
                        {
                            Console.WriteLine("{0} ({1}) in {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(responseObject));
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1}) in <Indexed>", prop.Name, prop.PropertyType.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to delete single Contact Role with ID and print the response.
        /// </summary>
        /// <param name="contactRoleId">The ID of the ContactRole to be deleted</param>
        public static void DeleteContactRole(long contactRoleId)
        {
            //ID of the ContactRole to be updated
            //long contactRoleId = 5255085067923;
            
            //Get instance of ContactRolesOperations Class
            ContactRolesOperations contactRolesOperations = new ContactRolesOperations();
            
            //Call DeleteContactRole which takes contactRoleId as parameter
            APIResponse<ActionHandler> response = contactRolesOperations.DeleteContactRole(contactRoleId);
            
            if(response != null)
            {
                //Get the status code from response
                Console.WriteLine("Status Code: " + response.StatusCode);
                
                //Check if expected response is received
                if(response.IsExpected)
                {
                    //Get object from response
                    ActionHandler actionHandler = response.Object;
                    
                    if(actionHandler is ActionWrapper)
                    {
                        //Get the received ActionWrapper instance
                        ActionWrapper actionWrapper = (ActionWrapper) actionHandler;
                        
                        //Get the list of obtained ActionResponse instances
                        List<ActionResponse> actionResponses = actionWrapper.ContactRoles;
                        
                        foreach(ActionResponse actionResponse in actionResponses)
                        {
                            //Check if the request is successful
                            if(actionResponse is SuccessResponse)
                            {
                                //Get the received SuccessResponse instance
                                SuccessResponse successResponse = (SuccessResponse)actionResponse;
                                
                                //Get the Status
                                Console.WriteLine("Status: " + successResponse.Status.Value);
                                
                                //Get the Code
                                Console.WriteLine("Code: " + successResponse.Code.Value);
                                
                                Console.WriteLine("Details: " );
                                
                                //Get the details map
                                foreach( KeyValuePair<string, object> entry in successResponse.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                                }
                                
                                //Get the Message
                                Console.WriteLine("Message: " + successResponse.Message.Value);
                            }
                            //Check if the request returned an exception
                            else if(actionResponse is APIException)
                            {
                                //Get the received APIException instance
                                APIException exception = (APIException) actionResponse;
                                
                                //Get the Status
                                Console.WriteLine("Status: " + exception.Status.Value);
                                
                                //Get the Code
                                Console.WriteLine("Code: " + exception.Code.Value);
                                
                                Console.WriteLine("Details: " );
                                
                                //Get the details map
                                foreach( KeyValuePair<string, object> entry in exception.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                                }
                                
                                //Get the Message
                                Console.WriteLine("Message: " + exception.Message.Value);
                            }
                        }
                    }
                    //Check if the request returned an exception
                    else if(actionHandler is APIException)
                    {
                        //Get the received APIException instance
                        APIException exception = (APIException) actionHandler;
                        
                        //Get the Status
                        Console.WriteLine("Status: " + exception.Status.Value);
                        
                        //Get the Code
                        Console.WriteLine("Code: " + exception.Code.Value);
                        
                        Console.WriteLine("Details: " );
                        
                        //Get the details map
                        foreach( KeyValuePair<string, object> entry in exception.Details)
                        {
                            //Get each value in the map
                            Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                        }
                        
                        //Get the Message
                        Console.WriteLine("Message: " + exception.Message.Value);
                    }
                }
                else
                { //If response is not as expected

                    //Get model object from response
                    Model responseObject = response.Model;

                    //Get the response object's class
                    Type type = responseObject.GetType();

                    //Get all declared fields of the response class
                    Console.WriteLine("Type is: {0}", type.Name);

                    PropertyInfo[] props = type.GetProperties();

                    Console.WriteLine("Properties (N = {0}):", props.Length);

                    foreach (var prop in props)
                    {
                        if (prop.GetIndexParameters().Length == 0)
                        {
                            Console.WriteLine("{0} ({1}) in {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(responseObject));
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1}) in <Indexed>", prop.Name, prop.PropertyType.Name);
                        }
                    }
                }
            }
        }

        public static void GetAllContactRolesOfDeal(long dealId)
        {
            //Get instance of ContactRolesOperations Class
            ContactRolesOperations contactRolesOperations = new ContactRolesOperations();

            //Get instance of ParameterMap Class
            ParameterMap paramInstance = new ParameterMap();

            List<string> contactRoleIds = new List<string>();

            foreach (string id in contactRoleIds)
            {
                paramInstance.Add(GetAllContactRolesOfDealParam.IDS, id);
            }

            //Call getAllContactRolesOfDeal method that takes Param instance as parameter 
            APIResponse<RecordResponseHandler> response = contactRolesOperations.GetAllContactRolesOfDeal(dealId, paramInstance);

            if (response != null)
            {
                //Get the status code from response
                Console.WriteLine("Status Code: " + response.StatusCode);

                if (new List<int>() { 204, 304 }.Contains(response.StatusCode))
                {
                    Console.WriteLine(response.StatusCode == 204 ? "No Content" : "Not Modified");

                    return;
                }

                //Check if expected response is received
                if (response.IsExpected)
                {
                    //Get the object from response
                    RecordResponseHandler responseHandler = response.Object;

                    if (responseHandler is RecordResponseWrapper)
                    {
                        //Get the received RecordResponseWrapper instance
                        RecordResponseWrapper responseWrapper = (RecordResponseWrapper)responseHandler;

                        //Get the obtained Record instances
                        List<API.Record.Record> records = responseWrapper.Data;

                        foreach (API.Record.Record record in records)
                        {
                            //Get the ID of each Record
                            Console.WriteLine("Record ID: " + record.Id);

                            //Get the createdBy User instance of each Record
                            User createdBy = record.CreatedBy;

                            //Check if createdBy is not null
                            if (createdBy != null)
                            {
                                //Get the ID of the createdBy User
                                Console.WriteLine("Record Created By User-ID: " + createdBy.Id);

                                //Get the name of the createdBy User
                                Console.WriteLine("Record Created By User-Name: " + createdBy.Name);

                                //Get the Email of the createdBy User
                                Console.WriteLine("Record Created By User-Email: " + createdBy.Email);
                            }

                            //Get the CreatedTime of each Record
                            Console.WriteLine("Record CreatedTime: " + JsonConvert.SerializeObject(record.CreatedTime));

                            //Get the modifiedBy User instance of each Record
                            User modifiedBy = record.ModifiedBy;

                            //Check if modifiedBy is not null
                            if (modifiedBy != null)
                            {
                                //Get the ID of the modifiedBy User
                                Console.WriteLine("Record Modified By User-ID: " + modifiedBy.Id);

                                //Get the name of the modifiedBy User
                                Console.WriteLine("Record Modified By User-Name: " + modifiedBy.Name);

                                //Get the Email of the modifiedBy User
                                Console.WriteLine("Record Modified By User-Email: " + modifiedBy.Email);
                            }

                            //Get the ModifiedTime of each Record
                            Console.WriteLine("Record CreatedTime: " + JsonConvert.SerializeObject(record.ModifiedTime));

                            //To get particular field value 
                            Console.WriteLine("Record Field Value: " + record.GetKeyValue("Last_Name"));// FieldApiName

                            Console.WriteLine("Record KeyValues: \n");

                            //Get the KeyValue map
                            foreach (KeyValuePair<string, object> entry in record.GetKeyValues())
                            {
                                string keyName = entry.Key;

                                object value = entry.Value;

                                if (value is IList)
                                {
                                    Console.WriteLine("Record KeyName : " + keyName);

                                    IList dataList = (IList)value;

                                    foreach (object data in dataList)
                                    {
                                        if (data is IDictionary)
                                        {
                                            Console.WriteLine("Record KeyName : " + keyName + " - Value : ");

                                            foreach (KeyValuePair<string, object> entry1 in (Dictionary<string, object>)data)
                                            {
                                                Console.WriteLine(entry1.Key + " : " + JsonConvert.SerializeObject(entry1.Value));
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine(JsonConvert.SerializeObject(data));
                                        }
                                    }
                                }
                                else if (value is IDictionary)
                                {
                                    Console.WriteLine("Record KeyName : " + keyName + " - Value : ");

                                    foreach (KeyValuePair<string, object> entry1 in (Dictionary<string, object>)value)
                                    {
                                        Console.WriteLine(entry1.Key + " : " + JsonConvert.SerializeObject(entry1.Value));
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Record KeyName : " + keyName + " - Value : " + JsonConvert.SerializeObject(value));
                                }
                            }
                        }

                        //Get the Object obtained Info instance
                        API.Record.Info info = responseWrapper.Info;

                        //Check if info is not null
                        if (info != null)
                        {
                            if (info.Count != null)
                            {
                                //Get the Count of the Info
                                Console.WriteLine("Record Info Count: " + info.Count.ToString());
                            }

                            if (info.MoreRecords != null)
                            {
                                //Get the MoreRecords of the Info
                                Console.WriteLine("Record Info MoreRecords: " + info.MoreRecords.ToString());
                            }
                        }
                    }
                    //Check if the request returned an exception
                    else if (responseHandler is APIException)
                    {
                        //Get the received APIException instance
                        APIException exception = (APIException)responseHandler;

                        //Get the Status
                        Console.WriteLine("Status: " + exception.Status.Value);

                        //Get the Code
                        Console.WriteLine("Code: " + exception.Code.Value);

                        Console.WriteLine("Details: ");

                        //Get the details map
                        foreach (KeyValuePair<string, object> entry in exception.Details)
                        {
                            //Get each value in the map
                            Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                        }

                        //Get the Message
                        Console.WriteLine("Message: " + exception.Message.Value);
                    }
                }
                else
                { //If response is not as expected

                    //Get model object from response
                    Model responseObject = response.Model;

                    //Get the response object's class
                    Type type = responseObject.GetType();

                    //Get all declared fields of the response class
                    Console.WriteLine("Type is: {0}", type.Name);

                    PropertyInfo[] props = type.GetProperties();

                    Console.WriteLine("Properties (N = {0}):", props.Length);

                    foreach (var prop in props)
                    {
                        if (prop.GetIndexParameters().Length == 0)
                        {
                            Console.WriteLine("{0} ({1}) : {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(responseObject));
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1}) : <Indexed>", prop.Name, prop.PropertyType.Name);
                        }
                    }
                }
            }
        }

        public static void GetContactRoleOfDeal(long contactId, long dealId)
        {
            //Get instance of ContactRolesOperations Class
            ContactRolesOperations contactRolesOperations = new ContactRolesOperations();

            //Get instance of ParameterMap Class
            ParameterMap paramInstance = new ParameterMap();

            //Call getContactRoleOfDeal method that takes Param instance as parameter 
            APIResponse<RecordResponseHandler> response = contactRolesOperations.GetContactRoleOfDeal(contactId, dealId);

            if (response != null)
            {
                //Get the status code from response
                Console.WriteLine("Status Code: " + response.StatusCode);

                if (new List<int>() { 204, 304 }.Contains(response.StatusCode))
                {
                    Console.WriteLine(response.StatusCode == 204 ? "No Content" : "Not Modified");

                    return;
                }

                //Check if expected response is received
                if (response.IsExpected)
                {
                    //Get the object from response
                    RecordResponseHandler responseHandler = response.Object;

                    if (responseHandler is RecordResponseWrapper)
                    {
                        //Get the received RecordResponseWrapper instance
                        RecordResponseWrapper responseWrapper = (RecordResponseWrapper)responseHandler;

                        //Get the obtained Record instances
                        List<API.Record.Record> records = responseWrapper.Data;

                        foreach (API.Record.Record record in records)
                        {
                            //Get the ID of each Record
                            Console.WriteLine("Record ID: " + record.Id);

                            //Get the createdBy User instance of each Record
                            User createdBy = record.CreatedBy;

                            //Check if createdBy is not null
                            if (createdBy != null)
                            {
                                //Get the ID of the createdBy User
                                Console.WriteLine("Record Created By User-ID: " + createdBy.Id);

                                //Get the name of the createdBy User
                                Console.WriteLine("Record Created By User-Name: " + createdBy.Name);

                                //Get the Email of the createdBy User
                                Console.WriteLine("Record Created By User-Email: " + createdBy.Email);
                            }

                            //Get the CreatedTime of each Record
                            Console.WriteLine("Record CreatedTime: " + JsonConvert.SerializeObject(record.CreatedTime));

                            //Get the modifiedBy User instance of each Record
                            User modifiedBy = record.ModifiedBy;

                            //Check if modifiedBy is not null
                            if (modifiedBy != null)
                            {
                                //Get the ID of the modifiedBy User
                                Console.WriteLine("Record Modified By User-ID: " + modifiedBy.Id);

                                //Get the name of the modifiedBy User
                                Console.WriteLine("Record Modified By User-Name: " + modifiedBy.Name);

                                //Get the Email of the modifiedBy User
                                Console.WriteLine("Record Modified By User-Email: " + modifiedBy.Email);
                            }

                            //Get the ModifiedTime of each Record
                            Console.WriteLine("Record CreatedTime: " + JsonConvert.SerializeObject(record.ModifiedTime));

                            //To get particular field value 
                            Console.WriteLine("Record Field Value: " + record.GetKeyValue("Last_Name"));// FieldApiName

                            Console.WriteLine("Record KeyValues: \n");

                            //Get the KeyValue map
                            foreach (KeyValuePair<string, object> entry in record.GetKeyValues())
                            {
                                string keyName = entry.Key;

                                object value = entry.Value;

                                if (value is IList)
                                {
                                    Console.WriteLine("Record KeyName : " + keyName);

                                    IList dataList = (IList)value;

                                    foreach (object data in dataList)
                                    {
                                        if (data is IDictionary)
                                        {
                                            Console.WriteLine("Record KeyName : " + keyName + " - Value : ");

                                            foreach (KeyValuePair<string, object> entry1 in (Dictionary<string, object>)data)
                                            {
                                                Console.WriteLine(entry1.Key + " : " + JsonConvert.SerializeObject(entry1.Value));
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine(JsonConvert.SerializeObject(data));
                                        }
                                    }
                                }
                                else if (value is IDictionary)
                                {
                                    Console.WriteLine("Record KeyName : " + keyName + " - Value : ");

                                    foreach (KeyValuePair<string, object> entry1 in (Dictionary<string, object>)value)
                                    {
                                        Console.WriteLine(entry1.Key + " : " + JsonConvert.SerializeObject(entry1.Value));
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Record KeyName : " + keyName + " - Value : " + JsonConvert.SerializeObject(value));
                                }
                            }
                        }

                        //Get the Object obtained Info instance
                        API.Record.Info info = responseWrapper.Info;

                        //Check if info is not null
                        if (info != null)
                        {
                            if (info.Count != null)
                            {
                                //Get the Count of the Info
                                Console.WriteLine("Record Info Count: " + info.Count.ToString());
                            }

                            if (info.MoreRecords != null)
                            {
                                //Get the MoreRecords of the Info
                                Console.WriteLine("Record Info MoreRecords: " + info.MoreRecords.ToString());
                            }
                        }
                    }
                    //Check if the request returned an exception
                    else if (responseHandler is APIException)
                    {
                        //Get the received APIException instance
                        APIException exception = (APIException)responseHandler;

                        //Get the Status
                        Console.WriteLine("Status: " + exception.Status.Value);

                        //Get the Code
                        Console.WriteLine("Code: " + exception.Code.Value);

                        Console.WriteLine("Details: ");

                        //Get the details map
                        foreach (KeyValuePair<string, object> entry in exception.Details)
                        {
                            //Get each value in the map
                            Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                        }

                        //Get the Message
                        Console.WriteLine("Message: " + exception.Message.Value);
                    }
                }
                else
                { //If response is not as expected

                    //Get model object from response
                    Model responseObject = response.Model;

                    //Get the response object's class
                    Type type = responseObject.GetType();

                    //Get all declared fields of the response class
                    Console.WriteLine("Type is: {0}", type.Name);

                    PropertyInfo[] props = type.GetProperties();

                    Console.WriteLine("Properties (N = {0}):", props.Length);

                    foreach (var prop in props)
                    {
                        if (prop.GetIndexParameters().Length == 0)
                        {
                            Console.WriteLine("{0} ({1}) : {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(responseObject));
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1}) : <Indexed>", prop.Name, prop.PropertyType.Name);
                        }
                    }
                }
            }
        }

        public static void AddContactRoleToDeal(long contactId, long dealId)
        {
            //Get instance of ContactRolesOperations Class
            ContactRolesOperations contactRolesOperations = new ContactRolesOperations();

            //Get instance of BodyWrapper Class that will contain the request body
            RecordBodyWrapper bodyWrapper = new RecordBodyWrapper();

            //Get instance of ContactRole Class
            ContactRoleWrapper contactRole = new ContactRoleWrapper();

            //Set name of the Contact Role
            contactRole.ContactRole = "contactRole1";

            //Set the list to contactRoles in BodyWrapper instance
            bodyWrapper.Data = new List<ContactRoleWrapper>() { contactRole };

            //Call createContactRoles method that takes BodyWrapper instance as parameter
            APIResponse<RecordActionHandler> response = contactRolesOperations.AddContactRoleToDeal(contactId, dealId, bodyWrapper);

            if (response != null)
            {
                //Get the status code from response
                Console.WriteLine("Status code" + response.StatusCode);

                //Check if expected response is received
                if (response.IsExpected)
                {
                    //Get object from response
                    RecordActionHandler actionHandler = response.Object;

                    if (actionHandler is RecordActionWrapper)
                    {
                        //Get the received RecordActionWrapper instance
                        RecordActionWrapper actionWrapper = (RecordActionWrapper)actionHandler;

                        //Get the list of obtained action responses
                        List<ActionResponse> actionResponses = actionWrapper.Data;

                        foreach (ActionResponse actionResponse in actionResponses)
                        {
                            //Check if the request is successful
                            if (actionResponse is SuccessResponse)
                            {
                                //Get the received SuccessResponse instance
                                SuccessResponse successResponse = (SuccessResponse)actionResponse;

                                //Get the Status
                                Console.WriteLine("Status: " + successResponse.Status.Value);

                                //Get the Code
                                Console.WriteLine("Code: " + successResponse.Code.Value);

                                Console.WriteLine("Details: ");

                                //Get the details map
                                foreach (KeyValuePair<string, object> entry in successResponse.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                                }

                                //Get the Message
                                Console.WriteLine("Message: " + successResponse.Message.Value);
                            }
                            //Check if the request returned an exception
                            else if (actionResponse is APIException)
                            {
                                //Get the received APIException instance
                                APIException exception = (APIException)actionResponse;

                                //Get the Status
                                Console.WriteLine("Status: " + exception.Status.Value);

                                //Get the Code
                                Console.WriteLine("Code: " + exception.Code.Value);

                                Console.WriteLine("Details: ");

                                //Get the details map
                                foreach (KeyValuePair<string, object> entry in exception.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                                }

                                //Get the Message
                                Console.WriteLine("Message: " + exception.Message.Value);
                            }
                        }
                    }
                    //Check if the request returned an exception
                    else if (actionHandler is APIException)
                    {
                        //Get the received APIException instance
                        APIException exception = (APIException)actionHandler;

                        //Get the Status
                        Console.WriteLine("Status: " + exception.Status.Value);

                        //Get the Code
                        Console.WriteLine("Code: " + exception.Code.Value);

                        Console.WriteLine("Details: ");

                        //Get the details map
                        foreach (KeyValuePair<string, object> entry in exception.Details)
                        {
                            //Get each value in the map
                            Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                        }

                        //Get the Message
                        Console.WriteLine("Message: " + exception.Message.Value);
                    }
                }
                else
                { //If response is not as expected

                    //Get model object from response
                    Model responseObject = response.Model;

                    //Get the response object's class
                    Type type = responseObject.GetType();

                    //Get all declared fields of the response class
                    Console.WriteLine("Type is: {0}", type.Name);

                    PropertyInfo[] props = type.GetProperties();

                    Console.WriteLine("Properties (N = {0}):", props.Length);

                    foreach (var prop in props)
                    {
                        if (prop.GetIndexParameters().Length == 0)
                        {
                            Console.WriteLine("{0} ({1}) in {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(responseObject));
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1}) in <Indexed>", prop.Name, prop.PropertyType.Name);
                        }
                    }
                }
            }
        }

        public static void RemoveContactRoleFromDeal(long contactId, long dealId)
        {
            //Get instance of ContactRolesOperations Class
            ContactRolesOperations contactRolesOperations = new ContactRolesOperations();

            //Call createContactRoles method that takes BodyWrapper instance as parameter
            APIResponse<RecordActionHandler> response = contactRolesOperations.RemoveContactRoleFromDeal(contactId, dealId);

            if (response != null)
            {
                //Get the status code from response
                Console.WriteLine("Status Code: " + response.StatusCode);

                //Check if expected response is received
                if (response.IsExpected)
                {
                    //Get object from response
                    RecordActionHandler actionHandler = response.Object;

                    if (actionHandler is RecordActionWrapper)
                    {
                        //Get the received ActionWrapper instance
                        RecordActionWrapper actionWrapper = (RecordActionWrapper)actionHandler;

                        //Get the list of obtained ActionResponse instances
                        List<ActionResponse> actionResponses = actionWrapper.Data;

                        foreach (ActionResponse actionResponse in actionResponses)
                        {
                            //Check if the request is successful
                            if (actionResponse is SuccessResponse)
                            {
                                //Get the received SuccessResponse instance
                                SuccessResponse successResponse = (SuccessResponse)actionResponse;

                                //Get the Status
                                Console.WriteLine("Status: " + successResponse.Status.Value);

                                //Get the Code
                                Console.WriteLine("Code: " + successResponse.Code.Value);

                                Console.WriteLine("Details: ");

                                //Get the details map
                                foreach (KeyValuePair<string, object> entry in successResponse.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                                }

                                //Get the Message
                                Console.WriteLine("Message: " + successResponse.Message.Value);
                            }
                            //Check if the request returned an exception
                            else if (actionResponse is APIException)
                            {
                                //Get the received APIException instance
                                APIException exception = (APIException)actionResponse;

                                //Get the Status
                                Console.WriteLine("Status: " + exception.Status.Value);

                                //Get the Code
                                Console.WriteLine("Code: " + exception.Code.Value);

                                Console.WriteLine("Details: ");

                                //Get the details map
                                foreach (KeyValuePair<string, object> entry in exception.Details)
                                {
                                    //Get each value in the map
                                    Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                                }

                                //Get the Message
                                Console.WriteLine("Message: " + exception.Message.Value);
                            }
                        }
                    }
                    //Check if the request returned an exception
                    else if (actionHandler is APIException)
                    {
                        //Get the received APIException instance
                        APIException exception = (APIException)actionHandler;

                        //Get the Status
                        Console.WriteLine("Status: " + exception.Status.Value);

                        //Get the Code
                        Console.WriteLine("Code: " + exception.Code.Value);

                        Console.WriteLine("Details: ");

                        //Get the details map
                        foreach (KeyValuePair<string, object> entry in exception.Details)
                        {
                            //Get each value in the map
                            Console.WriteLine(entry.Key + ": " + JsonConvert.SerializeObject(entry.Value));
                        }

                        //Get the Message
                        Console.WriteLine("Message: " + exception.Message.Value);
                    }
                }
                else
                { //If response is not as expected

                    //Get model object from response
                    Model responseObject = response.Model;

                    //Get the response object's class
                    Type type = responseObject.GetType();

                    //Get all declared fields of the response class
                    Console.WriteLine("Type is: {0}", type.Name);

                    PropertyInfo[] props = type.GetProperties();

                    Console.WriteLine("Properties (N = {0}):", props.Length);

                    foreach (var prop in props)
                    {
                        if (prop.GetIndexParameters().Length == 0)
                        {
                            Console.WriteLine("{0} ({1}) in {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(responseObject));
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1}) in <Indexed>", prop.Name, prop.PropertyType.Name);
                        }
                    }
                }
            }
        }
    }
}
