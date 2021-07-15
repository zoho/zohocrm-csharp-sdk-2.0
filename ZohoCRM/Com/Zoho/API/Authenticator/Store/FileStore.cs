
using System;

using System.Collections.Generic;

using System.IO;

using System.Linq;

using System.Text;

using Com.Zoho.API.Exception;

using Com.Zoho.Crm.API;

using Com.Zoho.Crm.API.Util;

namespace Com.Zoho.API.Authenticator.Store
{
    /// <summary>
    /// This class stores the user token details to the file.
    /// </summary>
    public class FileStore : TokenStore
    {
        private string filePath;

        private List<string> headers = new List<string>() { Constants.ID, Constants.USER_MAIL, Constants.CLIENT_ID, Constants.CLIENT_SECRET, Constants.REFRESH_TOKEN, Constants.ACCESS_TOKEN, Constants.GRANT_TOKEN, Constants.EXPIRY_TIME, Constants.REDIRECT_URL };

        /// <summary>
        /// Creates an FileStore class instance with the specified parameters.
        /// </summary>
        /// <param name="filePath">A String containing the absolute file path to store tokens.</param>
        public FileStore(string filePath)
        {
            this.filePath = filePath;

            string[] lines = null;

            if (File.Exists(this.filePath))
            {
                lines = File.ReadAllLines(this.filePath);
            }

            if (lines == null || lines.Length < 1)
            {
                using (FileStream fileStream = new FileStream(this.filePath, FileMode.Append))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        writer.WriteLine(string.Join(",", headers));

                        writer.Close();
                    }

                    fileStream.Close();
                }
            }
        }

        public Token GetToken(UserSignature user, Token token)
        {
            try
            {
                string[] allContents = File.ReadAllLines(this.filePath);

                if (allContents == null || allContents.Length < 1)
                {
                    return null;
                }

                if (token is OAuthToken)
                {
                    OAuthToken oauthToken = (OAuthToken)token;

                    foreach (string line in allContents)
                    {
                        string[] nextRecord = line.Split(',');

                        if (CheckTokenExists(user.Email, oauthToken, nextRecord))
                        {
                            oauthToken.AccessToken = nextRecord[5];

                            oauthToken.ExpiresIn = nextRecord[7];

                            oauthToken.RefreshToken = nextRecord[4];

                            oauthToken.Id = nextRecord[0];

                            oauthToken.UserMail = nextRecord[1];

                            return oauthToken;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new SDKException(Constants.TOKEN_STORE, Constants.GET_TOKEN_FILE_ERROR, ex);
            }

            return null;
        }

        public void SaveToken(UserSignature user, Token token)
        {
            try
            {
                List<string> data = null;

                if (token is OAuthToken)
                {
                    OAuthToken oauthToken = (OAuthToken)token;

                    oauthToken.UserMail = user.Email;

                    DeleteToken(oauthToken);

                    data = new List<string>
                    {
                        oauthToken.Id,

                        user.Email,

                        oauthToken.ClientId,

                        oauthToken.ClientSecret,

                        oauthToken.RefreshToken,

                        oauthToken.AccessToken,

                        oauthToken.GrantToken,

                        oauthToken.ExpiresIn,

                        oauthToken.RedirectURL
                    };
                }

                using (FileStream outFile = new FileStream(this.filePath, FileMode.Append))
                {
                    using (StreamWriter writer = new StreamWriter(outFile))
                    {
                        writer.WriteLine(string.Join(",", data));

                        writer.Close();
                    }

                    outFile.Close();
                }
            }
            catch (System.Exception ex) when (ex is UnauthorizedAccessException || ex is DirectoryNotFoundException)
            {
                throw new SDKException(Constants.TOKEN_STORE, Constants.SAVE_TOKEN_FILE_ERROR, ex);
            }
        }

        public void DeleteToken(Token token)
        {
            try
            {
                string[] lines = File.ReadAllLines(this.filePath);

                if (lines == null || lines.Length < 1)
                {
                    return;
                }

                File.WriteAllText(this.filePath, string.Empty);

                StringBuilder csvData = new StringBuilder();

                if (token is OAuthToken)
                {
                    OAuthToken oauthToken = (OAuthToken)token;

                    List<string> allContents = lines.ToList();

                    foreach (string value in allContents)
                    {
                        string[] nextRecord = value.Split(',');

                        if (!CheckTokenExists(oauthToken.UserMail, oauthToken, nextRecord))
                        {
                            csvData.Append(string.Join(",", nextRecord));

                            csvData.Append("\n");
                        }
                    }
                }

                File.WriteAllText(this.filePath, csvData.ToString());
            }
            catch (System.Exception ex) when (!(ex is SDKException) && (ex is UnauthorizedAccessException || ex is DirectoryNotFoundException))
            {
                throw new SDKException(Constants.TOKEN_STORE, Constants.DELETE_TOKEN_FILE_ERROR, ex);
            }
        }

        public List<Token> GetTokens()
        {
            List<Token> tokens = new List<Token>();

            try
            {
                string[] allContents = File.ReadAllLines(this.filePath);

                if (allContents == null || allContents.Length < 1)
                {
                    return null;
                }

                for (int index = 1; index < allContents.Length; index++)
                {
                    string line = allContents[index];

                    string[] nextRecord = line.Split(',');

                    string grantToken = !string.IsNullOrEmpty(nextRecord[6]) ? nextRecord[6] : null;

                    string redirectURL = !string.IsNullOrEmpty(nextRecord[8]) ? nextRecord[8] : null;

                    OAuthToken token = new OAuthToken.Builder().ClientId(nextRecord[2]).ClientSecret(nextRecord[3]).RefreshToken(nextRecord[4]).Build();

                    token.Id = nextRecord[0];

                    token.GrantToken = grantToken;

                    token.UserMail = nextRecord[1];

                    token.AccessToken = nextRecord[5];

                    token.ExpiresIn = nextRecord[7];

                    token.RedirectURL = redirectURL;

                    tokens.Add(token);
                }
            }
            catch (System.Exception ex)
            {
                throw new SDKException(Constants.TOKEN_STORE, Constants.GET_TOKENS_FILE_ERROR, ex);
            }

            return tokens;
        }

        public void DeleteTokens()
        {
            try
            {
                File.WriteAllText(this.filePath, string.Empty);

                File.WriteAllText(this.filePath, string.Join(",", headers));
            }
            catch (System.Exception ex) when (ex is UnauthorizedAccessException || ex is DirectoryNotFoundException)
            {
                throw new SDKException(Constants.TOKEN_STORE, Constants.DELETE_TOKENS_FILE_ERROR, ex);
            }
        }

        private bool CheckTokenExists(string email, OAuthToken oauthToken, string[] row)
        {
            if(string.IsNullOrEmpty(email))
            {
                throw new SDKException(Constants.USER_MAIL_NULL_ERROR, Constants.USER_MAIL_NULL_ERROR_MESSAGE);
            }

            string clientId = oauthToken.ClientId;

            string grantToken = oauthToken.GrantToken;

            string refreshToken = oauthToken.RefreshToken;

            bool tokenCheck = grantToken != null ? grantToken.Equals(row[6]) : refreshToken.Equals(row[4]);

            if(row[1].Equals(email) && row[2].Equals(clientId) && tokenCheck)
            {
                return true;
            }

            return false;
        }

        public Token GetTokenById(string id, Token token)
        {
            try
            {
                string[] allContents = File.ReadAllLines(this.filePath);

                bool isRowPresent = false;

                if (token is OAuthToken)
                {
                    OAuthToken oauthToken = (OAuthToken)token;

                    foreach (string line in allContents)
                    {
                        string[] nextRecord = line.Split(',');

                        if (nextRecord[0].Equals(id))
                        {
                            isRowPresent = true;

                            string grantToken = !string.IsNullOrEmpty(nextRecord[6]) ? nextRecord[6] : null;

                            string redirectURL = !string.IsNullOrEmpty(nextRecord[8]) ? nextRecord[8] : null;

                            oauthToken.ClientId = nextRecord[2];

                            oauthToken.ClientSecret = nextRecord[3];

                            oauthToken.RefreshToken = nextRecord[4];

                            oauthToken.Id = id;

                            oauthToken.GrantToken = grantToken;

                            oauthToken.UserMail = nextRecord[1];

                            oauthToken.AccessToken = nextRecord[5];

                            oauthToken.ExpiresIn = nextRecord[7];

                            oauthToken.RedirectURL = redirectURL;

                            return oauthToken;
                        }
                    }

                    if(!isRowPresent)
                    {
                        throw new SDKException(Constants.TOKEN_STORE, Constants.GET_TOKEN_BY_ID_FILE_ERROR);
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new SDKException(Constants.TOKEN_STORE, Constants.GET_TOKEN_FILE_ERROR, ex);
            }

            return null;
        }
    }
}
