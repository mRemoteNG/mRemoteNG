# SQL tree revision cutover (schema 3.4)

This migration is deliberately manual. The application refuses to save a shared SQL tree unless the loaded model has a revision snapshot and `tblTreeRevision` exists. Version 3.3 writers must be stopped before cutover; the 3.4 `tblRoot.ConfVersion` causes older mRemoteNG writers, which support only 3.3, to reject writes. Do not run this against a production store without an approved maintenance window and a verified backup.

## Preflight and deployment order

1. Test against disposable SQL Server and MySQL databases restored from representative 3.3 backups.
2. Stop every mRemoteNG process and other writer for the target database. Confirm no save is pending.
3. Take and verify a full database backup. Record the current root metadata and revision table absence.
4. Apply exactly one backend script below. Verify one row `(Id=1, Revision=0, MinimumWriterProtocol=1)` and `tblRoot.ConfVersion='3.4'`.
5. Roll out only the 3.4 writer build. Validate two concurrent loaded drafts: first save succeeds, stale second save is rejected while retaining its draft.

## SQL Server

```sql
SET XACT_ABORT ON;
BEGIN TRANSACTION;
IF (SELECT COUNT(*) FROM dbo.tblRoot) <> 1 OR EXISTS (SELECT 1 FROM dbo.tblRoot WHERE ConfVersion <> '3.3')
    THROW 51002, 'Expected exactly one schema 3.3 root row; stop and inspect the database.', 1;
IF OBJECT_ID(N'dbo.tblTreeRevision', N'U') IS NOT NULL
    THROW 51000, 'Revision table already exists; stop and inspect the database.', 1;
CREATE TABLE dbo.tblTreeRevision (
    Id int NOT NULL CONSTRAINT PK_tblTreeRevision PRIMARY KEY,
    Revision bigint NOT NULL,
    MinimumWriterProtocol int NOT NULL,
    CONSTRAINT CK_tblTreeRevision_Singleton CHECK (Id = 1),
    CONSTRAINT CK_tblTreeRevision_Nonnegative CHECK (Revision >= 0 AND MinimumWriterProtocol >= 1)
);
IF COL_LENGTH(N'dbo.tblCons', N'EnvironmentTags') IS NULL
    ALTER TABLE dbo.tblCons ADD EnvironmentTags varchar(2048) NOT NULL DEFAULT '' WITH VALUES;
IF COL_LENGTH(N'dbo.tblCons', N'InheritEnvironmentTags') IS NULL
    ALTER TABLE dbo.tblCons ADD InheritEnvironmentTags bit NOT NULL DEFAULT 0 WITH VALUES;
INSERT dbo.tblTreeRevision (Id, Revision, MinimumWriterProtocol) VALUES (1, 0, 1);
UPDATE dbo.tblRoot SET ConfVersion='3.4';
IF @@ROWCOUNT <> 1 THROW 51001, 'Expected exactly one tblRoot row.', 1;
COMMIT;
```

## MySQL/InnoDB

MySQL DDL implicitly commits. Run only while all writers are stopped and after the verified backup. If any statement fails, keep writers stopped and restore the pre-cutover backup or manually return the store to its reviewed state.

The script assumes the standard upstream 3.3 schema without `EnvironmentTags` and `InheritEnvironmentTags`. If either column already exists, stop and review its definition before adapting the corresponding `ALTER TABLE` statement.

```sql
CREATE TABLE tblTreeRevision (
    Id int NOT NULL PRIMARY KEY,
    Revision bigint NOT NULL,
    MinimumWriterProtocol int NOT NULL,
    CONSTRAINT CK_tblTreeRevision_Singleton CHECK (Id = 1),
    CONSTRAINT CK_tblTreeRevision_Nonnegative CHECK (Revision >= 0 AND MinimumWriterProtocol >= 1)
) ENGINE=InnoDB;
ALTER TABLE tblCons
    ADD COLUMN EnvironmentTags varchar(2048) NOT NULL DEFAULT '',
    ADD COLUMN InheritEnvironmentTags tinyint NOT NULL DEFAULT 0;
INSERT INTO tblTreeRevision (Id, Revision, MinimumWriterProtocol) VALUES (1, 0, 1);
UPDATE tblRoot SET ConfVersion='3.4';
```

## Rollback

Stop all 3.4 writers first. If no 3.4 save has committed, remove the singleton table and restore the original `ConfVersion='3.3'`; then older clients may resume. If any 3.4 revision advanced, do not downgrade metadata or drop the revision row: preserve the database and drafts, export/reconcile newer changes under the 3.4 build, and use the verified pre-cutover backup only as a separately reviewed recovery source. Never restore an old snapshot automatically over newer edits.
