# Options Management Database System - Developer Guide

## Overview

This feature provides a development-only options management system for mRemoteNG that allows manual addition, editing, and deletion of application options at runtime. The system is only available in DEBUG builds and is automatically initialized during application startup.

## Architecture

### Components

1. **OptionInfo** (`mRemoteNG/Config/Settings/OptionInfo.cs`)
   - Model class representing a single option
   - Properties: Id, Key, Value, Category, Description, OptionType, CreatedDate, ModifiedDate

2. **IOptionsRepository** (`mRemoteNG/Config/Settings/IOptionsRepository.cs`)
   - Interface defining CRUD operations for options
   - Async-based design for database operations

3. **OptionsRepository** (`mRemoteNG/Config/Settings/OptionsRepository.cs`)
   - Implementation of IOptionsRepository
   - Provides business logic and validation

4. **OptionsStore** (`mRemoteNG/Config/Settings/Store/OptionsStore.cs`)
   - SQLite-based persistence layer
   - Handles database schema creation and low-level CRUD operations
   - Uses WAL mode for better concurrent performance

5. **OptionsRepositoryManager** (`mRemoteNG/Config/Settings/OptionsRepositoryManager.cs`)
   - Manages lifecycle of the options store and repository
   - Handles initialization and cleanup

6. **OptionsManagementPage** (`mRemoteNG/UI/Forms/OptionsPages/OptionsManagementPage.cs`)
   - Windows Forms UI for managing options
   - DataGridView for displaying options
   - Add, Edit, Delete, and Refresh functionality

## Database Schema

The options are stored in a SQLite database table with the following structure:

```sql
CREATE TABLE options (
	id          INTEGER PRIMARY KEY AUTOINCREMENT,
	key         TEXT    NOT NULL UNIQUE,
	value       TEXT,
	category    TEXT,
	description TEXT,
	option_type TEXT    NOT NULL DEFAULT 'string',
	created_at  TEXT    NOT NULL DEFAULT (datetime('now')),
	modified_at TEXT    NOT NULL DEFAULT (datetime('now'))
);

CREATE INDEX idx_options_key ON options(key);
CREATE INDEX idx_options_category ON options(category);
```

## Usage

### Accessing the Options Management UI

1. Open mRemoteNG in DEBUG build
2. Go to **Tools → Options** (or press Ctrl+O)
3. In the Options dialog, you'll see a new page: **"Options Management (Dev)"**
4. This page is only visible in DEBUG builds

### Using the Options Management Page

#### Adding a New Option
1. Fill in the required fields: **Key** (required)
2. Optional fields: Value, Category, Description, Type
3. Click **Add** button
4. Option is immediately saved to the database

#### Editing an Option
1. Select an option from the grid
2. Modify the fields in the form below
3. Click **Edit** button to save changes

#### Deleting an Option
1. Select an option from the grid
2. Click **Delete** button
3. Confirm the deletion

#### Refreshing the List
1. Click **Refresh** button to reload all options from the database

### Programmatic Access

You can access the options repository programmatically:

```csharp
// Get the repository
IOptionsRepository repository = Runtime.OptionsRepositoryManager.Repository;

// Add an option
var option = new OptionInfo
{
	Key = "MyOption",
	Value = "MyValue",
	Category = "Development",
	Description = "My test option",
	OptionType = "string"
};
var added = await repository.AddOptionAsync(option);

// Get an option
var retrieved = await repository.GetOptionByKeyAsync("MyOption");

// Update an option
retrieved.Value = "NewValue";
bool updated = await repository.UpdateOptionAsync(retrieved);

// Delete an option
bool deleted = await repository.DeleteOptionAsync(added.Id);

// Get all options
var allOptions = await repository.GetAllOptionsAsync();

// Get options by category
var categoryOptions = await repository.GetOptionsByCategoryAsync("Development");
```

## Data Location

The options database file (`mremoteng.options.db`) is stored in the same directory as other mRemoteNG settings, typically:
- Windows: `%AppData%\mRemoteNG\`

## Conditional Compilation

The options management feature is only compiled into DEBUG builds via the `#if DEBUG` preprocessor directives:

- **Startup initialization**: `mRemoteNG/App/Startup.cs` - Options repository is initialized during app startup
- **UI page**: `mRemoteNG/UI/Forms/frmOptions.cs` - Options page is added to the Options dialog
- **Runtime property**: `mRemoteNG/App/Runtime.cs` - OptionsRepositoryManager is available

For RELEASE builds, all of this code is completely excluded, ensuring no production impact.

## Testing

Comprehensive unit tests are included: `mRemoteNGTests/Config/Settings/OptionsRepositoryTests.cs`

Test coverage includes:
- Adding options (valid, null, empty key, duplicate key)
- Getting options (by key, by ID, all, by category)
- Updating options (existing, non-existing)
- Deleting options (by ID, by key)
- Querying (exists check, count, clear all)
- Integration workflows

Run tests with:
```bash
dotnet test mRemoteNGTests/mRemoteNGTests.csproj --filter "OptionsRepository"
```

All 24 tests pass successfully.

## Localization

Localization strings have been added to `Language.resx`:
- UI labels: OptionKey, OptionValue, OptionCategory, OptionDescription, OptionType
- Button labels: ButtonAdd, ButtonEdit, ButtonDelete, ButtonRefresh
- Messages: Success, error, and confirmation messages

## Important Notes

### Development Only
- This feature is **exclusively for development/debugging purposes**
- Not intended for production use
- Only available in DEBUG builds
- Options are stored in a separate database file

### Security Considerations
- Options database is not encrypted (unlike main settings)
- Suitable only for development; not for storing sensitive data
- No authentication or access control

### Performance
- Uses SQLite WAL (Write-Ahead Logging) mode for better concurrency
- Asynchronous operations to prevent UI blocking
- Efficient indexing on key and category columns

## Extending the System

To add more functionality:

1. **Add database fields**: Modify `OptionsStore.CreateSchema()` to add columns
2. **Add properties to OptionInfo**: Update the model class
3. **Add UI controls**: Extend `OptionsManagementPage` with additional fields
4. **Add repository methods**: Implement new query methods in `IOptionsRepository`/`OptionsRepository`

## Troubleshooting

### Options page not appearing
- Verify you're running a DEBUG build (not RELEASE)
- Check that the options repository was initialized successfully
- Check the debug output for initialization messages

### Options not persisting
- Verify the database file exists in the settings directory
- Check that the options repository is properly initialized
- Ensure the key is unique (no duplicate keys allowed)

### Addition fails with "key already exists"
- Use unique keys for each option
- Delete duplicate options first

## Credits

Developed for mRemoteNG as a development utility to simplify options management during development and testing phases.
