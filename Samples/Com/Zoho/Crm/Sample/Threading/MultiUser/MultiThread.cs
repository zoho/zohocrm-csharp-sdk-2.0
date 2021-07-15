using System;
using System.Collections.Generic;
using System.Threading;
using Com.Zoho.API.Authenticator;
using Com.Zoho.API.Authenticator.Store;
using Com.Zoho.Crm.API;
using Com.Zoho.Crm.API.Dc;
using Com.Zoho.Crm.API.Logger;
using Com.Zoho.Crm.API.Record;
using Com.Zoho.Crm.API.Util;
using Newtonsoft.Json;
using static Com.Zoho.API.Authenticator.OAuthToken;
using SDKInitializer = Com.Zoho.Crm.API.Initializer;

namespace Com.Zoho.Crm.Sample.Threading.MultiUser
{
    /// <summary>
    /// 
    /// </summary>
    public class MultiThread
    {

        DataCenter.Environment environment;

        UserSignature user;

        Token token;

        string moduleAPIName;

        public MultiThread(UserSignature user, DataCenter.Environment environment, Token token, string moduleAPIName)
        {
            this.environment = environment;

            this.user = user;

            this.token = token;

            this.moduleAPIName = moduleAPIName;
        }

        public static void RunMultiThreadWithMultiUser()
        {
            Logger logger = new Logger.Builder()
                .Level(Logger.Levels.ALL)
                .FilePath("/Users/Documents/GitLab/csharp_sdk_log.log")
                .Build();

            DataCenter.Environment environment1 = USDataCenter.PRODUCTION;

            UserSignature user1 = new UserSignature("abc@zoho.com");

            TokenStore tokenstore = new FileStore("/Users/Documents/GitLab/csharp_sdk_token.txt");

            Token token1 = new OAuthToken.Builder()
                .ClientId("ClientId")
                .ClientSecret("ClientSecret")
                .RefreshToken("RefreshToken")
                .RedirectURL("https://www.zoho.com")
                .Build();

            string resourcePath = "/Users/Documents";

            DataCenter.Environment environment2 = USDataCenter.PRODUCTION;

            UserSignature user2 = new UserSignature("abc1@zoho.com");

            Token token2 = new OAuthToken.Builder()
                .ClientId("ClientId")
                .ClientSecret("ClientSecret")
                .RefreshToken("RefreshToken")
                .Build();

            SDKConfig config = new SDKConfig.Builder().AutoRefreshFields(true).Build();

            new SDKInitializer.Builder()
               .User(user1)
               .Environment(environment1)
               .Token(token1)
               .Store(tokenstore)
               .SDKConfig(config)
               .ResourcePath(resourcePath)
               .Logger(logger)
               .Initialize();

            MultiThread multiThread1 = new MultiThread(user1, environment1, token1, "Vendors");

            Thread thread1 = new Thread(() => multiThread1.GetRecords());

            thread1.Start();

            MultiThread multiThread2 = new MultiThread(user2, environment2, token2, "Quotes");

            Thread thread2 = new Thread(() => multiThread2.GetRecords());

            thread2.Start();

            thread1.Join();

            thread2.Join();
        }

        public void GetRecords()
        {
            try
            {
                SDKConfig config = new SDKConfig.Builder().AutoRefreshFields(true).Build();

                new SDKInitializer.Builder()
               .User(this.user)
               .Environment(this.environment)
               .Token(this.token)
               .SDKConfig(config)
               .SwitchUser();

                Console.WriteLine("Fetching Cr's for user - " + SDKInitializer.GetInitializer().User.Email);

                RecordOperations recordOperation = new RecordOperations();

                APIResponse<ResponseHandler> response = recordOperation.GetRecords(this.moduleAPIName, null, null);

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
                        //Get object from response
                        ResponseHandler responseHandler = response.Object;

                        if (responseHandler is ResponseWrapper)
                        {
                            //Get the received ResponseWrapper instance
                            ResponseWrapper responseWrapper = (ResponseWrapper)responseHandler;

                            //Get the list of obtained Record instances
                            List<API.Record.Record> records = responseWrapper.Data;

                            foreach (API.Record.Record record in records)
                            {
                                Console.WriteLine(JsonConvert.SerializeObject(record));
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
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }
        }
    }
}
