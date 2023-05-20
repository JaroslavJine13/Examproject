
using Dapper;

using WebMud.Dapper;
using System.Data;
using System.Data.SqlClient;
using WebMud.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace WebMud.Data
{
    public class UsersService : IUsersService
    {



        //dapper sql připojení
        private readonly IEmailServices _emailServices;
        private readonly SqlConnectionConfiguration _configuration;
        public UsersService(SqlConnectionConfiguration configuration, IEmailServices emailServices)
        {
            _configuration = configuration;
            _emailServices = emailServices;

        }


        public async Task<bool> AddUsers(Users users, string baseuri)
        {

            await Task.Delay(4000);
            var id = Guid.NewGuid();
            users.Id = id;
            users.IsAuthenticated = true;
            users.IsCustomer = false;
            users.IsChatAvailable = false;

            if (AdminSettings.IsAdminVerification)
            {

                users.IsVerified = false;

            }
            else
            {
                users.IsVerified = true;
            }


            if (AdminSettings.IsEmailVerification)
            {
                users.IsAuthenticated = false;

                var tokenid = Guid.NewGuid();
                DateTime createddate = DateTime.Now;
                DateTime expireddate = DateTime.Now.AddDays(3);


                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

                string tokenval = new string(Enumerable.Repeat(chars, 30)
                  .Select(s => s[random.Next(s.Length)]).ToArray());


                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"

insert into dbo.Tokens 
(

[ID] 
,[TOKENVALUE] 
,[CREATEDDATE]
,[EXPIREDDATE]
,[USERID]


) 
values 
(
@tokenid,
@tokenval,
@createddate,
@expireddate,
@id

)


";
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                        await conn.ExecuteAsync(query, new
                        {
                            tokenid,
                            tokenval,
                            createddate,
                            expireddate,
                            id

                        }, commandType: CommandType.Text);
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


                await _emailServices.SendEmailAsync
                (users.Email,
                "Activation",
               
             String.Format(AdminSettings.EmailTemplate, "<a href=" + "\"" + baseuri + "/pages/verifytoken/" + tokenval + "/\" >Link</a>")
                );


            }

            //sql 
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

insert into dbo.Users 
(
    [ID],    
    [EMAIL],  
    [FNAME], 
	[SNAME], 
	[PASSWORD], 
	[PASSWORD2],
	[POSITION], 
	[ISADMIN] ,
	[TOKEN] ,
	[ISVERIFIED],
	[ISDARK] ,
	[AVATARPATH] ,
	[TRAFFICCOUNT],
    [ISDELETED],
    [ISAUTHENTICATED],
    [ISCUSTOMER],
    [ISCHATAVAILABLE]

) 
values 
(
@Id,
@Email,
@FName,
@SName,
@Password,
@Password2,
@Position,
@IsAdmin,
@Token,
@IsVerified,
@IsDark,
@AvatarPath,
@TrafficCount,
0,
@IsAuthenticated,
@IsCustomer,
@IsChatAvailable
)


";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {

                    await conn.ExecuteAsync(query, new { users.Id, users.Email, users.FName, users.SName, users.Password, users.Password2, users.Position, users.IsAdmin, users.Token, users.IsVerified, users.IsDark, users.AvatarPath, users.TrafficCount, users.IsAuthenticated, users.IsCustomer, users.IsChatAvailable }, commandType: CommandType.Text);
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
            //
            return true;

        }


        //Chatuser
        public async Task<bool> AddGusetUser(Users users)
        {

            await Task.Delay(1000);
            var id = Guid.NewGuid();
            users.Id = id;
            users.IsAuthenticated = true;
            users.IsVerified = true;
            users.IsCustomer = true;
            users.IsChatAvailable = false;



            //sql 
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

insert into dbo.Users 
(
    [ID],    
    [EMAIL],  
    [FNAME], 
	[SNAME], 
	[PASSWORD], 
	[PASSWORD2],
	[POSITION], 
	[ISADMIN] ,
	[TOKEN] ,
	[ISVERIFIED],
	[ISDARK] ,
	[AVATARPATH] ,
	[TRAFFICCOUNT],
    [ISDELETED],
    [ISAUTHENTICATED],
    [ISCUSTOMER],
    [ISCHATAVAILABLE]

) 
values 
(
@Id,
@Email,
@FName,
@SName,
@Password,
@Password2,
@Position,
@IsAdmin,
@Token,
@IsVerified,
@IsDark,
@AvatarPath,
@TrafficCount,
0,
@IsAuthenticated,
@IsCustomer,
@IsChatAvailable
)


";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {

                    await conn.ExecuteAsync(query, new { users.Id, users.Email, users.FName, users.SName, users.Password, users.Password2, users.Position, users.IsAdmin, users.Token, users.IsVerified, users.IsDark, users.AvatarPath, users.TrafficCount, users.IsAuthenticated, users.IsCustomer, users.IsChatAvailable }, commandType: CommandType.Text);
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
            //
            return true;

        }


        public async Task<bool> DeleteUsers(Guid id)
        {

            using (var conn = new SqlConnection(_configuration.Value))
            {

                const string query = @"

update dbo.Users set 

	[ISDELETED] = 1,
	[ISVERIFIED] = 0

where Id=@Id";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new { id }, commandType: CommandType.Text);
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

                return true;

            }

        }

        public async Task<List<Users>> GetUsers()
        {

            List<Users> users = new List<Users>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"select * from dbo.Users where ISDELETED = 0";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    users = (await conn.QueryAsync<Users>(query)).ToList();

                    foreach (Users user in users)
                    {

                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }



            }
            return users;
        }

        public async Task<List<Users>> GetUsersIncludeDeleted()
        {

            List<Users> users = new List<Users>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"select * from dbo.Users";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    users = (await conn.QueryAsync<Users>(query)).ToList();

                    foreach (Users user in users)
                    {


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }



            }
            return users;
        }


        public async Task<Users> GetUsers(Guid id)
        {


            Users users = new Users();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"select * from dbo.Users where ID =@Id";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    users = await conn.QueryFirstOrDefaultAsync<Users>(query, new { id }, commandType: CommandType.Text);



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
            return users;


            //return Users.SingleOrDefault(x => x.Id == id);        

        }



        public async Task<Users> GetUsers(string email)
        {

            Users users = new Users();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"select * from dbo.Users where EMAIL = @email";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    users = await conn.QueryFirstOrDefaultAsync<Users>(query, new { email }, commandType: CommandType.Text);




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
            return users;

            //return Users.FirstOrDefault(x => x.Email == email);
        }

        public bool IfExists(string email)
        {
            if (GetUsers(email) != null)
                return true;
            return false;
        }

        public bool IfExists(Guid uid)
        {
            if (GetUsers(uid) != null)
                return true;
            return false;
        }

        public async Task<bool> InsertImage(string path, string name)
        {


            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
update dbo.Users set 

	[AVATARPATH] = @path

	
where EMAIL =@name

";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new { path, name }, commandType: CommandType.Text);
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


            return true;

        }

        public async Task<bool> UpdatePublicChatVisibility(bool res, Guid id)
        {


            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
update dbo.Users set 

	[ISCHATAVAILABLE] = @res

	
where ID =@id

";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new { res, id }, commandType: CommandType.Text);
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


            return true;

        }

        public async Task<bool> UpdateDarkMode(bool res, Guid id)
        {


            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
update dbo.Users set 

	[ISDARK] = @res

	
where ID =@id

";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new { res, id }, commandType: CommandType.Text);
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


            return true;

        }

        public async Task<bool> UpdatePass(string pass, string name)
        {



            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
update dbo.Users set 

	[PASSWORD] = @pass,
	[PASSWORD2] = @pass
	
where EMAIL =@name

";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new { pass, name }, commandType: CommandType.Text);




                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }

            return true;

        }

        public async Task<bool> UpdateUsers(Users users)
        {


            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
update dbo.Users set 

    [EMAIL]  = @Email,
    [FNAME] = @FName,
	[SNAME] = @SName,
	[PASSWORD] = @Password,
	[PASSWORD2] = @Password2,
	[POSITION] = @Position,
	[ISADMIN] = @IsAdmin,
	[TOKEN] = @Token,
	[ISVERIFIED] = @IsVerified,
	[ISDARK] = @IsDark,
	[AVATARPATH] = @AvatarPath,
	[TRAFFICCOUNT] = @TrafficCount,
    [ISCUSTOMER] = @IsCustomer,
    [ISCHATAVAILABLE] = @IsChatAvailable
 
where ID=@Id

";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new { users.Email, users.FName, users.SName, users.Password, users.Password2, users.Position, users.IsAdmin, users.Token, users.IsVerified, users.IsDark, users.AvatarPath, users.TrafficCount, users.Id, users.IsCustomer, users.IsChatAvailable }, commandType: CommandType.Text);
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

            return true;
        }



        public bool VerifyPass(string pass, string name)
        {
            if (pass == GetUsers(name).Result.Password)
                return true;
            return false;
        }

        public async Task<List<UsersNotified>> GetChatUsers(string toUserId)
        {


            List<UsersNotified> users = new List<UsersNotified>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
SELECT TOP 3
	   Users.[ID]
      ,Users.[EMAIL]
      ,Users.[FNAME]
      ,Users.[SNAME]
      ,Users.[PASSWORD]
      ,Users.[PASSWORD2]
      ,Users.[POSITION]
      ,Users.[ISADMIN]
      ,Users.[TOKEN]
      ,Users.[ISVERIFIED]
      ,Users.[ISDARK]
      ,Users.[AVATARPATH]
      ,Users.[ISDELETED]
      ,Users.[TRAFFICCOUNT]
    ,Users.[ISCUSTOMER]
    ,Users.[ISCHATAVAILABLE]
	  ,COUNT(Chat.id) AS MessageCount
	  ,MIN(Chat.BODY) AS LastMessage
	  ,MAX(Chat.CREATEDDATE) AS LastDate
FROM dbo.Users AS Users 

LEFT JOIN dbo.Chat AS Chat ON Users.ID = Chat.FROMUSERID

WHERE Users.ISDELETED = 0 AND Chat.ISNOTICE = 0 AND Chat.TOUSERID = @toUserId

GROUP BY 

	   Users.[ID]
      ,Users.[EMAIL]
      ,Users.[FNAME]
      ,Users.[SNAME]
      ,Users.[PASSWORD]
      ,Users.[PASSWORD2]
      ,Users.[POSITION]
      ,Users.[ISADMIN]
      ,Users.[TOKEN]
      ,Users.[ISVERIFIED]
      ,Users.[ISDARK]
      ,Users.[AVATARPATH]
      ,Users.[ISDELETED]
      ,Users.[TRAFFICCOUNT]
    ,Users.[ISCUSTOMER]
    ,Users.[ISCHATAVAILABLE]

	  ORDER BY MAX(Chat.CREATEDDATE) DESC



";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    users = (await conn.QueryAsync<UsersNotified>(query, new { toUserId })).ToList();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }



            }
            return users;

        }

        public async Task<List<UsersNotified>> GetChatUsersAll(string toUserId)
        {


            List<UsersNotified> users = new List<UsersNotified>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
SELECT 
	   Users.[ID]
      ,Users.[EMAIL]
      ,Users.[FNAME]
      ,Users.[SNAME]
      ,Users.[PASSWORD]
      ,Users.[PASSWORD2]
      ,Users.[POSITION]
      ,Users.[ISADMIN]
      ,Users.[TOKEN]
      ,Users.[ISVERIFIED]
      ,Users.[ISDARK]
      ,Users.[AVATARPATH]
      ,Users.[ISDELETED]
      ,Users.[TRAFFICCOUNT]
    ,Users.[ISCUSTOMER]
    ,Users.[ISCHATAVAILABLE]
	  ,COUNT(Chat.id) AS MessageCount
	  ,MIN(Chat.BODY) AS LastMessage
	  ,MAX(DateCol.CREATEDDATE) AS LastDate

FROM dbo.Users AS Users 

LEFT OUTER JOIN dbo.Chat AS Chat ON Users.ID = Chat.FROMUSERID AND Chat.TOUSERID = @toUserId AND ISNOTICE = 0
LEFT OUTER JOIN dbo.Chat AS DateCol ON Users.ID = DateCol.FROMUSERID AND DateCol.TOUSERID = @toUserId
WHERE Users.ISDELETED = 0 AND (Chat.TOUSERID = @toUserId OR Chat.TOUSERID IS NULL)
AND (DateCol.TOUSERID = @toUserId OR DateCol.TOUSERID IS NULL) 

GROUP BY 

	   Users.[ID]
      ,Users.[EMAIL]
      ,Users.[FNAME]
      ,Users.[SNAME]
      ,Users.[PASSWORD]
      ,Users.[PASSWORD2]
      ,Users.[POSITION]
      ,Users.[ISADMIN]
      ,Users.[TOKEN]
      ,Users.[ISVERIFIED]
      ,Users.[ISDARK]
      ,Users.[AVATARPATH]
      ,Users.[ISDELETED]
      ,Users.[TRAFFICCOUNT]
    ,Users.[ISCUSTOMER]
    ,Users.[ISCHATAVAILABLE]

	  ORDER BY MAX(DateCol.CREATEDDATE) DESC


";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    users = (await conn.QueryAsync<UsersNotified>(query, new { toUserId })).ToList();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally


                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }



            }
            return users;

        }

        public async Task<bool> VerifyUser(Guid id)
        {


            using (var conn = new SqlConnection(_configuration.Value))
            {

                const string query = @"

update dbo.Users set 

	[ISAUTHENTICATED] = 1

where Id=@Id";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new { id }, commandType: CommandType.Text);
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

                return true;

            }


        }

        public async Task<bool> ResetUsersPass(Guid id, string email, string baseuri)
        {


            var tokenid = Guid.NewGuid();
            DateTime createddate = DateTime.Now;
            DateTime expireddate = DateTime.Now.AddDays(3);


            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

            string tokenval = new string(Enumerable.Repeat(chars, 30)
              .Select(s => s[random.Next(s.Length)]).ToArray());


            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

insert into dbo.PwdTokens 
(

[ID] 
,[TOKENVALUE] 
,[CREATEDDATE]
,[EXPIREDDATE]
,[USERID]
,[ISFIRED]

) 
values 
(
@tokenid,
@tokenval,
@createddate,
@expireddate,
@id,
0

)


";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new
                    {
                        tokenid,
                        tokenval,
                        createddate,
                        expireddate,
                        id

                    }, commandType: CommandType.Text);
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


            await _emailServices.SendEmailAsync
            (email,
            "Activation",
            @" <p>Hello,</p><br/><p>I am Jirka's robot, I am sending you the password reset link, it will expire in three days:</p><br/> <a href=" + "\"" + baseuri + "/pages/approvepwd/" + tokenval + "/\">Link text</a>"

            );
            return true;
        }


        public async Task<bool> ResetUsersPassAssignPwd(Guid id)
        {


            string pwd;

            var tokenid = Guid.NewGuid();
            DateTime createddate = DateTime.Now;
            DateTime expireddate = DateTime.Now.AddDays(3);


            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

            pwd = new string(Enumerable.Repeat(chars, 12)
             .Select(s => s[random.Next(s.Length)]).ToArray());

            //hotfix
            pwd = pwd + "!";

            Users user = new Users();

            user = await GetUsers(id);
            if (user != null)
            {




                using (var conn = new SqlConnection(_configuration.Value))
                {

                    const string query = @"

update dbo.Users set 

	[PASSWORD] = @pwd,
	[PASSWORD2] = @pwd
	

where Id=@Id

update dbo.PwdTokens set 

	[ISFIRED] = 1
	

where USERID = @Id

";
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                        await conn.ExecuteAsync(query, new { pwd, id }, commandType: CommandType.Text);


                        await _emailServices.SendEmailAsync
    (user.Email,
    "Activation",
    @" <p>Hello,</p><br/><p>I am Jirka's robot, I am sending you a new password: " + pwd + "</p> "

    );


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

            }


            return true;
        }



        public async Task<bool> ResendToken(Users users, string baseuri)
        {
            if (AdminSettings.IsEmailVerification)
            {


                var tokenid = Guid.NewGuid();
                DateTime createddate = DateTime.Now;
                DateTime expireddate = DateTime.Now.AddDays(3);


                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

                string tokenval = new string(Enumerable.Repeat(chars, 30)
                  .Select(s => s[random.Next(s.Length)]).ToArray());


                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"

insert into dbo.Tokens 
(

[ID] 
,[TOKENVALUE] 
,[CREATEDDATE]
,[EXPIREDDATE]
,[USERID]


) 
values 
(
@tokenid,
@tokenval,
@createddate,
@expireddate,
@id

)


";
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                        await conn.ExecuteAsync(query, new
                        {
                            tokenid,
                            tokenval,
                            createddate,
                            expireddate,
                            users.Id

                        }, commandType: CommandType.Text);
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


                await _emailServices.SendEmailAsync
                (users.Email,
                "Activation",
                @" <p>Hello,</p><br/><p>I am Jirka's robot, I am sending you an activation link below, it will expire in three days:</p><br/> <a href=" + "\"" + baseuri + "/pages/verifytoken/" + tokenval + "/\">Link text</a>"

                );

                return true;
            }
            return true;
        }



    }
}
