using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistrationManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTeacherIdFromSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Classrooms_ClassroomId1",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Courses_CourseId1",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_ClassroomId1",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_CourseId1",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ClassroomId1",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "Schedules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassroomId1",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseId1",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ClassroomId1",
                table: "Schedules",
                column: "ClassroomId1");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_CourseId1",
                table: "Schedules",
                column: "CourseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Classrooms_ClassroomId1",
                table: "Schedules",
                column: "ClassroomId1",
                principalTable: "Classrooms",
                principalColumn: "ClassroomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Courses_CourseId1",
                table: "Schedules",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "CourseId");
        }
    }
}
