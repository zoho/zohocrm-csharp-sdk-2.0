
using System.Collections.Generic;

using Com.Zoho.Crm.API;

namespace Com.Zoho.API.Authenticator.Store
{
    /// <summary>
    /// This interface stores the user token details.
    /// </summary>
    public interface TokenStore
    {
        /// <summary>
        /// This method is used to get user token details.
        /// </summary>
        /// <param name="user">A UserSignature class instance.</param>
        /// <param name="token">A Token class instance.</param>
        /// <returns>A Token class instance representing the user token details.</returns>
        Token GetToken(UserSignature user, Token token);

        /// <summary>
        /// This method is used to store user token details.
        /// </summary>
        /// <param name="user">A UserSignature class instance.</param>
        /// <param name="token">A Token class instance.</param>
        void SaveToken(UserSignature user, Token token);

        /// <summary>
        /// This method is used to delete user token details.
        /// </summary>
        /// <param name="user">A UserSignature class instance.</param>
        /// <param name="token">A Token class instance.</param>
        void DeleteToken(Token token);

        /// <summary>
        /// The method to retrieve all the stored tokens.
        /// </summary>
        List<Token> GetTokens();

        /// <summary>
        /// The method to delete all the stored tokens.
        /// </summary>
        void DeleteTokens();

        /// <summary>
        /// This method is used to retrieve the user token details based on unique ID
        /// </summary>
        /// <param name="id">A String representing the unique ID</param>
        /// <param name="token">A Token class instance representing the user token details.</param>
        /// <returns>A Token class instance representing the user token details.</returns>
        ///
        Token GetTokenById(string id, Token token);
    }
}
