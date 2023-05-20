using Dapper;
using System.Data;
using System.Data.SqlClient;
using WebMud.Dapper;
using WebMud.Models;
using System.IO;


namespace WebMud.Data
{
    public class GalleryService: IGalleryService
    {



        private readonly SqlConnectionConfiguration _configuration;
        public GalleryService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<bool> AddGallery(Gallery gallery)
        {


            var id = Guid.NewGuid();
            gallery.Id = id;
            gallery.IsDeleted = false;

            //gallery.CreatedDate = DateTime.Now;


            await Task.Delay(500);
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

insert into dbo.Gallery
(
		[ID]
      ,[PATH]
      ,[INTERNALNOTE]
      ,[NOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[ISONWELCOME]
        ,[FOLDERLINK]
) 
values 
(

@Id,
ISNULL(@Path,''),
ISNULL(@InternalNote,''),
ISNULL(@Note,''),
@IsDeleted,
@IsActive,
@CreatorID,
@CreatedDate,
@IsOnWelcome,
@FolderLink

)


";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new
                    {
                        gallery.Id,
                        gallery.Path,
                        gallery.InternalNote,
                        gallery.Note,
                        gallery.IsDeleted,
                        gallery.IsActive,
                        gallery.CreatorID,
                        gallery.CreatedDate,
                        gallery.IsOnWelcome,
                        gallery.FolderLink



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




        public async Task<bool> DeleteGallery(Guid id, string file)
        {

            using (var conn = new SqlConnection(_configuration.Value))
            {

                const string query = @"

update dbo.Gallery set 

	[ISDELETED] = 1

where Id=@Id";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new { id }, commandType: CommandType.Text);

                    if (File.Exists(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot").ToString() + file))
                    {
                        File.Delete(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot").ToString() + file);
                    }




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


        public async Task<bool> ActivateGallery(Guid id, bool isactive)
        {

            using (var conn = new SqlConnection(_configuration.Value))
            {

                const string query = @"

update dbo.Gallery set 

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

        public async Task<Gallery> GetGallery(Guid id)
        {

            Gallery gallery = new Gallery();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select top 1 


		[ID]
      ,[PATH]
      ,[INTERNALNOTE]
      ,[NOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[ISONWELCOME]
        ,[FOLDERLINK]

from dbo.Gallery where ID = @id ";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    gallery = await conn.QueryFirstOrDefaultAsync<Gallery>(query, new { id }, commandType: CommandType.Text);

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
            return gallery;
        }


        public async Task<List<Gallery>> GetGalleryOnlyActiveAll()
        {

            List<Gallery> gallery = new List<Gallery>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select


		[ID]
      ,[PATH]
      ,[INTERNALNOTE]
      ,[NOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[ISONWELCOME]
        ,[FOLDERLINK]

from dbo.Gallery where ISACTIVE = 1 and ISDELETED = 0

ORDER BY [CREATEDDATE] DESC
";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    gallery = (await conn.QueryAsync<Gallery>(query)).ToList();

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
            return gallery;
        }

        public async Task<List<Gallery>> GetGalleryOnlyActiveWelcomeScr()
        {

            List<Gallery> gallery = new List<Gallery>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select


		[ID]
      ,[PATH]
      ,[INTERNALNOTE]
      ,[NOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[ISONWELCOME]
        ,[FOLDERLINK]

from dbo.Gallery where ISACTIVE = 1 and ISDELETED = 0 and ISONWELCOME = 1

ORDER BY [CREATEDDATE] DESC
";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    gallery = (await conn.QueryAsync<Gallery>(query)).ToList();

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
            return gallery;
        }

        public async Task<List<Gallery>> GetGalleryOnlyActiveAll(Guid linkuid)
        {

            List<Gallery> gallery = new List<Gallery>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select


		[ID]
      ,[PATH]
      ,[INTERNALNOTE]
      ,[NOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[ISONWELCOME]
        ,[FOLDERLINK]

from dbo.Gallery where ISACTIVE = 1 and ISDELETED = 0 and FOLDERLINK = @linkuid

ORDER BY [CREATEDDATE] DESC
";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    gallery = (await conn.QueryAsync<Gallery>(query, new {linkuid}, commandType: CommandType.Text)).ToList();

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
            return gallery;
        }



        public async Task<List<Gallery>> GetGalleryOnlyActive()
        {

            List<Gallery> gallery = new List<Gallery>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
SELECT TOP 12


		[ID]
      ,[PATH]
      ,[INTERNALNOTE]
      ,[NOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[ISONWELCOME]
        ,[FOLDERLINK] AS FolderLink

from dbo.Gallery where ISACTIVE = 1 and ISDELETED = 0

ORDER BY [CREATEDDATE] DESC
";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    gallery = (await conn.QueryAsync<Gallery>(query)).ToList();

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
            return gallery;
        }

        public async Task<List<Gallery>> GetGalleryAll()
        {

            List<Gallery> gallery = new List<Gallery>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select  



		[ID]
      ,[PATH]
      ,[INTERNALNOTE]
      ,[NOTE]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
        ,[ISONWELCOME]
        ,[FOLDERLINK] AS FolderLink

from dbo.Gallery where ISDELETED = 0

ORDER BY [CREATEDDATE] DESC
";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    gallery = (await conn.QueryAsync<Gallery>(query)).ToList();

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
            return gallery;
        }


        public async Task<int> GetTotalGalleryCount()
        {

            int count = 0;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select

	  COUNT([ID])

from dbo.Gallery where ISDELETED = 0";

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


        public async Task<bool> UpdateGallery(Gallery gallery)
        {


            await Task.Delay(2000);
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

UPDATE dbo.Gallery SET


       [PATH]               = ISNULL(@Path,'')
      ,[INTERNALNOTE]       = ISNULL(@InternalNote,'')
      ,[NOTE]               = ISNULL(@Note,'')
      ,[ISDELETED]          = @IsDeleted
      ,[ISACTIVE]           = @IsActive
      ,[CREATORID]          = @CreatorID
      ,[CREATEDDATE]        = @CreatedDate
      ,[ISONWELCOME]        = @IsOnWelcome
        ,[FOLDERLINK] = @FolderLink

WHERE ID = @id
";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new
                    {

             
                        gallery.Path,
                        gallery.InternalNote,
                        gallery.Note,
                        gallery.IsDeleted,
                        gallery.IsActive,
                        gallery.CreatorID,
                        gallery.CreatedDate,
                        gallery.IsOnWelcome,
                        gallery.FolderLink,
                        gallery.Id

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
