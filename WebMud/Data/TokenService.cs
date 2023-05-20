using Dapper;
using Microsoft.AspNetCore.Components;
using System.Data;
using System.Data.SqlClient;
using WebMud.Dapper;
using WebMud.Models;

namespace WebMud.Data
{
    public class TokenService: ITokenService
    {


        private readonly IUsersService _usersService;
        private readonly SqlConnectionConfiguration _configuration;
        public TokenService(
            IUsersService usersService,

            SqlConnectionConfiguration configuration
            )
        {
 
            _usersService = usersService;
            _configuration = configuration;
        }



        public async Task<bool> VerifyToken(string token )
        {
            await Task.Delay(5000);
            bool res;
            Tokens tokencls = new Tokens();
            using (var conn = new SqlConnection(_configuration.Value))
            {

                const string query = @"select TOP 1 * from dbo.Tokens where [TOKENVALUE] = @token AND EXPIREDDATE >= GETDATE() ";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    tokencls = await conn.QueryFirstOrDefaultAsync<Tokens>(query, new { token }, commandType: CommandType.Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

            }
            if (tokencls != null)
            {          
                    await _usersService.VerifyUser(new Guid(tokencls.UserID));
 
                    res = true;
            }
            else
            {

                res = false;
            }

            return res;


        }

        public async Task<bool> VerifyPwdToken(string token)
        {
            await Task.Delay(5000);
            bool res;
            Tokens tokencls = new Tokens();
            using (var conn = new SqlConnection(_configuration.Value))
            {

                const string query = @"select TOP 1 * from dbo.PwdTokens where [TOKENVALUE] = @token AND EXPIREDDATE >= GETDATE() AND ISFIRED = 0";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    tokencls = await conn.QueryFirstOrDefaultAsync<Tokens>(query, new { token }, commandType: CommandType.Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

            }
            if (tokencls != null)
            {
 
                 await _usersService.ResetUsersPassAssignPwd(new Guid(tokencls.UserID));
 

                res = true;
            }
            else
            {

                res = false;
            }

            return res;


        }

    }
}
