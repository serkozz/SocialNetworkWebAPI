using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Src.Migrations
{
    /// <inheritdoc />
    public partial class AddedFriendsTable_V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_FriendsList_FriendsListId",
                table: "Profile");

            migrationBuilder.DropTable(
                name: "FriendsList");

            migrationBuilder.DropIndex(
                name: "IX_Profile_FriendsListId",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "FriendsListId",
                table: "Profile");

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstProfileId = table.Column<int>(type: "integer", nullable: false),
                    SecondProfileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friends_Profile_FirstProfileId",
                        column: x => x.FirstProfileId,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friends_Profile_SecondProfileId",
                        column: x => x.SecondProfileId,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FirstProfileId",
                table: "Friends",
                column: "FirstProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_SecondProfileId",
                table: "Friends",
                column: "SecondProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.AddColumn<int>(
                name: "FriendsListId",
                table: "Profile",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FriendsList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendsList", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profile_FriendsListId",
                table: "Profile",
                column: "FriendsListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_FriendsList_FriendsListId",
                table: "Profile",
                column: "FriendsListId",
                principalTable: "FriendsList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
