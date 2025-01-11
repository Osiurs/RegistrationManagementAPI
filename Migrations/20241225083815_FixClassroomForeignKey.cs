using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistrationManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixClassroomForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Schedules_ScheduleId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Students_StudentId",
                table: "Attendances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageReadStatus",
                table: "MessageReadStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attendances",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "IsPresent",
                table: "Attendances");

            migrationBuilder.RenameTable(
                name: "Attendances",
                newName: "Attendance");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_StudentId",
                table: "Attendance",
                newName: "IX_Attendance_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_ScheduleId",
                table: "Attendance",
                newName: "IX_Attendance_ScheduleId");

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

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleDate",
                table: "Schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "MessageReadStatusId",
                table: "MessageReadStatus",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Attendance",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageReadStatus",
                table: "MessageReadStatus",
                column: "MessageReadStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attendance",
                table: "Attendance",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ClassroomId1",
                table: "Schedules",
                column: "ClassroomId1");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_CourseId1",
                table: "Schedules",
                column: "CourseId1");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReadStatus_MessageId",
                table: "MessageReadStatus",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Schedules_ScheduleId",
                table: "Attendance",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Students_StudentId",
                table: "Attendance",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Schedules_ScheduleId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Students_StudentId",
                table: "Attendance");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageReadStatus",
                table: "MessageReadStatus");

            migrationBuilder.DropIndex(
                name: "IX_MessageReadStatus_MessageId",
                table: "MessageReadStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attendance",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "ClassroomId1",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ScheduleDate",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Attendance");

            migrationBuilder.RenameTable(
                name: "Attendance",
                newName: "Attendances");

            migrationBuilder.RenameIndex(
                name: "IX_Attendance_StudentId",
                table: "Attendances",
                newName: "IX_Attendances_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendance_ScheduleId",
                table: "Attendances",
                newName: "IX_Attendances_ScheduleId");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "MessageReadStatusId",
                table: "MessageReadStatus",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "IsPresent",
                table: "Attendances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageReadStatus",
                table: "MessageReadStatus",
                columns: new[] { "MessageId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attendances",
                table: "Attendances",
                column: "AttendanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Schedules_ScheduleId",
                table: "Attendances",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Students_StudentId",
                table: "Attendances",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
