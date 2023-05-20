using WebMud.Shared;
using Dapper;
using WebMud.Dapper;
using System.Data;
using System.Data.SqlClient;
using WebMud.Models;

namespace WebMud.Data
{
    public class ChatService : IChatService
    {


        //dapper sql připojení
        private readonly SqlConnectionConfiguration _configuration;
        public ChatService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<bool> AddMessage(Message message)
        {
            var id = Guid.NewGuid();
            message.Id = id;
            //_Message.Add(message);

            //sql 
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

insert into dbo.Chat 
(

[ID], 
[USERNAME],
[BODY], 
[MINE], 
[ISALIGMENT], 
[ISNOTICE], 
[CSS], 
[ALIGMENT], 
[CREATEDDATE],
[FROMUSERID], 
[TOUSERID]


) 
values 
(
@Id,
@Username,
@Body,
@Mine,
@IsAligment,
@IsNotice,
@CSS,
@Aligment,
@CreatedDate,
@FromUserId,
@ToUserId
)


";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new { message.Id, message.Username, message.Body, message.Mine, message.IsAligment, message.IsNotice, message.CSS, message.Aligment, message.CreatedDate, message.FromUserId, message.ToUserId }, commandType: CommandType.Text);
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

        public async Task<bool> ChangeNotice(string toUser, string fromUser)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

UPDATE  dbo.Chat set [ISNOTICE] = 1 where TOUSERID = @fromUser AND FROMUSERID = @toUser

";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {

                    await conn.ExecuteAsync(query, new { toUser, fromUser }, commandType: CommandType.Text);
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

        public async Task<bool> ClearMessage(string toUser, string fromUser)
        {
            //_Message.RemoveAll(x => (x.ToUserId == toUser && x.FromUserId == fromUser) || (x.ToUserId == fromUser && x.FromUserId == toUser));

            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

DELETE from dbo.Chat where ((TOUSERID = @toUser AND FROMUSERID = @fromUser ) OR (TOUSERID = @fromUser AND FROMUSERID = @toUser))

";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {

                    await conn.ExecuteAsync(query, new { toUser, fromUser }, commandType: CommandType.Text);
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

        public async Task<List<Message>> GetNoticeMessage(string toUser)
        {

            List<Message> message = new List<Message>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

select * from dbo.Chat where [TOUSERID] = @toUser AND [ISNOTICE] = 0

";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    message = (await conn.QueryAsync<Message>(query, new { toUser })).ToList();
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
            return message;


        }

        public async Task<List<Message>> GetMessage(string toUser, string fromUser)
        {



            List<Message> message = new List<Message>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

select * from dbo.Chat where ((TOUSERID = @toUser AND FROMUSERID = @fromUser ) OR (TOUSERID = @fromUser AND FROMUSERID = @toUser))

";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    message = (await conn.QueryAsync<Message>(query, new { toUser, fromUser }, commandType: CommandType.Text)).ToList();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("chybaaa!!! " +ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

            }
            return message;


        }
    }
}
