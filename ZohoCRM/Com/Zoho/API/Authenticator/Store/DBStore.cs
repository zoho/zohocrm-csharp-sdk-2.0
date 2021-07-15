
using System.Collections.Generic;

using Com.Zoho.API.Exception;

using Com.Zoho.Crm.API;

using Com.Zoho.Crm.API.Util;

using MySql.Data.MySqlClient;

using System.Text;

namespace Com.Zoho.API.Authenticator.Store
{
    /// <summary>
    /// This class stores the user token details to the MySQL DataBase.
    /// </summary>
    public class DBStore : TokenStore
    {
        public class Builder
        {
            private string userName = Constants.MYSQL_USER_NAME;

            private string portNumber = Constants.MYSQL_PORT_NUMBER;

            private string password = "";

            private string host = Constants.MYSQL_HOST;

            private string databaseName = Constants.MYSQL_DATABASE_NAME;

            private string tableName = Constants.MYSQL_TABLE_NAME;

            public Builder UserName(string userName)
            {
                this.userName = userName;

                return this;
            }

            public Builder PortNumber(string portNumber)
            {
                if(portNumber != null)
                {
                    this.portNumber = portNumber;
                }

                return this;
            }

            public Builder Password(string password)
            {
                if (password != null)
                {
                    this.password = password;
                }

                return this;
            }

            public Builder Host(string host)
            {
                if (host != null)
                {
                    this.host = host;
                }

                return this;
            }

            public Builder DatabaseName(string databaseName)
            {
                if (databaseName != null)
                {
                    this.databaseName = databaseName;
                }

                return this;
            }

            public Builder TableName(string tableName)
            {
                if (tableName != null)
                {
                    this.tableName = tableName;
                }

                return this;
            }

            public DBStore Build()
            {
                return new DBStore(this.host, this.databaseName, this.tableName, this.userName, this.password, this.portNumber);
            }
        }

        private string userName;

        private string portNumber;

        private string password;

        private string host;

        private string databaseName;

        private string connectionString;

        private string tableName;


        /// <summary>
        /// Creates an DBStore class instance with the specified parameters.
        /// </summary>
        /// <param name="host">A string containing the DataBase host name. Default value localhost.</param>
        /// <param name="databaseName">A String containing the DataBase name. Default value zohooauth.</param>
        /// <param name="tableName">A String containing the Table name. Default value oauthtoken.</param>
        /// <param name="userName">A string containing the DataBase user name. Default value root.</param>
        /// <param name="password">A string containing the DataBase password. Default value "".</param>
        /// <param name="portNumber">A string containing the DataBase port number. Default value 3306.</param>
        private DBStore(string host, string databaseName, string tableName, string userName, string password, string portNumber)
        {
            this.host = host;

            this.databaseName = databaseName;

            this.tableName = tableName;

            this.userName = userName;

            this.password = password;

            this.portNumber = portNumber;

            this.connectionString = $"{Constants.SERVER}={this.host};{Constants.USERNAME}={this.userName};{Constants.PASSWORD}={this.password};{Constants.DATABASE}={this.databaseName};{Constants.PORT}={this.portNumber};persistsecurityinfo=True;SslMode=none;";
        }

