using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedicalSupplies.Mvc.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReceiverName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplyCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Barcode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SupplyCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Provider = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplies_SupplyCategories_SupplyCategoryId",
                        column: x => x.SupplyCategoryId,
                        principalTable: "SupplyCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IssueItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IssueId = table.Column<int>(type: "INTEGER", nullable: false),
                    SupplyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueItems_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssueItems_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SupplyCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Vật tư tiêu hao" },
                    { 2, "Dụng cụ tiêm truyền" },
                    { 3, "Thiết bị y tế" }
                });

            migrationBuilder.InsertData(
                table: "Supplies",
                columns: new[] { "Id", "Barcode", "Code", "LastUpdated", "Name", "Price", "Provider", "Quantity", "SupplyCategoryId" },
                values: new object[,]
                {
                    { 1, "893001", "MS001", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khẩu trang y tế 4 lớp", 50000m, "MediPlast", 1000, 1 },
                    { 2, "893002", "MS002", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Găng tay y tế Nitrile", 80000m, "VGlove", 500, 1 },
                    { 3, "893003", "MS003", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bơm tiêm nhựa 5ml", 1500m, "Vinahoc", 0, 2 },
                    { 4, "893004", "MS004", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dây truyền dịch", 8000m, "Danapha", 15, 2 },
                    { 5, "893005", "MS005", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Máy đo huyết áp điện tử", 1200000m, "Omron", 15, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssueItems_IssueId",
                table: "IssueItems",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueItems_SupplyId",
                table: "IssueItems",
                column: "SupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_SupplyCategoryId",
                table: "Supplies",
                column: "SupplyCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueItems");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DropTable(
                name: "SupplyCategories");
        }
    }
}
