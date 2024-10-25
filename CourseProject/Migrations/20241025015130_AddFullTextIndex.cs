using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProject.Migrations
{
    /// <inheritdoc />
    public partial class AddFullTextIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT;",
                suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON Templates(Name) KEY INDEX PK_Templates;",
                suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
        }
    }
}
