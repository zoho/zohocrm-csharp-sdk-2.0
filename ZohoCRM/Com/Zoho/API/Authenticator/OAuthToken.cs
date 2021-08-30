using System;

using System.Collections.Generic;

using System.IO;

using System.Net;

using System.Text;

using Com.Zoho.API.Authenticator.Store;

using Com.Zoho.API.Exception;

using Com.Zoho.Crm.API;

using Com.Zoho.Crm.API.Logger;

using Com.Zoho.Crm.API.Util;

using Newtonsoft.Json;

using Newtonsoft.Json.Linq;

namespace Com.Zoho.API.Authenticator
{
    /// <summary>
    /// This class gets the tokens and checks the expiry time.
    /// </summary>
    public class OAuthToken : Token
    {
        public class Builder
        {
            private string clientId;

            private string clientSecret;

            private string redirectURL;

            private string refreshToken;

            private string grantToken;

            private string accessToken;

            private string id;

            public Builder Id(string id)
            {
                this.id = id;

                return this;
            }

            public Builder ClientId(string clientId)
            {
                Utility.AssertNotNull(clientId, Constants.TOKEN_ERROR, Constants.CLIENT_ID_NULL_ERROR_MESSAGE);

                this.clientId = clientId;

                return this;
            }

            public Builder ClientSecret(string clientSecret)
            {
                Utility.AssertNotNull(clientSecret, Constants.TOKEN_ERROR, Constants.CLIENT_SECRET_NULL_ERROR_MESSAGE);

                this.clientSecret = clientSecret;

                return this;
            }

            public Builder RedirectURL(string redirectURL)
            {
                this.redirectURL = redirectURL;

                return this;
            }

            public Builder RefreshToken(string refreshToken)
            {
                this.refreshToken = refreshToken;

                return this;
            }

            public Builder GrantToken(string grantToken)
            {
                this.grantToken = grantToken;

                return this;
            }

            public Builder AccessToken(string accessToken)
            {
                this.accessToken = accessToken;

                return this;
            }

            public OAuthToken Build()
            {
                if (this.grantToken == null && this.refreshToken == null && this.id == null && this.accessToken == null)
                {
                    throw new SDKException(Constants.MANDATORY_VALUE_ERROR, Constants.MANDATORY_KEY_ERROR + "-" + JsonConvert.SerializeObject(Constants.OAUTH_MANDATORY_KEYS));
                }

                return new OAuthToken(this.clientId, this.clientSecret, this.grantToken, this.refreshToken, this.redirectURL, this.id, this.accessToken);
            }
        }

        private string clientID;

        private string clientSecret;

        private string redirectURL;

        private string grantToken;

        private string refreshToken;

        private string accessToken;

        private string expiresIn;

        private string userMail;

        private string id;

        /// <summary>
        /// This is a getter method to get OAuth client id.
        /// </summary>
        /// <returns>A string representing the OAuth client id.</returns>
        public string ClientId
        {
            get
            {
                return clientID;
            }
            set
            {
                clientID = value;
            }
        }

        /// <summary>
        /// This is a getter method to get OAuth client secret.
        /// </summary>
        /// <returns>A string representing the OAuth client secret.</returns>
        public string ClientSecret
        {
            get
            {
                return clientSecret;
            }
            set
            {
                clientSecret = value;
            }
        }

        /// <summary>
        /// This is a getter method to get OAuth redirect URL.
        /// </summary>
        /// <returns>A string representing the OAuth redirect URL.</returns>
        public string RedirectURL
        {
            get
            {
                return redirectURL;
            }
            set
            {
                redirectURL = value;
            }
        }

        /// <summary>
        /// This is a getter method to get grant token.
        /// </summary>
        /// <returns>A string representing the grant token.</returns>
        public string GrantToken
        {
            get
            {
                return grantToken;
            }
            set
            {
                grantToken = value;
            }
        }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>A string containing the refresh token.</value>
        /// <returns>A string representing the refresh token.</returns>
        public string RefreshToken
        {
            get
            {
                return refreshToken;
            }
            set
            {
                refreshToken = value;
            }
        }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>A string containing the access token.</value>
        /// <returns>A string representing the access token.</returns>
        public string AccessToken
        {
            get
            {
                return accessToken;
            }
            set
            {
                accessToken = value;
            }
        }

