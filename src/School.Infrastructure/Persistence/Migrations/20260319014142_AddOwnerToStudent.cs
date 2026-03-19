using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerUserId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Students_OwnerUserId",
                table: "Students",
                column: "OwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Users_OwnerUserId",
                table: "Students",
                column: "OwnerUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Users_OwnerUserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_OwnerUserId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "Students");
        }
    }
}
