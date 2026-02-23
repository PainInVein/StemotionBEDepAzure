using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMotion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ThanhDT_ErikDev : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    grade_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    grade_level = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    order_index = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.grade_id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    subscription_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    subscription_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    subscription_price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    billing_period = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.subscription_id);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    subject_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    grade_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.subject_id);
                    table.ForeignKey(
                        name: "FK_Subject.grade_id",
                        column: x => x.grade_id,
                        principalTable: "Grade",
                        principalColumn: "grade_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    firstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    roleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    gradeLevel = table.Column<int>(type: "int", nullable: true),
                    avatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userId);
                    table.ForeignKey(
                        name: "FK_User.role_id",
                        column: x => x.roleId,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    chapter_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    subject_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    chapter_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => x.chapter_id);
                    table.ForeignKey(
                        name: "FK_Chapter.subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    payment_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    info = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    payment_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_Payment_UserId",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    student_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    parent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    avatar_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    grade_level = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.student_id);
                    table.ForeignKey(
                        name: "FK_Student_parent_id",
                        column: x => x.parent_id,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    lesson_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    chapter_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lessonName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    estimated_time = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lesson", x => x.lesson_id);
                    table.ForeignKey(
                        name: "FK_Lesson.chapter_id",
                        column: x => x.chapter_id,
                        principalTable: "Chapter",
                        principalColumn: "chapter_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPayment",
                columns: table => new
                {
                    subscription_payment_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    payment_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    subscription_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_success = table.Column<bool>(type: "bit", nullable: true),
                    account_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    order_code = table.Column<long>(type: "bigint", nullable: true),
                    reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    payment_link_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    transaction_datetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    counter_account_bank_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    counter_account_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    counter_account_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPayment", x => x.subscription_payment_id);
                    table.ForeignKey(
                        name: "FK_SubscriptionPayment_Payment_payment_id",
                        column: x => x.payment_id,
                        principalTable: "Payment",
                        principalColumn: "payment_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubscriptionPayment_Subscription_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "Subscription",
                        principalColumn: "subscription_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    game_code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    lesson_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    config_data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    thumbnail_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.game_id);
                    table.ForeignKey(
                        name: "FK_Game.lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "Lesson",
                        principalColumn: "lesson_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessonContent",
                columns: table => new
                {
                    lesson_content_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    lesson_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    content_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    text_content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    media_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    formula_latex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    order_index = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonContent", x => x.lesson_content_id);
                    table.ForeignKey(
                        name: "FK_LessonContent.lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "Lesson",
                        principalColumn: "lesson_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentProgress",
                columns: table => new
                {
                    student_progress_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    student_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lesson_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_completed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    completion_percentage = table.Column<int>(type: "int", nullable: true),
                    started_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    completed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_accessed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "NotStarted"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProgress", x => x.student_progress_id);
                    table.ForeignKey(
                        name: "FK_StudentProgress_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "FK_StudentProgress_lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "Lesson",
                        principalColumn: "lesson_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentProgress_student_id",
                        column: x => x.student_id,
                        principalTable: "Student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameResult",
                columns: table => new
                {
                    game_result_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    student_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    score = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    correct_answers = table.Column<int>(type: "int", nullable: false),
                    total_questions = table.Column<int>(type: "int", nullable: false),
                    play_duration = table.Column<int>(type: "int", nullable: false),
                    played_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameResult", x => x.game_result_id);
                    table.ForeignKey(
                        name: "FK_GameResult.game_id",
                        column: x => x.game_id,
                        principalTable: "Game",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameResult.student_id",
                        column: x => x.student_id,
                        principalTable: "Student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_subject_id",
                table: "Chapter",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_Game_GameCode",
                table: "Game",
                column: "game_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_lesson_id",
                table: "Game",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_GameResult_game_id",
                table: "GameResult",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_GameResult_PlayedAt",
                table: "GameResult",
                column: "played_at");

            migrationBuilder.CreateIndex(
                name: "IX_GameResult_StudentId_GameId",
                table: "GameResult",
                columns: new[] { "student_id", "game_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Grade_grade_level",
                table: "Grade",
                column: "grade_level",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grade_OrderIndex",
                table: "Grade",
                column: "order_index");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_chapter_id",
                table: "Lesson",
                column: "chapter_id");

            migrationBuilder.CreateIndex(
                name: "IX_LessonContent_lesson_id",
                table: "LessonContent",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_user_id",
                table: "Payment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Role_name",
                table: "Role",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_parent_id",
                table: "Student",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Username",
                table: "Student",
                column: "username",
                unique: true,
                filter: "[username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgress_lesson_id",
                table: "StudentProgress",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgress_Student_Lesson",
                table: "StudentProgress",
                columns: new[] { "student_id", "lesson_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgress_UserId",
                table: "StudentProgress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_grade_id",
                table: "Subject",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayment_payment_id",
                table: "SubscriptionPayment",
                column: "payment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayment_subscription_id",
                table: "SubscriptionPayment",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_email",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_roleId",
                table: "User",
                column: "roleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameResult");

            migrationBuilder.DropTable(
                name: "LessonContent");

            migrationBuilder.DropTable(
                name: "StudentProgress");

            migrationBuilder.DropTable(
                name: "SubscriptionPayment");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "Grade");
        }
    }
}
