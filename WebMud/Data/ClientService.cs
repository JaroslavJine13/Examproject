using Dapper;
using System.Data;
using System.Data.SqlClient;
using WebMud.Dapper;
using WebMud.Models;

namespace WebMud.Data
{
    public class ClientService: IClientService
    {




            private readonly SqlConnectionConfiguration _configuration;
            public ClientService(SqlConnectionConfiguration configuration)
            {
                _configuration = configuration;

            }


            public async Task<bool> AddClient(Client interest)
            {




            interest.IsDeleted = false;
            if(interest.CreatedDate == null)
            { 
            interest.CreatedDate = DateTime.Now;
            }

            await Task.Delay(500);
                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"

insert into dbo.Client
(
[ID]
      ,[NAME]
      ,[SURNAME]
      ,[EMAIL]
      ,[ADRESS]
      ,[PHONE]
      ,[CITY]
      ,[PSC]
      ,[INTERNALNOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
) 
values 
(
@Id,
ISNULL(@Name,''),
ISNULL(@Surname,''),
ISNULL(@Email,''),
ISNULL(@Adress,''),
ISNULL(@Phone,''),
ISNULL(@City,''),
ISNULL(@PSC,''),
ISNULL(@InternalNote,''),
@IsDeleted,
@IsActive,
@CreatorID,
@CreatedDate

)


";
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                        await conn.ExecuteAsync(query, new
                        {
                            interest.Id,
                            interest.Name,
                            interest.Surname,
                            interest.Email,
                            interest.Adress,
                            interest.Phone,
                            interest.City,
                            interest.PSC,
                            interest.InternalNote,
                            interest.IsDeleted,
                            interest.IsActive,
                            interest.CreatorID,
                            interest.CreatedDate,
    



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



            public async Task<bool> DeleteClient(Guid id)
            {

                using (var conn = new SqlConnection(_configuration.Value))
                {

                    const string query = @"

update dbo.Client set 

	[ISDELETED] = 1

where Id=@Id



update dbo.Insurance set 

	[ISDELETED] = 1

where [CLIENTLINK] = @Id


";
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


            public async Task<bool> ActivateClient(Guid id, bool isactive)
            {

                using (var conn = new SqlConnection(_configuration.Value))
                {

                    const string query = @"

update dbo.Client set 

	[ISACTIVE] = @isactive

where Id=@Id 

";
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                        await conn.ExecuteAsync(query, new { id, isactive }, commandType: CommandType.Text);
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

            public async Task<Client> GetClient(Guid id)
            {

            Client interest = new Client();
                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"
select top 1 


[ID]
      ,[NAME]
      ,[SURNAME]
      ,[EMAIL]
      ,[ADRESS]
      ,[PHONE]
      ,[CITY]
      ,[PSC]
      ,[INTERNALNOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]

from dbo.Client where ID = @id ";

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                    interest = await conn.QueryFirstOrDefaultAsync<Client>(query, new { id }, commandType: CommandType.Text);

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
                return interest;
            }


            public async Task<List<Client>> GetClientOnlyActive()
            {

                List<Client> interest = new List<Client>();
                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"
select


[ID]
      ,[NAME]
      ,[SURNAME]
      ,[EMAIL]
      ,[ADRESS]
      ,[PHONE]
      ,[CITY]
      ,[PSC]
      ,[INTERNALNOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]

from dbo.Client where ISACTIVE = 1 and ISDELETED = 0

ORDER BY [CREATEDDATE] DESC
";

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                    interest = (await conn.QueryAsync<Client>(query)).ToList();

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
                return interest;
            }


            public async Task<List<Client>> GetClientAll()
            {

                List<Client> interest = new List<Client>();
                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"
select  


[ID]
      ,[NAME]
      ,[SURNAME]
      ,[EMAIL]
      ,[ADRESS]
      ,[PHONE]
      ,[CITY]
      ,[PSC]
      ,[INTERNALNOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]

from dbo.Client where ISDELETED = 0

ORDER BY [CREATEDDATE] DESC
";

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                    interest = (await conn.QueryAsync<Client>(query)).ToList();

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
                return interest;
            }


            public async Task<int> GetTotalClientCount()
            {

                int  count = 0;
                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"
select

	  COUNT([ID])

from dbo.Client where ISDELETED = 0";

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                         count = await conn.QueryFirstOrDefaultAsync<int>(query, commandType: CommandType.Text);

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
                return count;
            }


            public async Task<bool> UpdateClient(Client interest)
            {


                await Task.Delay(1500);
                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"

UPDATE dbo.Client SET



 
      [NAME]           = ISNULL(@Name,'')
      ,[SURNAME]        = ISNULL(@Surname,'')
      ,[EMAIL]          = ISNULL(@Email,'')
      ,[ADRESS]         = ISNULL(@Adress,'')
      ,[PHONE]          = ISNULL(@Phone,'')
      ,[CITY]           = ISNULL(@City,'')
      ,[PSC]            = ISNULL(@PSC,'')
      ,[INTERNALNOTE]   = ISNULL(@InternalNote,'')
      ,[ISDELETED]      = @IsDeleted
      ,[ISACTIVE]       = @IsActive
      ,[CREATORID]      = @CreatorID
      ,[CREATEDDATE]    = @CreatedDate



WHERE ID = @id
";
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                        await conn.ExecuteAsync(query, new
                        {

                            interest.Name,
                            interest.Surname,
                            interest.Email,
                            interest.Adress,
                            interest.Phone,
                            interest.City,
                            interest.PSC,
                            interest.InternalNote,
                            interest.IsDeleted,
                            interest.IsActive,
                            interest.CreatorID,
                            interest.CreatedDate,

                            interest.Id
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

        }
}
