using System;
using Com.Zoho.API.Authenticator;
using Com.Zoho.API.Authenticator.Store;
using Com.Zoho.Crm.API;
using Com.Zoho.Crm.API.Dc;
using Com.Zoho.Crm.API.Logger;
using static Com.Zoho.API.Authenticator.OAuthToken;
using Environment = Com.Zoho.Crm.API.Dc.DataCenter.Environment;
using SDKInitializer = Com.Zoho.Crm.API.Initializer;

namespace Com.Zoho.Crm.Sample.Initializer
{
    public class Initialize
    {
		public static void SDKInitialize()
		{
			/*
            * Create an instance of Logger Class that requires the following
            * Level -> Level of the log messages to be logged. Can be configured by typing Levels "." and choose any level from the list displayed.
            * FilePath -> Absolute file path, where messages need to be logged.
            */
			Logger logger = new Logger.Builder()
			.Level(Logger.Levels.ALL)
			.FilePath("/Users/Documents/GitLab/csharp_sdk_log.log")
			.Build();
			
		    //Create an UserSignature instance that takes user Email as parameter
		    UserSignature user = new UserSignature("abc@zoho.com");

			/*
		     * Configure the environment
		     * which is of the pattern Domain.Environment
		     * Available Domains: USDataCenter, EUDataCenter, INDataCenter, CNDataCenter, AUDataCenter
		     * Available Environments: PRODUCTION, DEVELOPER, SANDBOX
		     */
			Environment environment = USDataCenter.PRODUCTION;

			/*
            * Create a Token instance
            * ClientId -> OAuth client id.
            * ClientSecret -> OAuth client secret.
            * RefreshToken -> Refresh token.
            * RedirectURL -> OAuth redirect URL.
            */
			Token token = new OAuthToken.Builder()
			.ClientId("ClientId")
			.ClientSecret("ClientSecret")
			.RefreshToken("RefreshToken")
			//.RedirectURL("redirectURL")
			.Build();

			/*
            * Create an instance of DBStore.
            * Host -> DataBase host name. Default "localhost"
            * DatabaseName -> DataBase name. Default "zohooauth"
            * UserName -> DataBase user name. Default "root"
            * Password -> DataBase password. Default ""
            * PortNumber -> DataBase port number. Default "3306"
            * TableName -> Table Name. Default value "oauthtoken"
            */
			//TokenStore tokenstore = new DBStore.Builder().Build();

			//TokenStore tokenstore = new DBStore.Builder()
			//.Host("hostName")
			//.DatabaseName("dataBaseName")
			//.TableName("TableName")
			//.UserName("userName")
			//.Password("Password")
			//.PortNumber("portNumber")
			//.Build();

			TokenStore tokenstore = new FileStore("/Users/Documents/GitLab/csharp_sdk_token.txt");

			/*
            * autoRefreshFields
            * if true - all the modules' fields will be auto-refreshed in the background, every    hour.
            * if false - the fields will not be auto-refreshed in the background. The user can manually delete the file(s) or refresh the fields using methods from ModuleFieldsHandler(com.zoho.crm.api.util.ModuleFieldsHandler)
            * 
            * pickListValidation
            * if true - value for any picklist field will be validated with the available values.
            * if false - value for any picklist field will not be validated, resulting in creation of a new value.
            */
			SDKConfig config = new SDKConfig.Builder().AutoRefreshFields(false).PickListValidation(false).Build();

			string resourcePath = "/Users/Documents";

			/**
            * Create an instance of RequestProxy class that takes the following parameters
            * Host -> Host
            * Port -> Port Number
            * User -> User Name
            * Password -> Password
            * UserDomain -> User Domain
            */
			//RequestProxy requestProxy = new RequestProxy.Builder()
			//.Host("proxyHost")
			//.Port(proxyPort)
			//.User("proxyUser")
			//.Password("password")
			//.UserDomain("userDomain")
			//.Build();

			/*
            * The initialize method of Initializer class that takes the following arguments
            * User -> UserSignature instance
            * Environment -> Environment instance
            * Token -> Token instance
            * Store -> TokenStore instance
            * SDKConfig -> SDKConfig instance
            * ResourcePath -> resourcePath -A String
            * Logger -> Logger instance
            * RequestProxy -> RequestProxy instance
            */

			// Set the following in InitializeBuilder
			new SDKInitializer.Builder()
			.User(user)
			.Environment(environment)
			.Token(token)
			.Store(tokenstore)
			.SDKConfig(config)
			.ResourcePath(resourcePath)
			.Logger(logger)
			//.RequestProxy(requestProxy)
			.Initialize();

            //foreach (Token token1 in ((DBStore)tokenstore).GetTokens())
            //{
            //    OAuthToken authToken = (OAuthToken)token1;

            //    Console.WriteLine(authToken.AccessToken);

            //    Console.WriteLine(authToken.RefreshToken);

            //    Console.WriteLine(authToken.ExpiresIn);

            //    Console.WriteLine(authToken.GrantToken);

            //    Console.WriteLine(authToken.ClientId);

            //    Console.WriteLine(authToken.ClientSecret);

            //    Console.WriteLine(authToken.Id);

            //    Console.WriteLine(authToken.RedirectURL);

            //    Console.WriteLine(authToken.UserMail);
            //}
        }
	}
}
