namespace WebMud.Dapper
{
    public class SqlConnectionConfiguration
    {


        public SqlConnectionConfiguration(string value) => Value = value;
        public string Value { get; }
    }
}
