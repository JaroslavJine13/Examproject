using Dapper;
using MudBlazor;
using System.Data;
using System.Data.SqlClient;
using WebMud.Dapper;
using WebMud.Models;

namespace WebMud.Data
{
    public class VisitorsStatisticsService: IVisitorsStatisticsService
    {
        private readonly SqlConnectionConfiguration _configuration;
        public VisitorsStatisticsService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> NumberOfVisitors()
        {
            int i = 0;
            using (var conn = new SqlConnection(_configuration.Value))
            {


                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {

                    i = await conn.ExecuteScalarAsync<int>("SELECT COUNT(ID) FROM VisitorsLog");

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

        public async Task<int> NumberOfVisitorsToday()
        {
            int i = 0;
            using (var conn = new SqlConnection(_configuration.Value))
            {


                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {

                    i = await conn.ExecuteScalarAsync<int>("SELECT COUNT(ID) FROM VisitorsLog WHERE   CONVERT(VARCHAR(10),[DATEOFVISIT],110) = CONVERT(VARCHAR(10),GETDATE(),110)");

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


        public async Task<List<double>> GetArrayOfVisitsPerRange(DateRange dateRange)
        {

            List<double> interest = new List<double>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"

 


WITH calendar AS (
  SELECT CAST(@Start AS DateTime) as date -- replace with your desired start date
  UNION ALL
  SELECT DATEADD(day, 1, date)
  FROM calendar
  WHERE date < CAST(@End AS DateTime) -- replace with your desired end date
)
SELECT 

 
COALESCE(count_table.row_count, 0) as row_count

FROM calendar
LEFT JOIN (
  SELECT 
  CAST(DATEOFVISIT AS date) as date
  ,COUNT(COOKIEID) as row_count

  FROM VisitorsLog
    GROUP BY
    CAST(DATEOFVISIT AS date)

 

) count_table ON calendar.date = count_table.date
WHERE calendar.date BETWEEN @Start AND @End
OPTION (MAXRECURSION 0);

";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    interest = (await conn.QueryAsync<double>(query, new { dateRange.Start, dateRange.End }, commandType: CommandType.Text)).ToList();

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


        public async Task<List<DateTime>> GetVisits()
        {

            List<DateTime> val = new List<DateTime>();
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"select DATEOFVISIT from dbo.VisitorsLog  ";

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    val = (await conn.QueryAsync<DateTime>(query)).ToList();

      


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
            return val;
        }


    }
}