        /// <summary>
        /// Gets or sets the token expire time.
        /// </summary>
        /// <value>A string containing the token expire time.</value>
        /// <returns>A string representing the token expire time.</returns>
        public string ExpiresIn
        {
            get
            {
                return expiresIn;
            }
            set
            {
                expiresIn = value;
            }
        }

        public string UserMail
        {
            get
            {
                return userMail;
            }
            set
            {
                userMail = value;
            }
        }

        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public void Authenticate(APIHTTPConnector urlConnection)
        {
            lock (this)
            {
                try
                {
                    Initializer initializer = Initializer.GetInitializer();

                    TokenStore store = initializer.Store;

                    UserSignature user = initializer.User;

                    OAuthToken oauthToken = null;

                    if (this.accessToken == null)
                    {
                        if (this.id != null)
                        {
                            oauthToken = (OAuthToken)store.GetTokenById(this.id, this);
                        }
                        else
                        {
                            oauthToken = (OAuthToken)store.GetToken(user, this);
                        }
                    }
                    else
                    {
                        oauthToken = this;
                    }

                    string token = "";

                    if (oauthToken == null)//first time
                    {
                        token = this.refreshToken != null ? this.RefreshAccessToken(user, store).AccessToken : this.GenerateAccessToken(user, store).AccessToken;
                    }
                    else if (oauthToken.ExpiresIn != null && GetExpiryLapseInMillis(oauthToken.ExpiresIn) < 5L)//access token will expire in next 5 seconds or less
                    {
                        SDKLogger.LogInfo(Constants.REFRESH_TOKEN_MESSAGE);

                        token = oauthToken.RefreshAccessToken(user, store).AccessToken;
                    }
                    else
                    {
                        token = oauthToken.AccessToken;
                    }

                    urlConnection.AddHeader(Constants.AUTHORIZATION, Constants.OAUTH_HEADER_PREFIX + token);

                }
                catch (System.Exception ex) when (!(ex is SDKException))
                {
                    throw new SDKException(ex);
                }
            }
        }

