using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GarbageCollector.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Coordinates = table.Column<IPoint>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    CurrentLocationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_Locations_CurrentLocationId",
                        column: x => x.CurrentLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WasteTakePoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LocationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteTakePoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WasteTakePoints_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrashCan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WasteTakePointId = table.Column<Guid>(nullable: true),
                    GarbageAppUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrashCan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrashCan_AppUsers_GarbageAppUserId",
                        column: x => x.GarbageAppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrashCan_WasteTakePoints_WasteTakePointId",
                        column: x => x.WasteTakePointId,
                        principalTable: "WasteTakePoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WasteCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Material = table.Column<int>(nullable: false),
                    TrashCanId = table.Column<Guid>(nullable: true),
                    WasteTakePointId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WasteCategories_TrashCan_TrashCanId",
                        column: x => x.TrashCanId,
                        principalTable: "TrashCan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WasteCategories_WasteTakePoints_WasteTakePointId",
                        column: x => x.WasteTakePointId,
                        principalTable: "WasteTakePoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_CurrentLocationId",
                table: "AppUsers",
                column: "CurrentLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TrashCan_GarbageAppUserId",
                table: "TrashCan",
                column: "GarbageAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrashCan_WasteTakePointId",
                table: "TrashCan",
                column: "WasteTakePointId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteCategories_TrashCanId",
                table: "WasteCategories",
                column: "TrashCanId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteCategories_WasteTakePointId",
                table: "WasteCategories",
                column: "WasteTakePointId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteTakePoints_LocationId",
                table: "WasteTakePoints",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WasteCategories");

            migrationBuilder.DropTable(
                name: "TrashCan");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "WasteTakePoints");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
