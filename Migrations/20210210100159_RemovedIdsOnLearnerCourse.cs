using Microsoft.EntityFrameworkCore.Migrations;

namespace learner_portal.Migrations
{
    public partial class RemovedIdsOnLearnerCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CourseName",
                table: "LearnerCourse",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InstitutionName",
                table: "LearnerCourse",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "LearnerCourse");

            migrationBuilder.DropColumn(
                name: "InstitutionName",
                table: "LearnerCourse");
        }
    }
}