        private string GetResponseFromServer(Dictionary<string, string> requestParams)
        {
            try
            {
                string USER_AGENT = Constants.USER_AGENT;

                string url = Initializer.GetInitializer().Environment.GetAccountsUrl();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                string urlParameters = null;

                if (requestParams != null && requestParams.Count != 0)
                {
                    foreach (KeyValuePair<string, string> param in requestParams)
                    {
                        if (urlParameters == null)
                        {
                            urlParameters = param.Key + "=" + param.Value;
                        }
                        else
                        {
                            urlParameters += "&" + param.Key + "=" + param.Value;
                        }
                    }
                }

                request.UserAgent = USER_AGENT;

                var data = Encoding.UTF8.GetBytes(urlParameters);

                request.ContentType = Constants.URL_ENCODEED;

                request.ContentLength = data.Length;

                request.Method = Constants.REQUEST_METHOD_POST;

                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (System.Exception ex)
            {
                throw new SDKException(ex);
            }
        }

        private OAuthToken RefreshAccessToken(UserSignature user, TokenStore store)
        {
            try
            {
                Dictionary<string, string> requestParams = new Dictionary<string, string>();

                requestParams.Add(Constants.CLIENT_ID, ClientId);

                requestParams.Add(Constants.CLIENT_SECRET, ClientSecret);

                requestParams.Add(Constants.GRANT_TYPE, Constants.REFRESH_TOKEN);

                requestParams.Add(Constants.REFRESH_TOKEN, RefreshToken);

                string response = GetResponseFromServer(requestParams);

                ParseResponse(response);

                if(string.IsNullOrEmpty(this.id) || string.IsNullOrWhiteSpace(this.id))
                {
                    GenerateId();
                }

                store.SaveToken(user, this);
            }
            catch (System.Exception ex) when (!(ex is SDKException))
            {
                throw new SDKException(Constants.SAVE_TOKEN_ERROR, ex);
            }

            return this;
        }


        private OAuthToken GenerateAccessToken(UserSignature user,TokenStore store)
        {
            try
            {
                Dictionary<string, string> requestParams = new Dictionary<string, string>();

                requestParams.Add(Constants.CLIENT_ID, ClientId);

                requestParams.Add(Constants.CLIENT_SECRET, ClientSecret);

                if (RedirectURL != null)
                {
                    requestParams.Add(Constants.REDIRECT_URI, RedirectURL);
                }

                requestParams.Add(Constants.GRANT_TYPE, Constants.GRANT_TYPE_AUTH_CODE);

                requestParams.Add(Constants.CODE, GrantToken);

                string response = GetResponseFromServer(requestParams);

                ParseResponse(response);
                
                GenerateId();

                store.SaveToken(user, this);
            }
            catch (System.Exception ex) when (!(ex is SDKException))
            {
                throw new SDKException(Constants.SAVE_TOKEN_ERROR, ex);
            }

            return this;
        }

        private OAuthToken ParseResponse(string response)
        {
            JObject responseJSON = JObject.Parse(response);

            if (!responseJSON.ContainsKey(Constants.ACCESS_TOKEN))
            {
                throw new SDKException(Constants.INVALID_TOKEN_ERROR, (responseJSON.ContainsKey(Constants.ERROR_KEY))? responseJSON[Constants.ERROR_KEY].ToString() : Constants.NO_ACCESS_TOKEN_ERROR);
            }

            AccessToken = (string)responseJSON[Constants.ACCESS_TOKEN];

            ExpiresIn = GetTokenExpiresIn(responseJSON).ToString();

            if (responseJSON.ContainsKey(Constants.REFRESH_TOKEN))
            {
                refreshToken = (string)responseJSON[Constants.REFRESH_TOKEN];
            }

            return this;
        }

        private long GetTokenExpiresIn(JObject response)
        {
            long expiresInTime = response.ContainsKey(Constants.EXPIRES_IN_SEC) ? Convert.ToInt64(response[Constants.EXPIRES_IN]) : Convert.ToInt64(response[Constants.EXPIRES_IN]) * 1000;

            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + expiresInTime - 600000;
        }

        public long GetExpiryLapseInMillis(string ExpiryTime)
        {
            long time = Convert.ToInt64(ExpiryTime) - (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

            return time;
        }

        /// <summary>
        /// The method to remove the current token from the Store.
        /// </summary>
        /// <returns></returns>
        public bool Remove()
        {
            try
            {
                Initializer.GetInitializer().Store.DeleteToken(this);

                return true;
            }
            catch (System.Exception ex) when (!(ex is SDKException))
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creates an OAuthToken class instance with the specified parameters.
        /// </summary>
        /// <param name="clientID">A string containing the OAuth client id.</param>
        /// <param name="clientSecret">A string containing the OAuth client secret.</param>
        /// <param name="token">A string containing the REFRESH/GRANT token.</param>
        /// <param name="type">A enum containing the given token type.</param>
        /// <param name="redirectURL">A string containing the OAuth redirect URL.</param>
        private OAuthToken(string clientID, string clientSecret, string grantToken, string refreshToken, string redirectURL, string id, string accessToken)
        {
            this.clientID = clientID;

            this.clientSecret = clientSecret;

            this.grantToken = grantToken;

            this.refreshToken = refreshToken;

            this.redirectURL = redirectURL;

            this.accessToken = accessToken;

            this.id = id;
        }

        private void GenerateId()
        {
            StringBuilder builder = new StringBuilder();

            string email = Initializer.GetInitializer().User.Email;

            builder.Append(Constants.CSHARP).Append(email.Substring(0, email.IndexOf("@"))).Append("_");

            builder.Append(Initializer.GetInitializer().Environment.GetName()).Append("_");

            builder.Append(this.refreshToken.Substring(this.refreshToken.Length - 4));

            this.id = builder.ToString();
        }
    }
}
