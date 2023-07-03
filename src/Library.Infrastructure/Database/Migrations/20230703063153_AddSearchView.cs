using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW LibrarySearchView AS
                SELECT b.ID as BookID, CONCAT(a.FirstName, ' ', a.LastName) as Author, b.Title , b.Description, CONCAT(u.FirstName, ' ', u.LastName) as HoldingUser
                FROM dbo.Book b
                Join Author a on a.ID = b.AuthorID
                Join BooksTaken bt on bt.BookID = b.ID
                Join dbo.[User] u on u.ID = bt.UserID
                Group by b.ID, CONCAT(a.FirstName, ' ', a.LastName), b.Title , b.Description, CONCAT(u.FirstName, ' ', u.LastName)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW LibrarySearch");
        }
    }
}
