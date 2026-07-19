using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskListApi.Migrations
{
    public partial class AddGetTasksByUserFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_tasks_by_user(p_user_id uuid)
                RETURNS SETOF ""Tasks""
                LANGUAGE sql
                AS $$
                    SELECT * FROM ""Tasks"" WHERE ""UserId"" = p_user_id ORDER BY ""CreatedAt"" DESC;
                $$;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS get_tasks_by_user(uuid);");
        }
    }
}