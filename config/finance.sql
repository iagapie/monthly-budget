\connect finance

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "AspNetRoles" (
    "Id" text NOT NULL,
    "Name" character varying(256) NULL,
    "NormalizedName" character varying(256) NULL,
    "ConcurrencyStamp" text NULL,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetUsers" (
    "Id" text NOT NULL,
    "UserName" character varying(256) NULL,
    "NormalizedUserName" character varying(256) NULL,
    "Email" character varying(256) NULL,
    "NormalizedEmail" character varying(256) NULL,
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text NULL,
    "SecurityStamp" text NULL,
    "ConcurrencyStamp" text NULL,
    "PhoneNumber" text NULL,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone NULL,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" serial NOT NULL,
    "RoleId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" serial NOT NULL,
    "UserId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text NULL,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName")
VALUES ('d48ef9ed-313e-40d5-94f8-6fb1d0b20d3d', 0, '4c3d05fe-8792-4dc0-87b7-ca27b334d262', 'admin@finance.com', FALSE, TRUE, NULL, 'ADMIN@FINANCE.COM', 'ADMIN', 'AQAAAAEAACcQAAAAEFxH2wykvZDg+IuZsiD3qMuTagEKd73JG4lNDzzRhLN3vYsUrTqrCh0/IQgO+qVhAg==', NULL, FALSE, 'N2LVMJFJEXOV24XLKLO7P3EMAO66JSAZ', FALSE, 'admin');

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190617185652_InitialIdentity', '2.2.4-servicing-10062');


CREATE SEQUENCE "AppUserId" START WITH 2 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE;

CREATE TABLE "Directions" (
                              "Id" integer NOT NULL DEFAULT 1,
                              "Name" character varying(50) NOT NULL,
                              CONSTRAINT "PK_Directions" PRIMARY KEY ("Id")
);

CREATE TABLE "Users" (
                         "Id" bigint NOT NULL DEFAULT (nextval('"AppUserId"')),
                         "CreatedAt" timestamp with time zone NOT NULL,
                         "UpdatedAt" timestamp with time zone NULL,
                         "FirstName" character varying(255) NULL,
                         "LastName" character varying(255) NULL,
                         "UserName" character varying(255) NOT NULL,
                         "Email" character varying(255) NOT NULL,
                         "IdentityId" character varying(255) NOT NULL,
                         "Role" character varying(255) NOT NULL,
                         CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "Projects" (
                            "Id" bigserial NOT NULL,
                            "CreatedAt" timestamp with time zone NOT NULL,
                            "UpdatedAt" timestamp with time zone NULL,
                            "Name" character varying(255) NOT NULL,
                            "Currency" character varying(255) NOT NULL,
                            "OwnerId" bigint NOT NULL,
                            CONSTRAINT "PK_Projects" PRIMARY KEY ("Id"),
                            CONSTRAINT "FK_Projects_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "RefreshTokens" (
                                 "Id" bigserial NOT NULL,
                                 "CreatedAt" timestamp with time zone NOT NULL,
                                 "UpdatedAt" timestamp with time zone NULL,
                                 "Token" character varying(255) NOT NULL,
                                 "Expires" timestamp without time zone NOT NULL,
                                 "UserId" bigint NOT NULL,
                                 "RemoteIpAddress" character varying(255) NULL,
                                 CONSTRAINT "PK_RefreshTokens" PRIMARY KEY ("Id"),
                                 CONSTRAINT "FK_RefreshTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Movements" (
                             "Id" bigserial NOT NULL,
                             "CreatedAt" timestamp with time zone NOT NULL,
                             "UpdatedAt" timestamp with time zone NULL,
                             "ProjectId" bigint NOT NULL,
                             "DirectionId" integer NOT NULL,
                             "Name" text NOT NULL,
                             "PlanAmount" numeric NOT NULL,
                             "OwnerId" bigint NULL,
                             CONSTRAINT "PK_Movements" PRIMARY KEY ("Id"),
                             CONSTRAINT "FK_Movements_Directions_DirectionId" FOREIGN KEY ("DirectionId") REFERENCES "Directions" ("Id") ON DELETE CASCADE,
                             CONSTRAINT "FK_Movements_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "Users" ("Id") ON DELETE SET NULL,
                             CONSTRAINT "FK_Movements_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE
);

CREATE TABLE "MovementItems" (
                                 "Id" bigserial NOT NULL,
                                 "CreatedAt" timestamp with time zone NOT NULL,
                                 "UpdatedAt" timestamp with time zone NULL,
                                 "MovementId" bigint NOT NULL,
                                 "Date" timestamp with time zone NOT NULL,
                                 "Amount" numeric NOT NULL,
                                 "Description" text NULL,
                                 "OwnerId" bigint NULL,
                                 CONSTRAINT "PK_MovementItems" PRIMARY KEY ("Id"),
                                 CONSTRAINT "FK_MovementItems_Movements_MovementId" FOREIGN KEY ("MovementId") REFERENCES "Movements" ("Id") ON DELETE CASCADE,
                                 CONSTRAINT "FK_MovementItems_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "Users" ("Id") ON DELETE SET NULL
);

INSERT INTO "Directions" ("Id", "Name")
VALUES (1, 'in');
INSERT INTO "Directions" ("Id", "Name")
VALUES (-1, 'out');

INSERT INTO "Users" ("Id", "CreatedAt", "Email", "FirstName", "IdentityId", "LastName", "Role", "UpdatedAt", "UserName")
VALUES (1, TIMESTAMPTZ '2019-06-18 00:14:27.553267+00:00', 'admin@finance.com', NULL, 'd48ef9ed-313e-40d5-94f8-6fb1d0b20d3d', NULL, 'admin', NULL, 'admin');

CREATE INDEX "IX_MovementItems_MovementId" ON "MovementItems" ("MovementId");

CREATE INDEX "IX_MovementItems_OwnerId" ON "MovementItems" ("OwnerId");

CREATE INDEX "IX_Movements_DirectionId" ON "Movements" ("DirectionId");

CREATE INDEX "IX_Movements_OwnerId" ON "Movements" ("OwnerId");

CREATE INDEX "IX_Movements_ProjectId" ON "Movements" ("ProjectId");

CREATE INDEX "IX_Projects_Currency" ON "Projects" ("Currency");

CREATE INDEX "IX_Projects_Name" ON "Projects" ("Name");

CREATE INDEX "IX_Projects_OwnerId" ON "Projects" ("OwnerId");

CREATE INDEX "IX_RefreshTokens_UserId" ON "RefreshTokens" ("UserId");

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

CREATE UNIQUE INDEX "IX_Users_IdentityId" ON "Users" ("IdentityId");

CREATE UNIQUE INDEX "IX_Users_UserName" ON "Users" ("UserName");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190618001428_InitialFinance', '2.2.4-servicing-10062');
