using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectEditor.Migrations
{
    public partial class AddNonclusteredIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Task_Priority",
                table: "Task",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CompletionDate",
                table: "Project",
                column: "CompletionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Priority",
                table: "Project",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Project_StartDate",
                table: "Project",
                column: "StartDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Task_Priority",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Project_CompletionDate",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_Priority",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_StartDate",
                table: "Project");
        }
    }
}
