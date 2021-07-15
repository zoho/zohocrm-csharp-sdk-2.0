using System;

using System.IO;

using System.Reflection;

using Com.Zoho.API.Authenticator;

using Com.Zoho.API.Authenticator.Store;

using Com.Zoho.Crm.API.Logger;

using static Com.Zoho.Crm.API.Dc.DataCenter;

using Com.Zoho.Crm.API.Util;

using Newtonsoft.Json.Linq;

using System.Threading;

using Com.Zoho.API.Exception;

using Newtonsoft.Json;

using System.Text;

namespace Com.Zoho.Crm.API
{
    /// <summary>
    /// This class to initialize Zoho CRM SDK.
    /// </summary>
    public class Initializer : IDisposable
    {
        public class Builder
        {
            private Dc.DataCenter.Environment environment;

            private TokenStore store;

            private UserSignature user;

            private Token token;

            private string resourcePath;

            private RequestProxy requestProxy;

            private SDKConfig sdkConfig;

            private Logger.Logger logger;

            private string errorMessage = (Initializer.initializer != null) ? Constants.SWITCH_USER_ERROR : Constants.INITIALIZATION_ERROR;

            public Builder()
            {
                if(Initializer.initializer != null)
                {
                    user = Initializer.initializer.User;

                    environment = Initializer.initializer.Environment;

                    token = Initializer.initializer.Token;

                    sdkConfig = Initializer.initializer.SDKConfig;
                }
            }

            public void Initialize()
            {
                Utility.AssertNotNull(user, errorMessage, Constants.USERSIGNATURE_ERROR_MESSAGE);

                Utility.AssertNotNull(environment, errorMessage, Constants.ENVIRONMENT_ERROR_MESSAGE);

                Utility.AssertNotNull(token, errorMessage, Constants.TOKEN_ERROR_MESSAGE);

                Utility.AssertNotNull(store, errorMessage, Constants.STORE_ERROR_MESSAGE);

                Utility.AssertNotNull(sdkConfig, errorMessage, Constants.SDK_CONFIG_ERROR_MESSAGE);

                Utility.AssertNotNull(resourcePath, errorMessage, Constants.RESOURCE_PATH_ERROR_MESSAGE);

                if (logger == null)
                {
                    logger = new Logger.Logger.Builder().Level(API.Logger.Logger.Levels.INFO).FilePath(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + Path.DirectorySeparatorChar + Constants.LOG_FILE_NAME).Build();
                }

                Initializer.Initialize(this.user, this.environment, this.token, this.store, this.sdkConfig, this.resourcePath, this.logger, this.requestProxy);
            }

            public void SwitchUser()
            {
                Utility.AssertNotNull(Initializer.initializer, Constants.SDK_UNINITIALIZATION_ERROR, Constants.SDK_UNINITIALIZATION_MESSAGE);

                Initializer.SwitchUser(this.user, this.environment, this.token, this.sdkConfig, this.requestProxy);
            }

            public Builder Logger(Logger.Logger logger)
            {
                this.logger = logger;

                return this;
            }

            public Builder Token(Token token)
            {
                Utility.AssertNotNull(token, errorMessage, Constants.TOKEN_ERROR_MESSAGE);

                this.token = token;

                return this;
            }


            public Builder SDKConfig(SDKConfig sdkConfig)
            {
                Utility.AssertNotNull(sdkConfig, errorMessage, Constants.SDK_CONFIG_ERROR_MESSAGE);

			    this.sdkConfig = sdkConfig;

			    return this;
		    }

            public Builder RequestProxy(RequestProxy requestProxy)
            {
                this.requestProxy = requestProxy;

                return this;
            }

            public Builder ResourcePath(string resourcePath)
            {
                if (string.IsNullOrEmpty(resourcePath) || string.IsNullOrWhiteSpace(resourcePath))
                {
                    throw new SDKException(errorMessage, Constants.RESOURCE_PATH_ERROR_MESSAGE);
                }

                if(!Directory.Exists(resourcePath))
                {
                    throw new SDKException(errorMessage, Constants.RESOURCE_PATH_INVALID_ERROR_MESSAGE);
                }

                this.resourcePath = resourcePath;

                return this;
            }

            public Builder User(UserSignature user)
            {
                Utility.AssertNotNull(user, errorMessage, Constants.USERSIGNATURE_ERROR_MESSAGE);

			    this.user = user;

			    return this;
		    }

            public Builder Store(TokenStore store)
            {
                Utility.AssertNotNull(store, errorMessage, Constants.STORE_ERROR_MESSAGE);

			    this.store = store;

			    return this;
		    }

            public Builder Environment(Dc.DataCenter.Environment environment)
            {
                Utility.AssertNotNull(environment, errorMessage, Constants.ENVIRONMENT_ERROR_MESSAGE);

			    this.environment = environment;

			    return this;
            }
        }

        private static  ThreadLocal<Initializer> LOCAL = new ThreadLocal<Initializer>();

        private static Initializer initializer;

        private Dc.DataCenter.Environment environment;

        private TokenStore store;

        private UserSignature user;

        private Token token;

        public static JObject jsonDetails;

	    private string resourcePath;

        private RequestProxy requestProxy;

        private SDKConfig sdkConfig;

