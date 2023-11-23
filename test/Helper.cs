﻿using System.Net.Http.Json;
using Dapper;
using Newtonsoft.Json;
using Npgsql;

public static class Helper
{
    public static readonly Uri Uri;
    public static readonly string ProperlyFormattedConnectionString;
    public static readonly NpgsqlDataSource DataSource;
    private static HttpClient _httpClient;

    static Helper()
    {
        _httpClient = new HttpClient();
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
DROP TABLE IF EXISTS expenses.user_on_expense CASCADE;
DROP TABLE IF EXISTS expenses.expense CASCADE;
DROP SCHEMA IF EXISTS expenses CASCADE;

DROP TABLE IF EXISTS groups.group_members;
DROP TABLE IF EXISTS groups.group CASCADE;
DROP SCHEMA IF EXISTS groups CASCADE;

-- Drop the 'users.password_hash' table if it exists
DROP TABLE IF EXISTS users.password_hash;
-- Drop the 'users.user' table if it exists
DROP TABLE IF EXISTS users.user;
DROP SCHEMA IF EXISTS users;

-- Create the 'users' schema
CREATE SCHEMA users;

-- Create the 'users.user' table with 'Email' as the primary key
CREATE TABLE users.user (
    id SERIAL PRIMARY KEY,
    email VARCHAR(50) UNIQUE,
    full_name VARCHAR(50) NOT NULL,
    phone_number VARCHAR(15) NOT NULL,
    created TIMESTAMP NOT NULL,
    profile_url VARCHAR(100) NOT NULL
);

-- Create the 'password_hash' table with a foreign key reference to 'Email'
CREATE TABLE users.password_hash (
    user_id int PRIMARY KEY,
    hash VARCHAR(350) NOT NULL,
    salt VARCHAR(180) NOT NULL,
    algorithm VARCHAR(12) NOT NULL,
    FOREIGN KEY(user_id) REFERENCES users.user(id)
);

-- Create the groups schema
CREATE SCHEMA groups;

-- Create the 'groups.group' table
CREATE TABLE groups.group (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(200) NOT NULL,
    image_url VARCHAR(100) NOT NULL,
    created_date TIMESTAMP NOT NULL
);

-- Create the 'groups.group_members' table with foreign key references to users.user & groups.group tables
CREATE TABLE groups.group_members (
    user_id INT NOT NULL,
    group_id INT NOT NULL,
    owner BOOLEAN NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users.user(id),
    FOREIGN KEY (group_id) REFERENCES groups.group(id),
    PRIMARY KEY (user_id, group_id)
);

-- Create the expenses schema
CREATE SCHEMA expenses;

-- Create the 'expenses.expense' table with foreign key references to users.user & groups.group.
CREATE TABLE expenses.expense (
    id SERIAL PRIMARY KEY,
    group_id INT NOT NULL,
    description VARCHAR(200) NOT NULL,
    amount MONEY NOT NULL,
    created_date TIMESTAMP NOT NULL,
    FOREIGN KEY (group_id) REFERENCES groups.group(id)
);

-- Create the 'expenses.user_on_expense' table
CREATE TABLE expenses.user_on_expense (
    user_id INT NOT NULL,
    expense_id INT NOT NULL,
    payer BOOLEAN NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users.user(id),
    FOREIGN KEY (expense_id) REFERENCES expenses.expense(id),
    PRIMARY KEY (user_id, expense_id)
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

    public static async Task<string?> Authorize(string email)
    {
        //Register
        var registration = new
        {
            Email = email,
            FullName = "fullName",
            Password = "password",
            PhoneNumber = "12345678",
            Created = DateTime.Now,
            ProfileUrl = "https://cdn-icons-png.flaticon.com/512/615/615075.png"
        };

        string urlReg = "http://localhost:5100/api/account/register";

        try
        {
            await _httpClient.PostAsJsonAsync(urlReg, registration);
        }
        catch (Exception e)
        {
            throw new Exception(NoResponseMessage, e);
        }

        //Login
        var login = new
        {
            Email = email,
            Password = "password",
        };

        string urlLogin = "http://localhost:5100/api/account/login";
        HttpResponseMessage response;

        try
        {
            response = await _httpClient.PostAsJsonAsync(urlLogin, login);

            var body = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Response>(body);
            var token = data.token;
            return token;
        }
        catch (Exception e)
        {
            throw new Exception(NoResponseMessage, e);
        }
    }

    public class Response
    {
        public string? token { get; set; }
    }

    public static void RunScript(string script)
    {
        using var conn = DataSource.OpenConnection();
        try
        {
            conn.Execute(script);
        }
        catch (Exception e)
        {
            throw new Exception($@"THERE WAS AN ERROR RUNNING THE SCRIPT: " + script, e);
        }
    }
    
    public static string ExpensesScript = @"
insert into users.user (email, full_name, phone_number, created, profile_url) VALUES ('user2@example.com', 'string', '12341234', '2023-11-21 10:48:24.584797', 'https://cdn-icons-png.flaticon.com/512/615/615075.png');

insert into groups.group (id, name, description, image_url, created_date) VALUES (1, 'Studiegruppen', 'description', 'https://cdn-icons-png.flaticon.com/512/615/615075.png', '2023-11-21 10:48:24.584797');
insert into groups.group (id, name, description, image_url, created_date) VALUES (2, 'Weekend tur', 'description', 'https://cdn-icons-png.flaticon.com/512/615/615075.png', '2023-11-21 10:48:24.584797');

insert into groups.group_members (user_id, group_id, owner) VALUES (1, 1, true);
insert into groups.group_members (user_id, group_id, owner) VALUES (2, 1, false);
insert into groups.group_members (user_id, group_id, owner) VALUES (2, 2, true);

insert into expenses.expense (id, group_id, description, amount, created_date) values (1, 1, 'Første omgang', 40, '2023-11-21 10:48:24.584797');
insert into expenses.expense (id, group_id, description, amount, created_date) values (2, 1, 'Bare lige en mere bajs', 40, '2023-11-21 10:48:24.584797');
insert into expenses.expense (id, group_id, description, amount, created_date) values (3, 2, 'Sidste øl', 40, '2023-11-21 10:48:24.584797');
insert into expenses.expense (id, group_id, description, amount, created_date) values (4, 2, 'ALLERSIDSTE', 40, '2023-11-21 10:48:24.584797');

insert into expenses.user_on_expense (user_id, expense_id, payer) values (1, 1, true);
insert into expenses.user_on_expense (user_id, expense_id, payer) values (2, 1, false);
insert into expenses.user_on_expense (user_id, expense_id, payer) values (1, 2, false);
insert into expenses.user_on_expense (user_id, expense_id, payer) values (2, 2, true);
insert into expenses.user_on_expense (user_id, expense_id, payer) values (2, 3, true);
insert into expenses.user_on_expense (user_id, expense_id, payer) values (2, 4, true);

";
}