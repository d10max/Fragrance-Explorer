using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FragranceExplorer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeederPrep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PerfumeNotes",
                table: "PerfumeNotes");

            migrationBuilder.AddColumn<List<int>>(
                name: "SimilarPerfumeIds",
                table: "Perfumes",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Count",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DnaBadge",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DossierEyebrow",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Initials",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewsLabel",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Brands",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PerfumeNotes",
                table: "PerfumeNotes",
                columns: new[] { "PerfumeId", "NoteId", "Layer" });

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Slug",
                table: "Brands",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PerfumeNotes",
                table: "PerfumeNotes");

            migrationBuilder.DropIndex(
                name: "IX_Brands_Slug",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "SimilarPerfumeIds",
                table: "Perfumes");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DnaBadge",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DossierEyebrow",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Initials",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "ReviewsLabel",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Brands");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PerfumeNotes",
                table: "PerfumeNotes",
                columns: new[] { "PerfumeId", "NoteId" });
        }
    }
}
