using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmAssist.Repository.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddingQuestionsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "CurrentSymptoms",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HasChronicConditions",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PromptReason",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TakesMedicationsOrTreatments",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentSymptoms",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HasChronicConditions",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PromptReason",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TakesMedicationsOrTreatments",
                table: "AspNetUsers");

           
        }
    }
}
