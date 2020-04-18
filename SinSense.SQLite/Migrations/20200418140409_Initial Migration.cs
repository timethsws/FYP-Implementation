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
                    Text = table.Column<string>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    Language = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Text);
                });

            migrationBuilder.CreateTable(
                name: "WordRelations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FromWordText = table.Column<string>(nullable: true),
                    FromWordId = table.Column<Guid>(nullable: false),
                    ToWordText = table.Column<string>(nullable: true),
                    ToWordId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordRelations_Words_FromWordText",
                        column: x => x.FromWordText,
                        principalTable: "Words",
                        principalColumn: "Text",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WordRelations_Words_ToWordText",
                        column: x => x.ToWordText,
                        principalTable: "Words",
                        principalColumn: "Text",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordRelations_FromWordId",
                table: "WordRelations",
                column: "FromWordId");

            migrationBuilder.CreateIndex(
                name: "IX_WordRelations_FromWordText",
                table: "WordRelations",
                column: "FromWordText");

            migrationBuilder.CreateIndex(
                name: "IX_WordRelations_ToWordId",
                table: "WordRelations",
                column: "ToWordId");

            migrationBuilder.CreateIndex(
                name: "IX_WordRelations_ToWordText",
                table: "WordRelations",
                column: "ToWordText");

            migrationBuilder.CreateIndex(
                name: "IX_WordRelations_Type",
                table: "WordRelations",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Words_Language",
                table: "Words",
                column: "Language");
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
