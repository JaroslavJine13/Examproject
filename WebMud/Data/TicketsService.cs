using WebMud.Dapper;
using System.Data;
using System.Data.SqlClient;
using WebMud.Models;
using Dapper;

namespace WebMud.Data
{
    public class TicketsService: ITicketsService
    {


        private readonly SqlConnectionConfiguration _configuration;
        public TicketsService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Tickets>> GetTickets()
        {

            List<Tickets> users = new List<Tickets>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"select 

	  [ID]
      ,[EMAIL]
      ,[COMPANY]
      ,[FNAME]
      ,[SNAME]
      ,[PHONE]
      ,[TASK]
      ,[INTERNALNOTE]
      ,[ISNOTICED]
      ,[ISCOMPLETED]
      ,[ISDELETED]
      ,CAST([ASSIGNEDUSERID] AS NVARCHAR (MAX)) ASSIGNEDUSERID
      ,[CREATEDDATE]
      ,[COMPLETEDDATE]
      ,[ASSIGNEDDDATE]


from dbo.Tickets where ISDELETED = 0 ORDER BY [CREATEDDATE] DESC";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    users = (await conn.QueryAsync<Tickets>(query)).ToList();

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

        public async Task<Tickets> GetTicket(Guid id)
        {

            Tickets tickets = new Tickets();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select top 1 
	  [ID]
      ,[EMAIL]
      ,[COMPANY]
      ,[FNAME]
      ,[SNAME]
      ,[PHONE]
      ,[TASK]
      ,[INTERNALNOTE]
      ,[ISNOTICED]
      ,[ISCOMPLETED]
      ,[ISDELETED]
      ,CAST([ASSIGNEDUSERID] AS NVARCHAR (MAX)) ASSIGNEDUSERID
      ,[CREATEDDATE]
      ,[COMPLETEDDATE]
      ,[ASSIGNEDDDATE]

from dbo.Tickets where ID = @id ";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    tickets = await conn.QueryFirstOrDefaultAsync<Tickets>(query, new { id }, commandType: CommandType.Text);

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
            return tickets;
        }


        public async Task<bool> AddTicket(Tickets tickets)
        {


            var id = Guid.NewGuid();
            tickets.Id = id;
            tickets.IsDeleted = false;
            tickets.IsCompleted = false;
            tickets.IsNoticed = false;
            tickets.CreatedDate = DateTime.Now;


            await Task.Delay(1000);
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

insert into dbo.Tickets 
(
		[ID]
      ,[EMAIL]
      ,[COMPANY]
      ,[FNAME]
      ,[SNAME]
      ,[PHONE]
      ,[TASK]
      ,[INTERNALNOTE]
      ,[ISNOTICED]
      ,[ISCOMPLETED]
      ,[ISDELETED]
      ,[ASSIGNEDUSERID]
      ,[CREATEDDATE]
      ,[COMPLETEDDATE]
      ,[ASSIGNEDDDATE]
) 
values 
(
@Id,
ISNULL(@Email,''),
ISNULL(@Company,''),
ISNULL(@Fname,''),
ISNULL(@Sname,''),
ISNULL(@Phone,''),
ISNULL(@Task,''),
ISNULL(@InternalNote,''),
@IsNoticed,
@IsCompleted,
@IsDeleted,
@AssignedUserID,
@CreatedDate,
@CompletedDate,
@AssignedDate

)


";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new { tickets.Id, tickets.Email, tickets.Company, tickets.Fname, tickets.Sname, 
                        tickets.Phone, tickets.Task, tickets.InternalNote, tickets.IsNoticed, tickets.IsCompleted, tickets.IsDeleted, tickets.AssignedUserID,
                        tickets.CreatedDate, tickets.CompletedDate, tickets.AssignedDate }, commandType: CommandType.Text);
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


        public async Task<bool> UpdateTicket(Tickets tickets)
        {


            await Task.Delay(1000);
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

UPDATE dbo.Tickets SET


      [EMAIL]          = ISNULL(@Email,''),
      [COMPANY]        = ISNULL(@Company,''),
      [FNAME]          = ISNULL(@Fname,''),
      [SNAME]          = ISNULL(@Sname,''),
      [PHONE]          = ISNULL(@Phone,''),
      [TASK]           = ISNULL(@Task,''),
      [INTERNALNOTE]   = ISNULL(@InternalNote,''),
      [ISNOTICED]      = @IsNoticed,
      [ISCOMPLETED]    = @IsCompleted,
      [ISDELETED]      = @IsDeleted,
      [ASSIGNEDUSERID] = @AssignedUserID,
      [CREATEDDATE]    = @CreatedDate,
      [COMPLETEDDATE]  = @CompletedDate,
      [ASSIGNEDDDATE]  = @AssignedDate

WHERE ID = @id
";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new
                    {
               
                        tickets.Email,
                        tickets.Company,
                        tickets.Fname,
                        tickets.Sname,
                        tickets.Phone,
                        tickets.Task,
                        tickets.InternalNote,
                        tickets.IsNoticed,
                        tickets.IsCompleted,
                        tickets.IsDeleted,
                        tickets.AssignedUserID,
                        tickets.CreatedDate,
                        tickets.CompletedDate,
                        tickets.AssignedDate,
                        tickets.Id
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
            //
            return true;

        }


        public async Task<bool> DeleteTicket(Guid id)
        {

            using (var conn = new SqlConnection(_configuration.Value))
            {

                const string query = @"

update dbo.Tickets set 

	[ISDELETED] = 1

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

        public async Task<bool> TicketIsNoticed(Guid id, bool isnoticed)
        {

            using (var conn = new SqlConnection(_configuration.Value))
            {

                const string query = @"

update dbo.Tickets set 

	[ISNOTICED] = @isnoticed

where Id=@Id";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new { id , isnoticed }, commandType: CommandType.Text);
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

        public async Task<bool> TicketIsCompleted(Guid id)
        {

            using (var conn = new SqlConnection(_configuration.Value))
            {

                const string query = @"

update dbo.Tickets set 

	[ISCOMPLETED] = 1,
    [COMPLETEDDATE]= GETDATE()

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

        public async Task<int> TicketTotalCount()
        {
            int i = 0;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select 
	  COUNT([ID])


from dbo.Tickets  ";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    i = await conn.QueryFirstOrDefaultAsync<int>(query, commandType: CommandType.Text);

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
            return i;
        }

        public async Task<List<Tickets>> GetDashboardTickets()
        {
            List<Tickets> users = new List<Tickets>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"select TOP 3 * from dbo.Tickets where ISDELETED = 0 AND ISNOTICED = 0  ORDER BY [CREATEDDATE] DESC";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    users = (await conn.QueryAsync<Tickets>(query)).ToList();

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
    }
}
