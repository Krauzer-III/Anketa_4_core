using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Anketa_4_core.Migrations
{
    /// <inheritdoc />
    public partial class ank_migr_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comp_Blocks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BlockName = table.Column<string>(type: "text", nullable: false),
                    WordInReport = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comp_Blocks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Filials",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FilialName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filials", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KT_Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KT_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KT_Questions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KT_Questions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Header = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    DateNews = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReservLevels",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReservLevelName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservLevels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RespondentRoles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    isSelfMark = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespondentRoles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TestPeriods",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestPeriods", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Comp_Questions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionNumber = table.Column<int>(type: "integer", nullable: false),
                    QuestionText = table.Column<string>(type: "text", nullable: false),
                    BlockID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comp_Questions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comp_Questions_Comp_Blocks_BlockID",
                        column: x => x.BlockID,
                        principalTable: "Comp_Blocks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KT_Marks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryID = table.Column<int>(type: "integer", nullable: false),
                    ScoreFrom = table.Column<int>(type: "integer", nullable: false),
                    ScoreTo = table.Column<int>(type: "integer", nullable: false),
                    Mark = table.Column<int>(type: "integer", nullable: false),
                    WordInReport = table.Column<string>(type: "text", nullable: false),
                    LevelWord = table.Column<string>(type: "text", nullable: false),
                    LevelWordInReport = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KT_Marks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KT_Marks_KT_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "KT_Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KT_Answers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnswerNumber = table.Column<string>(type: "text", nullable: false),
                    AnswerText = table.Column<string>(type: "text", nullable: false),
                    QuestionID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KT_Answers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KT_Answers_KT_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "KT_Questions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Testables",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    YearTraining = table.Column<int>(type: "integer", nullable: false),
                    isArchived = table.Column<bool>(type: "boolean", nullable: false),
                    filialID = table.Column<int>(type: "integer", nullable: false),
                    reservLevelID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testables", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Testables_Filials_filialID",
                        column: x => x.filialID,
                        principalTable: "Filials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Testables_ReservLevels_reservLevelID",
                        column: x => x.reservLevelID,
                        principalTable: "ReservLevels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comp_Answers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionID = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    Correct_Answer = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comp_Answers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comp_Answers_Comp_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Comp_Questions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KT_Keys",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryID = table.Column<int>(type: "integer", nullable: false),
                    AnswerID = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KT_Keys", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KT_Keys_KT_Answers_AnswerID",
                        column: x => x.AnswerID,
                        principalTable: "KT_Answers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KT_Keys_KT_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "KT_Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessForTestables",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TestPeriodID = table.Column<int>(type: "integer", nullable: false),
                    TestableID = table.Column<int>(type: "integer", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessForTestables", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccessForTestables_TestPeriods_TestPeriodID",
                        column: x => x.TestPeriodID,
                        principalTable: "TestPeriods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessForTestables_Testables_TestableID",
                        column: x => x.TestableID,
                        principalTable: "Testables",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestableUsers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LK_Data = table.Column<string>(type: "text", nullable: false),
                    testableID = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestableUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TestableUsers_Testables_testableID",
                        column: x => x.testableID,
                        principalTable: "Testables",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comp_TestableAnswers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccessID = table.Column<int>(type: "integer", nullable: false),
                    AnswerID = table.Column<int>(type: "integer", nullable: false),
                    Mark = table.Column<int>(type: "integer", nullable: false),
                    DateTimeAnswer = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comp_TestableAnswers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comp_TestableAnswers_AccessForTestables_AccessID",
                        column: x => x.AccessID,
                        principalTable: "AccessForTestables",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comp_TestableAnswers_Comp_Answers_AnswerID",
                        column: x => x.AnswerID,
                        principalTable: "Comp_Answers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KT_Mains",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Years = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    AccessID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KT_Mains", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KT_Mains_AccessForTestables_AccessID",
                        column: x => x.AccessID,
                        principalTable: "AccessForTestables",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MotivationTests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsAgree = table.Column<bool>(type: "boolean", nullable: false),
                    DatetimeTest = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccessTestID = table.Column<int>(type: "integer", nullable: false),
                    Results = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivationTests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MotivationTests_AccessForTestables_AccessTestID",
                        column: x => x.AccessTestID,
                        principalTable: "AccessForTestables",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Test360",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DatetimeTest = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccessTestID = table.Column<int>(type: "integer", nullable: false),
                    RoleID = table.Column<int>(type: "integer", nullable: false),
                    IsSelfMark = table.Column<bool>(type: "boolean", nullable: false),
                    results = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test360", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Test360_AccessForTestables_AccessTestID",
                        column: x => x.AccessTestID,
                        principalTable: "AccessForTestables",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Test360_RespondentRoles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "RespondentRoles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccessID = table.Column<int>(type: "integer", nullable: false),
                    Results = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TestResults_AccessForTestables_AccessID",
                        column: x => x.AccessID,
                        principalTable: "AccessForTestables",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KT_TestableAnswers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MainInfoID = table.Column<int>(type: "integer", nullable: false),
                    AnswerID = table.Column<int>(type: "integer", nullable: false),
                    DateTimeAnswer = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KT_TestableAnswers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KT_TestableAnswers_KT_Answers_AnswerID",
                        column: x => x.AnswerID,
                        principalTable: "KT_Answers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KT_TestableAnswers_KT_Mains_MainInfoID",
                        column: x => x.MainInfoID,
                        principalTable: "KT_Mains",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessForTestables_TestableID",
                table: "AccessForTestables",
                column: "TestableID");

            migrationBuilder.CreateIndex(
                name: "IX_AccessForTestables_TestPeriodID",
                table: "AccessForTestables",
                column: "TestPeriodID");

            migrationBuilder.CreateIndex(
                name: "IX_Comp_Answers_QuestionID",
                table: "Comp_Answers",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_Comp_Questions_BlockID",
                table: "Comp_Questions",
                column: "BlockID");

            migrationBuilder.CreateIndex(
                name: "IX_Comp_TestableAnswers_AccessID",
                table: "Comp_TestableAnswers",
                column: "AccessID");

            migrationBuilder.CreateIndex(
                name: "IX_Comp_TestableAnswers_AnswerID",
                table: "Comp_TestableAnswers",
                column: "AnswerID");

            migrationBuilder.CreateIndex(
                name: "IX_KT_Answers_QuestionID",
                table: "KT_Answers",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_KT_Keys_AnswerID",
                table: "KT_Keys",
                column: "AnswerID");

            migrationBuilder.CreateIndex(
                name: "IX_KT_Keys_CategoryID",
                table: "KT_Keys",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_KT_Mains_AccessID",
                table: "KT_Mains",
                column: "AccessID");

            migrationBuilder.CreateIndex(
                name: "IX_KT_Marks_CategoryID",
                table: "KT_Marks",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_KT_TestableAnswers_AnswerID",
                table: "KT_TestableAnswers",
                column: "AnswerID");

            migrationBuilder.CreateIndex(
                name: "IX_KT_TestableAnswers_MainInfoID",
                table: "KT_TestableAnswers",
                column: "MainInfoID");

            migrationBuilder.CreateIndex(
                name: "IX_MotivationTests_AccessTestID",
                table: "MotivationTests",
                column: "AccessTestID");

            migrationBuilder.CreateIndex(
                name: "IX_Test360_AccessTestID",
                table: "Test360",
                column: "AccessTestID");

            migrationBuilder.CreateIndex(
                name: "IX_Test360_RoleID",
                table: "Test360",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Testables_filialID",
                table: "Testables",
                column: "filialID");

            migrationBuilder.CreateIndex(
                name: "IX_Testables_reservLevelID",
                table: "Testables",
                column: "reservLevelID");

            migrationBuilder.CreateIndex(
                name: "IX_TestableUsers_testableID",
                table: "TestableUsers",
                column: "testableID");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_AccessID",
                table: "TestResults",
                column: "AccessID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comp_TestableAnswers");

            migrationBuilder.DropTable(
                name: "KT_Keys");

            migrationBuilder.DropTable(
                name: "KT_Marks");

            migrationBuilder.DropTable(
                name: "KT_TestableAnswers");

            migrationBuilder.DropTable(
                name: "MotivationTests");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Test360");

            migrationBuilder.DropTable(
                name: "TestableUsers");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "Comp_Answers");

            migrationBuilder.DropTable(
                name: "KT_Categories");

            migrationBuilder.DropTable(
                name: "KT_Answers");

            migrationBuilder.DropTable(
                name: "KT_Mains");

            migrationBuilder.DropTable(
                name: "RespondentRoles");

            migrationBuilder.DropTable(
                name: "Comp_Questions");

            migrationBuilder.DropTable(
                name: "KT_Questions");

            migrationBuilder.DropTable(
                name: "AccessForTestables");

            migrationBuilder.DropTable(
                name: "Comp_Blocks");

            migrationBuilder.DropTable(
                name: "TestPeriods");

            migrationBuilder.DropTable(
                name: "Testables");

            migrationBuilder.DropTable(
                name: "Filials");

            migrationBuilder.DropTable(
                name: "ReservLevels");
        }
    }
}
