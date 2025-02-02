using FluentMigrator;
using Itmo.Dev.Platform.Postgres.Migrations;

namespace Abstractions.Migration;

[Migration(1, "Initial")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider)
    {
        const string sqlUp = """
                             create table users
                             (
                                 name varchar(20) ,
                                 balance integer not null ,
                                 id integer primary key not null ,
                                 role integer not null ,
                                 password varchar(100)
                             );
                             """;

        return sqlUp;
    }

    protected override string GetDownSql(IServiceProvider serviceProvider)
    {
        const string sqlDown = """
                               drop table users;
                               """;

        return sqlDown;
    }
}