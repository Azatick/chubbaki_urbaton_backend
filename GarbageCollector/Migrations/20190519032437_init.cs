using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GarbageCollector.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS postgis;");
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    CurrentLocation_Address = table.Column<string>(nullable: true),
                    CurrentLocation_Coordinates = table.Column<IPoint>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WasteCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Material = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WasteTakePoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Location_Address = table.Column<string>(nullable: true),
                    Location_Coordinates = table.Column<IPoint>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteTakePoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrashCanDbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    WasteTakePointId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrashCanDbo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrashCanDbo_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrashCanDbo_WasteTakePoints_WasteTakePointId",
                        column: x => x.WasteTakePointId,
                        principalTable: "WasteTakePoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WasteTakePointToCategoryLinkDbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WasteTakePointId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteTakePointToCategoryLinkDbo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WasteTakePointToCategoryLinkDbo_WasteCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "WasteCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WasteTakePointToCategoryLinkDbo_WasteTakePoints_WasteTakePo~",
                        column: x => x.WasteTakePointId,
                        principalTable: "WasteTakePoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrashCanToCategoryLinkDbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrashCanId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrashCanToCategoryLinkDbo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrashCanToCategoryLinkDbo_WasteCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "WasteCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrashCanToCategoryLinkDbo_TrashCanDbo_TrashCanId",
                        column: x => x.TrashCanId,
                        principalTable: "TrashCanDbo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrashCanDbo_UserId",
                table: "TrashCanDbo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrashCanDbo_WasteTakePointId",
                table: "TrashCanDbo",
                column: "WasteTakePointId");

            migrationBuilder.CreateIndex(
                name: "IX_TrashCanToCategoryLinkDbo_CategoryId",
                table: "TrashCanToCategoryLinkDbo",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrashCanToCategoryLinkDbo_TrashCanId",
                table: "TrashCanToCategoryLinkDbo",
                column: "TrashCanId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteTakePointToCategoryLinkDbo_CategoryId",
                table: "WasteTakePointToCategoryLinkDbo",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteTakePointToCategoryLinkDbo_WasteTakePointId",
                table: "WasteTakePointToCategoryLinkDbo",
                column: "WasteTakePointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrashCanToCategoryLinkDbo");

            migrationBuilder.DropTable(
                name: "WasteTakePointToCategoryLinkDbo");

            migrationBuilder.DropTable(
                name: "TrashCanDbo");

            migrationBuilder.DropTable(
                name: "WasteCategories");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "WasteTakePoints");
        }
    }
}
