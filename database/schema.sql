CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "Users" (
    "Id" uuid NOT NULL,
    "Username" text NOT NULL,
    "Email" text NOT NULL,
    "PasswordHash" text,
    "AuthProvider" text NOT NULL,
    "ProviderUserId" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "Tasks" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Title" text NOT NULL,
    "Description" text,
    "DueDate" timestamp with time zone,
    "Priority" integer NOT NULL,
    "Category" text,
    "Status" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Tasks" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Tasks_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Tasks_UserId" ON "Tasks" ("UserId");

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

CREATE UNIQUE INDEX "IX_Users_Username" ON "Users" ("Username");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260718140631_InitialCreate', '10.0.10');

CREATE OR REPLACE FUNCTION get_tasks_by_user(p_user_id uuid)
RETURNS SETOF "Tasks"
LANGUAGE sql
AS $$
    SELECT * FROM "Tasks" WHERE "UserId" = p_user_id ORDER BY "CreatedAt" DESC;
$$;

CREATE OR REPLACE FUNCTION get_task_by_id(p_id uuid, p_user_id uuid)
RETURNS SETOF "Tasks"
LANGUAGE sql
AS $$
    SELECT * FROM "Tasks" WHERE "Id" = p_id AND "UserId" = p_user_id;
$$;

COMMIT;

