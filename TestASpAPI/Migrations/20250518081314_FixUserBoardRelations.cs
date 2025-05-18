using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestASpAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixUserBoardRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Users_CreatedBy",
                table: "Boards");

            
            //migrationBuilder.DropIndex(
            //    name: "IX_BoardMembers_UserId1",
            //    table: "BoardMembers");

            //migrationBuilder.DropColumn(
            //    name: "UserId1",
            //    table: "BoardMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Users_CreatedBy",
                table: "Boards",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Users_CreatedBy",
                table: "Boards");

            //migrationBuilder.AddColumn<int>(
            //    name: "UserId1",
            //    table: "BoardMembers",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_BoardMembers_UserId1",
            //    table: "BoardMembers",
            //    column: "UserId1");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_BoardMembers_Users_UserId1",
            //    table: "BoardMembers",
            //    column: "UserId1",
            //    principalTable: "Users",
            //    principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Users_CreatedBy",
                table: "Boards",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
