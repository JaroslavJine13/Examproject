using Dapper;

using System.Data;
using System.Data.SqlClient;
using WebMud.Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace WebMud.Data
{
    public class VisitorsCounterService
    {


        private readonly RequestDelegate _requestDelegate;

        public VisitorsCounterService(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }



        public async Task Invoke(HttpContext context)
        {
            string json = System.IO.File.ReadAllText("appsettings.json");
            var data = JObject.Parse(json);
            var connectionstring = data["ConnectionStrings"]["DefaultConnection"].ToString();


            string visitorId = context.Request.Cookies["VisitorId"];
            if (visitorId == null)
            {


                string guid = Guid.NewGuid().ToString();

                //Console.WriteLine(connectionstring);
                using (var conn = new SqlConnection(connectionstring))
                {


                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    try
                    {
                        await conn.ExecuteScalarAsync(@"
                        

INSERT INTO [dbo].[VisitorsLog]
(

[ID],
[DATEOFVISIT], 
[COOKIEID]

)
VALUES
(
NEWID(),
GETDATE(),
@guid

)
                        
                        ", new { guid }, commandType: CommandType.Text);
                        //AdminSettings.VisitorsCounter = await conn.ExecuteScalarAsync<int>("SELECT TOP 1 VISITORSCOUNTER FROM Settings;");


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




                context.Response.Cookies.Append("VisitorId", guid, new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                    Expires = DateTime.Now.AddDays(1)
                });
                //context.Abort();
            }

            await _requestDelegate(context);
        }
    }
}
