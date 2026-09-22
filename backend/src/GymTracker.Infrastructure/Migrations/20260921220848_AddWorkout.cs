using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GymTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "workouts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exercise_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    duration_minutes = table.Column<int>(type: "integer", nullable: false),
                    calories_burned = table.Column<int>(type: "integer", nullable: false),
                    intensity = table.Column<int>(type: "integer", nullable: false),
                    fatigue = table.Column<int>(type: "integer", nullable: false),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    performed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workouts", x => x.id);
                    table.CheckConstraint("CK_Workouts_Calories", "calories_burned >= 0");
                    table.CheckConstraint("CK_Workouts_Duration", "duration_minutes > 0");
                    table.CheckConstraint("CK_Workouts_Fatigue", "fatigue BETWEEN 1 AND 10");
                    table.CheckConstraint("CK_Workouts_Intensity", "intensity BETWEEN 1 AND 10");
                    table.ForeignKey(
                        name: "fk_workouts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_workouts_user_id_performed_at",
                table: "workouts",
                columns: new[] { "user_id", "performed_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "workouts");
        }
    }
}
