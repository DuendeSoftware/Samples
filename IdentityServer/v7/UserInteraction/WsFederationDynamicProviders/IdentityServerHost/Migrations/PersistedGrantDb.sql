﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "DeviceCodes" (
    "UserCode" TEXT NOT NULL CONSTRAINT "PK_DeviceCodes" PRIMARY KEY,
    "DeviceCode" TEXT NOT NULL,
    "SubjectId" TEXT NULL,
    "SessionId" TEXT NULL,
    "ClientId" TEXT NOT NULL,
    "Description" TEXT NULL,
    "CreationTime" TEXT NOT NULL,
    "Expiration" TEXT NOT NULL,
    "Data" TEXT NOT NULL
);

CREATE TABLE "Keys" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Keys" PRIMARY KEY,
    "Version" INTEGER NOT NULL,
    "Created" TEXT NOT NULL,
    "Use" TEXT NULL,
    "Algorithm" TEXT NOT NULL,
    "IsX509Certificate" INTEGER NOT NULL,
    "DataProtected" INTEGER NOT NULL,
    "Data" TEXT NOT NULL
);

CREATE TABLE "PersistedGrants" (
    "Key" TEXT NOT NULL CONSTRAINT "PK_PersistedGrants" PRIMARY KEY,
    "Type" TEXT NOT NULL,
    "SubjectId" TEXT NULL,
    "SessionId" TEXT NULL,
    "ClientId" TEXT NOT NULL,
    "Description" TEXT NULL,
    "CreationTime" TEXT NOT NULL,
    "Expiration" TEXT NULL,
    "ConsumedTime" TEXT NULL,
    "Data" TEXT NOT NULL
);

CREATE UNIQUE INDEX "IX_DeviceCodes_DeviceCode" ON "DeviceCodes" ("DeviceCode");

CREATE INDEX "IX_DeviceCodes_Expiration" ON "DeviceCodes" ("Expiration");

CREATE INDEX "IX_Keys_Use" ON "Keys" ("Use");

CREATE INDEX "IX_PersistedGrants_ConsumedTime" ON "PersistedGrants" ("ConsumedTime");

CREATE INDEX "IX_PersistedGrants_Expiration" ON "PersistedGrants" ("Expiration");

CREATE INDEX "IX_PersistedGrants_SubjectId_ClientId_Type" ON "PersistedGrants" ("SubjectId", "ClientId", "Type");

CREATE INDEX "IX_PersistedGrants_SubjectId_SessionId_Type" ON "PersistedGrants" ("SubjectId", "SessionId", "Type");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220104195544_Grants', '6.0.0');

COMMIT;

