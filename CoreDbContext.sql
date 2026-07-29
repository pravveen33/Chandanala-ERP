CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "MedicineKnowledge" (
    "Id" uuid NOT NULL,
    "Code" character varying(50) NOT NULL,
    "BrandName" character varying(200) NOT NULL,
    "GenericName" character varying(200) NOT NULL,
    "Strength" character varying(50) NOT NULL,
    "DosageForm" character varying(100) NOT NULL,
    "Barcode" character varying(20) NOT NULL,
    "Status" text NOT NULL,
    "DrugSchedule" text NOT NULL,
    "StorageType" text NOT NULL,
    "CreatedOnUtc" timestamp without time zone NOT NULL,
    "UpdatedOnUtc" timestamp without time zone,
    "IsDeleted" boolean NOT NULL,
    CONSTRAINT "PK_MedicineKnowledge" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_MedicineKnowledge_Code" ON "MedicineKnowledge" ("Code");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260726084339_InitialCore', '10.0.9');

COMMIT;

