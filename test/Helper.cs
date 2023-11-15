﻿using Dapper;
using Newtonsoft.Json;
using Npgsql;

public static class Helper
{
    public static readonly Uri Uri;
    public static readonly string ProperlyFormattedConnectionString;
    public static readonly NpgsqlDataSource DataSource;

    static Helper()
    {
        string rawConnectionString;
        string envVarKeyName = "pgconn";

        rawConnectionString = Environment.GetEnvironmentVariable(envVarKeyName)!;
        if (rawConnectionString == null)
        {
            throw new Exception($@"
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
YOUR CONN STRING PGCONN IS EMPTY.
Solution: Go to Settings, search for Test Runner, and add the environment variable in there.
Currently, your program looks for an environment variable is called {envVarKeyName}.

Best regards, Alex
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
");
        }

        try
        {
            Uri = new Uri(rawConnectionString);
            ProperlyFormattedConnectionString = string.Format(
                "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=3",
                Uri.Host,
                Uri.AbsolutePath.Trim('/'),
                Uri.UserInfo.Split(':')[0],
                Uri.UserInfo.Split(':')[1],
                Uri.Port > 0 ? Uri.Port : 5432);
            DataSource =
                new NpgsqlDataSourceBuilder(ProperlyFormattedConnectionString).Build();
            DataSource.OpenConnection().Close();
        }
        catch (Exception e)
        {
            throw new Exception($@"
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
Your connection string is found, but could not be used. Are you sure you correctly inserted
the connection-string to Postgres?

Best regards, Alex
(Below is the inner exception)
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨", e);
        }
    }

    public static void TriggerRebuild()
    {
        using (var conn = DataSource.OpenConnection())
        {
            try
            {
                conn.Execute(RebuildScript);
            }
            catch (Exception e)
            {
                throw new Exception($@"
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
THERE WAS AN ERROR REBUILDING THE DATABASE.

Check the following: Are you running the postgres DB at Amazon Web Services in Stockholm?

Best regards, Alex.
(Below is the inner exception)
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨", e);
            }
        }
    }

    public static string MyBecause(object actual, object expected)
    {
        string expectedJson = JsonConvert.SerializeObject(expected, Formatting.Indented);
        string actualJson = JsonConvert.SerializeObject(actual, Formatting.Indented);

        return $"because we want these objects to be equivalent:\nExpected:\n{expectedJson}\nActual:\n{actualJson}";
    }

    public static string RebuildScript = @"
-- Drop the 'users.user' table if it exists
DROP TABLE IF EXISTS users.user CASCADE;

-- Drop the 'password_hash' table if it exists
DROP TABLE IF EXISTS users.password_hash CASCADE;


-- Create the 'users.user' table with 'Email' as the primary key
CREATE TABLE users.user (
    Email VARCHAR(50) PRIMARY KEY,
    FullName VARCHAR(50) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    Created TIMESTAMP NOT NULL,
    ProfileUrl VARCHAR(100) NOT NULL
);

-- Create the 'users.password_hash' table with a foreign key reference to 'Email'
CREATE TABLE users.password_hash (
    user_email VARCHAR(50),
    hash VARCHAR(350) NOT NULL,
    salt VARCHAR(180) NOT NULL,
    algorithm VARCHAR(12) NOT NULL,
    FOREIGN KEY(user_email) REFERENCES users.user(Email)
);
 ";

    public static string NoResponseMessage = $@"
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
It looks like you failed to get a response from the API.
Are you 100% sure the API is already running on localhost port 5000?
Below is the inner exception.
Best regards, Alex
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
";
}