        public Token GetToken(UserSignature user, Token token)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(this.connectionString))
                {
                    connection.Open();

                    if (token is OAuthToken)
                    {
                        OAuthToken oauthToken = (OAuthToken)token;

                        string query = ConstructDBQuery(user.Email, oauthToken, false);

                        using (MySqlCommand statement = new MySqlCommand(query, connection))
                        {
                            using (MySqlDataReader result = statement.ExecuteReader())
                            {
                                while (result.Read())
                                {
                                    oauthToken.Id = result[Constants.ID].ToString();

                                    oauthToken.AccessToken = result[Constants.ACCESS_TOKEN].ToString();

                                    oauthToken.ExpiresIn = result[Constants.EXPIRY_TIME].ToString();

                                    oauthToken.RefreshToken = result[Constants.REFRESH_TOKEN].ToString();

                                    oauthToken.UserMail = result[Constants.USER_MAIL].ToString();

                                    return oauthToken;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new SDKException(Constants.TOKEN_STORE, Constants.GET_TOKEN_DB_ERROR, ex);
            }

            return null;
        }

        public void SaveToken(UserSignature user, Token token)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(this.connectionString))
                {
                    if (token is OAuthToken)
                    {
                        OAuthToken oauthToken = (OAuthToken)token;

                        oauthToken.UserMail  = user.Email;

                        DeleteToken(oauthToken);

                        string query = "insert into " + this.tableName + "(id,user_mail,client_id,client_secret,refresh_token,access_token,grant_token,expiry_time,redirect_url) values (@id, @user_mail, @client_id, @client_secret, @refresh_token, @access_token, @grant_token, @expiry_time, @redirect_url);";

                        connection.Open();

                        using (MySqlCommand statement = new MySqlCommand(query, connection))
                        {
                            statement.Parameters.AddWithValue("@id", oauthToken.Id);

                            statement.Parameters.AddWithValue("@user_mail", user.Email);

                            statement.Parameters.AddWithValue("@client_id", oauthToken.ClientId);

                            statement.Parameters.AddWithValue("@client_secret", oauthToken.ClientSecret);

                            statement.Parameters.AddWithValue("@refresh_token", oauthToken.RefreshToken);

                            statement.Parameters.AddWithValue("@access_token", oauthToken.AccessToken);

                            statement.Parameters.AddWithValue("@grant_token", oauthToken.GrantToken);

                            statement.Parameters.AddWithValue("@expiry_time", oauthToken.ExpiresIn);

                            statement.Parameters.AddWithValue("@redirect_url", oauthToken.RedirectURL);

                            statement.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new SDKException(Constants.TOKEN_STORE, Constants.SAVE_TOKEN_DB_ERROR, ex);
            }
        }

        public void DeleteToken(Token token)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    if (token is OAuthToken)
                    {
                        string query = ConstructDBQuery(((OAuthToken)token).UserMail, (OAuthToken)token, true);

                        using (MySqlCommand statement = new MySqlCommand(query, connection))
                        {
                            statement.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (System.Exception ex) when (!(ex is SDKException))
            {
                throw new SDKException(Constants.TOKEN_STORE, Constants.DELETE_TOKEN_DB_ERROR, ex);
            }
        }

        public List<Token> GetTokens()
        {
            List<Token> tokens = new List<Token>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(this.connectionString))
                {
                    connection.Open();

                    string query = "select * from " + this.tableName + ";";

                    using (MySqlCommand statement = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader result = statement.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                string grantToken = result[Constants.GRANT_TOKEN] != null && !result[Constants.GRANT_TOKEN].ToString().Equals(Constants.NULL_VALUE, System.StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(result[Constants.GRANT_TOKEN].ToString()) ? result[Constants.GRANT_TOKEN].ToString() : null;

                                string redirectURL = result[Constants.REDIRECT_URL] != null && !result[Constants.REDIRECT_URL].ToString().Equals(Constants.NULL_VALUE, System.StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(result[Constants.REDIRECT_URL].ToString()) ? result[Constants.REDIRECT_URL].ToString() : null;

                                OAuthToken token = new OAuthToken.Builder().ClientId(result[Constants.CLIENT_ID].ToString()).ClientSecret(result[Constants.CLIENT_SECRET].ToString()).RefreshToken(result[Constants.REFRESH_TOKEN].ToString()).Build();

                                token.Id = result[Constants.ID].ToString();

                                token.GrantToken = grantToken;

                                token.UserMail = result[Constants.USER_MAIL].ToString();

                                token.AccessToken = result[Constants.ACCESS_TOKEN].ToString();

                                token.ExpiresIn = result[Constants.EXPIRY_TIME].ToString();

                                token.RedirectURL = redirectURL;

                                tokens.Add(token);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new SDKException(Constants.TOKEN_STORE, Constants.GET_TOKENS_DB_ERROR, ex);
            }

            return tokens;
        }

        public void DeleteTokens()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "delete from " + this.tableName + ";";

                    using (MySqlCommand statement = new MySqlCommand(query, connection))
                    {
                        statement.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new SDKException(Constants.TOKEN_STORE, Constants.DELETE_TOKENS_DB_ERROR, ex);
            }
        }

        private string ConstructDBQuery(string email, OAuthToken token, bool isDelete = true)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new SDKException(Constants.USER_MAIL_NULL_ERROR, Constants.USER_MAIL_NULL_ERROR_MESSAGE);
            }

            StringBuilder queryBuilder = new StringBuilder();

            queryBuilder.Append(isDelete ? "delete from " : "select * from ");

            queryBuilder.Append(this.tableName).Append(" where user_mail='").Append(email);

            queryBuilder.Append("' and client_id='").Append(token.ClientId).Append("' and ");

            if (token.GrantToken != null)
            {
                queryBuilder.Append("grant_token='").Append(token.GrantToken).Append("'");
            }
            else
            {
                queryBuilder.Append("refresh_token='").Append(token.RefreshToken).Append("'");
            }

            return queryBuilder.ToString();
        }

        public Token GetTokenById(string id, Token token)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(this.connectionString))
                {
                    connection.Open();

                    if (token is OAuthToken)
                    {
                        OAuthToken oauthToken = (OAuthToken)token;

                        string query = "select * from " + this.tableName + " where id='" + id + "'";

                        using (MySqlCommand statement = new MySqlCommand(query, connection))
                        {
                            using (MySqlDataReader result = statement.ExecuteReader())
                            {
                                if(!result.HasRows)
                                {
                                    throw new SDKException(Constants.TOKEN_STORE, Constants.GET_TOKEN_BY_ID_DB_ERROR);
                                }

                                while (result.Read())
                                {
                                    string grantToken = result[Constants.GRANT_TOKEN] != null && !result[Constants.GRANT_TOKEN].ToString().Equals(Constants.NULL_VALUE, System.StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(result[Constants.GRANT_TOKEN].ToString()) ? result[Constants.GRANT_TOKEN].ToString() : null;

                                    string redirectURL = result[Constants.REDIRECT_URL] != null && !result[Constants.REDIRECT_URL].ToString().Equals(Constants.NULL_VALUE, System.StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(result[Constants.REDIRECT_URL].ToString()) ? result[Constants.REDIRECT_URL].ToString() : null;

                                    oauthToken.ClientId = result[Constants.CLIENT_ID].ToString();

                                    oauthToken.ClientSecret = result[Constants.CLIENT_SECRET].ToString();

                                    oauthToken.RefreshToken = result[Constants.REFRESH_TOKEN].ToString();

                                    oauthToken.Id = id;

                                    oauthToken.GrantToken = grantToken;

                                    oauthToken.UserMail = result[Constants.USER_MAIL].ToString();

                                    oauthToken.AccessToken = result[Constants.ACCESS_TOKEN].ToString();

                                    oauthToken.ExpiresIn = result[Constants.EXPIRY_TIME].ToString();

                                    oauthToken.RedirectURL = redirectURL;

                                    return oauthToken;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new SDKException(Constants.TOKEN_STORE, Constants.GET_TOKEN_DB_ERROR, ex);
            }

            return null;
        }
    }
}
