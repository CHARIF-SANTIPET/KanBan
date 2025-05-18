using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KanBan.Migrations
{
    /// <inheritdoc />
    public partial class nullableBoardCreatBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Users_CreatedBy",
                table: "Boards");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Boards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Users_CreatedBy",
                table: "Boards",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Users_CreatedBy",
                table: "Boards");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Boards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Users_CreatedBy",
                table: "Boards",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