        /// <summary>
        /// This method to initialize the SDK.
        /// </summary>
        /// <param name="user">A User class instance represents the CRM user.</param>
        /// <param name="environment">A Environment class instance containing the CRM API base URL and Accounts URL.</param>
        /// <param name="token">A Token class instance containing the OAuth client application information.</param>
        /// <param name="store">A TokenStore class instance containing the token store information.</param>
        /// <param name="sdkConfig">A SDKConfig class instance containing the configuration.</param>
        /// <param name="sdkConfig">A SDKConfig class instance containing the configuration.</param>
        /// <param name="logger">A Logger class instance containing the log file path and Logger type.</param>
        /// <param name="proxy">An RequestProxy class instance containing the proxy properties of the user.</param>
        private static void Initialize(UserSignature user, Dc.DataCenter.Environment environment, Token token, TokenStore store, SDKConfig sdkConfig, string resourcePath, Logger.Logger logger, RequestProxy proxy)
        {
            try
            {
                SDKLogger.Initialize(logger);

                try
                {
                    string result = "";

                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Constants.JSON_DETAILS_FILE_PATH))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            result = reader.ReadToEnd();
                        }
                    }

                    jsonDetails = JObject.Parse(result);
                }
                catch (System.Exception e)
                {
                    throw new SDKException(Constants.JSON_DETAILS_ERROR, e);
                }

                initializer = new Initializer();

                initializer.user = user;

                initializer.environment = environment;

                initializer.token = token;

                initializer.store = store;

                initializer.sdkConfig = sdkConfig;

                initializer.resourcePath = resourcePath;

                initializer.requestProxy = proxy;

                SDKLogger.LogInfo(Constants.INITIALIZATION_SUCCESSFUL + initializer.ToString());
            }
            catch(SDKException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SDKException(Constants.INITIALIZATION_EXCEPTION, e);
            }
        }

        /// <summary>
        /// This method to get record field information details.
        /// </summary>
        /// <param name="filePath">A String containing the class information details file path.</param>
        /// <returns></returns>
        public static JObject GetJSON(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);

            string fileContent = sr.ReadToEnd();

            sr.Close();

            return JObject.Parse(fileContent);
        }

        /// <summary>
        /// This method to get Initializer class instance.
        /// </summary>
        /// <returns>A Initializer class instance representing the SDK configuration details.</returns>
        public static Initializer GetInitializer()
        {
            if (Initializer.LOCAL.Value != null)
            {
                return Initializer.LOCAL.Value;
            }

            return initializer;
        }

        /// <summary>
        /// The method to switch the different user in SDK environment.
        /// </summary>
        /// <param name="user">A User class instance represents the CRM user.</param>
        /// <param name="environment">A Environment class instance containing the CRM API base URL and Accounts URL.</param>
        /// <param name="token">A Token class instance containing the OAuth client application information.</param>
        /// <param name="sdkConfig">A SDKConfig class instance containing the configuration.</param>
        /// <param name="proxy">An RequestProxy class instance containing the proxy properties of the user.
        private static void SwitchUser(UserSignature user, Dc.DataCenter.Environment environment, Token token, SDKConfig sdkConfig, RequestProxy proxy)
        {
            Initializer initializer = new Initializer();

            initializer.user = user;

            initializer.environment = environment;

            initializer.token = token;

            initializer.store = Initializer.initializer.store;

            initializer.sdkConfig = sdkConfig;

            initializer.requestProxy = proxy;

            initializer.resourcePath = Initializer.initializer.resourcePath;

            LOCAL.Value = initializer;

            SDKLogger.LogInfo(Constants.INITIALIZATION_SWITCHED + initializer.ToString());
        }

        /// <summary>
        /// This is a getter method to get API environment.
        /// </summary>
        /// <returns>A Environment representing the API environment.</returns>
        public Dc.DataCenter.Environment Environment
        {
            get
            {
                return environment;
            }
        }

        /// <summary>
        /// This is a getter method to get API environment.
        /// </summary>
        /// <returns>A TokenStore class instance containing the token store information.</returns>
        public TokenStore Store
        {
            get
            {
                return store;
            }
        }

        /// <summary>
        /// This is a getter method to get CRM User.
        /// </summary>
        /// <returns>A User class instance representing the CRM user.</returns>
        public UserSignature User
        {
            get
            {
                return user;
            }
        }

        /// <summary>
        /// This is a getter method to get Proxy information.
        /// </summary>
        /// <returns>A RequestProxy class instance representing the API Proxy information.</returns>
        public RequestProxy RequestProxy
        {
            get
            {
                return requestProxy;
            }
        }

        /// <summary>
        /// This is a getter method to get OAuth client application information.
        /// </summary>
        /// <returns>A Token class instance representing the OAuth client application information.</returns>
        public Token Token
        {
            get
            {
                return token;
            }
        }

        public string ResourcePath
        {
            get
            {
                return resourcePath;
            }
        }

        /// <summary>
        /// This is a getter method to get the SDK Configuration
        /// </summary>
        /// <returns>A SDKConfig instance representing the configuration</returns>
        public SDKConfig SDKConfig
        {
            get
            {
                return sdkConfig;
            }
        }

        public override string ToString()
        {
            return new StringBuilder().Append(Constants.FOR_EMAIL_ID).Append(GetInitializer().User.Email)
                .Append(Constants.IN_ENVIRONMENT).Append(GetInitializer().Environment.GetUrl()).Append(".").ToString();
        }

        public void Dispose()
        {
            ((IDisposable)LOCAL).Dispose();
        }
    }
}
