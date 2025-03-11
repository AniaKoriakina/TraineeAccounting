using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraineeAccounting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetTraineeshipProjectIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Projects_ProjectId",
                table: "Trainees");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Traineeships_TraineeshipId",
                table: "Trainees");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Projects_ProjectId",
                table: "Trainees",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Traineeships_TraineeshipId",
                table: "Trainees",
                column: "TraineeshipId",
                principalTable: "Traineeships",
                principalColumn: "TraineeshipId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Projects_ProjectId",
                table: "Trainees");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Traineeships_TraineeshipId",
                table: "Trainees");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Projects_ProjectId",
                table: "Trainees",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Traineeships_TraineeshipId",
                table: "Trainees",
                column: "TraineeshipId",
                principalTable: "Traineeships",
                principalColumn: "TraineeshipId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
