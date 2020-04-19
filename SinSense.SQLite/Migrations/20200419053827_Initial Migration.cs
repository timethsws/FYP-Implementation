using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SinSense.SQLite.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Language = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordRelations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FromWordId = table.Column<Guid>(nullable: false),
                    ToWordId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordRelations_Words_FromWordId",
                        column: x => x.FromWordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordRelations_Words_ToWordId",
                        column: x => x.ToWordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordRelations_FromWordId",
                table: "WordRelations",
                column: "FromWordId");

            migrationBuilder.CreateIndex(
                name: "IX_WordRelations_ToWordId",
                table: "WordRelations",
                column: "ToWordId");

            migrationBuilder.CreateIndex(
                name: "IX_WordRelations_Type",
                table: "WordRelations",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_WordRelations_Type_FromWordId_ToWordId",
                table: "WordRelations",
                columns: new[] { "Type", "FromWordId", "ToWordId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Words_Language_Text",
                table: "Words",
                columns: new[] { "Language", "Text" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordRelations");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
