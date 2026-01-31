using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathTestSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SubmittedResult",
                table: "TaskResults",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "Exams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskResults_MathTaskId",
                table: "TaskResults",
                column: "MathTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskResults_MathTasks_MathTaskId",
                table: "TaskResults",
                column: "MathTaskId",
                principalTable: "MathTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskResults_MathTasks_MathTaskId",
                table: "TaskResults");

            migrationBuilder.DropIndex(
                name: "IX_TaskResults_MathTaskId",
                table: "TaskResults");

            migrationBuilder.DropColumn(
                name: "SubmittedResult",
                table: "TaskResults");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "Exams",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
