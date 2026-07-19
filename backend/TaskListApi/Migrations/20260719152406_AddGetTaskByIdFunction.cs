using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskListApi.Migrations
{
    public partial class AddGetTaskByIdFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_task_by_id(p_id uuid, p_user_id uuid)
                RETURNS SETOF ""Tasks""
                LANGUAGE sql
                AS $$
                    SELECT * FROM ""Tasks"" WHERE ""Id"" = p_id AND ""UserId"" = p_user_id;
                $$;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS get_task_by_id(uuid, uuid);");
        }
    }
}