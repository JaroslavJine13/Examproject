using Dapper;
using System.Data;
using System.Data.SqlClient;
using WebMud.Dapper;
using WebMud.Models;

namespace WebMud.Data
{
    public class FolderService: IFolderService
    {




            private readonly SqlConnectionConfiguration _configuration;
            public FolderService(SqlConnectionConfiguration configuration)
            {
                _configuration = configuration;
            }


            public async Task<bool> AddFolder(Folder interest)
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

insert into dbo.Folder
(
        [ID]
      ,[NAME]
      ,[INTERNALNOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[NOTE]
) 
values 
(
@Id,
ISNULL(@Name,''),
ISNULL(@InternalNote,''),
@IsDeleted,
@IsActive,
@CreatorID,
@CreatedDate,
@Note

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
                            interest.InternalNote,
                            interest.IsDeleted,
                            interest.IsActive,
                            interest.CreatorID,
                            interest.CreatedDate,
                            interest.Note,




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



            public async Task<bool> DeleteFolder(Guid id)
            {

                using (var conn = new SqlConnection(_configuration.Value))
                {

                    const string query = @"

update dbo.Folder set 

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


            public async Task<bool> ActivateFolder(Guid id, bool isactive)
            {

                using (var conn = new SqlConnection(_configuration.Value))
                {

                    const string query = @"

update dbo.Folder set 

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

            public async Task<Folder> GetFolder(Guid id)
            {

            Folder interest = new Folder();
                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"
select top 1 


		[ID]
      ,[NAME]
      ,[INTERNALNOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[NOTE]

from dbo.Folder where ID = @id ";

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                    interest = await conn.QueryFirstOrDefaultAsync<Folder>(query, new { id }, commandType: CommandType.Text);

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


            public async Task<List<Folder>> GetFolderOnlyActive()
            {

                List<Folder> interest = new List<Folder>();
                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"
select


		[ID]
      ,[NAME]
      ,[INTERNALNOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[NOTE]

from dbo.Folder where ISACTIVE = 1 and ISDELETED = 0

ORDER BY [CREATEDDATE] DESC
";

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                    interest = (await conn.QueryAsync<Folder>(query)).ToList();

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


            public async Task<List<Folder>> GetFolderAll()
            {

                List<Folder> interest = new List<Folder>();
                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"
select  


		[ID]
      ,[NAME]
      ,[INTERNALNOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[NOTE]

from dbo.Folder where ISDELETED = 0

ORDER BY [CREATEDDATE] DESC
";

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                    interest = (await conn.QueryAsync<Folder>(query)).ToList();

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


            public async Task<int> GetTotalFolderCount()
            {

                int  count = 0;
                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"
select

	  COUNT([ID])

from dbo.Folder where ISDELETED = 0";

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


            public async Task<bool> UpdateFolder(Folder interest)
            {


                await Task.Delay(1500);
                using (var conn = new SqlConnection(_configuration.Value))
                {
                    const string query = @"

UPDATE dbo.Folder SET



      [NAME]               = ISNULL(@Name,'')
      ,[INTERNALNOTE]       = @InternalNote
      ,[ISDELETED]          = @IsDeleted
      ,[ISACTIVE]           = @IsActive
      ,[CREATEDDATE]        = @CreatedDate
      ,[NOTE]               = @Note

WHERE ID = @id
";
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                        await conn.ExecuteAsync(query, new
                        {

                            interest.Name,
                            interest.InternalNote,
                            interest.IsDeleted,
                            interest.IsActive,
                            interest.CreatedDate,
                            interest.Note,

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
