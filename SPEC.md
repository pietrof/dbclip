# DBClip - Database Script Manager

## Project Overview
- **Project Name**: DBClip
- **Type**: .NET 9 WinForms Desktop Application
- **Core Feature**: A tool for managing, editing, and running database scripts with template support
- **Target Users**: Developers and DBAs who need to organize and execute SQL scripts

## UI/UX Specification

### Layout Structure
- **Main Form**: Single window with split panels
  - **Top Section**: Toolbar with buttons
  - **Middle Section**: Horizontal split
    - **Left Panel** (250px): TreeView for script organization
    - **Right Panel**: Vertical split
      - **Top Right**: Script editor (TextBox with undo/redo)
      - **Bottom Right**: Results grid (DataGridView)
  - **Bottom Section**: Status bar

### Visual Design
- **Color Palette**:
  - Primary Background: #F5F5F5
  - Secondary Background: #FFFFFF
  - Accent: #0078D4 (Windows blue)
  - Text Primary: #1E1E1E
  - Text Secondary: #6E6E6E
  - Border: #E0E0E0
  - Success: #107C10
  - Error: #D13438

- **Typography**:
  - Font Family: Segoe UI
  - Editor Font: Consolas 12pt
  - Tree Font: Segoe UI 10pt

- **Spacing**:
  - Panel margins: 5px
  - Toolbar button spacing: 3px

### Components

#### Toolbar
- **Play Button** (green triangle icon): Run current script
- **Save Button** (disk icon): Force save (autosave also enabled)
- **Database Settings Button** (gear icon): Open database config dialog
- **Add Script Button** (plus icon): Add new script node
- **Add Folder Button** (folder plus icon): Add folder node
- **Delete Button** (X icon): Delete selected node
- **Undo Button**: Undo
- **Redo Button**: Redo

#### Tree View Panel
- TreeView control with folder and script nodes
- Context menu: Add Script, Add Folder, Rename, Delete
- Drag and drop support for reordering
- Icons: Folder icon for folders, document icon for scripts

#### Script Editor Panel
- Multi-line TextBox with monospace font
- Line numbers (optional)
- Undo/Redo support via Edit menu or Ctrl+Z/Ctrl+Y

#### Results Grid
- DataGridView with DataTable source
- Support for cell and row selection
- Cut/Copy/Paste support (Ctrl+C, Ctrl+X, Ctrl+V)
- Double-click cell to edit

#### Database Settings Dialog
- Connection string input
- Database provider dropdown (SQL Server, PostgreSQL, SQLite, MySQL)
- Test Connection button
- Save/Cancel buttons

#### Template Prompt Dialog
- List of required parameters with input fields
- Option to use values from previous result row
- OK/Cancel buttons

## Functional Specification

### Core Features

1. **Script Management**
   - Create, rename, delete scripts
   - Organize scripts in folders
   - Drag and drop to reorder/move scripts
   - Each script has: Name, ScriptContent, CreatedDate, ModifiedDate

2. **Script Editor**
   - Syntax-free text editing
   - Full undo/redo stack (unlimited)
   - Auto-save on changes (debounced 2 seconds)
   - Tab support for indentation

3. **Template Variables**
   - Syntax: `{VariableName}` in script
   - Prompt dialog shows all variables
   - Pre-populate from result row context if matching names exist
   - Variables replaced at runtime

4. **Script Execution**
   - Execute against configured database
   - Support for SQL Server (primary), PostgreSQL, SQLite, MySQL
   - Show execution time
   - Display results in grid
   - Handle errors gracefully

5. **Results Grid**
   - Display query results in DataGridView
   - Copy cells/rows to clipboard
   - Paste rows from clipboard (tab-separated)
   - Store row context on selection

6. **Context Propagation**
   - Clicking a row stores all column name/value pairs
   - When running a script with templates, check context for matching names
   - Offer to pre-fill template values from context

7. **Data Persistence**
   - Scripts stored in JSON file in AppData folder
   - Database settings stored in app.config or separate config file
   - Auto-save with debouncing

### Data Flow

```
User Action → Command Handler → Service Layer → Repository → FileSystem/DB
                    ↓
              Update UI ← ViewModel ← Data Binding
```

### Key Classes

1. **Models**
   - `ScriptNode`: Name, ScriptContent, NodeType (Folder/Script), Children, Parent
   - `DatabaseSettings`: Provider, ConnectionString
   - `RowContext`: Dictionary<string, object> of column name/values

2. **ViewModels**
   - `MainViewModel`: Orchestrates all operations
   - `ScriptTreeViewModel`: Manages tree state
   - `EditorViewModel`: Manages editor state and undo/redo
   - `ResultsViewModel`: Manages grid data

3. **Services**
   - `IScriptRepository`: Load/save scripts to filesystem
   - `IDatabaseService`: Execute queries
   - `ITemplateParser`: Extract and replace template variables
   - `IClipboardService`: Handle cut/paste operations

### Edge Cases
- Empty script execution: Show warning
- Invalid template syntax: Show error
- Database connection failure: Show error with details
- Large result sets: Paginate or limit (10,000 rows)
- Concurrent edits: Last write wins
- File corruption: Backup and restore

## Acceptance Criteria

1. ✓ Application launches without errors
2. ✓ Can create folders and scripts in tree
3. ✓ Can drag/drop to reorganize
4. ✓ Can edit script content in editor
5. ✓ Undo/redo works for all edits
6. ✓ Scripts auto-save to filesystem
7. ✓ Can configure database connection
8. ✓ Can execute script and see results
9. ✓ Template variables are detected and prompted
10. ✓ Result row context is stored and used for defaults
11. ✓ Grid supports cut/copy/paste
12. ✓ Application handles errors gracefully
