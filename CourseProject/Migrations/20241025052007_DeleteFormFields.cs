using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProject.Migrations
{
    /// <inheritdoc />
    public partial class DeleteFormFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Forms");

            migrationBuilder.AddColumn<int>(
                name: "FormId",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 24, 22, 20, 7, 87, DateTimeKind.Local).AddTicks(7681));

            migrationBuilder.UpdateData(
                table: "Templates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 24, 22, 20, 7, 87, DateTimeKind.Local).AddTicks(7635));

            migrationBuilder.CreateIndex(
                name: "IX_Answers_FormId",
                table: "Answers",
                column: "FormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Forms_FormId",
                table: "Answers",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Forms_FormId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_FormId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "Answers");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 24, 18, 51, 29, 559, DateTimeKind.Local).AddTicks(6673));

            migrationBuilder.UpdateData(
                table: "Templates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 24, 18, 51, 29, 559, DateTimeKind.Local).AddTicks(6627));
        }
    }
}
