using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.DAL.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "publishers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publishers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "types",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "writings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_writings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "literature",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Isbn = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    NumberOfPages = table.Column<int>(type: "int", nullable: true),
                    PublishingYear = table.Column<int>(type: "int", nullable: true),
                    PublisherId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorId = table.Column<long>(type: "bigint", nullable: false),
                    GenreId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    IsLendable = table.Column<bool>(type: "bit", nullable: true),
                    LendPeriodInDays = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_literature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_literature_authors",
                        column: x => x.AuthorId,
                        principalTable: "authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_literature_genres",
                        column: x => x.GenreId,
                        principalTable: "genres",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_literature_publishers",
                        column: x => x.PublisherId,
                        principalTable: "publishers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_literature_types",
                        column: x => x.TypeId,
                        principalTable: "types",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "writing_has_authors",
                columns: table => new
                {
                    WritingId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_writing_has_authors", x => new { x.WritingId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_writing_has_authors_authors",
                        column: x => x.AuthorId,
                        principalTable: "authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_writing_has_authors_writings",
                        column: x => x.WritingId,
                        principalTable: "writings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exemplars",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiteratureId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsLend = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exemplars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exemplars_literature",
                        column: x => x.LiteratureId,
                        principalTable: "literature",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "literature_has_writings",
                columns: table => new
                {
                    LiteratureId = table.Column<long>(type: "bigint", nullable: false),
                    WritingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_literature_has_writings", x => new { x.LiteratureId, x.WritingId });
                    table.ForeignKey(
                        name: "FK_literature_has_writings_literature",
                        column: x => x.LiteratureId,
                        principalTable: "literature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_literature_has_writings_writings",
                        column: x => x.WritingId,
                        principalTable: "writings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_exemplars_LiteratureId",
                table: "exemplars",
                column: "LiteratureId");

            migrationBuilder.CreateIndex(
                name: "IX_literature_AuthorId",
                table: "literature",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_literature_GenreId",
                table: "literature",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_literature_PublisherId",
                table: "literature",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_literature_TypeId",
                table: "literature",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "UK_literature",
                table: "literature",
                column: "Isbn",
                unique: true,
                filter: "[Isbn] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_literature_has_writings_WritingId",
                table: "literature_has_writings",
                column: "WritingId");

            migrationBuilder.CreateIndex(
                name: "IX_writing_has_authors_AuthorId",
                table: "writing_has_authors",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exemplars");

            migrationBuilder.DropTable(
                name: "literature_has_writings");

            migrationBuilder.DropTable(
                name: "writing_has_authors");

            migrationBuilder.DropTable(
                name: "literature");

            migrationBuilder.DropTable(
                name: "writings");

            migrationBuilder.DropTable(
                name: "authors");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "publishers");

            migrationBuilder.DropTable(
                name: "types");
        }
    }
}
