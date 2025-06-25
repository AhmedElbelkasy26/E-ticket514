using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_ticket514.Migrations
{
    /// <inheritdoc />
    public partial class DeleteActorRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorMovie");

            migrationBuilder.AddColumn<int>(
                name: "ActorId",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ActorId",
                table: "Movies",
                column: "ActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Actors_ActorId",
                table: "Movies",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Actors_ActorId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ActorId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "ActorMovie",
                columns: table => new
                {
                    ActorsId = table.Column<int>(type: "int", nullable: false),
                    moviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorMovie", x => new { x.ActorsId, x.moviesId });
                    table.ForeignKey(
                        name: "FK_ActorMovie_Actors_ActorsId",
                        column: x => x.ActorsId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorMovie_Movies_moviesId",
                        column: x => x.moviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovie_moviesId",
                table: "ActorMovie",
                column: "moviesId");
        }
    }
}
