using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToBaseDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeLimitID",
                table: "TimeLimits",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "QuizID",
                table: "Quizzes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "QuestionID",
                table: "Questions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "OptionID",
                table: "Options",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TimeLimits",
                newName: "TimeLimitID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Quizzes",
                newName: "QuizID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Questions",
                newName: "QuestionID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Options",
                newName: "OptionID");
        }
    }
}
