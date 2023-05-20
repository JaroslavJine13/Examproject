using Dapper;
using System.Data;
using System.Data.SqlClient;
using WebMud.Dapper;
using WebMud.Models;
using System.IO;


namespace WebMud.Data
{
    public class InsuranceService: IInsuranceService
    {



        private readonly SqlConnectionConfiguration _configuration;
        public InsuranceService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<bool> AddInsurance(Insurance Insurance)
        {


            var id = Guid.NewGuid();
            Insurance.Id = id;
            Insurance.IsDeleted = false;

            //Insurance.CreatedDate = DateTime.Now;


            await Task.Delay(500);
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

insert into dbo.Insurance
(
[ID]
      ,[TYPEENUM]
      ,[SUBJECT]
      ,[PRICE]
      ,[DATEFROM]
      ,[DATETO]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[CLIENTLINK]
) 
values 
(

@Id,
@TypeEnum,
ISNULL(@Subject,''),
@Price,
@DateFrom,
@DateTo,
@IsDeleted,
@IsActive,
@CreatorID,
@CreatedDate,
@ClientLink

)


";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new
                    {
                        Insurance.Id,
                        Insurance.TypeEnum,
                        Insurance.Subject,
                        Insurance.Price,
                        Insurance.DateFrom,
                        Insurance.DateTo,
                        Insurance.IsDeleted,
                        Insurance.IsActive,
                        Insurance.CreatorID,
                        Insurance.CreatedDate,
 
                        Insurance.ClientLink



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




        public async Task<bool> DeleteInsurance(Guid id )
        {

            using (var conn = new SqlConnection(_configuration.Value))
            {

                const string query = @"

update dbo.Insurance set 

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


        public async Task<bool> ActivateInsurance(Guid id, bool isactive)
        {

            using (var conn = new SqlConnection(_configuration.Value))
            {

                const string query = @"

update dbo.Insurance set 

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

        public async Task<Insurance> GetInsurance(Guid id)
        {

            Insurance Insurance = new Insurance();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select top 1 


[ID]
      ,[TYPEENUM]
      ,[SUBJECT]
      ,[PRICE]
      ,[DATEFROM]
      ,[DATETO]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[CLIENTLINK]

from dbo.Insurance where ID = @id ";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    Insurance = await conn.QueryFirstOrDefaultAsync<Insurance>(query, new { id }, commandType: CommandType.Text);

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
            return Insurance;
        }


        public async Task<List<Insurance>> GetInsuranceOnlyActiveAll()
        {

            List<Insurance> Insurance = new List<Insurance>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select


[ID]
      ,[TYPEENUM]
      ,[SUBJECT]
      ,[PRICE]
      ,[DATEFROM]
      ,[DATETO]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[CLIENTLINK]

from dbo.Insurance where ISACTIVE = 1 and ISDELETED = 0

ORDER BY [CREATEDDATE] DESC
";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    Insurance = (await conn.QueryAsync<Insurance>(query)).ToList();

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
            return Insurance;
        }

   

        public async Task<List<Insurance>> GetInsuranceOnlyActiveAll(Guid linkuid)
        {

            List<Insurance> Insurance = new List<Insurance>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select


[ID]
      ,[TYPEENUM]
      ,[SUBJECT]
      ,[PRICE]
      ,[DATEFROM]
      ,[DATETO]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[CLIENTLINK]

from dbo.Insurance where ISACTIVE = 1 and ISDELETED = 0 and CLIENTLINK = @linkuid

ORDER BY [CREATEDDATE] DESC
";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    Insurance = (await conn.QueryAsync<Insurance>(query, new {linkuid}, commandType: CommandType.Text)).ToList();

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
            return Insurance;
        }



   

        public async Task<List<Insurance>> GetInsuranceAll()
        {

            List<Insurance> Insurance = new List<Insurance>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select  



[ID]
      ,[TYPEENUM]
      ,[SUBJECT]
      ,[PRICE]
      ,[DATEFROM]
      ,[DATETO]
      ,[ISDELETED]
      ,[ISACTIVE]
      ,[CREATORID]
      ,[CREATEDDATE]
      ,[CLIENTLINK]

from dbo.Insurance where ISDELETED = 0

ORDER BY [CREATEDDATE] DESC
";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    Insurance = (await conn.QueryAsync<Insurance>(query)).ToList();

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
            return Insurance;
        }


        public async Task<int> GetTotalInsuranceCount()
        {

            int count = 0;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"
select

	  COUNT([ID])

from dbo.Insurance where ISDELETED = 0";

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


        public async Task<bool> UpdateInsurance(Insurance Insurance)
        {


            await Task.Delay(2000);
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

UPDATE dbo.Insurance SET





      [TYPEENUM]            = @TypeEnum
      ,[SUBJECT]        = ISNULL(@Subject,'')
      ,[PRICE]          = @Price
      ,[DATEFROM]       = @DateFrom
      ,[DATETO]         = @DateTo
      ,[ISDELETED]      = @IsDeleted
      ,[ISACTIVE]       = @IsActive
      ,[CREATORID]      = @CreatorID
      ,[CREATEDDATE]    = @CreatedDate
      ,[CLIENTLINK]     = @ClientLink

WHERE ID = @id
";
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync(query, new
                    {


                        Insurance.TypeEnum,
                        Insurance.Subject,
                        Insurance.Price,
                        Insurance.DateFrom,
                        Insurance.DateTo,
                        Insurance.IsDeleted,
                        Insurance.IsActive,
                        Insurance.CreatorID,
                        Insurance.CreatedDate,

                        Insurance.ClientLink,
                        Insurance.Id

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
