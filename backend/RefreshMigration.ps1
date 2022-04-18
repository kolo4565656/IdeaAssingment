$PrevMigration=$args[0]
$NewMigrationName=$args[1]
Update-database $PrevMigration
remove-migration
add-migration $NewMigrationName
Update-database