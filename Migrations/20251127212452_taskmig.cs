using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramma_Ganta.Migrations
{
    /// <inheritdoc />
    public partial class taskmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskDependency_Tasks_PredecessorId",
                table: "TaskDependency");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskDependency_Tasks_TaskId",
                table: "TaskDependency");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskDependency",
                table: "TaskDependency");

            migrationBuilder.RenameTable(
                name: "TaskDependency",
                newName: "TaskDependencies");

            migrationBuilder.RenameIndex(
                name: "IX_TaskDependency_PredecessorId",
                table: "TaskDependencies",
                newName: "IX_TaskDependencies_PredecessorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskDependencies",
                table: "TaskDependencies",
                columns: new[] { "TaskId", "PredecessorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskDependencies_Tasks_PredecessorId",
                table: "TaskDependencies",
                column: "PredecessorId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskDependencies_Tasks_TaskId",
                table: "TaskDependencies",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskDependencies_Tasks_PredecessorId",
                table: "TaskDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskDependencies_Tasks_TaskId",
                table: "TaskDependencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskDependencies",
                table: "TaskDependencies");

            migrationBuilder.RenameTable(
                name: "TaskDependencies",
                newName: "TaskDependency");

            migrationBuilder.RenameIndex(
                name: "IX_TaskDependencies_PredecessorId",
                table: "TaskDependency",
                newName: "IX_TaskDependency_PredecessorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskDependency",
                table: "TaskDependency",
                columns: new[] { "TaskId", "PredecessorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskDependency_Tasks_PredecessorId",
                table: "TaskDependency",
                column: "PredecessorId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskDependency_Tasks_TaskId",
                table: "TaskDependency",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
