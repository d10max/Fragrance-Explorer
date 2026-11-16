using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FragranceExplorer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    OriginLabel = table.Column<string>(type: "text", nullable: true),
                    EstablishedLabel = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ArchiveCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Rating = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    SimilarHouseIds = table.Column<List<string>>(type: "text[]", nullable: false),
                    BrandAliases = table.Column<List<string>>(type: "text[]", nullable: false),
                    FamilyCounts = table.Column<string>(type: "text", nullable: false),
                    OlfactoryDna = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perfumes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    BrandId = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Rating = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    Slug = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    People = table.Column<int>(type: "integer", nullable: true),
                    Perfumers = table.Column<List<string>>(type: "text[]", nullable: false),
                    Longevity = table.Column<double>(type: "double precision", nullable: true),
                    Sillage = table.Column<double>(type: "double precision", nullable: true),
                    BestSeasons = table.Column<string>(type: "text", nullable: true),
                    VibeQuote = table.Column<string>(type: "text", nullable: true),
                    Family = table.Column<string>(type: "text", nullable: true),
                    Concentration = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Perfumes_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PerfumeAccords",
                columns: table => new
                {
                    AccordId = table.Column<int>(type: "integer", nullable: false),
                    PerfumeId = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfumeAccords", x => new { x.PerfumeId, x.AccordId });
                    table.ForeignKey(
                        name: "FK_PerfumeAccords_Accords_AccordId",
                        column: x => x.AccordId,
                        principalTable: "Accords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerfumeAccords_Perfumes_PerfumeId",
                        column: x => x.PerfumeId,
                        principalTable: "Perfumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerfumeNotes",
                columns: table => new
                {
                    NoteId = table.Column<int>(type: "integer", nullable: false),
                    PerfumeId = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    Layer = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfumeNotes", x => new { x.PerfumeId, x.NoteId });
                    table.ForeignKey(
                        name: "FK_PerfumeNotes_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerfumeNotes_Perfumes_PerfumeId",
                        column: x => x.PerfumeId,
                        principalTable: "Perfumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accords_Name",
                table: "Accords",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Name",
                table: "Brands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_Name",
                table: "Notes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerfumeAccords_AccordId",
                table: "PerfumeAccords",
                column: "AccordId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfumeNotes_NoteId",
                table: "PerfumeNotes",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Perfumes_BrandId",
                table: "Perfumes",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Perfumes_Name",
                table: "Perfumes",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PerfumeAccords");

            migrationBuilder.DropTable(
                name: "PerfumeNotes");

            migrationBuilder.DropTable(
                name: "Accords");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Perfumes");

            migrationBuilder.DropTable(
                name: "Brands");
        }
    }
}
