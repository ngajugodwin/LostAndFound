using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LostAndFound_API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBookmark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserItemBookmarks",
                columns: table => new
                {
                    BookmarkByUserId = table.Column<long>(type: "bigint", nullable: false),
                    ItemBookmarkId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItemBookmarks", x => new { x.BookmarkByUserId, x.ItemBookmarkId });
                    table.ForeignKey(
                        name: "FK_UserItemBookmarks_AspNetUsers_BookmarkByUserId",
                        column: x => x.BookmarkByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserItemBookmarks_Items_ItemBookmarkId",
                        column: x => x.ItemBookmarkId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserItemBookmarks_ItemBookmarkId",
                table: "UserItemBookmarks",
                column: "ItemBookmarkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserItemBookmarks");
        }
    }
}
